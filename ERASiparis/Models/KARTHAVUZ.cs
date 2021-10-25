using AYAK.Common.NetCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ERASiparis.Models
{
    [Table(TableName ="KARTHAVUZ",PrimaryKey ="ID",IdentityColumn ="YOK")]
    public class KARTHAVUZ
    {
        public int ID { get; set; }
        public int? KARTISLEMID { get; set; }
        public int? TAKSITSIRA { get; set; }
        public DateTime? TARIH { get; set; }
        public DateTime? VADE { get; set; }
        public int? BANKAID { get; set; }
        public int? BANKAHARID { get; set; }
        public int? CARIKARTID { get; set; }
        public int? CARIHARID { get; set; }
        public decimal? TUTAR { get; set; }
        public decimal? KESINTI { get; set; }
        public decimal? NETTUTAR { get; set; }
        public bool? ISLENDI { get; set; }
        public int? KARTID { get; set; }
        public int? YETKI { get; set; }
        public string DR { get; set; }
        public string SUBE { get; set; }
    }
    public class KARTHAVUZORM:ORMBase<KARTHAVUZ,KARTHAVUZORM>
    {

    }
}