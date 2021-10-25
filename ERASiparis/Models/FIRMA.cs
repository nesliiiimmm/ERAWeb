using AYAK.Common.NetCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ERASiparis.Models
{
    [Table(TableName ="FIRMA", PrimaryKey = "ID", IdentityColumn = "YOK", ConnectionName = "GENEL")]
    public class FIRMA
    {
        public int ID { get; set; }
        public string KODU { get; set; }
        public string FIRMAADI { get; set; }
        public string TAMUNVAN { get; set; }
        public string ADRES { get; set; }
        public string VD { get; set; }
        public string VN { get; set; }
        public string TELEFON { get; set; }
        public string TICARETSICILNO { get; set; }
        public string EMAIL { get; set; }
        public string MERSISNO { get; set; }
        public string FAX { get; set; }
        public string WEBADRES { get; set; }
    }
    public class FIRMAORM:ORMBase<FIRMA,FIRMAORM>
    {

    }
}
