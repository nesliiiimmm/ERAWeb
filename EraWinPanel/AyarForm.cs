using AYAK.Common.NetCore;
using System.Text.Json;
using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Data.SqlClient;

namespace EraWinPanel
{
    public partial class AyarForm : Form
    {
        public AyarForm()
        {
            InitializeComponent();
        }

        private void AyarForm_Load(object sender, EventArgs e)
        {
            txtGenelServer_TextChanged(null, null);
            txtDBServer_TextChanged(null, null);

            var a = Ayarlar.AyarGetir();
            if (a != null)
            {
                txtPort.Text = a.Port.ToString();

                txtFirmaAdi.Text = a.FirmaAdi;
                txtSehir.Text = a.FirmaSehir;
                txtVergiNo.Text = a.FirmaVergiNo;
                txtLisans.Text = a.LisansAnahtar;
                txtLisansDurum.Text = Enum.GetName<LisansDurum>(a.LisansDurum);
                txtLisansKontrol.Text = a.LisansKontrol.ToString();

                SqlConnectionStringBuilder gsb = new SqlConnectionStringBuilder(a.GenelConnectionString);
                SqlConnectionStringBuilder dsb = new SqlConnectionStringBuilder(a.DBConnectionString);
                txtDBServer.Text = dsb.DataSource;
                txtDBUser.Text = dsb.UserID;
                txtDBPassword.Text = dsb.Password;
                txtDBDataBase.Text = dsb.InitialCatalog;
                rdbDBWinAuth.Checked = dsb.IntegratedSecurity;

                txtGenelServer.Text = gsb.DataSource;
                txtGenelUser.Text = gsb.UserID;
                txtGenelPassword.Text = gsb.Password;
                txtGenelDatabase.Text = gsb.InitialCatalog;
                rdbGenelWinAuth.Checked = gsb.IntegratedSecurity;

                txtDBConStr.Text = a.DBConnectionString;
                txtGenelConStr.Text = a.GenelConnectionString;
            }
        }

        private void btnKaydet_Click(object sender, EventArgs e)
        {
            Ayarlar a = new Ayarlar
            {
                Port = txtPort.Text.ToInt(),
                DBConnectionString = txtDBConStr.Text,
                GenelConnectionString = txtGenelConStr.Text,
                FirmaAdi = txtFirmaAdi.Text,
                FirmaSehir = txtSehir.Text,
                FirmaVergiNo = txtVergiNo.Text,
                LisansAnahtar = txtLisans.Text
            };
            var talep = LisansTalep.AyardanDoldur(a);
            var cevap = LisansProvider.Kontrol(talep);
            a.LisansDurum = cevap.Durum;
            a.LisansKontrol = cevap.Tarih;
            a.LisansHash = cevap.Hash;
            Calistirici.execMod = chkexec.Checked;
            a.AyarKaydet();
            this.Close();
        }

        private void txtGenelServer_TextChanged(object sender, EventArgs e)
        {
           SqlConnectionStringBuilder sb = new SqlConnectionStringBuilder();
            sb.InitialCatalog = txtGenelDatabase.Text;
            sb.DataSource = txtGenelServer.Text;
            if (rdbGenelSqlAuth.Checked)
            {
                sb.UserID = txtGenelUser.Text;
                sb.Password = txtGenelPassword.Text;
            }
            else
            {
                txtGenelUser.Text = "";
                txtGenelPassword.Text = "";
            }
            sb.IntegratedSecurity = rdbGenelWinAuth.Checked;
            txtGenelConStr.Text = sb.ToString();
        }

        private void txtDBServer_TextChanged(object sender, EventArgs e)
        {
            SqlConnectionStringBuilder sb = new SqlConnectionStringBuilder();
            sb.InitialCatalog = txtDBDataBase.Text;
            sb.DataSource = txtDBServer.Text;
            if (rdbDBSQLAuth.Checked)
            {
                sb.UserID = txtDBUser.Text;
                sb.Password = txtDBPassword.Text;
            }
            else
            {
                txtDBUser.Text = "";
                txtDBPassword.Text = "";
            }
            sb.IntegratedSecurity = rdbDBWinAuth.Checked;
            txtDBConStr.Text = sb.ToString();
        }

    }
    public class Ayarlar
        {
            public static Ayarlar AyarGetir(string adi = "EraSettings.js")
            {
                if (File.Exists(Directory.GetCurrentDirectory() + "\\EraSettings.js"))
                {
                    string json = string.Empty;
                    using (FileStream fs = new FileStream(Directory.GetCurrentDirectory() + "\\" + adi, FileMode.OpenOrCreate))
                    {
                        StreamReader rdr = new StreamReader(fs);
                        json = rdr.ReadToEnd();
                    }
                    if (!string.IsNullOrEmpty(json))
                    {

                        var a = JsonSerializer.Deserialize<Ayarlar>(json);
                        return a;
                    }
                }

                return null;
            }

            public void AyarKaydet(string adi = "EraSettings.js")
            {
                var json = JsonSerializer.Serialize<Ayarlar>(this);

                using (FileStream fs = new FileStream(Directory.GetCurrentDirectory() + "\\" + adi, FileMode.Create))
                {
                    StreamWriter sw = new StreamWriter(fs);
                    sw.Write(json);
                    sw.Flush();
                    sw.Close();
                    fs.Close();
                }
            }
            public static bool AyarDefault(string adi = "EraSettings.js")
            {
                if (!File.Exists(Directory.GetCurrentDirectory() + "\\EraSettings.js"))
                {
                    Ayarlar a = new Ayarlar { DBConnectionString = "Data Source=Localhost;Initial Catalog=db_100;Integrated Security=True", GenelConnectionString = "Data Source=Localhost;Initial Catalog=GENEL;Integrated Security=True", FirmaAdi = "", FirmaSehir = "", FirmaVergiNo = "", LisansAnahtar = "", LisansDurum = 0, LisansHash = "", LisansKontrol = DateTime.Now, Port = 5001 };
                    var json = JsonSerializer.Serialize<Ayarlar>(a);

                    using (FileStream fs = new FileStream(Directory.GetCurrentDirectory() + "\\" + adi, FileMode.Create))
                    {
                        StreamWriter sw = new StreamWriter(fs);
                        sw.Write(json);
                        sw.Flush();
                        sw.Close();
                        fs.Close();
                    }
                    return true;
                }
                return false;
            }
            public void CevapIsle(LisansCevap cevap)
            {
                if (cevap.Durum != LisansDurum.BaglantiHatasi)
                {
                    var hash = Tools.CreateHash($"{(int)cevap.Durum}{cevap.Tarih}", LisansProvider.sk);
                    if (hash == cevap.Hash)
                    {
                        LisansDurum = cevap.Durum;
                        LisansHash = cevap.Hash;
                        LisansKontrol = cevap.Tarih;
                    }
                    else
                    {
                        //hash eşleşmiyorsa
                    }
                }
                else
                {
                    //bağlantı hatası olduysa 
                    var hash = Tools.CreateHash($"{(int)LisansDurum}{LisansKontrol}", LisansProvider.sk);
                    if (hash != LisansHash)
                    {
                        LisansDurum = LisansDurum.LisansYok;
                        LisansKontrol = DateTime.Now;
                        LisansHash = Tools.CreateHash($"{(int)LisansDurum}{LisansKontrol}", LisansProvider.sk);
                    }
                }
            }
            public int Port { get; set; }
            public string GenelConnectionString { get; set; }
            public string DBConnectionString { get; set; }
            public string FirmaAdi { get; set; }
            public string FirmaSehir { get; set; }
            public string FirmaVergiNo { get; set; }
            public string LisansAnahtar { get; set; }
            public DateTime LisansKontrol { get; set; }
            public LisansDurum LisansDurum { get; set; }
            public string LisansHash { get; set; }
        }
}
