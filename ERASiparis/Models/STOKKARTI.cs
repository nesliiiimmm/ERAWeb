using AYAK.Common.NetCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ERASiparis.Models
{
    [Table(TableName = "STOKKARTI", PrimaryKey = "ID", IdentityColumn ="ID")]
    public class STOKKARTI
    {
        public int ID { get; set; }
        public string SUBEKODU { get; set; }
        public string KODU { get; set; }
        public string ADI { get; set; }
        public string ADI1 { get; set; }
        public string KARTTIPI { get; set; }
        public string BRM1 { get; set; }
        public string BRM1BARKOD { get; set; }
        public string BRMISLAM { get; set; }
        public string BRM2 { get; set; }
        public int BRM2ORAN { get; set; }
        public string BRM2BARKOD { get; set; }
        public decimal B1FIYAT_AL1 { get; set; }
        public decimal B2FIYAT_AL1 { get; set; }
        public decimal B1FIYAT_AL2 { get; set; }
        public decimal B2FIYAT_AL2 { get; set; }
        public decimal B1FIYAT_AL3 { get; set; }
        public decimal B2FIYAT_AL3 { get; set; }
        public decimal B1FIYAT_ST1 { get; set; }
        public decimal B2FIYAT_ST2 { get; set; }
        public decimal B1FIYAT_ST2 { get; set; }
        public decimal B2FIYAT_ST1 { get; set; }
        public decimal B1FIYAT_ST3 { get; set; }
        public decimal B2FIYAT_ST3 { get; set; }
        public decimal B1FIYAT_STWEB { get; set; }
        public decimal B2FIYAT_STWEB { get; set; }
        public decimal KDV_AL { get; set; }
        public decimal OTVORANAL { get; set; }
        public decimal OTVORANST { get; set; }
        public decimal KDV_ST { get; set; }
        public decimal MINIMUMMIKTAR { get; set; }
        public decimal MAKSIMUMMIKTAR { get; set; }
        public decimal PESINPRIM { get; set; }
        public decimal VADELIPRIM { get; set; }
        public string OZEL1 { get; set; }
        public string OZEL2 { get; set; }
        public string OZEL3 { get; set; }
        public string OZEL4 { get; set; }
        public string OZEL5 { get; set; }
        public string OZEL6 { get; set; }
        public string OZEL7 { get; set; }
        public string ACIKLAMA { get; set; }
        public decimal DEVIRMIKTAR { get; set; }
        public decimal DEVIRFIYAT { get; set; }
        public decimal DEVIRTUTAR { get; set; }
        public decimal AGIRLIK1 { get; set; }
        public decimal AGIRLIK2 { get; set; }
        public string RAF { get; set; }
        public string RAFGOZ { get; set; }
        public decimal OIV { get; set; }
        public bool AKTIF { get; set; }
        public string BRM3 { get; set; }
        public decimal B3FIYAT_AL1 { get; set; }
        public decimal B3FIYAT_AL2 { get; set; }
        public decimal B3FIYAT_AL3 { get; set; }
        public decimal B3FIYAT_ST1 { get; set; }
        public decimal B3FIYAT_ST2 { get; set; }
        public decimal B3FIYAT_ST3 { get; set; }
        public decimal AGIRLIK3 { get; set; }
        public int BRM3ORAN { get; set; }
        public string BRM3BARKOD { get; set; }
        public string DR { get; set; }
        public decimal ALTFIYATSINIR { get; set; }
        public decimal USTFIYATSINIR { get; set; }
        public int YETKI { get; set; }
        public string TRYOLKODU { get; set; }
        public string N11KODU { get; set; }
        public string EPTT { get; set; }
        public string HEPSIBURADAKODU { get; set; }
        public string GITGKODU { get; set; }
        public int DOVIZTIPI { get; set; }
        public decimal DOVIZFIYAT { get; set; }
        public decimal STOK { get; set; }
    }
    public class STOKKARTIORM:ORMBase<STOKKARTI,STOKKARTIORM>
    {
    }
}