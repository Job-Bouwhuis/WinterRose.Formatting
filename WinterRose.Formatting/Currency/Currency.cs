using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinterRose.Formatting.Currency;

using System.ComponentModel.DataAnnotations;

/// <summary>
/// Types of currency the currency formatting supports
/// </summary>
public enum Currency
{
    [Display(Name = "AUD", Description = "$")]
    AustralianDollar,

    [Display(Name = "BGN", Description = "лв")]
    BulgarianLev,

    [Display(Name = "BRL", Description = "R$")]
    BrazilianReal,

    [Display(Name = "CAD", Description = "$")]
    CanadianDollar,

    [Display(Name = "CHF", Description = "CHF")]
    SwissFranc,

    [Display(Name = "CNY", Description = "¥")]
    ChineseYuan,

    [Display(Name = "CZK", Description = "Kč")]
    CzechKoruna,

    [Display(Name = "DKK", Description = "kr")]
    DanishKrone,

    [Display(Name = "EUR", Description = "€")]
    Euro,

    [Display(Name = "GBP", Description = "£")]
    PoundSterling,

    [Display(Name = "HKD", Description = "$")]
    HongKongDollar,

    [Display(Name = "HUF", Description = "Ft")]
    HungarianForint,

    [Display(Name = "IDR", Description = "Rp")]
    IndonesianRupiah,

    [Display(Name = "ILS", Description = "₪")]
    IsraeliNewShekel,

    [Display(Name = "INR", Description = "₹")]
    IndianRupee,

    [Display(Name = "ISK", Description = "kr")]
    IcelandicKrona,

    [Display(Name = "JPY", Description = "¥")]
    JapaneseYen,

    [Display(Name = "KRW", Description = "₩")]
    SouthKoreanWon,

    [Display(Name = "MXN", Description = "$")]
    MexicanPeso,

    [Display(Name = "MYR", Description = "RM")]
    MalaysianRinggit,

    [Display(Name = "NOK", Description = "kr")]
    NorwegianKrone,

    [Display(Name = "NZD", Description = "$")]
    NewZealandDollar,

    [Display(Name = "PHP", Description = "₱")]
    PhilippinePeso,

    [Display(Name = "PLN", Description = "zł")]
    PolishZloty,

    [Display(Name = "RON", Description = "lei")]
    RomanianLeu,

    [Display(Name = "SEK", Description = "kr")]
    SwedishKrona,

    [Display(Name = "SGD", Description = "$")]
    SingaporeDollar,

    [Display(Name = "THB", Description = "฿")]
    ThaiBaht,

    [Display(Name = "TRY", Description = "₺")]
    TurkishLira,

    [Display(Name = "USD", Description = "$")]
    UnitedStatesDollar,

    [Display(Name = "ZAR", Description = "R")]
    SouthAfricanRand,
}