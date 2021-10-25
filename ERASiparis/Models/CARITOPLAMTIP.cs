using AYAK.Common.NetCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ERASiparis.Models
{
    [Table(TableName = "V_CARITOPLAMTIP", PrimaryKey = "CARIKARTID")]
    public class CARITOPLAMTIP
    {
        public int CARIKARTID { get; set; }
        public int TP { get; set; }
        public decimal Borc { get; set; }
        public decimal Alacak { get; set; }
        public decimal Bakiye { get; set; }
    }
    public class CARITOPLAMTIPORM:ORMBase<CARITOPLAMTIP,CARITOPLAMTIPORM>
    {

    }
}
