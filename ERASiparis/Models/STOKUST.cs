using AYAK.Common.NetCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ERASiparis.Models
{
    [Table(TableName = "STOKUST",PrimaryKey ="ID", IdentityColumn ="yok")]
    public class STOKUST
    {
        public int ID { get; set; }
        public string SUBEKODU { get; set; }
        public string STOK_FISNO { get; set; }
        public string STOK_FISTIPI { get; set; }
        public DateTime? STOK_TARIH { get; set; }
        public string STOK_OZEL1 { get; set; }
        public string STOK_OZEL2 { get; set; }
        public string STOK_OZEL3 { get; set; }
        public string STOK_ACIKLAMA { get; set; }
        public int? IRSALIYE_ID { get; set; }
        public string IRSALIYE_FISNO { get; set; }
        public DateTime? IRSALIYE_TARIH { get; set; }
        public string IRSALIYE_OZEL1 { get; set; }
        public string IRSALIYE_OZEL2 { get; set; }
        public string IRSALIYE_OZEL3 { get; set; }
        public string IRSALIYE_ACIKLAMA { get; set; }
        public string IRSALIYE_FISTIPI { get; set; }
        public int? FATURA_ID { get; set; }
        public string FATURA_FISNO { get; set; }
        public DateTime? FATURA_TARIH { get; set; }
        public string FATURA_OZEL1 { get; set; }
        public string FATURA_OZEL2 { get; set; }
        public string FATURA_OZEL3 { get; set; }
        public string FATURA_ACIKLAMA { get; set; }
        public string FATURA_FISTIPI { get; set; }
        public int? SUBE { get; set; }
        public string FATURANO { get; set; }
        public int CARIID { get; set; }
        public int? FATBILGIID { get; set; }
        public string BARKOD { get; set; }
        public string KARGO { get; set; }
        public decimal? TOPLAM { get; set; }
        public decimal? KDV1Y { get; set; }
        public decimal? KDV2Y { get; set; }
        public decimal? KDV3Y { get; set; }
        public decimal? KDV4Y { get; set; }
        public decimal? KDV5Y { get; set; }
        public decimal? KDV1MATRAH { get; set; }
        public decimal? KDV2MATRAH { get; set; }
        public decimal? KDV3MATRAH { get; set; }
        public decimal? KDV4MATRAH { get; set; }
        public decimal? KDV5MATRAH { get; set; }
        public decimal? KDVTUTAR1 { get; set; }
        public decimal? KDVTUTAR2 { get; set; }
        public decimal? KDVTUTAR3 { get; set; }
        public decimal? KDVTUTAR4 { get; set; }
        public decimal? KDVTUTAR5 { get; set; }
        public decimal? DIPISKY1 { get; set; }
        public decimal? DIPISKY2 { get; set; }
        public decimal? DIPISKY3 { get; set; }
        public decimal? DIPISKY4 { get; set; }
        public decimal? DIPISKONTOTUTAR1 { get; set; }
        public decimal? DIPISKONTOTUTAR2 { get; set; }
        public decimal? DIPISKONTOTUTAR3 { get; set; }
        public decimal? DIPISKONTOTUTAR4 { get; set; }
        public decimal? DIPISKY { get; set; }
        public decimal? DIPISKONTOTUTAR { get; set; }
        public DateTime? TESLIMTARIHI { get; set; }
        public int? TEDARIKSURESI { get; set; }
        public string EFAT_DURUMU { get; set; }
        public int? ONAYVEREN { get; set; }
        public DateTime? ONAYTARIHI { get; set; }
        public decimal? GENELTOPLAM { get; set; }
        public string T_FIRMAADI { get; set; }
        public string T_ADI { get; set; }
        public string T_SOYADI { get; set; }
        public string T_ADRES { get; set; }
        public string T_TELNO { get; set; }
        public string T_YETKILI { get; set; }
        public string T_EMAIL { get; set; }
        public string T_VNO { get; set; }
        public string T_VDA { get; set; }
        public string T_IL { get; set; }
        public string T_ILCE { get; set; }
        public bool? KDVDAHIL { get; set; }
        public decimal? DIPISKONTOSABIT { get; set; }
        public string PROJEKODU { get; set; }
        public string DEPOGIRIS { get; set; }
        public string DEPOCIKIS { get; set; }
        public string IRSLIYENO { get; set; }
        public string ISLEMTIPI { get; set; }
        public int? BAGLANTI_ID { get; set; }
        public string FISTIPI { get; set; }
        public string TIP { get; set; }
        public string ACIKLAMA1 { get; set; }
        public string ACIKLAMA2 { get; set; }
        public DateTime? VADE { get; set; }
        public bool? FATURA_CARIISLE { get; set; }
        public decimal? TEKIFATORAN1 { get; set; }
        public decimal? TEKIFATORAN2 { get; set; }
        public decimal? TEKIFATTUTAR { get; set; }
        public string YAZIILE { get; set; }
        public string DR { get; set; }
        public decimal? OIVY { get; set; }
        public decimal? OIVYTUTAR { get; set; }
        public decimal? ISKONTOTOPLAM { get; set; }
        public string SATICIKODU { get; set; }
        public decimal? SATIRISKONTOTOPLAM { get; set; }
        public int? YETKI { get; set; }
        public int? MUSTERIID { get; set; }
        public int? TP { get; set; }
        public string PLAKA { get; set; }
        public string IRSNOLARI { get; set; }
        public string IRSFISNOLARI { get; set; }
        public string IRSTARIH { get; set; }
        public decimal? OTVTUTAR { get; set; }
        public string FATURASAAT { get; set; }
        public string IRSALIYESAAT { get; set; }
        public string KARGOTAKIPNO { get; set; }
        public string KARGOTESLIMFISNO { get; set; }
        public string ETNNO { get; set; }
        //public string ETNNOIRSALIYE { get; set; }
        public string ESIPNO { get; set; }
        public string EFATURATIPI { get; set; }
        public string EFATURSENERYO { get; set; }
        public int? FATURAYAZSAY { get; set; }
        //public string TASIYICIVKN { get; set; }
        //public string TASIYICIUNVAN { get; set; }
       
    }
    public class STOKUSTORM:ORMBase<STOKUST,STOKUSTORM>
    {

    }
}