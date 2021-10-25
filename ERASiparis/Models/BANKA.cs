using AYAK.Common.NetCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ERASiparis.Models
{
    [Table(TableName ="BANKA", PrimaryKey = "ID", IdentityColumn = "YOK")]
    public class BANKA
    {
        public int ID { get; set; }
        public string SUBEKODU { get; set; }
        public string KODU { get; set; }
        public string ADI { get; set; }
        public string YETKILI { get; set; }
        public int? CARIKARTID { get; set; }
        public int? GIDERID { get; set; }
        public bool? POS { get; set; }
        public string TIP { get; set; }
        public string TELEFON { get; set; }
        public string OZEL1 { get; set; }
        public string OZEL2 { get; set; }
        public string HESAPSUBESI { get; set; }
        public string HESAPKODU { get; set; }
        public string IBAN { get; set; }
        public DateTime? KAYITTARIHI { get; set; }
        public DateTime? DUZELTMETARIHI { get; set; }
        public string DR { get; set; }
        public int? YETKI { get; set; }

    }
    public class BANKAORM:ORMBase<BANKA,BANKAORM>
    {

    }
}