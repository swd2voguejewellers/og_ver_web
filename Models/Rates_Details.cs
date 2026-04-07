using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

[Keyless]
[Table("Rates_Details")]
public class RatesDetails
{
    public int Id { get; set; }   // add this in model ONLY (not DB)

    public DateTime? Entered_Date { get; set; }

    [MaxLength(50)]
    public string? Entered_User { get; set; }

    public decimal? OG_22_KT { get; set; }
    public decimal? OG_21_KT { get; set; }
    public decimal? OG_20_KT { get; set; }
    public decimal? OG_19_KT { get; set; }
    public decimal? OG_18_KT { get; set; }

    public decimal? KT_24 { get; set; }
    public decimal? KT_22 { get; set; }

    public decimal? WG { get; set; }
    public decimal? Platinum { get; set; }
    public decimal? Silver { get; set; }

    public decimal? Coin_CASH { get; set; }
    public decimal? Coin_CARD { get; set; }
}