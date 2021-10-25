using AYAK.Common.NetCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ERASiparis.Models
{
    [Table(TableName = "", IdentityColumn = "Yok", PrimaryKey = "ID")]
    public class CEKDURUM
    {
        public int ID { get; set; }
        public int BORDROID { get; set; }
        public int CEKID { get; set; }
        public string DURUMU { get; set; }
        public string ACIKLAMA { get; set; }
        public string DR { get; set; }
        public int CARIID { get; set; }
        public int BANKAID { get; set; }
        public int YETKI { get; set; }
        public decimal TUTAR { get; set; }
    }
    public class CEKDURUMORM : ORMBase<CEKDURUM, CEKDURUMORM>
    {

    }
}
