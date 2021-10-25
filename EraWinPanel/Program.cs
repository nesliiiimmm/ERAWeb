using AYAK.Guncelleyici;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EraWinPanel
{
    static class Program
    {
        const string regName = "EraB2B";
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            GuncelleyiciProvider.ProgramAdi = "EraB2B";

            RegistryKey rkApp = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);
            rkApp.SetValue(regName, Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + "\\" + Path.GetFileNameWithoutExtension(System.Reflection.Assembly.GetExecutingAssembly().Location) + ".exe");


            var a = Ayarlar.AyarGetir();
            var talep = LisansTalep.AyardanDoldur(a);
            var cevap = LisansProvider.Kontrol(talep);
            Application.SetHighDpiMode(HighDpiMode.SystemAware);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            if (a.LisansDurum == LisansDurum.Lisansli)
            {
                Application.Run(new Form1());
            }
            else if (a.LisansDurum == LisansDurum.Demo)
            {
                DateTime date;
                var client = new TcpClient("time.nist.gov", 13);
                using (var streamReader = new StreamReader(client.GetStream()))
                {
                    var response = streamReader.ReadToEnd();
                    var utcDateTimeString = response.Substring(7, 17);
                    date = DateTime.ParseExact(utcDateTimeString, "yy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal);
                }

                if (a.LisansKontrol >= date)
                {
                    Application.Run(new Form1());
                }
                else
                {
                    MessageBox.Show("Deneme sürümünüz bitmiþtir. Lütfen diðer kullanýmlar için +905457166322 numarasý ile iletiþime geçiniz..");
                }
            }
            else
            {
                MessageBox.Show("Uppss.. Bir sorunla karþýlaþtýk. Lisansýnýz yok veya internet baðlantýnýz ile ilgili bir sýkýntý var. bir sorun var. Lütfen +905457166322 numarasý ile iletiþime geçiniz..");
            }
        }
    }
}
