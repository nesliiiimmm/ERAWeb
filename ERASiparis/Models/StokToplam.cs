using AYAK.Common.NetCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ERASiparis.Models
{
    [Table(TableName ="StokToplam", PrimaryKey = "TURUNID")]
    public class StokToplam
    {
        public int TURUNID { get; set; }
        public decimal GIRIS { get; set; }
        public decimal CIKIS { get; set; }
        public decimal MIKTAR { get; set; }
    }
    public class StokToplamORM:ORMBase<StokToplam,StokToplamORM>
    {

    }
}