using AYAK.Common.NetCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ERASiparis.Models
{
    [Table(TableName ="SIPALT",PrimaryKey ="ID", IdentityColumn ="yok")]
    public class SIPALT
    {
        public int ID { get; set; }
        public string SUBEKODU { get; set; }
        public int? SIPID { get; set; }
        public int? URUNID { get; set; }
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
        public DateTime? TESLIMTARIH { get; set; }
        public decimal? TESLIMEDILENMIKTAR { get; set; }
        public decimal? KALANMIKTAR { get; set; }
        public decimal? KDVMATRAH { get; set; }
        public string ACIKLAMA { get; set; }
        public string DR { get; set; }
        public string GC { get; set; }
        public string DEPOGIRIS { get; set; }
        public string DEPOCIKIS { get; set; }
        public string BARKOD { get; set; }
        public string PLAKA { get; set; }
        public int? YETKI { get; set; }
        public DateTime? TESLIMATTARIHI { get; set; }
        public int? TP { get; set; }
        public decimal? IADE { get; set; }
        public decimal? ISKONTOTOPLAM { get; set; }
        public decimal? SATIRISKONTOTOPLAM { get; set; }
        public decimal? TEKLIFUST_ID { get; set; }
        public DateTime? SEVKTARIHI { get; set; }
        public string SEVKIPTAL { get; set; }
    }
    public class SIPALTORM:ORMBase<SIPALT,SIPALTORM>
    {

    }

    

}