
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;


namespace AYAK.Guncelleyici
{
    public class GuncelleyiciProvider
    {
        static string server = "http://servis.ayasak.com";
        static string server1 = "http://localhost:45794";
        public static string ProgramAdi { get; set; }
        static string apiURL = $"{server}/api/updateservice";
        static string fileURL = $"{server}/Home/GetFile";

        static MD5 md5 = MD5.Create();

        public static string AktifVers
        {
            get
            {
                return Assembly.GetExecutingAssembly().GetName().Version.ToString();
            }
        }

        public static void BatSil()
        {
            if (BatVarmi)
            {
                File.Delete(BatPath);
            }
        }

        public static string Root
        {
            get
            {
                return Directory.GetCurrentDirectory() ;
            }
        }
        public static Action<string> Messager;
        public static Action<bool, string> Log;
        public static bool GuncellemeVar { get; set; }
        static JsonSerializer ser = new JsonSerializer();
        public static Result CheckUpdate()
        {
            Log?.Invoke(true, "Güncelleme Kontrol Ediliyor");
            var prms = $"Program={ProgramAdi}&Tarih={DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}&AktifVers={AktifVers}";
            var result = WebTools.POSTSayfa($"{apiURL}/DosyaDenetle?{prms}", string.Empty);
            if (!result.StartsWith("{")&&(result.Contains("(404)") || result.Contains("(500)") || result.Contains("403")))
            {
                Log?.Invoke(true, "Güncelleme Datası Alınamadı :" + result);
                return new Result { Data=false, State = ResultState.Exception, Message = "Sunucu Bilgi vermedi" };
            }
            Log?.Invoke(true, "Güncelleme Datası Alındı");
            var klasorRes = ser.Deserialize<Klasor>(new JsonTextReader(new StringReader(result)));

            if (!Directory.Exists($"{Root}\\Guncelleme"))
                Directory.CreateDirectory($"{Root}\\Guncelleme");

            KlasorIsle("", klasorRes);
            if (GuncellemeVar)
            {
                Log?.Invoke(true, "Güncelleme Var");

                return new Result { Data = true, State = ResultState.Success, Message = "Güncelleme Var" };
            }
            else
            {
                Log?.Invoke(true, "Güncelleme Yok");

                return new Result { Data = false, State = ResultState.Success, Message = "Güncelleme Yok" };

            }

        }
        public static Result GetUpdates()
        {
            var prms = $"Program={ProgramAdi}&Tarih={DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}&AktifVers={AktifVers}";
            var result = WebTools.POSTSayfa($"{apiURL}/DosyaDenetle?{prms}", string.Empty);
            if (!result.StartsWith("{") && (result.Contains("(404)") || result.Contains("(500)") || result.Contains("403")))
            {
                Log?.Invoke(true, "Güncelleme Datası Alınamadı :" + result);
                return new Result { State = ResultState.Exception, Message = "Sunucu Bilgi vermedi" };
            }
            Log?.Invoke(true, "Güncelleme Datası Alındı");
            var klasorRes = ser.Deserialize<Klasor>(new JsonTextReader(new StringReader(result)));

            if (!Directory.Exists($"{Root}\\Guncelleme"))
                Directory.CreateDirectory($"{Root}\\Guncelleme");

            KlasorIsle("", klasorRes);

            DosyaDegistir("", klasorRes);

            if (GuncellemeVar)
            {
                return new Result { State = ResultState.Success, Message = "Güncelleme Var" };
            }
            else
            {
                return new Result { State = ResultState.Success, Message = "Güncelsiniz" };
            }
        }

        private static void DosyaDegistir(string root, Klasor klasorRes)
        {
            Log?.Invoke(true, $"Dosyalar Değiştiriliyor");

            if (!Directory.Exists($"{Root}\\Guncelleme"))
            {
                try
                {
                    Directory.CreateDirectory($"{Root}\\Guncelleme");
                    Log?.Invoke(true, $"Güncelleme Klasörü Oluşturuldu");
                }
                catch (Exception ex)
                {
                    Log?.Invoke(true, $"Güncelleme Klasörü Oluşturulamadı :{ex.Message}");

                }
            }
            foreach (var dosya in klasorRes.Dosyalar.Where(x => x.Adi.Contains(".exe")))
            {
                string path = $"{Root}\\{root}\\{dosya.Adi}";
                path = path.Replace("\\\\", "\\");
                string tempPath = $"{Root}\\Guncelleme\\{root}\\{dosya.Adi}";
                tempPath = tempPath.Replace("\\\\", "\\");
                if (File.Exists(tempPath))
                {

                    if (File.Exists(path))
                    {
                        Log?.Invoke(true, $"Dosya Var :{path}");
                        int silsayac = 0;
                        do
                        {


                            try
                            {
                                silsayac++;
                                try
                                {
                                    File.Delete(path);
                                    Log?.Invoke(true, $"Eski Dosya Silindi :{path}");
                                }
                                catch (Exception ex)
                                {
                                    Log?.Invoke(false, $"Eski Dosya Silinemedi :{path}");
                                    throw;
                                }
                                try
                                {
                                    File.Copy(tempPath, path);
                                    Log?.Invoke(true, $"Yeni Dosya Kopyalandı :{path}");
                                }
                                catch (Exception ex)
                                {
                                    Log?.Invoke(false, $"Yeni Dosya Kopyalanamadı :{path}");
                                }
                                try
                                {
                                    File.Delete(tempPath);
                                    Log?.Invoke(true, $"Geçici Dosya Silindi :{tempPath}");
                                }
                                catch (Exception ex)
                                {
                                    Log?.Invoke(false, $"Geçici Dosya Silinemedi :{tempPath}");

                                }
                                break;
                            }
                            catch (Exception ex)
                            {
                                Log?.Invoke(true, $"({silsayac}) Eski Dosya Silinemedi Kapatılmaya çalışılıyor");

                                using (var pcs = Process.GetProcessesByName(Path.GetFileNameWithoutExtension(dosya.Adi)).FirstOrDefault(p => p.MainModule.FileName.StartsWith($"{Root}")))
                                {
                                    if (pcs != null)
                                    {
                                        Log?.Invoke(true, $"Proses Öldürülüyor :{pcs.ProcessName}");

                                        pcs.Kill();
                                        pcs.WaitForExit();
                                    }
                                    else
                                    {
                                        Log?.Invoke(false, $"{dosya.Adi} isimli dosyayı güncelleyemedim Programınız çalışmayabilir");

                                        Messager?.Invoke($"{dosya.Adi} isimli dosyayı güncelleyemedim Programınız çalışmayabilir");
                                    }
                                }
                                Thread.Sleep(1000);
                                if (silsayac == 100)
                                {
                                    Messager($"{dosya.Adi} isimli dosyayı güncelleyemedim bir türlü uygulama kapanmadı");
                                    break;
                                }
                            }
                        } while (silsayac < 100);
                    }
                    else
                    {
                        try
                        {
                            File.Copy(tempPath, path);
                            File.Delete(tempPath);
                            Log?.Invoke(true, $"Eski dosya yoktu Dosya Kopyalandı :{path}");
                        }
                        catch (Exception ex)
                        {
                            Log?.Invoke(false, $"Dosya Kopyalanamadı: {path} :{ex.Message}");
                        }

                    }
                }
                else
                {
                    Log?.Invoke(false, $"İnmesi gereken dosya inmemiş :{tempPath}");
                }
            }

            foreach (var dosya in klasorRes.Dosyalar.Where(x => !x.Adi.Contains(".exe")))
            {
                string path = $"{Root}\\{root}\\{dosya.Adi}";
                path = path.Replace("\\\\", "\\");

                string tempPath = $"{Root}\\Guncelleme\\{root}\\{dosya.Adi}";
                tempPath = tempPath.Replace("\\\\", "\\");
                if (File.Exists(tempPath))
                {
                    try
                    {
                        File.Delete(path);
                        Log?.Invoke(true, $"{dosya.Adi} isimli dosyayı Sildim.");

                        File.Copy(tempPath, path);
                        Log?.Invoke(true, $"{dosya.Adi} isimli dosyayı Kopyaladım.");
                    }
                    catch (Exception ex)
                    {
                        Log?.Invoke(false, $"{dosya.Adi} isimli dosyayı güncelleyemedim");

                        Messager?.Invoke($"{dosya.Adi} isimli dosyayı güncelleyemedim");
                    }

                    File.Delete(tempPath);
                    Log?.Invoke(true, $"{tempPath} isimli klasörü Sildim.");

                }
            }
            foreach (var item in klasorRes.Klasorler)
            {
                var dir = $"{Root}\\{root.Replace("/", "\\")}\\{item.Adi}";
                dir = dir.Replace("\\\\", "\\");
                if (Directory.Exists(dir))
                    Directory.CreateDirectory(dir);
                DosyaDegistir($"{root}\\{item.Adi}".Replace("\\\\", "\\"), item);
            }
        }

        static void KlasorIsle(string root, Klasor k)
        {
            Log?.Invoke(true, $"Klasör Kontrol Ediliyor :{k.Adi}");


            foreach (var item in k.Dosyalar)
            {
                
                if (item.Adi == Assembly.GetExecutingAssembly().GetName().Name+".exe")
                    continue;
                Log?.Invoke(true, $"Dosya Kontrol Ediliyor :{item.Adi}");

                string filePath = $"{Root}\\{root.Replace("/", "\\")}\\{item.Adi}";
                string tempPath = $"{Root}\\Guncelleme\\{root.Replace("/", "\\")}\\{item.Adi}";
                filePath = filePath.Replace("\\\\", "\\");
                tempPath = tempPath.Replace("\\\\", "\\");
                try
                {
                    if (File.Exists(tempPath))
                    {

                        File.Delete(tempPath);
                        Log?.Invoke(true, $"Geçici Dosya Silindi :{tempPath}");
                    }
                }
                catch (Exception ex)
                {
                    Log?.Invoke(false, $"Geçici Dosya Silinemedi :{tempPath}");
                }

                if (File.Exists(filePath))
                {
                    var localVersion = FileVersionInfo.GetVersionInfo(filePath).ProductVersion;

                    if (localVersion != item.Vers)
                    {
                        Log?.Invoke(true, $"Dosya Versiyonu Farklı :{item.Adi}");

                        GuncellemeVar = true;

                        var res = WebTools.DosyaCek($"{fileURL}?path={root}/{item.Adi}&program={ProgramAdi}", tempPath);

                        if (res)
                        {
                            Log?.Invoke(true, $"Dosya İndirildi :{tempPath}");
                        }
                        else
                        {
                            Log?.Invoke(false, $"Dosya İndirilemedi :{tempPath}");
                        }
                    }
                    else
                    {
                        Log?.Invoke(true, $"Dosya Güncel :{item.Adi}");

                    }
                }
                else
                {
                    Log?.Invoke(true, $"Dosya Yok İndiriliyor :{item.Adi}");

                    var res = WebTools.DosyaCek($"{fileURL}?path={root}/{item.Adi}&program={ProgramAdi}", filePath);

                    if (res)
                    {
                        Log?.Invoke(true, $"Dosya İndirildi :{tempPath}");
                    }
                    else
                    {
                        Log?.Invoke(false, $"Dosya İndirilemedi :{tempPath}");
                    }
                }
            }
            foreach (var item in k.Klasorler)
            {
                Log?.Invoke(true, $"Alt Klasore Git :{item.Adi}");

                KlasorIsle($"{root}/{item.Adi}", item);
            }
        }

        public static void UpdateBaslat()
        {
            Process p = new Process();
            p.StartInfo.FileName = BatPath;
            p.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            p.Start();
        }
        public static string BatPath
        {
            get
            {
                return AppDomain.CurrentDomain.BaseDirectory + "\\update.bat";
            }
        }
        public static bool BatVarmi
        {
            get
            {
                return File.Exists(BatPath);
            }
        }
    }

    public class Klasor
    {
        public Klasor()
        {
            Dosyalar = new List<Dosya>();
            Klasorler = new List<Klasor>();
        }
        public string Adi { get; set; }


        public List<Klasor> Klasorler { get; set; }
        public List<Dosya> Dosyalar { get; set; }

    }

    public class Dosya
    {
        public string Adi { get; set; }
        public string Vers { get; set; }
        public long Boyut { get; set; }
        public string Hash { get; set; }
    }
}
