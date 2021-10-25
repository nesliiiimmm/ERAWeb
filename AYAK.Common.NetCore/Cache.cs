using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace AYAK.Common.NetCore
{
    public class CacheCollection<T>
    {
        Dictionary<string, CacheCollection<object>> _caches = new Dictionary<string, CacheCollection<object>>();
        public static CacheCollection<T> GetCollection(string appName)
        {
            //HttpContext ctx = HttpContext.Current;
            //if (ctx != null)
            //{
            //    ctx.Application[appName] = ctx.Application[appName] ?? new CacheCollection<T>();
            //    return (CacheCollection<T>)ctx.Application[appName];
            //}
            //else
            {
                return null;
            }
        }
        Table GetTable<K>()
        {
            Table t = (Table)Table.GetCustomAttribute(typeof(K), typeof(Table));
            t = t ?? Table;
            return t;
        }

        public Table Table
        {
            get
            {
                return GetTable<T>();
            }
        }

        List<Cache<T>> cache = new List<Cache<T>>();
        public List<T> GetItems(string query, Dictionary<string, object> prms, SelectType selectType)
        {
            if (Table.CacheState==CacheType.NoCache)
            {
                return null;
            }
            string pString = Cache<T>.GetParamString(prms);
            string Key = Cache<T>.GetKey(query, pString);
            var result = cache.FirstOrDefault(x => x.Key == Key);
            return result != null ? result.Data : null;
        }
        public void InsertItems(List<T> Data, string query, Dictionary<string, object> prms, SelectType selectType)
        {
            if (Table.CacheState == CacheType.NoCache)
            {
                return; 
            }
            if (Data == null) return;
            string pString = Cache<T>.GetParamString(prms);
            string Key = Cache<T>.GetKey(query, pString);
            var value = cache.FirstOrDefault(x => x.Key == Key);
            if (value != null)
            {
                cache.Remove(value);
            }
            Cache<T> data = new Cache<T>();
            data.Query = query;
            data.Parameters = prms;
            data.Data = Data;
            cache.Add(data);
        }

        public void Clear()
        {
            cache.Clear();
        }
        public void Refresh()
        {
            cache.Clear();
        }
    }
    public class Cache<T>
    {
        public List<T> Data { get; set; }
        private Dictionary<string, object> parameters;
        public Dictionary<string, object> Parameters
        {
            get { return parameters; }
            set
            {
                parameters = value;

                ParamString = GetParamString(value);
            }
        }
        public string Query { get; set; }
        public string ParamString { get; set; }
        public string Key
        {
            get
            {
                return GetKey(Query, ParamString);
            }
        }
        public static string GetParamString(Dictionary<string, object> prms)
        {
            string result = "";
            if (prms != null)
                foreach (var item in prms)
                {
                    if (item.Value is DataTable)
                    {
                        DataTable dt = (DataTable)item.Value;

                        foreach (DataRow row in dt.Rows)
                        {
                            foreach (DataColumn col in dt.Columns)
                            {
                                result += string.Format("{2}.{0}={1}", col.ColumnName, row[col.ColumnName], item.Key);
                            }

                        }
                    }
                    else
                    {
                        result += string.Format("{0}={1},", item.Key, item.Value);
                    }

                }
            return result;
        }
        public static string GetKey(string Query, string ParamString)
        {
            return string.Format("{0};{1}", Query, ParamString);
        }
    }

}
