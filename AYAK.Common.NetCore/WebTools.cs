using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace AYAK.Common.NetCore
{
    public class WebTools
    {
        public static string HtmlDuzelt(string html)
        {
            List<Tuple<string, string>> list = new List<Tuple<string, string>>() {
                new Tuple<string, string>("&ouml","ö"),
                new Tuple<string, string>("&Ouml;","Ö"),
                new Tuple<string, string>("&uuml;","ü"),
                new Tuple<string, string>("&Uuml;","Ü"),
                new Tuple<string, string>("&#305;","ı"),
                new Tuple<string, string>("&#304;","İ"),
                new Tuple<string, string>("&#350;","Ş"),
                new Tuple<string, string>("&#351","ş"),
                new Tuple<string, string>("&#199;","Ç"),
                new Tuple<string, string>("&#231;","ç"),
                new Tuple<string, string>("&#286;","Ğ"),
                new Tuple<string, string>("&#287;","ğ"),
                new Tuple<string, string>("&#214;","Ö"),
                new Tuple<string, string>("&#246;","ö"),
                new Tuple<string, string>("&#220;","Ü"),
                new Tuple<string, string>("&#252;","ü"),
                new Tuple<string, string>("&#160;"," "),
                new Tuple<string, string>("&apos;","'"),
                new Tuple<string, string>("&quot;","\""),
                new Tuple<string, string>("&amp;","&"),
                new Tuple<string, string>("&lt;","<"),
                new Tuple<string, string>("&gt;",">"),
                new Tuple<string, string>("&ccedil;","ç"),
            };


            list.ForEach(x => html = html.Replace(x.Item1, x.Item2));

            return html;
        }

        public static string SayfaCek(string adres)
        {
            try
            {
                HttpWebRequest request33 = WebRequest.Create(adres) as HttpWebRequest;

                request33.KeepAlive = false;

                //request33.Timeout = 3000;

                HttpWebResponse response33 = request33.GetResponse() as HttpWebResponse;
                string cs = response33.CharacterSet;
                if (response33 == null) return "Çözümlenmeyen cevapsız çağrı";

                Stream responseStream = response33.GetResponseStream();
                StreamReader reader = new StreamReader(responseStream, Encoding.UTF8);
                string r = reader.ReadToEnd();
                return HtmlDuzelt(r);
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        public static string POSTJson(string adres, string json)
        {

            try
            {
                HttpWebRequest request33 = WebRequest.Create(adres) as HttpWebRequest;

                request33.Method = "POST";
                request33.KeepAlive = false;
                request33.ContentType = "application/json; charset=utf-8";
                //request33.Timeout = 30000;

                Stream sr = request33.GetRequestStream();
                byte[] baytdizisi33 = Encoding.UTF8.GetBytes(json);
                sr.Write(baytdizisi33, 0, baytdizisi33.Length);

                HttpWebResponse response33 = request33.GetResponse() as HttpWebResponse;
                if (response33 == null)
                    return "Çözümlenmeyen cevapsız çağrı";
                //string cs = response33.CharacterSet;
                //string en = response33.ContentEncoding;
                Stream responseStream = response33.GetResponseStream();
                StreamReader reader = new StreamReader(responseStream, Encoding.UTF8);
                string r = reader.ReadToEnd();
                return HtmlDuzelt(r);
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        public static string POSTSayfa(string adres, string postData)
        {

            try
            {
                HttpWebRequest request33 = WebRequest.Create(adres) as HttpWebRequest;

                request33.Method = "POST";
                request33.KeepAlive = false;
                request33.ContentType = "application/x-www-form-urlencoded";
                //request33.Timeout = 30000;

                Stream sr = request33.GetRequestStream();
                byte[] baytdizisi33 = Encoding.UTF8.GetBytes(postData);
                sr.Write(baytdizisi33, 0, baytdizisi33.Length);

                HttpWebResponse response33 = request33.GetResponse() as HttpWebResponse;
                if (response33 == null)
                    return "Çözümlenmeyen cevapsız çağrı";
                //string cs = response33.CharacterSet;
                //string en = response33.ContentEncoding;
                Stream responseStream = response33.GetResponseStream();
                StreamReader reader = new StreamReader(responseStream, Encoding.UTF8);
                string r = reader.ReadToEnd();
                return HtmlDuzelt(r);
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        public static string POSTSayfa(string adres, string postData, Encoding encoding)
        {

            try
            {
                HttpWebRequest request33 = WebRequest.Create(adres) as HttpWebRequest;

                request33.Method = "POST";
                request33.KeepAlive = false;
                request33.ContentType = "application/x-www-form-urlencoded";
                //request33.Timeout = 30000;

                Stream sr = request33.GetRequestStream();
                byte[] baytdizisi33 = encoding.GetBytes(postData);
                sr.Write(baytdizisi33, 0, baytdizisi33.Length);

                HttpWebResponse response33 = request33.GetResponse() as HttpWebResponse;
                if (response33 == null)
                    return "Çözümlenmeyen cevapsız çağrı";
                //string cs = response33.CharacterSet;
                //string en = response33.ContentEncoding;
                Stream responseStream = response33.GetResponseStream();
                StreamReader reader = new StreamReader(responseStream, encoding);
                string r = reader.ReadToEnd();
                return HtmlDuzelt(r);
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        public static string POSTXML(string adres, string postxml)
        {
            try
            {
                HttpWebRequest request33 = WebRequest.Create(adres) as HttpWebRequest;

                request33.Method = "POST";
                request33.KeepAlive = false;
                request33.ContentType = "text/xml;charset=utf-8";
                //request33.Timeout = 30000;
                request33.Accept = "*/*";

                Stream sr = request33.GetRequestStream();
                byte[] baytdizisi33 = Encoding.UTF8.GetBytes(postxml);
                sr.Write(baytdizisi33, 0, baytdizisi33.Length);

                HttpWebResponse response33 = request33.GetResponse() as HttpWebResponse;
                if (response33 == null)
                    return "Çözümlenmeyen cevapsız çağrı";
                //string cs = response33.CharacterSet;
                //string en = response33.ContentEncoding;
                Stream responseStream = response33.GetResponseStream();
                StreamReader reader = new StreamReader(responseStream, Encoding.UTF8);
                string r = reader.ReadToEnd();
                return HtmlDuzelt(r);
            }
            catch (Exception ex)
            {
                return ex.Message;
            }

        }

        public static string SayfaCek(string adres, CookieContainer cookie)
        {
            try
            {
                HttpWebRequest request33 = WebRequest.Create(adres) as HttpWebRequest;
                request33.CookieContainer = cookie;

                request33.KeepAlive = false;

                //request33.Timeout = 3000;

                HttpWebResponse response33 = request33.GetResponse() as HttpWebResponse;
                string cs = response33.CharacterSet;
                if (response33 == null) return "Çözümlenmeyen cevapsız çağrı";

                Stream responseStream = response33.GetResponseStream();
                StreamReader reader = new StreamReader(responseStream, Encoding.UTF8);
                string r = reader.ReadToEnd();
                return HtmlDuzelt(r);
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        public static string POSTJson(string adres, CookieContainer cookie, string json)
        {

            try
            {
                HttpWebRequest request33 = WebRequest.Create(adres) as HttpWebRequest;
                request33.CookieContainer = cookie;
                request33.Method = "POST";
                request33.KeepAlive = false;
                request33.ContentType = "application/json; charset=utf-8";
                //request33.Timeout = 30000;

                Stream sr = request33.GetRequestStream();
                byte[] baytdizisi33 = Encoding.UTF8.GetBytes(json);
                sr.Write(baytdizisi33, 0, baytdizisi33.Length);

                HttpWebResponse response33 = request33.GetResponse() as HttpWebResponse;
                if (response33 == null)
                    return "Çözümlenmeyen cevapsız çağrı";
                //string cs = response33.CharacterSet;
                //string en = response33.ContentEncoding;
                Stream responseStream = response33.GetResponseStream();
                StreamReader reader = new StreamReader(responseStream, Encoding.UTF8);
                string r = reader.ReadToEnd();
                return HtmlDuzelt(r);
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        public static string POSTSayfa(string adres, CookieContainer cookie, string postData)
        {

            try
            {
                HttpWebRequest request33 = WebRequest.Create(adres) as HttpWebRequest;
                request33.CookieContainer = cookie;
                request33.Method = "POST";
                request33.KeepAlive = false;
                request33.ContentType = "application/x-www-form-urlencoded";
                //request33.Timeout = 30000;

                Stream sr = request33.GetRequestStream();
                byte[] baytdizisi33 = Encoding.UTF8.GetBytes(postData);
                sr.Write(baytdizisi33, 0, baytdizisi33.Length);

                HttpWebResponse response33 = request33.GetResponse() as HttpWebResponse;
                if (response33 == null)
                    return "Çözümlenmeyen cevapsız çağrı";
                //string cs = response33.CharacterSet;
                //string en = response33.ContentEncoding;
                Stream responseStream = response33.GetResponseStream();
                Encoding enc = Encoding.GetEncoding("windows-1254");
                StreamReader reader = new StreamReader(responseStream, enc);
                string r = reader.ReadToEnd();
                return HtmlDuzelt(r);
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        public static string POSTXML(string adres, CookieContainer cookie, string postxml)
        {
            try
            {
                HttpWebRequest request33 = WebRequest.Create(adres) as HttpWebRequest;
                request33.CookieContainer = cookie;
                request33.Method = "POST";
                request33.KeepAlive = false;
                request33.ContentType = "text/xml;charset=utf-8";
                //request33.Timeout = 30000;

                Stream sr = request33.GetRequestStream();
                byte[] baytdizisi33 = Encoding.UTF8.GetBytes(postxml);
                sr.Write(baytdizisi33, 0, baytdizisi33.Length);

                HttpWebResponse response33 = request33.GetResponse() as HttpWebResponse;
                if (response33 == null)
                    return "Çözümlenmeyen cevapsız çağrı";
                //string cs = response33.CharacterSet;
                //string en = response33.ContentEncoding;
                Stream responseStream = response33.GetResponseStream();
                StreamReader reader = new StreamReader(responseStream, Encoding.UTF8);
                string r = reader.ReadToEnd();
                return HtmlDuzelt(r);
            }
            catch (Exception ex)
            {
                return ex.Message;
            }

        }

        public static bool DosyaCek(string adres, string kayitYeri)
        {
            bool sonuc = false;
            using (WebClient wc = new WebClient())
            {
                string dir = Path.GetDirectoryName(kayitYeri);
                if (!Directory.Exists(dir))
                    Directory.CreateDirectory(dir);
                try
                {
                    byte[] data = wc.DownloadData(adres);
                    using (FileStream fs = new FileStream(kayitYeri, FileMode.OpenOrCreate))
                    {
                        fs.Write(data, 0, data.Length);
                        fs.Close();
                    }
                    sonuc = true;
                }
                catch (Exception e)
                {
                    sonuc = false;
                }
            }
            return sonuc;
        }
        public static byte[] DosyaCek(string adres)
        {

            byte[] sonuc = null;
            using (WebClient wc = new WebClient())
            {

                try
                {
                    sonuc = wc.DownloadData(adres);

                }
                catch (Exception e)
                {

                }
            }
            return sonuc;
        }


    }
}
