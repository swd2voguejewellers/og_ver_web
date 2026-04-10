using TestSPA.Models;

namespace TestSPA.ViewModel
{
    public class OGVerificationPrintVM
    {
        public int OGVerifyNo { get; set; }
        public DateTime? OGVerifyDate { get; set; }
        public long? ReceiptNo { get; set; }

        public string? SalesPerson { get; set; }
        public string? AssSalesPerson { get; set; }
        public string? PriceGivBy { get; set; }

        public decimal? KT_24 { get; set; }
        public decimal? OG_22_KT { get; set; }
        public decimal? OG_21_KT { get; set; }
        public decimal? OG_20_KT { get; set; }
        public decimal? OG_19_KT { get; set; }
        public decimal? OG_18_KT { get; set; }

        public decimal? AvgGoldRate { get; set; }
        public string? AvgGRReason { get; set; }

        public decimal TotalWeight { get; set; }
        public decimal TotalActValue { get; set; }
        public decimal TotalGivValue { get; set; }

        public List<OGVerification> Details { get; set; } = new();
    }
}

