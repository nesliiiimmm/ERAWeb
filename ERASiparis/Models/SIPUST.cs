using AYAK.Common.NetCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ERASiparis.Models
{
    [Table(TableName = "SIPUST",PrimaryKey ="ID", IdentityColumn ="yok")]
    public class SIPUST
    {
        public int ID { get; set; }
        public string SUBEKODU { get; set; }
        public string FISNO { get; set; }
        public string FISTIPI { get; set; }
        public DateTime? TARIH { get; set; }
        public int? CARIID { get; set; }
        public string OZEL1 { get; set; }
        public string OZEL2 { get; set; }
        public string OZEL3 { get; set; }
        public string ACIKLAMA { get; set; }
        public decimal? TOPLAM { get; set; }
        public decimal? KDV1Y { get; set; }
        public decimal? KDV2Y { get; set; }
        public decimal? KDV3Y { get; set; }
        public decimal? KDVTUTAR1 { get; set; }
        public decimal? KDVTUTAR2 { get; set; }
        public decimal? DIPISKY { get; set; }
        public decimal? DIPISKONTOTUTAR { get; set; }
        public DateTime? TESLIMTARIHI { get; set; }
        public int? TEDARIKSURESI { get; set; }
        public string DURUMU { get; set; }
        public int? ONAYVEREN { get; set; }
        public DateTime? ONAYTARIHI { get; set; }
        public decimal? GENELTOPLAM { get; set; }
        //public string FIRMAADI { get; set; }
        public string ADRES { get; set; }
        public string TELNO { get; set; }
        public string YETKILI { get; set; }
        public string EMAIL { get; set; }
        public bool? KDVDAHIL { get; set; }
        public decimal? DIPISKONTOSABIT { get; set; }
        public DateTime? TESLIMATTARIHI { get; set; }
        public decimal? KDV4Y { get; set; }
        public decimal? KDV5Y { get; set; }
        public decimal? KDVTUTAR3 { get; set; }
        public decimal? KDVTUTAR4 { get; set; }
        public decimal? KDVTUTAR5 { get; set; }
        public decimal? KDV1MATRAH { get; set; }
        public decimal? KDV2MATRAH { get; set; }
        public decimal? KDV3MATRAH { get; set; }
        public decimal? KDV4MATRAH { get; set; }
        public decimal? KDV5MATRAH { get; set; }
        public string DR { get; set; }
        public string DEPOGIRIS { get; set; }
        public string DEPOCIKIS { get; set; }
        public string PROJEKODU { get; set; }
        public string SATICIKODU { get; set; }
        public string PLAKA { get; set; }
        public decimal? SATIRISKONTOTOPLAM { get; set; }
        public int? YETKI { get; set; }
        public int? TP { get; set; }
        public bool? CARIYEISLE { get; set; }
        public decimal? ISKONTOTOPLAM { get; set; }
        public bool? WEBSIPARIS { get; set; }
        public bool? GORULDU { get; set; }
    }
    public class SIPUSTORM:ORMBase<SIPUST,SIPUSTORM>
    {

    }
}