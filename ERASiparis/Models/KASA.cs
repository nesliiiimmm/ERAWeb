using AYAK.Common.NetCore;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ERASiparis.Models
{
    [Table(TableName ="KASA",IdentityColumn ="ID",PrimaryKey ="YOK",ConnectionName ="GENEL")]
    public class KASA
    {
        public int ID { get; set; }
        public string FIRMAKODU { get; set; }
        public string KODU { get; set; }
        public string ADI { get; set; }
        public string DR { get; set; }
        public int YETKI { get; set; }
    }
    public class KASAORM:ORMBase<KASA,KASAORM>
    {
        public override Result<List<KASA>> Select()
        {
            string database = Tools.Connections["database"].ConnectionString;
            SqlConnectionStringBuilder csb = new SqlConnectionStringBuilder(database);
            var catalog = csb.InitialCatalog.ToUpper();
            string fc = catalog.Replace("DB_", "").Replace("DB", "").ToString();
            var res = Tools.Select<KASA>(@$"Select * from KASA where FIRMAKODU = @f",fc.CreateParameters("@f"),connectionName:"GENEL");
            return res;
        }
    }
}