using AYAK.Common.NetCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ERASiparis.Models
{
    [Table(TableName ="CARIKART", PrimaryKey = "ID", IdentityColumn ="YOK")]
    public class CARIKART
    {
        public int ID { get; set; }
        public string SUBEKODU { get; set; }
        public string KODU { get; set; }
        public string FIRMAADI { get; set; }
        public string ADRES { get; set; }
        public string VD { get; set; }
        public string VN { get; set; }
        public string REFERANS { get; set; }
        public string TEL1 { get; set; }
        public string TEL1NOT { get; set; }
        public string TEL2 { get; set; }
        public string TEL2NOT { get; set; }
        public string TEL3 { get; set; }
        public string TEL3NOT { get; set; }
        public string TEL4 { get; set; }
        public string TEL4NOT { get; set; }
        public string FAX { get; set; }
        public string OZEL1 { get; set; }
        public string OZEL2 { get; set; }
        public string OZEL3 { get; set; }
        public string OZEL4 { get; set; }
        public string WEBKULKODU { get; set; }
        public string WEBSIFRE { get; set; }
        public string EMAIL { get; set; }
        public string WEB { get; set; }
        public decimal? RISKCEK { get; set; }
        public decimal? RSIKACIK { get; set; }
        public decimal? RISKKREDI { get; set; }
        public decimal? ISKONTOALIS { get; set; }
        public decimal? ISKONTOSATIS { get; set; }
        public string BANKAADI1 { get; set; }
        public string ULKE { get; set; }
        public string IL { get; set; }
        public string ILCE { get; set; }
        public string MAHALLE { get; set; }
        public string BINA { get; set; }
        public string POSTAKODU { get; set; }
        public DateTime? KAYITTARIHI { get; set; }
        public string IBAN1 { get; set; }
        public string BANKAADI2 { get; set; }
        public string IBAN2 { get; set; }
        public string BANKAADI3 { get; set; }
        public string IBAN3 { get; set; }
        public int? VADEALIS { get; set; }
        public int? VADESATIS { get; set; }
        public decimal? CARIRISK { get; set; }
        public int? KEFIL1ID { get; set; }
        public int? KEFIL2ID { get; set; }
        public bool? AKTIF { get; set; }
        public string YETKILI { get; set; }
        public bool? SMS1 { get; set; }
        public bool? SMS2 { get; set; }
        public bool? SMS3 { get; set; }
        public bool? SMS4 { get; set; }
        public string DR { get; set; }
        public string KARTTIPI { get; set; }
        public string ADI { get; set; }
        public string SOYADI { get; set; }
        public bool? EFATURAMI { get; set; }
        public string PK { get; set; }
        public int? YETKI { get; set; }
        public string KANAADI { get; set; }
        public string KBABAADI { get; set; }
        public string KDORUNYERI { get; set; }
        public DateTime? KDORUNTARIHI { get; set; }
        public string FATURATIPI { get; set; }
        public string ISADI { get; set; }
        public string ISADRES { get; set; }
        public string ISTEL { get; set; }
        public string AVUKATKODU { get; set; }
        public DateTime? AVUKATTARIHI { get; set; }
        public string ACILISNOTU { get; set; }
        public bool? SATISKAPALI { get; set; }

    }
    public class CARIKARTORM:ORMBase<CARIKART,CARIKARTORM>
    {

    }
}