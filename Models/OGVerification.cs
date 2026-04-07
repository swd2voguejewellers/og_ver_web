using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;


[Table("OG_Verification")]

public class OGVerification
{
    [Key]
    public int Id { get; set; }
    public int OGVerifyNo { get; set; }
    public DateTime? OGVerifyDate { get; set; }
    public int? Num { get; set; }
    public string? ItemDesc { get; set; }
    public string? Caratage { get; set; }
    public decimal? OGWgt { get; set; }
    public decimal? OGActVal { get; set; }
    public decimal? OGGivVal { get; set; }
    public string? ReleaseReason { get; set; }
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
    public int? Branch { get; set; }
    public decimal? AvgGoldRate { get; set; }
    public string? AvgGRReason { get; set; }
}