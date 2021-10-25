using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using AYAK.Guncelleyici;

namespace EraWinPanel
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            notifyIcon1.Icon=new Icon("wwwroot/icon.ico");
            bool dosyavarmi = Ayarlar.AyarDefault();
            GuncelleyiciProvider.Messager += MesajVer;
            GuncelleyiciProvider.Log += Logla;

            Calistirici.Start();
            new Thread(() =>
            {
                var res = GuncelleyiciProvider.CheckUpdate();

                if ((bool)res.Data)
                {
                    Calistirici.Stop();

                    var gunres = GuncelleyiciProvider.GetUpdates();
                    if (gunres.State == ResultState.Success)
                    {
                        Calistirici.Start();
                        //MessageBox.Show("EraB2B Güncellendi");
                    }
                    else
                    {
                        Calistirici.Start();
                        //MessageBox.Show($"EraB2B Güncellenirken Hata Oluştu Eski Versiyon Açılıyor :\n{gunres.Message}");
                    }
                }
            }).Start();
        }
        void MesajVer(string mesaj)
        {
            label1.Text = mesaj;
            Show();
        }
        void Logla(bool durum, string mesaj)
        {

        }

        private void btnAyarlar_Click(object sender, EventArgs e)
        {
            AyarForm f = new AyarForm();
            f.Show();
        }

        private void btnBaslat_Click(object sender, EventArgs e)
        {
            var res = Calistirici.Start();
            if (!res.State)
            {
                Show();
                label1.Text = res.Message + "\n" + res?.Exception.Message;
            }
        }

        private void btnCikis_Click(object sender, EventArgs e)
        {
            Calistirici.Stop();
            DialogResult dialog = new DialogResult();

            dialog = MessageBox.Show("Kapatmak istediğinize emin misiniz ?", "Uyarı!", MessageBoxButtons.YesNo);

            if (dialog == DialogResult.Yes)
            {
                System.Environment.Exit(1);
            }

        }

        private void btnDurdur_Click(object sender, EventArgs e)
        {
            Calistirici.Stop();
        }

        private void btnGuncelle_Click(object sender, EventArgs e)
        {
            var chk = GuncelleyiciProvider.CheckUpdate();
            if ((bool)chk.Data)
            {
                var stopRes = Calistirici.Stop();
                if (stopRes.State)
                {
                    var gunres = GuncelleyiciProvider.GetUpdates();
                    if (gunres.State == ResultState.Success)
                    {
                        Calistirici.Start();
                        MessageBox.Show("EraB2B Güncellendi");
                    }
                    else
                    {
                        MessageBox.Show($"EraB2B Güncellenirken Hata Oluştu :\n{gunres.Message}");
                    }

                }
                else
                {
                    MessageBox.Show($"Açık olan site durdurulamadığından güncelleme yapılamadı\n{stopRes.Message}");
                }
            }
            else
            {
                MessageBox.Show("Zaten Güncelsiniz");
            }
        }

        private void btnYenidenBaslat_Click(object sender, EventArgs e)
        {
            btnDurdur_Click(sender, e);
            btnBaslat_Click(sender, e);
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            Hide();
            e.Cancel = true;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Hide();
        }

    }
}
