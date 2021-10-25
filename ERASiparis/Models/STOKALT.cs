using AYAK.Common.NetCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ERASiparis.Models
{
    [Table(TableName = "STOKALT", PrimaryKey = "ID", IdentityColumn = "yok")]
    public class STOKALT
    {
        public int ID { get; set; }
        public string SUBEKODU { get; set; }
        public int? STOKUST_ID { get; set; }
        public int? IRSALIYEUST_ID { get; set; }
        public int? FATURAUST_ID { get; set; }
        public int? TEKLIFUST_ID { get; set; }
        public int? URUNID { get; set; }
        public string URUNADI { get; set; }
        public int? CARIID { get; set; }
        public decimal? MIKTAR { get; set; }
        public decimal? FIYAT { get; set; }
        public decimal? TUTAR { get; set; }
        public decimal? ISK1Y { get; set; }
        public decimal? ISK2Y { get; set; }
        public decimal? ISK1TUTAR { get; set; }
        public decimal? ISK2TUTAR { get; set; }
        public decimal? KDVY { get; set; }
        public decimal? KDVTUTAR { get; set; }
        public decimal? GENELTOPLAM { get; set; }
        public string ACIKLAMA1 { get; set; }
        public string ACIKLAMA2 { get; set; }
        public int? TERAIRIKCICARIID { get; set; }
        public int? SIPARISALTID { get; set; }
        public int? IRSALIYEALTID { get; set; }
        public int? FATURAALTID { get; set; }
        public int? TEKLIFALT_ID { get; set; }
        public decimal? ARATOPLAM1 { get; set; }
        public decimal? ARATOPLAM2 { get; set; }
        public decimal? ARATOPLAM3 { get; set; }
        public decimal? DIPISKONTO1Y { get; set; }
        public decimal? DIPISKONTOTUTAR { get; set; }
        public string BIRIM { get; set; }
        public decimal? FIYATKDAHIL { get; set; }
        public decimal? TUTARKDAHIL { get; set; }
        public string DEPOGIRIS { get; set; }
        public string DEPOCIKIS { get; set; }
        public decimal? KDVMATRAH { get; set; }
        public string ISLEMTIPI { get; set; }
        public string GC { get; set; }
        public string ACIKLAMA { get; set; }
        public string DR { get; set; }
        public decimal? OTVY { get; set; }
        public decimal? OTVYTUTAR { get; set; }
        public decimal? SATIRISKONTOTOPLAM { get; set; }
        public decimal? SATIRISKOTOTOPLAMYUZDE { get; set; }
        public int? SIPID { get; set; }
        public string BARKOD { get; set; }
        public int? YETKI { get; set; }
        public int? TP { get; set; }
        public string GECICIFATNO { get; set; }
        public string PLAKA { get; set; }
        public DateTime? GECICITARIHI { get; set; }

    }
    public class STOKALTORM : ORMBase<STOKALT, STOKALTORM>
    {

    }



}