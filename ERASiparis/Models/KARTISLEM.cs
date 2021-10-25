using AYAK.Common.NetCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ERASiparis.Models
{
    [Table(TableName ="KARTISLEM",PrimaryKey ="ID",IdentityColumn ="YOK")]
    public class KARTISLEM
    {
        public int ID { get; set; }
        public string FISNO { get; set; }
        public DateTime? TARIH { get; set; }
        public string FISTIPI { get; set; }
        public int? CARIKARTID { get; set; }
        public int? BANKAID { get; set; }
        public int? STOKKARTIID { get; set; }
        public int? BANKAKAYITID { get; set; }
        public int? MAILORDERCARIID { get; set; }
        public int? KARTID { get; set; }
        public int? KASAID { get; set; }
        public int? SIPARISID { get; set; }
        public string SUBEKODU { get; set; }
        public string SATICIKODU { get; set; }
        public string OZEL1 { get; set; }
        public string OZEL2 { get; set; }
        public string OZEL3 { get; set; }
        public string PROJE { get; set; }
        public int? BAGLANTIID { get; set; }
        public string ACIKLAMA { get; set; }
        public decimal? BORC { get; set; }
        public decimal? ALACAK { get; set; }
        public int? TAKSITSAYISI { get; set; }
        public decimal? KKDFORANI { get; set; }
        public decimal? BSMVORAN { get; set; }
        public decimal? FAISORANI { get; set; }
        public decimal? KKFDFTUTAR { get; set; }
        public decimal? BSMVTUTAR { get; set; }
        public decimal? FAIZTUTAR { get; set; }
        public string SAHIBIADI { get; set; }
        public string KARTNO { get; set; }
        public string SONKULLANIMTARIHI { get; set; }
        public string GUVENLIKKODU { get; set; }
        public string KAYITYERI { get; set; }
        public int? YETKI { get; set; }
        public string DR { get; set; }
        public int? TP { get; set; }
        public int? SUBE { get; set; }
    }             
    public class KARTISLEMORM:ORMBase<KARTISLEM,KARTISLEMORM>
    {

    }
}