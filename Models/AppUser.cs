using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

[Keyless]
[Table("Users")]
public class AppUser
{
    public string? UserName { get; set; }
    public string? Password { get; set; }
    public string? Level { get; set; }
    public DateTime? CommentsDate { get; set; }
    public string? EnterPerson { get; set; }
    public string? Comment { get; set; }
    public bool Off { get; set; }
    public string? print_auth { get; set; }
    public int? Branch { get; set; }
    public string? Key_word { get; set; }
    public decimal? EMPNO { get; set; }
    public int? Authority { get; set; }
    public string? PreviousName { get; set; }
    public int? Tab { get; set; }
    public int? OrderRelease { get; set; }
    public string? UserType { get; set; }
    public int? Perf_View_Br { get; set; }
    public int? Sell_MC_Edit { get; set; }
    public int? Bulk_FGR_Edit { get; set; }
    public int? Order_Release { get; set; }
    public int? Early_Leaving { get; set; }
    public int? Perf_View_Des { get; set; }
}
