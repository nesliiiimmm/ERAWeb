using AYAK.Common.NetCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ERASiparis.Models
{
    [Table(TableName ="SIRA", PrimaryKey = "ID", IdentityColumn ="ID")]
    public class SIRAT
    {
        public int ID { get; set; }
        public int SIRA { get; set; }
    }
    public class SIRAORM:ORMBase<SIRAT,SIRAORM>
    {
        public int SiraVer()
        {
            Update("SIRA = SIRA+1", "WHERE ID=1", null);
            var rest = Select();
            return rest.Data.FirstOrDefault().SIRA;
        }
    }
}