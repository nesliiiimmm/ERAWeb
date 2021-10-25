using AYAK.Common.NetCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace EraWinPanel
{
    public class Calistirici
    {
        public const string AppName = "ERASiparis.exe";
        public const string ProcessName = "ERASiparis";
        private const string sk = "f5547f1d-d923-4bcc-a0ff-2dea8654b4b5";
        public static bool execMod = false;
        public static string Dir
        {
            get
            {
                return Directory.GetCurrentDirectory() + "\\Web\\";
                // return Directory.GetCurrentDirectory() + "D:\\Masaüstü\\MVC Project\\ERA Siparis\\EraWeb\\EraWebCore\\bin\\Debug\\net5.0\\Web\\";
            }
        }
        public static string Path
        {
            get
            {
                return $"{Dir}{AppName}";
            }
        }
        private static Process _process;

        public static Process MyProcess
        {
            get
            {
                _process = Process.GetProcessesByName(ProcessName)?.FirstOrDefault();

                if (_process == null)
                {
                    var a = Ayarlar.AyarGetir();
                    if (a != null)
                    {

                        _process = new Process();
                        _process.StartInfo.FileName = Path;
                        _process.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                        _process.StartInfo.Arguments = $"--tarih {DateTime.Now.ToString("yyyyMMddHHmmss")} --urls http://0.0.0.0:{a.Port} --genel \"{a.GenelConnectionString}\" --database \"{a.DBConnectionString}\" --hash {Tools.CreateHash($"{DateTime.Now.ToString("yyyyMMddHHmmss")}http://0.0.0.0:{a.Port}{a.GenelConnectionString}{a.DBConnectionString}", sk)}";
                        if (!execMod)
                        {
                            _process.StartInfo.UseShellExecute = false;
                            _process.StartInfo.CreateNoWindow = true;
                        }

                        _process.StartInfo.Verb = "runas";
                    }

                }
                return _process;
            }
            set { _process = value; }
        }

        public static Result Start()
        {
            try
            {
                if (MyProcess != null)
                {

                    MyProcess.Start();
                    return new Result { State = true, Message = "Başarıyla Başladı" };
                }
                else
                {
                    return new Result { State = false, Message = "Process Yoktu Başlatamadım" };

                }
            }
            catch (Exception ex)
            {
                return new Result { State = false, Message = "Uygulama Başlatılamadı :", Exception = ex };

            }
        }
        public static Result Stop()
        {
            try
            {
                if (Kontrol())
                {
                    MyProcess.Kill();
                    MyProcess.WaitForExit();
                    return new Result { State = true, Message = "Başarıyla Durduruldu" };
                }
                else
                {
                    return new Result { State = true, Message = "Zaten Duruktu" };

                }

            }
            catch (Exception ex)
            {
                return new Result { State = false, Message = "Uygulama Durdurulamadı", Exception = ex };
            }
        }
        public static Result Restart()
        {
            try
            {
                Stop();
                if (Kontrol())
                    Start();
                else
                    return new Result { State = false, Message = "Uygulama Durdurulamadığı için Yeniden Başlatılamadı", Exception = new Exception() };

                return new Result { State = true, Message = "Başarıyla Yeniden Başladı" };

            }
            catch (Exception ex)
            {
                return new Result { State = false, Message = "Uygulama Yeniden Başlat Yapamadı", Exception = ex };


            }
        }

        public static bool Kontrol()
        {
            Process prcs = null;
            int sayac = 0;
            do
            {
                prcs = Process.GetProcessesByName(ProcessName).FirstOrDefault();
                if (sayac > 3)
                    return false;
                sayac++;
                Thread.Sleep(1000);
            }
            while (prcs == null);

            return true;
        }
    }

    public class Result
    {
        public bool State { get; set; }
        public string Message { get; set; }
        public Exception Exception { get; set; }

    }
}
