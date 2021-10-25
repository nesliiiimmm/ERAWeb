using AYAK.Common.NetCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ERASiparis.Models
{
    [Table(TableName ="KASAHAR",PrimaryKey ="ID",IdentityColumn ="YOK")]
    public class KASAHAR
    {
        public int ID { get; set; }
        public string SUBEKODU { get; set; }
        public int? KASAID { get; set; }
        public int? CARIKARTID { get; set; }
        public int? PERSONELID { get; set; }
        public int? FATURAID { get; set; }
        public int? CEKSENETID { get; set; }
        public int? STOKKARTIID { get; set; }
        public int? BAGLANTIID { get; set; }
        public int? BANKAKARTID { get; set; }
        public int? VIRMANKASAKARTID { get; set; }
        public int? VIRMANID { get; set; }
        public string FISNO { get; set; }
        public string FISTIPI { get; set; }
        public DateTime? TARIH { get; set; }
        public string ACIKLAMA { get; set; }
        public string ACIKLAMA1 { get; set; }
        public decimal? BORC { get; set; }
        public decimal? ALACAK { get; set; }
        public string OZEL1 { get; set; }
        public string OZEL2 { get; set; }
        public string SATICIKODU { get; set; }
        public string PROJEKODU { get; set; }
        public string ORTAK2 { get; set; }
        public string KAYITYERI { get; set; }
        public string DR { get; set; }
        public int? YETKI { get; set; }
        public int? TP { get; set; }
    }
    public class KASAHARORM:ORMBase<KASAHAR,KASAHARORM>
    {

    }
}