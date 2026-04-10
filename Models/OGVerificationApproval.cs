using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

[Table("OG_Verification_Approval")]
public class OGVerificationApproval
{
    [Key]
    public int Id { get; set; }
    public int OGVerifyNo { get; set; }
    public decimal? TotalWeight { get; set; }
    public decimal? TotalValue { get; set; }
    public decimal? CurrentValueToBeReduced { get; set; }
    public string? ApprovedUser { get; set; }
    public DateTime ApprovedDate { get; set; }
}
