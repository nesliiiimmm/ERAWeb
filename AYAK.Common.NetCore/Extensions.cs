using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;


namespace AYAK.Common.NetCore
{
    public static class Extensions
    {
        public static Action<string> Notification;
        public static List<T> ToList<T>(this DataTable dt)
        {

            if (dt == null)
            {
                return null;
            }
            var propList = typeof(T).GetProperties();
            //T instance = Activator.CreateInstance<T>();


            Dictionary<string, PropertyInfo> map = new Dictionary<string, PropertyInfo>();
            foreach (var prp in propList)
            {
                if (dt.Columns[prp.Name] != null && !map.Keys.Contains(prp.Name))
                {
                    //int index = dt.Columns.IndexOf(dt.Columns[prp.Name]);
                    map.Add(prp.Name, prp);
                }
            }
            List<T> collection = new List<T>();
            foreach (DataRow row in dt.Rows)
            {
                T data = Activator.CreateInstance<T>();
                foreach (var item in map)
                {
                    if (row[item.Key] is DBNull)
                        item.Value.SetValue(data, null);
                    else
                    {
                        var gp = item.Value.PropertyType.GenericTypeArguments.FirstOrDefault();
                        if (item.Value.PropertyType.IsEnum)
                        {
                            item.Value.SetValue(data, Enum.ToObject(item.Value.PropertyType, row[item.Key]), null);
                        }
                        else if(gp != null && gp.IsEnum)
                        {
                            item.Value.SetValue(data, Enum.ToObject(gp, row[item.Key]), null);
                        }
                        else
                        {
                            item.Value.SetValue(data, row[item.Key]);
                        }
                    }
                }
                collection.Add(data);
            }
            return collection;
        }

        //map to metodunda atlanması gereken tüm property tiplerini buraya yazılacak.
        static string[] ignoredPropertyTypes = { "String" };
        public static T MapTo<T>(this object source, T result = null) where T : class
        {
            if (source == null)
                return null;
            return (T)MapTo(typeof(T), source.GetType(), source, result);
        }
        static object MapTo(Type TargetType, Type SourceType, object source, object result = null)
        {
            if (source == null) return null;
            if (TargetType.Name.Contains("List") || TargetType.Name.Contains("Collection"))
                return null;
            var tp = TargetType.GetProperties();
            var sp = SourceType.GetProperties();

            result = result ?? Activator.CreateInstance(TargetType);
            foreach (var t in tp)
            {
                PropertyInfo s = sp.FirstOrDefault(x => x.Name == t.Name);
                if (s != null && t.GetSetMethod() != null)
                {
                    Type stype = s.PropertyType;


                    if (!ignoredPropertyTypes.Contains(stype.Name) && (stype.IsClass || stype.IsInterface))
                    {
                        t.SetValue(result, MapTo(t.PropertyType, s.PropertyType, s.GetValue(source)));
                    }
                    else
                    {

                        object value = s.GetValue(source);
                        t.SetValue(result, s.GetValue(source));
                    }
                }
            }
            return result;
        }

        public static object GetField(this DataRow row, string field)
        {

            if (!(row[field] is DBNull))
                return row[field];
            else
                return null;

        }

        static string ReplaceURL(string url)
        {
            if (string.IsNullOrEmpty(url))
                url = "link";
            string u = url.ToLower().Replace(" ", "-").Replace("ü", "u").Replace("ö", "o").Replace("ı", "i").Replace('ç', 'c').Replace('ş', 's').Replace('ğ', 'g').Replace('/', '-').Replace('.', '-').Replace("%", "").Replace("$", "").Replace("&", "").Replace('>', '-').Replace('<', '-').Replace('*', '-').Replace(':', '-').Replace("-----", "-").Replace("----", "-").Replace("---", "-").Replace("--", "-");
            return u;
        }
        public static string ToLinkText(this string url)
        {
            return ReplaceURL(url);
        }
        public static string ListToString(this IEnumerable<string> list)
        {
            if (list == null)
                return "";
            StringBuilder sb = new StringBuilder();
            foreach (var item in list)
            {
                sb.Append(item);
                sb.Append("-");
            }
            if (sb.Length > 0)
                sb.Remove(sb.Length - 1, 1);
            return sb.ToString();
        }

        public static Dictionary<string, object> CreateParameters(this object t, string name)
        {
            Dictionary<string, object> prm = new Dictionary<string, object>();
            prm.Add(name, t);
            return prm;

        }

        public static byte[] ToByte(this int number)
        {
            byte[] b = new byte[0];
            if (number == 0)
            {
                Array.Resize(ref b, b.Length + 1);
                b[b.Length - 1] = 0;
            }
            while (number > 0)
            {
                Array.Resize(ref b, b.Length + 1);
                if (number >= 255)
                {
                    b[b.Length - 1] = 255;
                    number = number / 255;
                }
                else
                {
                    b[b.Length - 1] = (byte)number;
                    number = 0;
                }

            }

            return b;
        }

        public static int? PrimaryKeyValue<Entity>(this Entity e) where Entity : class
        {
            var t = e.GetType();
            Table tbl = (Table)Table.GetCustomAttribute(t, typeof(Table));
            if (tbl != null)
            {
                var prop = t.GetProperty(tbl.PrimaryKey);
                if (prop != null)
                {
                    var val = prop.GetValue(e);
                    return (int)val;
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
        }

        public static DataTable ToDataTable<T>(this T data, string name = "")
        {
            DataTable dt = new DataTable();
            List<PropertyInfo> l = new List<PropertyInfo>();
            foreach (var item in typeof(T).GetProperties())
            {
                Type t = item.PropertyType;
                if (t.IsGenericType)
                {
                    t = t.GenericTypeArguments.FirstOrDefault();
                }
                dt.Columns.Add(item.Name, t);
                l.Add(item);
            }
            if (data != null)
            {
                object[] values = new object[l.Count];
                for (int i = 0; i < l.Count; i++)
                {
                    PropertyInfo p = l[i];
                    values[i] = p.GetValue(data);
                }
                dt.Rows.Add(values);
            }
            if (dt != null) dt.TableName = name;
            return dt;
        }

        /// <summary>
        /// Class tan DataTable üretir class kopyasından kaç adet alacağı rowcount ile verilir
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"> kimden üreteceğiz</param>
        /// <param name="rowCopyCount">kaç kopya yapıp koyacağız</param>
        /// <returns></returns>
        public static DataTable ToDataTable<T>(this T data, int rowCopyCount)
        {
            DataTable dt = data.ToDataTable();
            for (int i = 1; i <= rowCopyCount; i++)
            {
                object[] values = new object[dt.Columns.Count];
                Array.Copy(dt.Rows[0].ItemArray, values, values.Length);
                dt.Rows.Add(values);
            }


            return dt;
        }

        public static DataTable AddClass<T>(this DataTable dt, T data)
        {
            object[] values = new object[dt.Columns.Count];
            foreach (PropertyInfo p in typeof(T).GetProperties())
            {
                int index = dt.Columns.IndexOf(p.Name);
                if (index != -1 && index < values.Length)
                {
                    values[index] = p.GetValue(data);
                }
            }
            dt.Rows.Add(values);
            return dt;
        }

        public static string Kisalt(this string text, int harf = 10)
        {
            if (text == null)
            {
                return null;
            }
            if (text.Length > harf)
            {
                return text.Substring(0, harf) + "...";
            }
            else
            {
                return text;
            }
        }

        public static string ToSqlType(this Type t)
        {
            string res = "sql_variant";
            switch (t.Name)
            {
                case "Boolean": res = "bit"; break;
                case "Byte": res = "tinyint"; break;
                case "Int16": res = "smallint"; break;
                case "Int32": res = "int"; break;
                case "Int64": res = "bigint"; break;
                case "Single": res = "real"; break;
                case "Double": res = "float"; break;
                case "Decimal": res = "decimal(18,4)"; break;
                case "Char": res = "nchar(1)"; break;
                case "String": res = "nvarchar(MAX)"; break;
                case "DateTime": res = "datetime"; break;
                case "TimeSpan": res = "time(7)"; break;
                case "Guid": res = "uniqueidentifier"; break;

                //case "": res = ""; break;
                //case "": res = " "; break;
                default:
                    break;
            }
            return res;
        }

        public static MemoryStream ToMemoryStream(this string text)
        {
            MemoryStream ms = new MemoryStream();
            StreamWriter sw = new StreamWriter(ms);
            sw.Write(text);
            sw.Flush();
            ms.Position = 0;
            return ms;
        }
        public static string MemToString(this MemoryStream ms)
        {
            if (ms == null) return null;
            ms.Position = 0;
            StreamReader rdr = new StreamReader(ms);
            return rdr.ReadToEnd();
        }
        #region Convert
        public static short ToShort(this object value)
        {
            if (value is string && string.IsNullOrEmpty((string)value))
            {
                return 0;
            }
            if (value is short) return (short)value;
            return Convert.ToInt16(value);
        }
        public static ushort ToUShort(this object value)
        {
            if (value is string && string.IsNullOrEmpty((string)value))
            {
                return 0;
            }
            if (value is ushort) return (ushort)value;
            return Convert.ToUInt16(value);
        }
        public static byte ToByte(this object value)
        {
            if (value is string && string.IsNullOrEmpty((string)value))
            {
                return 0;
            }
            if (value is byte) return (byte)value;
            return Convert.ToByte(value);
        }
        public static sbyte ToSByte(this object value)
        {
            if (value is string && string.IsNullOrEmpty((string)value))
            {
                return 0;
            }
            if (value is sbyte) return (sbyte)value;
            return Convert.ToSByte(value);
        }
        public static int ToInt(this object value)
        {
            if (value is string && string.IsNullOrEmpty((string)value))
            {
                return 0;
            }
            if (value is int) return (int)value;
            return Convert.ToInt32(value);
        }
        public static uint ToUInt(this object value)
        {
            if (value is string && string.IsNullOrEmpty((string)value))
            {
                return 0;
            }
            if (value is uint) return (uint)value;
            return Convert.ToUInt32(value);
        }
        public static long ToLong(this object value)
        {
            if (value is string && string.IsNullOrEmpty((string)value))
            {
                return 0;
            }
            if (value is long) return (long)value;
            return Convert.ToInt64(value);
        }
        public static ulong ToULong(this object value)
        {
            if (value is string && string.IsNullOrEmpty((string)value))
            {
                return 0;
            }
            if (value is ulong) return (ulong)value;
            return Convert.ToUInt64(value);
        }
        public static float ToFloat(this object value)
        {
            if (value is string && string.IsNullOrEmpty((string)value))
            {
                return 0;
            }
            if (value is float) return (float)value;
            return Convert.ToSingle(value);
        }
        public static decimal ToDecimal(this object value)
        {
            if (value is string)
            {
                if (string.IsNullOrEmpty((string)value))
                    return 0;
                else
                {
                    //value = ((string)value).Replace(".", "");
                }
            }
            if (value is decimal) return (decimal)value;
            return Convert.ToDecimal(value);
        }
        public static double ToDouble(this object value)
        {
            if (value is string && string.IsNullOrEmpty((string)value))
            {
                return 0;
            }
            if (value is double) return (double)value;
            return Convert.ToDouble(value);
        }
        public static bool ToBool(this object value)
        {
            if (value is string && string.IsNullOrEmpty((string)value))
            {
                return false;
            }
            if (value is bool) return (bool)value;
            return Convert.ToBoolean(value);
        }
        public static Guid? ToGuid(this object value)
        {
            if (value is string && string.IsNullOrEmpty((string)value))
            {
                return null;
            }
            if (value is Guid) return (Guid)value;
            return Guid.Parse(value.ToString());
        }

        public static MemoryStream ToMemoryStream(this byte[] data)
        {
            MemoryStream m = new MemoryStream();
            m.Write(data, 0, data.Length);
            m.Position = 0;
            return m;
        }
        public static string GetString(this Stream fs)
        {
            using (StreamReader sr = new StreamReader(fs))
            {
                string text = sr.ReadToEnd();
                sr.Close();
                fs.Close();
                return text;

            }
        }
        public static decimal Yuvarla(this decimal sayi, int kusurat)
        {
            return Math.Round(sayi, kusurat);
        }
        public static string ToPara(this decimal tutar)
        {
            int hane = 2;
            var s = tutar.ToString("N" + hane);
            if (s.Substring(s.Length - (hane + 1), 1) == ".")
            {
                s = s.Replace(",", "");
            }
            else if (s.Substring(s.Length - (hane + 1), 1) == ",")
            {
                s = s.Replace(".", "").Replace(",", ".");
            }
            return s;
        }
        #endregion
    }
}

