using AYAK.Common.NetCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ERASiparis.Models
{
    [Table(TableName = "CEKBOR", PrimaryKey ="ID",IdentityColumn ="YOK")]
    public class CEKBOR
    {
        public int ID { get; set; }
        public string SUBEKODU { get; set; }
        public string FISNO { get; set; }
        public DateTime TARIH { get; set; }
        public string FISTIPI { get; set; }
        public string ACIKLAMA { get; set; }
        public int CARIKARTID { get; set; }
        public int KASAHARID { get; set; }
        public int BANKAKARTID { get; set; }
        public string OZEL1 { get; set; }
        public string OZEL2 { get; set; }
        public string PROJEKODU { get; set; }
        public string ORTAK2 { get; set; }
        public string SATICI { get; set; }
        public int BAGLANTIID { get; set; }
        public int SATICIKODU { get; set; }
        public int EVRAKSAYISI { get; set; }
        public decimal TOPLAMTUTAR { get; set; }
        public DateTime ORTVADE { get; set; }
        public int URUNID { get; set; }
        public string ACIKLAMA1 { get; set; }
        public string DURUMU { get; set; }
        public int CEKID { get; set; }
        public string DR { get; set; }
        public int YETKI { get; set; }
        public int GUN { get; set; }
        public int TP { get; set; }
    }
    public class CEKBORORM:ORMBase<CEKBOR,CEKBORORM>
    {

    }
}
