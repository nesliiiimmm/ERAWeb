using AYAK.Common.NetCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ERASiparis.Models
{
    [Table(TableName ="OZEL",PrimaryKey ="ID",IdentityColumn ="yok")]
    public class OZEL
    {
        public int ID { get; set; }
        public string SUBEKODU { get; set; }
        public string KODU { get; set; }
        public string ADI { get; set; }
        public string YER { get; set; }
        public string RENK { get; set; }
        public int YETKI { get; set; }
    }
    public class OZELORM:ORMBase<OZEL,OZELORM>
    {
        public List<OZEL> OzelGetir(string yer)
        {
            var res = Where(x => x.YER == yer);
            return res.Data;
        }
    }
}