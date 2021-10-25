
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web;


namespace AYAK.Common.NetCore
{


    public class ORMBase<T, OT> : ORMBase<T>
        where T : class
        where OT : ORMBase<T>
    {
        public ORMBase()
        {
            //CacheContext = new CacheCollection<T>();
        }

        private static OT _current;
        public static OT Current
        {
            get
            {
                //HttpContext context = HttpContext.Current;
                //if (context != null && context.Session != null)
                //{
                //    var instance = Activator.CreateInstance<OT>();
                //    string sessionName = string.Format("_{0}.{1}", instance.Table.SchemaName, instance.Table.TableName);
                //    context.Session[sessionName] = context.Session[sessionName] ?? instance;
                //    return context.Session[sessionName] as OT;
                //}
                //else
                {
                    _current = _current ?? Activator.CreateInstance<OT>();
                    return _current;
                }
            }
        }
    }
    public class ORMBase<T> where T : class
    {
        static bool tableChecked = false;
        public ORMBase()
        {
            if (!tableChecked)
            {
                if (!Tools.TableCheck<T>())
                    Tools.CreateTable<T>();
            }
        }
        private static int? pageSize;

        public static int PageSize
        {
            get
            {
                if (pageSize == null)
                {
                    pageSize = Convert.ToInt32(ConfigurationManager.AppSettings["PageSize"]);
                }
                return pageSize != null ? pageSize.Value : 20;
            }
            set { pageSize = value; }
        }

        public T this[int? id]
        {
            get
            {
                if (!id.HasValue)
                {
                    return null;
                }
                var res = Select(id.Value);
                return res;
            }
        }

        static CacheCollection<T> _cacheCollection;
        public CacheCollection<T> CacheContext
        {
            get
            {
                //HttpContext ctx = HttpContext.Current;
                //if (ctx != null)
                //{
                //    string appName = string.Format("{0}.{1}_Cache", Table.SchemaName, Table.TableName);
                //    ctx.Application[appName] = ctx.Application[appName] ?? new CacheCollection<T>();
                //    return (CacheCollection<T>)ctx.Application[appName];
                //}
                //else
                {
                    _cacheCollection = _cacheCollection ?? new CacheCollection<T>();
                    return _cacheCollection;
                }

            }
        }
        /// <summary>
        /// Class üzerinde belirtilmiş olan Table Attribute'unu getirir
        /// </summary>
        public Table Table
        {
            get
            {

                Type ty = typeof(T);
                var table = Attribute.GetCustomAttribute(ty, typeof(Table), true);
                return (Table)table;
                //var attrs=Table.GetCustomAttributes(ty);
                //if (attrs != null && attrs.Length != 0)
                //    return (Table)attrs[0];
                //else
                //    return null;
            }
        }
        /// <summary>
        /// T Generic tipinin Type halini getirir
        /// </summary>
        Type Type
        {
            get
            {
                return typeof(T);
            }
        }

        Table GetTable<K>()
        {
            Table t = (Table)Table.GetCustomAttribute(typeof(K), typeof(Table));
            t = t ?? Table;
            return t;
        }

        protected object GetPrimaryValue(T entity)
        {
            Type t = typeof(T);
            foreach (PropertyInfo item in t.GetProperties())
            {
                if (item.Name.ToLower() == Table.PrimaryKey.ToLower())
                {
                    return item.GetValue(entity);
                }
                else
                    return null;
            }
            return null;
        }

        string GetColumnsString()
        {
            StringBuilder sb = new StringBuilder();

            foreach (PropertyInfo col in Type.GetProperties())
            {
                sb.Append(string.Format("{0}, ", col.Name));
            }
            sb.Remove(sb.Length - 1, 1);
            return sb.ToString();
        }

        string CreateSelect<T>(string extension = "", string columnExtension = "")
        {
            Table t = (Table)Table.GetCustomAttribute(typeof(T), typeof(Table));
            t = t ?? Table;
            StringBuilder sb = new StringBuilder();
            sb.Append("SELECT ");
            sb.Append(columnExtension);
            sb.Append(" * FROM ");
            sb.Append(string.Format("{0}.{1}", t.SchemaName, t.TableName));
            sb.Append(" ");
            if (Table.IsActiveValid)
            {
                if (string.IsNullOrEmpty(extension.Trim()))
                {
                    sb.Append($"WHERE {Table.IsActiveField}=1");
                }
                else
                {
                    if (extension.ToLower().Contains("where"))
                    {

                        int whereIndex = extension.ToLower().IndexOf("where");
                        extension = extension.Insert(whereIndex + 5, " (");
                        int whereEndIndex = extension.Length;
                        if (extension.ToLower().Contains("order by"))
                        {
                            whereEndIndex = extension.ToLower().IndexOf("order by");
                        }
                        else if (extension.ToLower().Contains("group by"))
                        {
                            whereEndIndex = extension.ToLower().IndexOf("group by");
                        }
                        if (!extension.Contains(Table.IsActiveField))
                        {
                            extension = extension.Insert(whereEndIndex, $")  AND {Table.IsActiveField}=1 ");

                        }
                        else
                        {
                            extension = extension.Insert(whereEndIndex, $")");
                        }

                        sb.Append(extension);

                    }
                    else
                    {
                        sb.Append($"WHERE {Table.IsActiveField}=1 ");
                        sb.Append(extension);
                    }

                }
            }
            else
            {
                sb.Append(extension);
            }


            return sb.ToString();
        }

        #region Insert

        public virtual Result Insert(T entity)
        {


            PropertyInfo primaryCol = Type.GetProperties().FirstOrDefault(x => x.Name.ToLower() == Table.PrimaryKey.ToLower());

            if (Table.TableType == TableType.BusinessEntityTable)
            {
                string createBusinessEntityQuery = "Insert Base.BusinessEntities values(@type,getdate(),1);select Scope_Identity();";
                var cbePar = Table.BETypeID.CreateParameters("@type");

                var BEResult = Tools.ExecuteScalar(createBusinessEntityQuery, cbePar);
                if (BEResult.State == ResultState.Success)
                {

                    if (primaryCol != null)
                    {
                        primaryCol.SetValue(entity, Convert.ToInt64(BEResult.Data));

                    }
                    else
                    {
                        return new Result
                        {

                            State = ResultState.Exception,
                            Message = "Business Entity üretilirken bir sorun oluştu. Oluşturulan BusinessEntity Nesneye eklenemiyor."
                        };
                    }
                }
                else
                {
                    return BEResult;
                }

            }

            Dictionary<string, object> parameters = new Dictionary<string, object>();
            StringBuilder sb = new StringBuilder();

            sb.Append("INSERT INTO ");
            sb.Append(string.Format("{0}.{1}(", Table.SchemaName, Table.TableName));
            PropertyInfo identityColumn = null;
            foreach (PropertyInfo col in Type.GetProperties())
            {
                var ignore = Attribute.GetCustomAttribute(col, typeof(IgnoreCRUD));
                if (ignore != null) continue;

                if (Table.TableType == TableType.PrimaryTable && col.Name.ToLower() == Table.IdentityColumn.ToLower())
                {
                    identityColumn = col;
                }
                object value = col.GetValue(entity);
                if (value != null && col.Name.ToLower() != Table.IdentityColumn.ToLower())
                {
                    sb.Append(string.Format("{0},", col.Name));
                }
            }
            sb.Remove(sb.Length - 1, 1);
            sb.Append(") values (");
            foreach (PropertyInfo col in Type.GetProperties())
            {
                var ignore = Attribute.GetCustomAttribute(col, typeof(IgnoreCRUD));
                if (ignore != null) continue;

                object value = col.GetValue(entity);
                if (value != null && col.Name.ToLower() != Table.IdentityColumn.ToLower())
                {
                    sb.Append(string.Format("@{0},", col.Name));
                    parameters.Add(string.Format("@{0}", col.Name), value);
                }

            }
            sb.Remove(sb.Length - 1, 1);
            sb.Append("); ");
            Result result;
            if (Table.TableType == TableType.PrimaryTable)
            {
                sb.Append("Select Scope_Identity();");
                result = Tools.ExecuteScalar(sb.ToString(), parameters, connectionName: Table.ConnectionName);

                if (identityColumn != null)
                {//TODO:tipine bakarak çevir identity colon int olmayabiliyor.

                    identityColumn.SetValue(entity, Convert.ChangeType(result.Data, identityColumn.PropertyType));
                }
            }
            else if (Table.TableType == TableType.BusinessEntityTable)
            {
                result = Tools.ExecuteNonQuery(sb.ToString(), parameters, connectionName: Table.ConnectionName);
                result.Data = primaryCol.GetValue(entity);
            }
            else
            {
                result = Tools.ExecuteNonQuery(sb.ToString(), parameters, connectionName: Table.ConnectionName);

            }

            if (result.State == ResultState.Success)
            {
                CacheContext.Refresh();
            }




            return result;
        }

        /// <summary>
        /// Birden fazla kayıdın tek seferde eklenmesini sağlar. 
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public virtual Result Insert(IEnumerable<T> list)
        {
            var resultList = new List<Result>();
            foreach (var item in list)
            {
                var result = Insert(item);
                resultList.Add(result);
            }
            var exResult = resultList.FirstOrDefault(x => x.State != ResultState.Success);
            if (exResult != null)
            {
                return exResult;
            }
            else
            {
                return resultList.FirstOrDefault();
            }
        }

        public virtual Result Insert(T entity, long RelatedBEID, int relatedTypeId, int op1 = 0, int op2 = 0)
        {
            var r = Insert(entity);
            if (r.State == ResultState.Success)
            {

                string query = "Insert Base.BusinessEntityRelations values(@p,@t,@s,@o)";
                var p1 = r.Data.CreateParameters("@p");
                p1.Add("@s", RelatedBEID);
                p1.Add("@t", relatedTypeId);
                p1.Add("@o", op1);


                var p2 = RelatedBEID.CreateParameters("@p");
                p2.Add("@s", r.Data);
                p2.Add("@t", Table.BETypeID);
                p2.Add("@o", op2);


                var r1 = Tools.ExecuteNonQuery(query, p1);
                var r2 = Tools.ExecuteNonQuery(query, p2);

                if (r1.State == ResultState.Success && r2.State == ResultState.Success)
                {
                    return r;
                }
                else
                {
                    return r1.State == ResultState.Success ? r2 : r1;
                }

            }
            else
            {
                return new Result
                {
                    State = ResultState.Exception,
                    Message = "Business Entity İlişkisi Kurulamadı. Tablo bilgileri eksik olabilir"
                };
            }

        }

        public virtual Result Insert<K>(T entity, K related, int op1 = 0, int op2 = 0) where K : class
        {
            var r = Insert(entity);
            if (r.State == ResultState.Success)
            {
                Type relatedType = typeof(K);
                Table relatedTable = (Table)(Table.GetCustomAttribute(relatedType, typeof(Table)));
                PropertyInfo primaryProp = relatedType.GetProperties().FirstOrDefault(x => x.Name.ToLower() == relatedTable.PrimaryKey.ToLower());
                if (primaryProp != null)
                {
                    string query = "Insert Base.BusinessEntityRelations values(@p,@t,@s,@o)";
                    var p1 = r.Data.CreateParameters("@p");
                    p1.Add("@s", primaryProp.GetValue(related));
                    p1.Add("@t", relatedTable.BETypeID);
                    p1.Add("@o", op1);


                    var p2 = primaryProp.GetValue(related).CreateParameters("@p");
                    p2.Add("@s", r.Data);
                    p2.Add("@t", Table.BETypeID);
                    p2.Add("@o", op2);


                    var r1 = Tools.ExecuteNonQuery(query, p1);
                    var r2 = Tools.ExecuteNonQuery(query, p2);

                    if (r1.State == ResultState.Success && r2.State == ResultState.Success)
                    {
                        return r;
                    }
                    else
                    {
                        return r1.State == ResultState.Success ? r2 : r1;
                    }

                }
                else
                {
                    return new Result
                    {
                        State = ResultState.Exception,
                        Message = "Business Entity İlişkisi Kurulamadı. Tablo bilgileri eksik olabilir"
                    };
                }
            }
            else
            {
                return r;
            }
        }

        #endregion

        #region Update
        public virtual Result Update(IEnumerable<T> list)
        {
            var resultList = new List<Result>();
            foreach (var item in list)
            {
                var result = Update(item);
                resultList.Add(result);
            }
            var exResult = resultList.FirstOrDefault(x => x.State != ResultState.Success);
            if (exResult != null)
            {
                return exResult;
            }
            else
            {
                return resultList.FirstOrDefault();
            }
        }
        public virtual Result Update(T entity)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            StringBuilder sb = new StringBuilder();
            sb.Append("UPDATE ");
            sb.Append(string.Format("{0}.{1}", Table.SchemaName, Table.TableName));
            sb.Append(" SET ");
            List<PropertyInfo> compositeKeys = new List<PropertyInfo>();
            foreach (PropertyInfo item in Type.GetProperties())
            {
                if (item.GetCustomAttribute<IgnoreCRUD>() != null)
                {
                    continue;
                }
                object value = item.GetValue(entity);
                if (Table.CompositeKeys != null && Table.CompositeKeys.Contains(item.Name))
                {
                    compositeKeys.Add(item);
                }
                if (item.Name.ToLower() == Table.PrimaryKey.ToLower() || item.Name.ToLower() == Table.IdentityColumn.ToLower() || (Table.CompositeKeys != null && Table.CompositeKeys.Contains(item.Name)))
                {
                    parameters.Add(string.Format("@{0}", item.Name), value);
                    continue;
                }

                if (value != null)
                {
                    parameters.Add(string.Format("@{0}", item.Name), value);
                    sb.Append(string.Format("{0}=@{0},", item.Name));
                }
                else
                {
                    parameters.Add(string.Format("@{0}", item.Name), DBNull.Value);
                    sb.Append(string.Format("{0}=@{0},", item.Name));
                }


            }
            sb.Remove(sb.Length - 1, 1);
            switch (Table.TableType)
            {
                case TableType.PrimaryTable:
                case TableType.BusinessEntityTable:
                    sb.Append(string.Format(" WHERE {0}=@{0}", Table.PrimaryKey, GetPrimaryValue(entity)));
                    break;
                case TableType.CompositTable:
                    sb.Append(" WHERE ");
                    foreach (var item in compositeKeys)
                    {
                        var cv = item.GetValue(entity);
                        sb.Append(string.Format("{0}=@{0}", item.Name));
                        sb.Append(" AND ");
                    }
                    sb.Remove(sb.Length - 4, 4);
                    break;

                default:

                    return new Result { State = ResultState.Exception, Message = "Tablo tipi belirtilmeli" };
                    break;
            }

            var result = Tools.ExecuteNonQuery(sb.ToString(), parameters, CommandType.Text, connectionName: Table.ConnectionName);
            if (result.State == ResultState.Success)
            {
                CacheContext.Refresh();
            }
            return result;
        }


        /// <summary>
        /// Update sorgusunu part part yazarsın
        /// </summary>
        /// <param name="set">SET yazmaya gerek yok</param>
        /// <param name="where">WHERE deyimini eklemek zorundasın</param>
        /// <param name="parameters">Parametreler Dictionary olacak</param>
        /// <returns></returns>
        public virtual Result Update(string set, string where, Dictionary<string, object> parameters)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("UPDATE ");
            sb.Append(string.Format("{0}.{1}", Table.SchemaName, Table.TableName));
            sb.Append(" SET ");
            sb.Append(set);
            sb.Append(" ");
            sb.Append(where);

            var result = Tools.ExecuteNonQuery(sb.ToString(), parameters, CommandType.Text, connectionName: Table.ConnectionName);
            if (result.State == ResultState.Success)
            {
                CacheContext.Refresh();
            }
            return result;

        }

        public virtual Result UpdatePart(string key, object value, object id)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            StringBuilder sb = new StringBuilder();
            sb.Append("UPDATE ");
            sb.Append(string.Format("{0}.{1}", Table.SchemaName, Table.TableName));
            sb.Append(" SET ");
            sb.Append(key);
            sb.Append("=@");
            sb.Append(key);

            sb.Append(string.Format(" WHERE {0}=@{0}", Table.PrimaryKey, id));
            parameters.Add("@" + key, value);
            parameters.Add("@" + Table.PrimaryKey, id);
            var result = Tools.ExecuteNonQuery(sb.ToString(), parameters, CommandType.Text, connectionName: Table.ConnectionName);
            if (result.State == ResultState.Success)
            {
                CacheContext.Refresh();
            }
            return result;
        }
        public virtual Result UpdatePart(string key, object value, params Tuple<string, object>[] ids)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            StringBuilder sb = new StringBuilder();
            sb.Append("UPDATE ");
            sb.Append(string.Format("{0}.{1}", Table.SchemaName, Table.TableName));
            sb.Append(" SET ");
            sb.Append(key);
            sb.Append("=@");
            sb.Append(key);
            sb.Append(" WHERE ");
            foreach (var item in ids)
            {
                sb.Append(string.Format(" {0}=@{0} AND", item.Item1));
                parameters.Add("@" + item.Item1, item.Item2);
            }
            sb.Remove(sb.Length - 4, 4);
            parameters.Add("@" + key, value);
            var result = Tools.ExecuteNonQuery(sb.ToString(), parameters, CommandType.Text, connectionName: Table.ConnectionName);
            if (result.State == ResultState.Success)
            {
                CacheContext.Refresh();
            }
            return result;
        }

        public virtual Result UpdatePart(Dictionary<string, object> updateFields, object id)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            StringBuilder sb = new StringBuilder();
            sb.Append("UPDATE ");
            sb.Append(string.Format("{0}.{1}", Table.SchemaName, Table.TableName));
            sb.Append(" SET ");
            foreach (var item in updateFields)
            {
                sb.Append(item.Key);
                sb.Append("=@");
                sb.Append(item.Key);
                sb.Append(",");
                parameters.Add("@" + item.Key, item.Value);
            }
            sb.Remove(sb.Length - 1, 1);

            sb.Append(" WHERE ");

            sb.Append(Table.PrimaryKey);
            sb.Append("=@");
            sb.Append(Table.PrimaryKey);

            parameters.Add("@" + Table.PrimaryKey, id);

            var result = Tools.ExecuteNonQuery(sb.ToString(), parameters, CommandType.Text, connectionName: Table.ConnectionName);
            if (result.State == ResultState.Success)
            {
                CacheContext.Refresh();
            }
            return result;
        }

        public virtual Result UpdatePart(Dictionary<string, object> updateFields, params Tuple<string, object>[] ids)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            StringBuilder sb = new StringBuilder();
            sb.Append("UPDATE ");
            sb.Append(string.Format("{0}.{1}", Table.SchemaName, Table.TableName));
            sb.Append(" SET ");
            foreach (var item in updateFields)
            {
                sb.Append(item.Key);
                sb.Append("=@");
                sb.Append(item.Key);
                sb.Append(",");
                parameters.Add("@" + item.Key, item.Value);
            }
            sb.Remove(sb.Length - 1, 1);
            sb.Append(" WHERE ");
            foreach (var item in ids)
            {
                sb.Append(string.Format(" {0}=@{0} AND", item.Item1));
                parameters.Add("@" + item.Item1, item.Item2);
            }
            sb.Remove(sb.Length - 4, 4);

            var result = Tools.ExecuteNonQuery(sb.ToString(), parameters, CommandType.Text, connectionName: Table.ConnectionName);

            if (result.State == ResultState.Success)
            {
                CacheContext.Refresh();
            }

            return result;
        }

        public virtual Result Update<Tkey>(Expression<Func<T, Tkey>> set, Expression<Func<T, bool>> where)
        {
            Dictionary<string, object> prm = new Dictionary<string, object>();
            StringBuilder wh = Where(where.Body, prm);
            StringBuilder st = CreateSet(set, prm);
            var r = Update(st.ToString(), wh.ToString(), prm);
            return r;
        }

        public virtual StringBuilder CreateSet(Expression exp, Dictionary<string, object> prm)
        {
            StringBuilder sb = new StringBuilder();
            return sb;
        }

        #endregion

        #region Delete

        public virtual Result Delete(T entity)
        {
            //update tablo set IsActive=0 where PrimaryKey=@pid
            if (Table.IsActiveValid)
            {
                string sorgu = $"UPDATE {Table.TableName} set {Table.IsActiveField} = 0 where {Table.PrimaryKey} = @pid";
                var primaryColumn = typeof(T).GetProperties().FirstOrDefault(x => x.Name == Table.PrimaryKey);
                if (primaryColumn != null)
                {
                    var value = primaryColumn.GetValue(entity);
                    if (value != null)
                    {
                        var prms = value.CreateParameters("@pid");
                        var result = Tools.ExecuteNonQuery(sorgu, prms);
                        return result;
                    }
                    else
                    {
                        return new Result { State = ResultState.Exception, Message = $"{Table.PrimaryKey} bilgisi olmadan silme yapılamaz." };
                    }
                }
                else
                {
                    return new Result { State = ResultState.Exception, Message = $"Entity içerisinde {Table.PrimaryKey} alanı bulunamadı." };

                }
            }
            else
            {


                StringBuilder sb = new StringBuilder();
                sb.Append("DELETE FROM ");
                sb.Append(string.Format("{0}.{1} WHERE ", Table.SchemaName, Table.TableName));
                Dictionary<string, object> parameters = new Dictionary<string, object>();
                switch (Table.TableType)
                {
                    case TableType.PrimaryTable:
                    case TableType.BusinessEntityTable:
                        sb.Append(string.Format("{0}=@{0}", Table.PrimaryKey));
                        parameters.Add(string.Format("@{0}", Table.PrimaryKey), GetPrimaryValue(entity));
                        break;
                    case TableType.CompositTable:
                        foreach (var item in Table.CompositeKeys)
                        {
                            var prp = entity.GetType().GetProperties().FirstOrDefault(x => x.Name.ToLower() == item.ToLower());
                            if (prp != null)
                            {
                                sb.Append(string.Format("{0}=@{0} AND ", item));
                                parameters.Add(string.Format("@{0}", item), prp.GetValue(entity));
                            }
                        }

                        sb.Remove(sb.Length - 4, 4);

                        break;


                    default:
                        return new Result { Message = "Tablo Tipi Bulunamadığından silme işlemi iptal edildi", State = ResultState.Exception };
                        break;
                }



                var result = Tools.ExecuteNonQuery(sb.ToString(), parameters, CommandType.Text, connectionName: Table.ConnectionName);

                if (result.State == ResultState.Success)
                {
                    CacheContext.Refresh();
                }
                return result;

            }

        }
        public virtual Result DeleteWithID(object ID)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("DELETE FROM ");
            sb.Append(string.Format("{0}.{1} WHERE ", Table.SchemaName, Table.TableName));
            Dictionary<string, object> parameters = new Dictionary<string, object>();

            sb.Append(string.Format("{0}=@{0}", Table.PrimaryKey));
            parameters.Add(string.Format("@{0}", Table.PrimaryKey), ID);

            var result = Tools.ExecuteNonQuery(sb.ToString(), parameters, CommandType.Text, connectionName: Table.ConnectionName);
            if (result.State == ResultState.Success)
            {
                CacheContext.Refresh();
            }
            return result;

        }
        /// <summary>
        /// Table Tanımındaki sıraya göre değerler gonderilerek silme işlemi yapar
        /// </summary>
        /// <param name="IDs">Composite alanlara denk gelecek id ler(sırası entity deki Table Attr'de verilmiştir</param>
        /// <returns></returns>
        public virtual Result DeleteWithID(params object[] IDs)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("DELETE FROM ");
            sb.Append(string.Format("{0}.{1} WHERE ", Table.SchemaName, Table.TableName));
            Dictionary<string, object> parameters = new Dictionary<string, object>();

            for (var i = 0; i < Table.CompositeKeys.Length; i++)
            {
                var item = Table.CompositeKeys[i];
                sb.Append(string.Format("{0}=@{0} AND ", item));
                parameters.Add(string.Format("@{0}", item), IDs[i]);

            }

            sb.Remove(sb.Length - 4, 4);

            var result = Tools.ExecuteNonQuery(sb.ToString(), parameters, CommandType.Text, connectionName: Table.ConnectionName);
            if (result.State == ResultState.Success)
            {
                CacheContext.Refresh();
            }
            return result;

        }
        public virtual Result DeleteWithField(string fieldName, object fieldValue)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("DELETE FROM ");
            sb.Append(string.Format("{0}.{1} WHERE ", Table.SchemaName, Table.TableName));
            Dictionary<string, object> parameters = new Dictionary<string, object>();

            sb.Append(string.Format("{0}=@{0}", fieldName));
            parameters.Add(string.Format("@{0}", fieldName), fieldValue);

            var result = Tools.ExecuteNonQuery(sb.ToString(), parameters, CommandType.Text, connectionName: Table.ConnectionName);
            if (result.State == ResultState.Success)
            {
                CacheContext.Refresh();
            }
            return result;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="where"> başına WHERE yaz</param>
        /// <param name="prm"></param>
        /// <returns></returns>
        public virtual Result Delete(string where, Dictionary<string, object> prm)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("DELETE FROM ");
            sb.Append(string.Format("{0}.{1} {2}", Table.SchemaName, Table.TableName, where));

            var result = Tools.ExecuteNonQuery(sb.ToString(), prm, CommandType.Text, connectionName: Table.ConnectionName);
            if (result.State == ResultState.Success)
            {
                CacheContext.Refresh();
            }
            return result;
        }

        #endregion

        #region Select 
        Result<List<T>> ExecuteSelect(string query, Dictionary<string, object> parameters, string connectionName, SelectType type)
        {
            var result = CacheContext.GetItems(query, parameters, type);
            if (result != null)
            {
                return new Result<List<T>>
                {
                    Data = result,
                    State = ResultState.Success
                };
            }
            else
            {
                CommandType ct = CommandType.Text;
                switch (type)
                {
                    case SelectType.StoredProcedure:
                        ct = CommandType.StoredProcedure;
                        break;
                    case SelectType.Text:
                        ct = CommandType.Text;
                        break;
                    case SelectType.TableDirect:
                        ct = CommandType.TableDirect;
                        break;
                    case SelectType.Where:
                        ct = CommandType.Text;
                        break;
                    default:
                        break;
                }
                var q = Tools.Select<T>(query, parameters, connectionName, ct);
                if (q.State == ResultState.Success)
                {

                    CacheContext.InsertItems(q.Data, query, parameters, type);
                }
                return q;
            }


        }

        public virtual Result<List<T>> Select()
        {
            string query = CreateSelect<T>();
            var dt = ExecuteSelect(query, null, Table.ConnectionName, SelectType.Text);
            return new Result<List<T>>
            {
                Data = dt.Data,
                Message = dt.Message,
                State = dt.State
            };
        }

        public virtual T Select(object value)
        {

            Dictionary<string, object> p = new Dictionary<string, object>();
            p.Add("@" + Table.PrimaryKey, value);
            var r = ExecuteSelect(CreateSelect<T>(extension: string.Format("WHERE {0}=@{0}", Table.PrimaryKey)), p, Table.ConnectionName, SelectType.Where);
            return r.Data != null ? r.Data.FirstOrDefault() : null;

        }

        public virtual Result<List<T>> Select(string query, Dictionary<string, object> p, SelectType type)
        {
            string q = "";
            switch (type)
            {
                case SelectType.StoredProcedure:
                    q = query;
                    break;
                case SelectType.Text:
                    q = query;
                    break;
                case SelectType.TableDirect:
                    q = query;
                    break;
                case SelectType.Where:
                    q = CreateSelect<T>(query);
                    break;
                default:
                    break;
            }

            return ExecuteSelect(q, p, Table.ConnectionName, type);
        }

        public virtual PagedResult<List<T>> Select(string commandText = "", Dictionary<string, object> parameters = null, SelectType type = SelectType.Text, int page = 1, int count = 999999)
        {

            CommandType ct;
            string query;
            switch (type)
            {
                case SelectType.StoredProcedure:
                    ct = CommandType.StoredProcedure;
                    query = commandText;
                    break;
                case SelectType.Text:
                    ct = CommandType.Text;
                    query = commandText;
                    break;
                case SelectType.TableDirect:
                    ct = CommandType.TableDirect;
                    query = commandText;
                    break;
                case SelectType.Where:
                    //ct = CommandType.Text;
                    //StringBuilder sb = new StringBuilder();
                    //sb.Append("Select * from (Select ");


                    //if (Table.TableType == TableType.PrimaryTable)
                    //{
                    //    sb.Append(string.Format(" Row_Number() over (order by {0}) as RowNum, * ", Table.PrimaryKey));
                    //}
                    //else
                    //{
                    //    if (Table.CompositeKeys.Any())
                    //    {
                    //        sb.Append("  Row_Number() over (order by ");
                    //        foreach (var clm in Table.CompositeKeys)
                    //        {
                    //            sb.Append(clm + ",");
                    //        }

                    //    }
                    //    else
                    //    {
                    //        foreach (var clm in Type.GetProperties())
                    //        {
                    //            sb.Append(clm.Name + ",");
                    //        }
                    //    }
                    //    sb.Remove(sb.Length - 1, 1);
                    //    sb.Append(") as RowNum, *");
                    //}
                    //sb.Append(" FROM (");

                    //sb.Append(CreateSelect<T>(commandText, " Top 999999999999 "));
                    //sb.Append(" ) as t) as k where RowNum between @f and @t ");
                    //query = sb.ToString();
                    //parameters = parameters ?? new Dictionary<string, object>();
                    //int start = (page - 1) * count,
                    //    end = start + count;
                    //parameters.Add("@f", start + 1);
                    //parameters.Add("@t", end);
                    query = CreateSelect<T>(commandText);

                    break;
                default:
                    ct = CommandType.Text;
                    query = commandText;
                    break;
            }
            var result = ExecuteSelect(query, parameters, Table.ConnectionName, type);
            string countCommantText = commandText.ToLower().Replace("ı", "I");
            if (countCommantText.Contains("order by"))
            {
                if (countCommantText.IndexOf("order by") == 0)
                {
                    countCommantText = "";
                }
                else
                {
                    countCommantText = countCommantText.Substring(0, countCommantText.IndexOf("order by")).Replace('ı', 'i');
                }

            }
            int rowcount = RowCount(countCommantText, parameters);
            if (result.Data != null)
            {
                return new PagedResult<List<T>>
                {
                    //sayfalama aktif olunca kapatılacak.
                    Data = result.Data.ToList<T>().Skip(count * (page - 1)).Take(count).ToList(),
                    Message = result.Message,
                    State = result.State,
                    TotalRowCount = rowcount,
                    PageSize = count,
                    ActivePage = page
                };
            }
            else
            {
                return new PagedResult<List<T>>
                {
                    //Data = result.Data.ToList<K>(),
                    Message = result.Message,
                    State = result.State
                };
            }

        }

        public virtual PagedResult<List<T>> Select(string commandText, int page, int count, Dictionary<string, object> parameters = null, SelectType type = SelectType.Text)
        {
            return Select(commandText, parameters, type, page, count);
        }

        public virtual Result<List<T>> Select(long primaryId, int typeId, int operationID = 0)
        {

            var result = Select(primaryId, typeId, 1, 999999, operationID);
            return result;
        }

        public virtual PagedResult<List<T>> Select(long primaryId, int typeId, int page, int count, int operationID = 0)
        {
            var prm = primaryId.CreateParameters("@pid");
            prm.Add("@type", typeId);
            prm.Add("@operation", operationID);

            StringBuilder sb = new StringBuilder();
            sb.Append("WHERE BEID in (Select SecondaryBEID from Base.BusinessEntityRelations Where PrimaryBEID=@pid and TypeID=@type AND OperationID=@operation )");

            var result = Select(sb.ToString(), page, count, prm, SelectType.Where);
            return result;
        }

        #endregion

        public virtual int RowCount(string commandText = "", Dictionary<string, object> parameters = null)
        {

            string query = string.Format("SELECT Count(*) FROM {0}.{1} {2}", Table.SchemaName, Table.TableName, commandText);
            var result = Tools.ExecuteScalar(query, parameters);
            if (result.State == ResultState.Success)
            {
                return Convert.ToInt32(result.Data);
            }
            else
            {
                return 0;
            }


        }

        public int GetRowCount(string commandText, Dictionary<string, object> parameters)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("Select Count(*) from ");
            sb.Append(string.Format("{0}.{1}", Table.SchemaName, Table.TableName));
            sb.Append(" ");
            sb.Append(commandText);
            return Convert.ToInt32(Tools.ExecuteScalar(sb.ToString(), parameters: parameters));
        }



        public virtual Result<List<T>> Search(object text)
        {
            if (text is string)
            {
                text = ((string)text).Replace(' ', '%').Replace(',', '%');
            }
            StringBuilder sb = new StringBuilder();
            sb.Append("WHERE ");
            Dictionary<string, object> prm = new Dictionary<string, object>();

            foreach (var item in Table.SearchFields)
            {
                sb.Append(string.Format("{0} like '%'+@{0}+'%'", item));
                prm.Add("@" + item, text);
                sb.Append(" OR ");
            }
            if (sb.Length > 6)//where olduğu için
                sb.Remove(sb.Length - 3, 3);
            else
                sb.Remove(0, sb.Length);

            var result = Select(sb.ToString(), prm, SelectType.Where);
            return result;

        }

        public virtual PagedResult<List<T>> Search(object text, int count, int page)
        {
            if (text is string)
            {
                text = ((string)text).Replace(' ', '%').Replace(',', '%');
            }
            StringBuilder sb = new StringBuilder();
            sb.Append("WHERE ");
            Dictionary<string, object> prm = new Dictionary<string, object>();

            foreach (var item in Table.SearchFields)
            {
                sb.Append(string.Format("{0} like '%'+@{0}+'%'", item));
                prm.Add("@" + item, text);
                sb.Append(" OR ");
            }
            if (sb.Length > 6)//where olduğu için
                sb.Remove(sb.Length - 3, 3);
            else
                sb.Remove(0, sb.Length);

            var result = Select(sb.ToString(), page, count, prm, SelectType.Where);
            return result;

        }

        public object LastID()
        {
            string query = $"Select max({Table.PrimaryKey}) from {Table.SchemaName}.{Table.TableName}";
            var res = Tools.ExecuteScalar(query);
            if (res.State == ResultState.Success)
            {
                return res.Data;
            }
            else
            {
                return null;
            }
        }


        #region Expression

        public virtual T FirstOrDefault(Expression<Func<T, bool>> exp)
        {
            var r = Where(exp);
            return r?.Data?.FirstOrDefault();
        }

        public virtual Result<List<T>> Where(Expression<Func<T, bool>> exp)
        {
            Dictionary<string, object> prm = new Dictionary<string, object>();
            StringBuilder sb = Where(exp?.Body, prm);
            if (sb != null && sb.Length > 0)
            {
                return Select("WHERE " + sb.ToString(), prm, SelectType.Where);
            }
            else
            {
                return Select("", prm, SelectType.Where);

            }
        }

        StringBuilder Where(Expression exp, Dictionary<string, object> prm)
        {
            StringBuilder sb = new StringBuilder();
            if (exp == null)
            {
                return sb;
            }

            if (exp is BinaryExpression)
            {
                BinaryExpression b = (BinaryExpression)exp;
                sb.Append("(");
                sb.Append(Where(b.Left, prm));

                switch (b.NodeType)
                {
                    case ExpressionType.And:
                    case ExpressionType.AndAlso:
                        sb.Append(" AND ");
                        break;
                    case ExpressionType.Or:
                    case ExpressionType.OrElse:
                        sb.Append(" OR ");
                        break;
                    case ExpressionType.Equal:
                        sb.Append(" = ");
                        break;
                    case ExpressionType.NotEqual:
                        sb.Append(" <> ");
                        break;
                    case ExpressionType.GreaterThan:
                        sb.Append(" > ");
                        break;
                    case ExpressionType.GreaterThanOrEqual:
                        sb.Append(" >= ");
                        break;
                    case ExpressionType.LessThan:
                        sb.Append(" < ");
                        break;
                    case ExpressionType.LessThanOrEqual:
                        sb.Append(" <= ");
                        break;
                    case ExpressionType.Add:
                        sb.Append("+");
                        break;
                    case ExpressionType.Subtract:
                        sb.Append("-");
                        break;
                    case ExpressionType.Multiply:
                        sb.Append("*");
                        break;
                    case ExpressionType.Divide:
                        sb.Append("/");
                        break;
                    case ExpressionType.Modulo:
                        sb.Append("%");
                        break;
                }


                sb.Append(Where(b.Right, prm));
                sb.Append(")");
            }
            //else if(exp is PropertyExpression)
            //{

            //}            
            else if (exp is MemberExpression)
            {
                MemberExpression m = exp as MemberExpression;
                if (m.Expression == null)
                {
                    if (m.Member is PropertyInfo)
                    {
                        PropertyInfo pi = (PropertyInfo)m.Member;
                        var val = pi.GetValue(null);
                        if (val != null)
                        {
                            string prmName = $"@{m.Member.Name}";
                            int pnum = 1;
                            while (prm.ContainsKey(prmName))
                            {
                                prmName = $"@{m.Member.Name}{pnum}"; ;
                                pnum++;
                            }
                            prm.Add(prmName, val);
                            sb.Append(prmName);
                        }
                    }

                }
                else
                {
                    switch (m.Expression.NodeType)
                    {
                        case ExpressionType.Parameter:
                            sb.Append(m.Member.Name);
                            break;
                        case ExpressionType.Constant:
                            {
                                ConstantExpression c = (ConstantExpression)m.Expression;
                                var value = GetConstant(c, m.Member.Name);
                                string prmName = $"@{m.Member.Name}";
                                int pnum = 1;
                                while (prm.ContainsKey(prmName))
                                {
                                    prmName = $"@{m.Member.Name}{pnum}"; ;
                                    pnum++;
                                }
                                if (value != null)
                                {
                                    prm.Add(prmName, value);
                                }
                                else
                                {
                                    prm.Add(prmName, DBNull.Value);
                                }
                                sb.Append($"{prmName}");
                                break;
                            }
                        case ExpressionType.MemberAccess:
                            {
                                string propName = m.Member.Name;
                                var mb = m.Expression as MemberExpression;
                                object val = null;
                                if (mb.Expression != null)
                                {
                                    //static değildir instance expression içindedir.
                                    if (mb.Expression is ConstantExpression)
                                    {
                                        var obj = GetConstant(mb.Expression as ConstantExpression, mb.Member.Name);
                                        val = obj.GetType().GetProperty(propName).GetValue(obj);
                                    }
                                    //else if(mb.Member is FieldInfo)
                                    //{
                                    //    FieldInfo fi = (FieldInfo)mb.Member;
                                    //    var value = fi.GetValue(m.Expression);
                                    //    string prmName = $"@{m.Member.Name}";
                                    //    int pnum = 1;
                                    //    while (prm.ContainsKey(prmName))
                                    //    {
                                    //        prmName = $"@{m.Member.Name}{pnum}"; ;
                                    //        pnum++;
                                    //    }
                                    //    prm.Add(prmName, val);
                                    //    sb.Append(prmName);
                                    //}
                                }
                                else
                                {
                                    // static bir alandan çekiyordur. 
                                    PropertyInfo pi = (PropertyInfo)mb.Member;
                                    var obj = pi.GetValue(null);

                                    PropertyInfo mpi = (PropertyInfo)m.Member;
                                    if (obj != null)
                                    {
                                        val = mpi.GetValue(obj);
                                    }
                                }
                                if (val != null)
                                {
                                    string prmName = $"@{m.Member.Name}";
                                    int pnum = 1;
                                    while (prm.ContainsKey(prmName))
                                    {
                                        prmName = $"@{m.Member.Name}{pnum}"; ;
                                        pnum++;
                                    }
                                    prm.Add(prmName, val);
                                    sb.Append(prmName);
                                }
                                break;
                            }
                    }
                }
            }
            else if (exp is ConstantExpression)
            {
                ConstantExpression c = (ConstantExpression)exp;
                string prmName = $"@p0";
                int pnum = 1;
                while (prm.ContainsKey(prmName))
                {
                    prmName = $"@p{pnum}"; ;
                    pnum++;
                }
                if (c.Value.GetType().IsEnum)
                {
                    prm.Add(prmName, (int)c.Value);


                }
                else
                if (c.Value.GetType().Name == "Boolean")
                {
                    prm.Add(prmName, ((bool)c.Value?1:0));
                }
                else
                {
                    prm.Add(prmName, c.Value);

                }
                sb.Append(prmName);

            }
            else if (exp is MethodCallExpression)
            {
                MethodCallExpression m = exp as MethodCallExpression;
                List<object> arguments = new List<object>();
                foreach (object item in m.Arguments)
                {
                    if (item is ConstantExpression)
                    {
                        arguments.Add(((ConstantExpression)item).Value);
                    }
                    else if (item is MemberExpression)
                    {
                        MemberExpression mem = item as MemberExpression;
                        switch (mem.Expression.NodeType)
                        {
                            case ExpressionType.Constant:
                                {
                                    ConstantExpression c = (ConstantExpression)mem.Expression;
                                    var memvalue = GetConstant(c, mem.Member.Name);

                                    arguments.Add(memvalue);

                                    break;
                                }
                            case ExpressionType.MemberAccess:
                                {
                                    string propName = mem.Member.Name;
                                    var mb = mem.Expression as MemberExpression;
                                    var obj = GetConstant(mb.Expression as ConstantExpression, mb.Member.Name);
                                    var val = obj.GetType().GetProperty(propName).GetValue(obj);

                                    arguments.Add(val);
                                    break;
                                }
                        }
                    }
                }
                MemberExpression obex = (MemberExpression)m.Object;
                var value = m.Method.Invoke(obex == null ? null : GetConstant((ConstantExpression)obex.Expression, obex.Member.Name), arguments.ToArray());

                string prmName = $"@{m.Method.Name}";
                int pnum = 1;
                while (prm.ContainsKey(prmName))
                {
                    prmName = $"@{m.Method.Name}{pnum}"; ;
                    pnum++;
                }
                prm.Add(prmName, value);
                sb.Append(prmName);
            }
            else if (exp is UnaryExpression)
            {
                UnaryExpression u = exp as UnaryExpression;
                sb.Append(Where(u.Operand, prm));

            }
            else
            {
                throw new Exception("Buraya Bi Bak (ORMBase.Where)");
            }

            return sb;
        }
        object GetProp(object obj, string fieldName)
        {
            var prp = obj.GetType().GetProperty(fieldName);
            return prp?.GetValue(obj);
        }
        object GetConstant(ConstantExpression exp, string fieldName)
        {
            if (exp == null) return null;
            if (string.IsNullOrEmpty(fieldName)) return exp.Value;

          
            FieldInfo field = exp.Type.GetField(fieldName);
            PropertyInfo prop = exp.Type.GetProperty(fieldName);

            return field != null ? field.GetValue(exp.Value) : prop != null ? prop.GetValue(exp.Value) : null;
        }
       

        public Result<List<T>> OrderBy<TKey>(Expression<Func<T, TKey>> exp)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("Order By ");
            if (exp.Body is NewExpression)
            {
                NewExpression n = (NewExpression)exp.Body;
                foreach (MemberExpression item in n.Arguments)
                {
                    string fn = item.Member.Name;

                    sb.Append(fn);
                    sb.Append(",");

                }
                sb.Remove(sb.Length - 1, 1);
            }
            else
            {
                MemberExpression m = exp.Body as MemberExpression;

                string fn = m.Member.Name;
                sb.Append(fn);
            }


            return Select(sb.ToString(), null, SelectType.Where);

        }



        #endregion


    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="ET">Entity Tipi.</typeparam>
    /// <typeparam name="OT">ORM in kendi tipi</typeparam>
    /// <typeparam name="VT">View'in tipi ConvertToView metodunu ezmeyi unutmayın</typeparam>
    public class ViewedORMBase<ET, OT, VT> : ORMBase<ET, OT>, IViewORM<ET, VT>
         where ET : class
        where OT : ORMBase<ET>
        where VT : class
    {
        public virtual VT ConvertToView(ET entity)
        {
            VT view = entity.MapTo<VT>();
            return view;
        }

        public virtual VT RelatedView(long beid, int operationID = 0)
        {

            var r = Select(beid, Table.BETypeID, operationID);
            if (r.DataReady)
            {
                return ConvertToView(r.Data.FirstOrDefault());
            }
            else
            {
                return null;
            }
        }

        public virtual VT SingleView(long beid)
        {
            var r = Select(beid);
            if (r != null)
            {
                return ConvertToView(r);
            }
            else
            {
                return null;
            }
        }

        public virtual new List<VT> View()
        {
            var r = Select();
            if (r.DataReady)
            {
                return r.Data.Select(ConvertToView).ToList();
            }
            else
            {
                return null;
            }
        }

        public virtual new List<VT> View(long beid, int operationID = 0)
        {
            var r = Select(beid, Table.BETypeID, operationID);
            if (r.DataReady)
            {
                return r.Data.Select(ConvertToView).ToList();
            }
            else
            {
                return null;
            }
        }

        public virtual new List<VT> View(string query, Dictionary<string, object> prm)
        {
            var r = Select(query, prm, SelectType.Where);
            if (r.DataReady)
            {
                return r.Data.Select(ConvertToView).ToList();
            }
            else
            {
                return null;
            }
        }

    }



}
