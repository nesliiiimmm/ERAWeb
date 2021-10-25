using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AYAK.Common.NetCore
{
    public class Table : Attribute
    {
        public Table()
        {
            SchemaName = "dbo";
            TableName = "sys.Tables";
            PrimaryKey = "Id";
            IdentityColumn = "Id";
            TableType = TableType.PrimaryTable;
            ConnectionName = "DataBase";
            SearchFields=new string[]{"Name"};
            CacheState = CacheType.NoCache;
            DisplayMember = "Name";
            IsActiveField = "IsActive";
        }
       
        public string SchemaName { get; set; }
        public string TableName { get; set; }
        public string PrimaryKey { get; set; }
        public string IdentityColumn { get; set; }
        public TableType TableType { get; set; }
        public string[] CompositeKeys { get; set; }
        public string[] SearchFields { get; set; }
        public string ConnectionName { get; set; }
        public CacheType CacheState { get; set; }
        public bool IsActiveValid { get; set; }
        public int BETypeID{ get; set; }
        public string DisplayMember { get; set; }
        public string IsActiveField { get; set; }
    }

    public class Column : Attribute
    {
        public Column()
        {
            Visible = false;
        }
        public string DisplayName { get; set; }
        public bool Visible { get; set; }
        public bool Required { get; set; }
        public string Description { get; set; }
        public bool ShowList { get; set; }
        public bool ShowInsert { get; set; }
        public bool ShowEdit { get; set; }
        public bool ShowDetail { get; set; }
        public int Queue { get; set; }

    }
    
    public class IgnoreCRUD : Attribute
    {

    }
    
    public enum TableType
    {
        PrimaryTable,
        CompositTable,
        BusinessEntityTable
    }

    public enum CacheType
    {
        Cache,
        NoCache
    }
}
