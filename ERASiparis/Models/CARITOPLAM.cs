using AYAK.Common.NetCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ERASiparis.Models
{
    [Table(TableName ="V_CARITOPLAM",PrimaryKey = "CARIKARTID")]
    public class CARITOPLAM
    {
        public int CARIKARTID { get; set; }
        public int? TP { get; set; }
        public decimal Borc { get; set; }
        public decimal Alacak { get; set; }
        public decimal Bakiye { get; set; }
    }
    public class CARITOPLAMORM:ORMBase<CARITOPLAM,CARITOPLAMORM>
    {

    }
}