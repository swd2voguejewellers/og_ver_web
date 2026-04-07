using Microsoft.EntityFrameworkCore;
using TestSPA.Interfaces;
using TestSPA.Models;
using TestSPA.ViewModel;

namespace TestSPA.Repository
{
    public class OGRepository : IOGRepository
    {
        private readonly AppDbContext _context;
        private readonly AppDbContext1 _context1;
        private readonly AppDbContext2 _context2;
        public OGRepository(AppDbContext context, AppDbContext1 context1, AppDbContext2 context2)
        {
            _context = context;
            _context1 = context1;
            _context2 = context2;
        }
        public async Task<int> GetNextOGVerifyNoAsync()
        {
            int maxNo = await _context.OGVerifications
                .Select(x => (int?)x.OGVerifyNo)
                .MaxAsync() ?? 0;

            return maxNo + 1;
        }

        public async Task<(bool success, int ogVerifyNo)> SaveOrUpdateAsync(OGVerificationVM model)
        {
            await using var tran = await _context.Database.BeginTransactionAsync();

            try
            {
                int ogVerifyNo;

                // 🔹 Insert or Update
                if (model.OGVerifyNo.HasValue)
                {
                    ogVerifyNo = model.OGVerifyNo.Value;

                    var oldRows = await _context.OGVerifications
                        .Where(x => x.OGVerifyNo == ogVerifyNo)
                        .ToListAsync();

                    _context.OGVerifications.RemoveRange(oldRows);
                    await _context.SaveChangesAsync();
                }
                else
                {
                    ogVerifyNo = await GetNextOGVerifyNoAsync();
                }

                // 🔹 Insert fresh rows
                int num = 1;

                foreach (var item in model.Details)
                {
                    item.Id = 0;
                    item.OGVerifyNo = ogVerifyNo;
                    item.Num = num++;
                    item.OGVerifyDate = DateTime.Now.Date;

                    await _context.OGVerifications.AddAsync(item);
                }

                await _context.SaveChangesAsync();
                await tran.CommitAsync();

                return (true, ogVerifyNo); // ✅ RETURN NUMBER
            }
            catch
            {
                await tran.RollbackAsync();
                return (false, 0);
            }
        }

        public async Task<RatesDetails?> GetLatestRatesAsync()
        {
            try
            {
                return await _context1.ratesDetails
                    .OrderByDescending(x => x.Entered_Date)
                    .Select(x => new RatesDetails
                    {
                        KT_24 = x.KT_24,
                        OG_22_KT = x.OG_22_KT,
                        OG_21_KT = x.OG_21_KT,
                        OG_20_KT = x.OG_20_KT,
                        OG_19_KT = x.OG_19_KT,
                        OG_18_KT = x.OG_18_KT
                    })
                    .FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public async Task<List<OGVerification>> GetOGVerificationsByNumberAsync(int ogVerifyNo)
        {
            try
            {
                return await _context.OGVerifications
                    .Where(x => x.OGVerifyNo == ogVerifyNo)
                    .OrderBy(x=>x.Num)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                // You can log the error here
                Console.WriteLine(ex.Message);

                // Return empty list to avoid breaking UI
                return new List<OGVerification>();
            }
        }

        public async Task<decimal?> CalculateActValueAsync(decimal? weight, decimal? carat)
        {
            try
            {
                // ❌ եթե weight or carat missing → clear value
                if (!weight.HasValue || !carat.HasValue || weight <= 0 || carat <= 0)
                    return null;

                // ✅ Get latest rates from DB
                var rates = await _context1.ratesDetails
                    .OrderByDescending(x => x.Entered_Date)
                    .Select(x => new RatesDetails
                    {
                        KT_24 = x.KT_24,
                        OG_22_KT = x.OG_22_KT,
                        OG_21_KT = x.OG_21_KT,
                        OG_20_KT = x.OG_20_KT,
                        OG_19_KT = x.OG_19_KT,
                        OG_18_KT = x.OG_18_KT
                    })
                    .FirstOrDefaultAsync();

                if (rates == null)
                    return null;

                decimal rate = GetInterpolatedRate(carat.Value, rates);

                return Math.Round(weight.Value * rate/8, 2);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Calculation error: {ex.Message}");
                return null;
            }
        }
        private decimal GetInterpolatedRate(decimal carat, RatesDetails rates)
        {
            var ktRates = new SortedDictionary<decimal, decimal>
                            {
                                {24m, rates.KT_24 ?? 0},
                                {22m, rates.OG_22_KT ?? 0},
                                {21m, rates.OG_21_KT ?? 0},
                                {20m, rates.OG_20_KT ?? 0},
                                {19m, rates.OG_19_KT ?? 0},
                                {18m, rates.OG_18_KT ?? 0},
                                {0m, 0},
                            };

            // ✅ Exact match ONLY (no floor)
            if (ktRates.ContainsKey(carat))
                return ktRates[carat];

            // ✅ Get bounds
            var lower = ktRates.LastOrDefault(x => x.Key < carat);
            var upper = ktRates.FirstOrDefault(x => x.Key > carat);

            // ✅ Interpolate
            if (upper.Key != 0)
            {
                return lower.Value + (carat - lower.Key) *
                       (upper.Value - lower.Value) /
                       (upper.Key - lower.Key);
            }

            // ✅ Edge cases (below 18 or above 24)
            if (lower.Key != 0) return lower.Value;
            if (upper.Key != 0) return upper.Value;

            return 0;
        }

        public async Task<List<string>> GetActiveStaffAsync()
        {
            return await _context2.Database
                .SqlQuery<string>($"SELECT Nickname FROM Staff WHERE Status = 1")
                .ToListAsync();
        }


    }
}
