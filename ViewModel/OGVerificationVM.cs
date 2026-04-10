namespace TestSPA.ViewModel
{
    public class OGVerificationVM
    {
        public int Id { get; set; }
        public int? OGVerifyNo { get; set; }
        public List<OGVerification> Details { get; set; } = new();
    }
}
