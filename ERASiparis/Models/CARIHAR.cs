using AYAK.Common.NetCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ERASiparis.Models
{
    [Table(TableName ="CARIHAR",PrimaryKey ="ID", IdentityColumn = "yok")]
    public class CARIHAR
    {
        public int ID { get; set; }
        public string SUBEKODU { get; set; }
        public int? CARIKARTID { get; set; }
        public string FISNO { get; set; }
        public string FISTIPI { get; set; }
        public DateTime? TARIH { get; set; }
        public DateTime? VADE { get; set; }
        public string ACIKLAMA { get; set; }
        public string ACIKLAMA1 { get; set; }
        public decimal? BORC { get; set; }
        public decimal? ALACAK { get; set; }
        public decimal? DOVKURU { get; set; }
        public decimal? DOVTUTAR { get; set; }
        public string OZEL1 { get; set; }
        public string OZEL2 { get; set; }
        public string PROJEKODU { get; set; }
        public string ORTAK2 { get; set; }
        public string SATICIKODU { get; set; }
        public int? VIRMANID { get; set; }
        public int? VIRMANCARIKARTID { get; set; }
        public int? KAPALIFIS { get; set; }
        public int? BAGLANTIID { get; set; }
        public int? KASAID { get; set; }
        public int? BANKAID { get; set; }
        public string DR { get; set; }
        public string KAYITYERI { get; set; }
        public string PLAKA { get; set; }
        public int? YETKI { get; set; }
        public int? TP { get; set; }
        public decimal? KOMISYONTUTARI { get; set; }
        public int? FATURAID { get; set; }
    }
    public class CARIHARORM:ORMBase<CARIHAR,CARIHARORM>
    {

    }
}