
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace AYAK.Guncelleyici
{


    public static class GuncellemeTools
    {
        #region Properties

       

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

        public static bool BenimMakina()
        {
            string[] benimMakinalar = new string[] { "DESKTOP-VIMETFE" };
            return benimMakinalar.Contains(Dns.GetHostName());
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
