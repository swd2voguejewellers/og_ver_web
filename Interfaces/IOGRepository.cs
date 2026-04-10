using TestSPA.Models;
using TestSPA.ViewModel;

namespace TestSPA.Interfaces
{
    public interface IOGRepository
    {
        Task<int> GetNextOGVerifyNoAsync();
        Task<RatesDetails?> GetLatestRatesAsync();
        Task<List<OGVerification>> GetOGVerificationsByNumberAsync(int ogVerifyNo);
        Task<bool> IsApprovedAsync(int ogVerifyNo);
        Task<bool> CanApproveBranchAsync(int ogVerifyNo);
        Task<decimal?> CalculateActValueAsync(decimal? weight, decimal? carat);
        Task<List<string>> GetActiveStaffAsync();
        Task<(bool success, int ogVerifyNo)> SaveOrUpdateAsync(OGVerificationVM model);
        Task<(bool success, int ogVerifyNo)> ApproveAsync(OGVerificationVM model);

    }
}

