
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using Microsoft.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Net.Http;

namespace AYAK.Common.NetCore
{


    public static class Tools
    {
        #region Properties

        private static bool? isLog = null;

        public static bool IsLog
        {
            get
            {
                if (!isLog.HasValue)
                {
                    string log = ConfigurationManager.AppSettings["IsLogging"];
                    if (!string.IsNullOrEmpty(log))
                    {
                        isLog = Convert.ToBoolean(log);
                    }
                    else
                    {
                        isLog = false;
                    }

                }
                isLog = true;
                return isLog.Value;
            }
            set { isLog = value; }
        }

        private static string logTable = null;

        public static string LogTable
        {
            get
            {
                if (string.IsNullOrEmpty(logTable))
                {
                    string lgt = ConfigurationManager.AppSettings["LogTableName"];
                    if (!string.IsNullOrEmpty(lgt))
                    {
                        logTable = lgt;
                    }
                    else
                    {
                        isLog = false;
                    }
                }
                else
                {
                    logTable = "Logs";
                }
                return logTable;
            }
            set { logTable = value; }
        }

        private static int? activeUserID;

        public static int ActiveUserID
        {
            get
            {
                if (!activeUserID.HasValue)
                {
                    //HttpContext context = HttpContext.Current;
                    //if (context != null && context.Session["ActiveUserID"] != null)
                    //{
                    //    activeUserID = (int)context.Session["ActiveUserID"];
                    //}

                    //else
                    //{
                    //    activeUserID = 0;
                    //}

                    if (!activeUserID.HasValue)
                    {
                        activeUserID = 0;
                    }
                }


                return activeUserID.Value;
            }
            set { activeUserID = value; }
        }

        public static string AppRoot
        {
            get
            {
                return Path.GetDirectoryName(AppPath);
            }
        }
        public static string AppPath
        {
            get
            {
                return Assembly.GetExecutingAssembly().GetName().FullName;
            }
        }
        public static string AppName
        {
            get
            {
                return Path.GetFileName(AppPath);
            }
        }

        #endregion
        static CacheCollection<object> _generalCacheContext;
        public static CacheCollection<object> GeneralCacheContext
        {
            get
            {
                //HttpContext ctx = HttpContext.Current;
                //if (ctx != null)
                //{
                //    string appName = "_GeneralDataCache_";
                //    ctx.Application[appName] = ctx.Application[appName] ?? new CacheCollection<object>();
                //    return (CacheCollection<Object>)ctx.Application[appName];
                //}
                //else
                {
                    _generalCacheContext = _generalCacheContext ?? new CacheCollection<object>();
                    return _generalCacheContext;
                }

            }
        }
        //public static HttpContext HttpContext {get;set;}
        public static Table GetTable<T>()
        {
            Table t = (Table)Table.GetCustomAttribute(typeof(T), typeof(Table));
            return t;
        }
        public static string CreateSelect<T>(string extension = "", string columnExtension = "")
        {
            Table t = (Table)Table.GetCustomAttribute(typeof(T), typeof(Table));

            StringBuilder sb = new StringBuilder();
            sb.Append("SELECT ");
            sb.Append(columnExtension);
            sb.Append(" * FROM ");
            sb.Append(string.Format("{0}.{1}", t.SchemaName, t.TableName));
            sb.Append(" ");
            sb.Append(extension);
            return sb.ToString();
        }

        static Tools()
        {
            _connections = new Dictionary<string, SqlConnection>();
            //Connections["DataBase"] = new SqlConnection(ConfigurationManager.ConnectionStrings["DataBase"].ConnectionString);
        }
        static Dictionary<string, SqlConnection> _connections;
        public static Action<string, bool> Messager;
        public static SqlConnection Connection
        {
            get
            {

                if (!_connections.ContainsKey("database"))
                    _connections["DataBase"] = new SqlConnection(ConfigurationManager.ConnectionStrings["DataBase"].ConnectionString);

                return Tools.Connections["DataBase"];

                //return new SqlConnection(MyAppSettings.DataBase);
             

            }
            set { Tools.Connections["DataBase"] = value; }
        }

        public static Dictionary<string, SqlConnection> Connections
        {
            get
            {
                _connections = _connections ?? new Dictionary<string, SqlConnection>();
                return _connections;
                //var c = new Dictionary<string, SqlConnection>();
                //c.Add("DataBase", new SqlConnection(MyAppSettings.DataBase));
                //c.Add("GENEL", new SqlConnection(MyAppSettings.GENEL));
                //return c;
            }
            set { _connections = value; }
        }
        private static Result Log(LogTypes logType, string query, Dictionary<string, object> parameters, string message)
        {
            if (IsLog && LogTable != null)
            {
                StringBuilder logprms = new StringBuilder();
                foreach (var item in parameters)
                {
                    logprms.Append(string.Format("{0}={1},", item.Key, item.Value));

                }

                string logQuery = string.Format("Insert {0}(LogType,IP,UserID,Date,Query,Parameters,Message) values(@l,@ip,@u,@d,@q,@p,@m)", LogTable);
                var logPrm = DateTime.Now.CreateParameters("@d");
                logPrm.Add("@q", query.Length < 442 ? query : query.Substring(0, 442));
                logPrm.Add("@u", ActiveUserID);
                logPrm.Add("@p", logprms.ToString());
                logPrm.Add("@m", message);
                logPrm.Add("@l", (int)logType);
                //if (HttpContext.Current != null)
                //{
                //    string ip = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
                //    if (!string.IsNullOrEmpty(ip))
                //    {
                //        logPrm.Add("@ip", ip);
                //    }
                //    else
                //    {
                //        logPrm.Add("@ip", "");
                //    }


                //}
                //else
                {
                    logPrm.Add("@ip", "");
                }
                try
                {
                    return ExecuteNonQuery(logQuery, logPrm, MustLog: false);
                }
                catch (Exception ex2)
                {
                    return new Result
                    {

                        Message = ex2.Message,
                        State = ResultState.Exception
                    };

                }

            }
            else
            {
                string l = ConfigurationManager.AppSettings["IsLogging"];
                return new Result { State = ResultState.Success, Message = "Loglama Kapalı IsLogging=" + l + " IsLog=" + IsLog + " LogtableName=" + LogTable };
            }
        }
        public static SqlConnection GetConnection(string name)
        {
            name = name.ToLower();
            if (Connections.ContainsKey(name.ToLower()))
            {
                return Connections[name.ToLower()];
            }
            else
            {
                if (ConfigurationManager.ConnectionStrings.Count > 0)
                {
                    bool valid = false;
                    foreach (ConnectionStringSettings item in ConfigurationManager.ConnectionStrings)
                    {
                        if (item.Name == name)
                        {
                            valid = true;
                            break;
                        }
                        else
                        {

                        }
                    }
                    if (valid)
                    {
                        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings[name].ConnectionString);
                        Connections.Add(name.ToLower(), con);
                        return con;
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
        }
        public static Result<DataTable> Select(string query, Dictionary<string, object> parameters = null, string connectionName = "DataBase", CommandType commandType = CommandType.Text)
        {
            try
            {
                var connection = GetConnection(connectionName);
                SqlDataAdapter adp = new SqlDataAdapter(query, connection);
                adp.SelectCommand.CommandType = commandType;
                adp.SelectCommand.CommandTimeout = 300;
                while (connection.State != ConnectionState.Closed) ;
                if (parameters != null)
                {
                    foreach (var item in parameters)
                    {
                        adp.SelectCommand.Parameters.AddWithValue(item.Key, item.Value);
                    }
                }
                DataTable dt = new DataTable();
                adp.Fill(dt);

                Result logRes = Log(LogTypes.Exception, query, parameters, "OK");

                return new Result<DataTable>
                {
                    Data = dt,
                    Message = "Kayıt Listeleme Başarılı Log:" + logRes.Message,
                    State = ResultState.Success
                };
            }
            catch (Exception ex)
            {
                Result logRes = Log(LogTypes.Exception, query, parameters, ex.GetType().Name + ":" + ex.Message);

                return new Result<DataTable>
                {
                    Data = null,
                    Message = "Kayıt Listeleme sırasında hata oluştu. " + ex.Message + " Log: " + logRes.Message,
                    State = ResultState.Exception
                };
            }

        }

        public static Result<List<T>> Select<T>(string query, Dictionary<string, object> parameters = null, string connectionName = "DataBase", CommandType commandType = CommandType.Text)
        {

            var qr = Select(query, parameters, connectionName, commandType);
            Result<List<T>> result = new Result<List<T>>();
            result.Data = qr.Data.ToList<T>();
            result.Message = qr.Message;
            result.State = qr.State;
            result.ExDetail = qr.ExDetail;

            return result;

        }

        public static Result ExecuteNonQuery(string query, Dictionary<string, object> parameters = null, CommandType commandType = CommandType.Text, string connectionName = "DataBase", bool MustLog = true)
        {
            SqlCommand cmd = new SqlCommand(query, GetConnection(connectionName));
            cmd.CommandType = commandType;
            try
            {

                if (parameters != null)
                {
                    foreach (var parameter in parameters)
                    {
                        if (parameter.Value == null)
                        {
                            cmd.Parameters.AddWithValue(parameter.Key, DBNull.Value);
                        }
                        else
                        {
                            cmd.Parameters.AddWithValue(parameter.Key, parameter.Value);
                        }
                    }
                }

                if (cmd.Connection.State != ConnectionState.Open)
                    cmd.Connection.Open();

                int rowcount = cmd.ExecuteNonQuery();
                Result logRes = new Result();
                if (MustLog)
                {
                    logRes = Log(LogTypes.Exception, query, parameters, "OK");
                }

                if (rowcount > 0)
                    return new Result
                    {
                        Data = rowcount,
                        State = ResultState.Success,
                        Message = "Log:" + logRes.Message
                    };
                else
                    return new Result
                    {
                        Data = 0,
                        State = ResultState.Success,
                        Message = "Log:" + logRes.Message
                    };

            }
            catch (Exception ex)
            {
                Result logRes = Log(LogTypes.Exception, query, parameters, ex.GetType().Name + ":" + ex.Message);

                return new Result
                {
                    Data = 0,
                    State = ResultState.Exception,
                    Message = ex.Message + " Log:" + logRes.Message
                };
            }
            finally
            {
                if (cmd.Connection.State != ConnectionState.Closed)
                    cmd.Connection.Close();
            }

        }

        public static Result ExecuteNonQuery(SqlCommand cmd)
        {

            try
            {

                if (cmd.Connection.State != ConnectionState.Open)
                    cmd.Connection.Open();

                int rowcount = cmd.ExecuteNonQuery();
                Result logRes = new Result();

                if (rowcount > 0)
                    return new Result
                    {
                        Data = rowcount,
                        State = ResultState.Success,
                        Message = "Log:" + logRes.Message
                    };
                else
                    return new Result
                    {
                        Data = 0,
                        State = ResultState.Success,
                        Message = "Log:" + logRes.Message
                    };

            }
            catch (Exception ex)
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>();
                foreach (SqlParameter item in cmd.Parameters)
                {
                    parameters.Add(item.ParameterName, item.Value);
                }
                Result logRes = Log(LogTypes.Exception, cmd.CommandText, parameters, ex.GetType().Name + ":" + ex.Message);

                return new Result
                {
                    Data = 0,
                    State = ResultState.Exception,
                    Message = ex.Message + " Log:" + logRes.Message
                };
            }
            finally
            {
                if (cmd.Connection.State != ConnectionState.Closed)
                    cmd.Connection.Close();
            }
        }

        public static Result ExecuteScalar(string query, Dictionary<string, object> parameters = null, CommandType commandType = CommandType.Text, string connectionName = "DataBase")
        {
            SqlCommand cmd = new SqlCommand(query, GetConnection(connectionName));
            cmd.CommandType = commandType;
            try
            {

                if (parameters != null)
                {
                    foreach (var parameter in parameters)
                    {
                        cmd.Parameters.AddWithValue(parameter.Key, parameter.Value);
                    }
                }
                if (cmd.Connection.State != ConnectionState.Open)
                    cmd.Connection.Open();

                object data = cmd.ExecuteScalar();
                if (query.Contains("Scope_Identity()"))
                {
                    return new Result
                    {
                        Data = data,
                        State = ResultState.Success
                    };
                }

                Result logRes = Log(LogTypes.Scalar, query, parameters, "OK");

                if (!(data is DBNull) && data != null)
                    return new Result
                    {
                        Data = data,
                        State = ResultState.Success,
                        Message = "Log : " + logRes.Message

                    };
                else
                    return new Result
                    {
                        Data = null,
                        State = ResultState.Success,
                        Message = ""
                    };

            }
            catch (Exception ex)
            {
                Result logRes = Log(LogTypes.Exception, query, parameters, ex.GetType().Name + ":" + ex.Message);
                return new Result
                {
                    Data = 0,
                    State = ResultState.Exception,
                    Message = ex.Message + " Log : " + logRes.Message
                };
            }
            finally
            {
                if (cmd.Connection.State != ConnectionState.Closed)
                    cmd.Connection.Close();
            }

        }

        public static Result<List<T>> View<T>()
        {
            var query = CreateSelect<T>();
            var result = Select(query, null, GetTable<T>().ConnectionName, CommandType.Text);
            return new Result<List<T>>
            {
                Data = result.State == ResultState.Success ? result.Data.ToList<T>() : null,
                Message = result.Message,
                State = result.State
            };
        }
        public static Result<List<T>> View<T>(object id)
        {
            Table t = GetTable<T>();
            Dictionary<string, object> p = new Dictionary<string, object>();
            p.Add("@" + t.PrimaryKey, id);
            var result = Select(string.Format("WHERE {0}=@{0}", t.PrimaryKey), p, t.ConnectionName, CommandType.Text);
            return new Result<List<T>>
            {
                Data = result.State == ResultState.Success ? result.Data.ToList<T>() : null,
                Message = result.Message,
                State = result.State
            };
        }
        public static Result<List<T>> View<T>(string query, Dictionary<string, object> p)
        {
            Table t = GetTable<T>();
            string q = CreateSelect<T>(query);
            var result = Select(q, p, t.ConnectionName, CommandType.Text);
            return new Result<List<T>>
            {
                Data = result.State == ResultState.Success ? result.Data.ToList<T>() : null,
                Message = result.Message,
                State = result.State
            };
        }

        public static string CreateHash(string data, string key)
        {

            HMACMD5 md = new HMACMD5(Encoding.UTF8.GetBytes(key));

            byte[] keys = Encoding.UTF8.GetBytes(data.ToString());
            byte[] hash = md.ComputeHash(keys);
            string result = Encoding.UTF8.GetString(hash);
            string res = "";
            foreach (var item in hash)
            {
                res += item.ToString("x2");
            }
            result = res;
            return result;
        }

        public static Result BackupDatabase(string yedekKonum, string databaseName = "DataBase")
        {
            string dbName = new SqlConnectionStringBuilder(Connection?.ConnectionString)?.InitialCatalog;

            string yedekAdi = string.Format("{0}_yedek_{1}.bak", dbName, DateTime.Now.ToString("yyyy_MM_dd_HH_mm_ss"));
            //string yedekKonum = string.Format("{0}\\{1}", baseBackupDir, yedekAdi);
            yedekKonum += yedekAdi;
            string query = $"BACKUP DATABASE [{dbName}] TO  DISK = N'{yedekKonum}' WITH NOFORMAT, NOINIT,  NAME = N'{dbName}-Full Database Backup', SKIP, NOREWIND, NOUNLOAD,  STATS = 10";

            var connection = (Connections[databaseName]);
            SqlConnectionStringBuilder conBuilder = new SqlConnectionStringBuilder(Connection.ConnectionString);
            conBuilder.InitialCatalog = "master";
            conBuilder.ConnectTimeout = 900;
            SqlConnection backupCon = new SqlConnection(conBuilder.ToString());
            SqlCommand backupCommand = new SqlCommand(query, backupCon);
            backupCommand.CommandTimeout = 900;

            var res = ExecuteNonQuery(backupCommand);
            return res;
        }

        //public static string GetServerBackupDir
        //{
        //    get
        //    {
        //        SqlCommand cmd = new SqlCommand(@"master.dbo.xp_instance_regread 
        //N'HKEY_LOCAL_MACHINE', 
        //N'Software\Microsoft\MSSQLServer\MSSQLServer',N'BackupDirectory', 
        //@path OUTPUT,  
        //'no_output' ", Connection);
        //        SqlParameter path = new SqlParameter
        //        {
        //            ParameterName = "@path",
        //            Direction = ParameterDirection.Output
        //        };
        //        cmd.Parameters.Add(path);

        //        var res=ExecuteNonQuery(cmd);

        //        return (string)path.Value;


        //    }
        //}
        public static bool TableCheck(string tablename, string connectionName = "DataBase")
        {

            var res = ExecuteScalar("select count(*) from sys.tables where name =@n", tablename.CreateParameters("@n"), CommandType.Text, connectionName);
            if (res.State == ResultState.Success)
            {
                int adt = res.Data.ToInt();
                return adt == 1;
            }
            else
            {
                throw new Exception(res.Message);
            }


        }

        public static bool TableCheck<T>()
        {
            Table t = GetTable<T>();
            return TableCheck(t.TableName, t.ConnectionName);
        }

        public static void CreateTable<T>()
        {
            string nullableTypes = "String,Object";
            Table t = GetTable<T>();
            StringBuilder sb = new StringBuilder($"CREATE TABLE {t.SchemaName}.{t.TableName}(");
            foreach (PropertyInfo col in typeof(T).GetProperties())
            {
                var ignore = Attribute.GetCustomAttribute(col, typeof(IgnoreCRUD));
                if (ignore != null) continue;

                sb.Append($"{col.Name} ");
                string coltype = "";
                bool nullable = false;
                if (col.PropertyType.Name.Contains("Nullable"))
                {
                    var gtyp = col.PropertyType.GenericTypeArguments.FirstOrDefault();
                    if (gtyp.IsEnum)
                        coltype = "int";
                    else
                        coltype = gtyp.ToSqlType();
                    nullable = true;
                }
                else if (col.PropertyType.IsEnum)
                {
                    coltype = "int";
                }
                else if (col.PropertyType.Name.ToLower().Contains("string"))
                {
                    coltype = col.PropertyType.ToSqlType();
                    nullable = true;
                }
                else
                {
                    coltype = col.PropertyType.ToSqlType();
                    nullable = false;
                }

                if (t.TableType == TableType.CompositTable)
                {
                    if (t.CompositeKeys.Contains(col.Name))
                    {
                        nullable = false;
                        coltype = coltype.Replace("(MAX)", "(400)");
                    }
                }
                else if (t.TableType == TableType.PrimaryTable && t.PrimaryKey == col.Name)
                {
                    nullable = false;
                    coltype = coltype.Replace("(MAX)", "(400)");
                }

                sb.Append(coltype);


                if (nullable)
                    sb.Append(" NULL");
                else
                    sb.Append(" NOT NULL");

                if (t.TableType == TableType.PrimaryTable && t.PrimaryKey.ToLower() == col.Name.ToLower())
                {

                    sb.Append(" Primary Key");
                }

                if (t.IdentityColumn.ToLower() == col.Name.ToLower())
                    sb.Append(" Identity(1,1)");
                sb.Append(",");
            }
            sb.Remove(sb.Length - 1, 1);
            sb.Append(")");

            var res = ExecuteNonQuery(sb.ToString(), connectionName: t.ConnectionName);
            if (res.State == ResultState.Success)
            {
                if (t.TableType == TableType.CompositTable && t.CompositeKeys != null)
                {
                    StringBuilder csb = new StringBuilder($"ALTER TABLE {t.SchemaName}.{t.TableName} ADD CONSTRAINT PK_{t.TableName}_Oto PRIMARY KEY CLUSTERED (");
                    foreach (var item in t.CompositeKeys)
                    {
                        csb.Append($"{item},");
                    }
                    csb.Remove(csb.Length - 1, 1);
                    csb.Append(") WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]");
                    var compoRes = ExecuteNonQuery(csb.ToString(), connectionName: t.ConnectionName);
                }
            }
        }

        public static bool BenimMakina()
        {
            string[] benimMakinalar = new string[] { "DESKTOP-VIMETFE" };
            return benimMakinalar.Contains(Dns.GetHostName());
        }

        public static string SecretKey
        {
            get
            {
                string k = ConfigurationManager.AppSettings["secretKey"];
                return k;
            }
        }

        public static string YaziYap(int sayi)
        {
            if (sayi == 0)
            {
                return "Sıfır";
            }
            string metinSayi = sayi.ToString();
            string metin = YaziYap(metinSayi);
            return metin;
        }

        private static string YaziYap(string metinSayi)
        {
            string[] birler = { "", "Bir", "İki", "Üç", "Dört", "Beş", "Altı", "Yedi", "Sekiz", "Dokuz" };
            string[] onlar = { "", "On", "Yirmi", "Otuz", "Kırk", "Elli", "Altmış", "Yetmiş", "Seksen", "Doksan" };
            string metin = "";
            for (int i = 0; i < metinSayi.Length; i++)
            {
                int hane = metinSayi.Length - i;
                int rakam = Convert.ToInt32(metinSayi.Substring(i, 1));
                metin += hane == 2 ? onlar[rakam] : birler[rakam] + (hane == 3 ? "Yüz" : "") + (hane == 4 ? "Bin" : "") + (hane == 5 ? "Milyon" : "") + (hane == 6 ? "Milyar" : "");
                metin = metin.Replace("BirYüz", "Yüz").Replace("BirBin", "Bin");

            }

            return metin;
        }

        public static string YaziYap(decimal tutar)
        {
            if (tutar == 0)
            {
                return "SıfırTL";
            }
            string[] birler = { "", "Bir", "İki", "Üç", "Dört", "Beş", "Altı", "Yedi", "Sekiz", "Dokuz" };
            string[] onlar = { "", "On", "Yirmi", "Otuz", "Kırk", "Elli", "Altmış", "Yetmiş", "Seksen", "Doksan" };
            string tutarSayi = tutar.ToString("N2");
            string[] kisimlar = tutarSayi.Split(',');
            string tamKisim = kisimlar[0];
            string ondaKisim = kisimlar[1];
            int tamSayi = tamKisim.ToInt();
            int ondaSayi = ondaKisim.ToInt();

            string tamMetin = YaziYap(tamSayi);
            string ondaMetin = YaziYap(ondaSayi);
            string sonuc = "";
            if (tamSayi != 0)
            {
                sonuc += $"{tamMetin}TL";
            }
            if (ondaSayi != 0)
            {
                sonuc += $"{ondaMetin}KRŞ";
            }
            return sonuc;
        }
        public static byte[] GetBytes(this Stream stream)
        {
            byte[] buffer = new byte[stream.Length];
            stream.Read(buffer, 0, buffer.Length);
            return buffer;
        }
        public static string GetFileHash(this string filePath)
        {
            using (var file = File.Open(filePath, FileMode.Open))
            {
                var buf = file.GetBytes();
                return buf.GetHash();
            }
        }
        public static string GetHash(this Stream stream)
        {
            var buf = stream.GetBytes();
            return buf.GetHash();
        }
        public static string GetHash(this byte[] array)
        {
            MD5 md = MD5.Create();
            var buffer = md.ComputeHash(array);
            return buffer.ToHex();
        }
        public static string ToHex(this byte[] array)
        {
            StringBuilder sb = new StringBuilder();
            foreach (var item in array)
            {
                sb.Append(item.ToString("x2"));
            }

            return sb.ToString();
        }

        public static Result Dene(DeneHandler action)
        {
            return Dene(action, 5);
        }
        public static Result Dene(DeneHandler action, int kere = 5)
        {
            Result r = new Result();
            int s = 0;
            do
            {
                try
                {
                    bool? b = action?.Invoke();
                    if (b != null)
                    {
                        if (b.Value)
                        {
                            r.State = ResultState.Success;
                            break;
                        }
                    }
                    else
                    {
                        r.Message = "Action is null";
                        r.State = ResultState.Exception;
                    }
                }
                catch (Exception ex)
                {
                    r.State = ResultState.Exception;
                    r.Message = ex.Message;

                }
                s++;
            } while (s < kere);

            return r;
        }
        public static void Asenkron(Action action)
        {
            action.BeginInvoke((a) => { }, null);
        }
        public static void ArkadaDene(DeneHandler action, Action<Result> cevap, int kere = 5)
        {
            if (kere <= 0) return;
            try
            {
                action?.BeginInvoke((a) =>
                {
                    if (a.IsCompleted)
                    {
                        cevap?.Invoke(new Result
                        {
                            State = a.IsCompleted ? ResultState.Success : ResultState.Exception
                        });
                    }
                    else
                    {
                        ArkadaDene(action, cevap, kere - 1);
                    }

                }, null);

            }
            catch (Exception ex)
            {

            }
        }

        public static string SayidanYaziya(this decimal sayii, bool para = false)
        {
            string sayi = sayii.ToString();
            sayi = sayi.Replace('.', ',');
            if (!sayi.Contains(",")) sayi += ",";
            var rakamlar = sayi.Split(',');
            rakamlar[0] = "000000000000" + rakamlar[0].Replace(",", "");
            if (rakamlar.Count() > 1)
                if (rakamlar[1].Length == 1) rakamlar[1] += "0";
                else if (rakamlar[1].Length > 2) rakamlar[1] = rakamlar[1].Substring(rakamlar[1].Length - 2);

            rakamlar[0] = rakamlar[0].Substring(rakamlar[0].Length - 12);

            var yuzler = new[] { "", "yüz", "ikiyüz", "üçyüz", "dörtyüz", "beşyüz", "altıyüz", "yediyüz", "sekizyüz", "dokuzyüz" };
            var onlar = new string[] { "", "on", "yirmi", "otuz", "kırk", "elli", "altmış", "yetmiş", "seksen", "doksan" };
            var birler = new string[] { "", "bir", "iki", "üç", "dört", "beş", "altı", "yedi", "sekiz", "dokuz" };

            var milyarlar = rakamlar[0].Substring(0, 3);
            var milyonlar = rakamlar[0].Substring(3, 3);
            var binler = rakamlar[0].Substring(6, 3);
            var sonuc = rakamlar[0].Substring(9, 3);

            var milyari = yuzler[milyarlar.Substring(0, 1).ToInt()];
            milyari += onlar[milyarlar.Substring(1, 1).ToInt()];
            milyari += birler[milyarlar.Substring(2, 1).ToInt()];

            var milyonu = yuzler[milyonlar.Substring(0, 1).ToInt()];
            milyonu += onlar[milyonlar.Substring(1, 1).ToInt()];
            milyonu += birler[milyonlar.Substring(2, 1).ToInt()];

            var bini = yuzler[binler.Substring(0, 1).ToInt()];
            bini += onlar[binler.Substring(1, 1).ToInt()];
            bini += birler[binler.Substring(2, 1).ToInt()];

            var biri = yuzler[sonuc.Substring(0, 1).ToInt()];
            biri += onlar[sonuc.Substring(1, 1).ToInt()];
            biri += birler[sonuc.Substring(2, 1).ToInt()];

            var yollanacakyazi = "";
            if (milyari.Length > 0) yollanacakyazi += milyari + "milyar";
            if (milyonu.Length > 0) yollanacakyazi += milyonu + "milyon";
            if (bini.Length > 0)
            {
                if (bini == "bir") bini = "";
                yollanacakyazi += bini + "bin";
            }
            if (biri.Length > 0) yollanacakyazi += biri;

            var kurus = "";
            if (rakamlar.Count() > 1)
            {
                if (rakamlar[1].Length > 0) kurus += onlar[rakamlar[1].Substring(0, 1).ToInt()];
                if (rakamlar[1].Length > 1) kurus += birler[rakamlar[1].Substring(1, 1).ToInt()];
            }
            if (para)
            {
                yollanacakyazi += " TL.";
                if (kurus.Length > 0) kurus += " KR.";
            }
            else
            {

                if (rakamlar[1].Length > 0)
                    if (rakamlar[1].ToInt() <= 15)
                        kurus = "";
                    else if (rakamlar[1].ToInt() <= 35)
                        kurus = "ÇEYREK";
                    else if (rakamlar[1].ToInt() <= 65)
                        if (yollanacakyazi.Length == 0)
                        {
                            kurus = "YARIM";
                        }
                        else
                        {
                            kurus = "BUÇUK";
                        }
                    else if (rakamlar[1].ToInt() <= 85)
                        kurus = "ÜÇÇEYREK";

                //if (kurus.Length > 0) yollanacakyazi += "_";
            }

            if (!String.IsNullOrEmpty(yollanacakyazi))
                yollanacakyazi = yollanacakyazi.First().ToString().ToUpper() + String.Join("", yollanacakyazi.Skip(1));
            //throw new ArgumentException("ARGH!");
            if (!String.IsNullOrEmpty(kurus))
                kurus = kurus.First().ToString().ToUpper() + String.Join("", kurus.Skip(1));

            return yollanacakyazi + kurus;
        }

    }
    public delegate bool DeneHandler();
    public enum LogTypes
    {
        Select = 1,
        NonQuery,
        Scalar,
        Exception
    }

}
