using AYAK.Common.NetCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ERASiparis.Models
{
    [Table(TableName ="KULLAN",PrimaryKey ="ID",IdentityColumn ="YOK",ConnectionName ="GENEL")]
    public class KULLAN
    {
        public int ID { get; set; }
        public string ADI { get; set; }
        public string SIFRE { get; set; }
        public string SATICIKODU { get; set; }
        public bool AKTIF { get; set; }
        public bool? WEBKULLANICI { get; set; }
        public bool? NAKITTAHSILAT { get; set; }
        public bool? KREDITAHSILAT { get; set; }
        public bool? CEKSENETTAHSILAT { get; set; }
        public bool? SIPARIS { get; set; }
        public bool? FATURA { get; set; }
        public bool? CARIACMA { get; set; }
        public bool? CARIARAMA { get; set; }
    }
    public class KULLANORM:ORMBase<KULLAN,KULLANORM>
    {

    }
}