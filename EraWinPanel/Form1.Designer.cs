
namespace EraWinPanel
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.notifyIcon1 = new System.Windows.Forms.NotifyIcon(this.components);
            this.ctxNotify = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.btnBaslat = new System.Windows.Forms.ToolStripMenuItem();
            this.btnDurdur = new System.Windows.Forms.ToolStripMenuItem();
            this.btnYenidenBaslat = new System.Windows.Forms.ToolStripMenuItem();
            this.btnGuncelle = new System.Windows.Forms.ToolStripMenuItem();
            this.btnAyarlar = new System.Windows.Forms.ToolStripMenuItem();
            this.btnCikis = new System.Windows.Forms.ToolStripMenuItem();
            this.label1 = new System.Windows.Forms.Label();
            this.ctxNotify.SuspendLayout();
            this.SuspendLayout();
            // 
            // notifyIcon1
            // 
            this.notifyIcon1.ContextMenuStrip = this.ctxNotify;
            this.notifyIcon1.Text = "Era B2B";
            this.notifyIcon1.Visible = true;
            // 
            // ctxNotify
            // 
            this.ctxNotify.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnBaslat,
            this.btnDurdur,
            this.btnYenidenBaslat,
            this.btnGuncelle,
            this.btnAyarlar,
            this.btnCikis});
            this.ctxNotify.Name = "ctxNotify";
            this.ctxNotify.Size = new System.Drawing.Size(151, 136);
            // 
            // btnBaslat
            // 
            this.btnBaslat.Name = "btnBaslat";
            this.btnBaslat.Size = new System.Drawing.Size(150, 22);
            this.btnBaslat.Text = "Başlat";
            this.btnBaslat.Click += new System.EventHandler(this.btnBaslat_Click);
            // 
            // btnDurdur
            // 
            this.btnDurdur.Name = "btnDurdur";
            this.btnDurdur.Size = new System.Drawing.Size(150, 22);
            this.btnDurdur.Text = "Durdur";
            this.btnDurdur.Click += new System.EventHandler(this.btnDurdur_Click);
            // 
            // btnYenidenBaslat
            // 
            this.btnYenidenBaslat.Name = "btnYenidenBaslat";
            this.btnYenidenBaslat.Size = new System.Drawing.Size(150, 22);
            this.btnYenidenBaslat.Text = "Yeniden Başlat";
            this.btnYenidenBaslat.Click += new System.EventHandler(this.btnYenidenBaslat_Click);
            // 
            // btnGuncelle
            // 
            this.btnGuncelle.Name = "btnGuncelle";
            this.btnGuncelle.Size = new System.Drawing.Size(150, 22);
            this.btnGuncelle.Text = "Güncelle";
            this.btnGuncelle.Click += new System.EventHandler(this.btnGuncelle_Click);
            // 
            // btnAyarlar
            // 
            this.btnAyarlar.Name = "btnAyarlar";
            this.btnAyarlar.Size = new System.Drawing.Size(150, 22);
            this.btnAyarlar.Text = "Ayarlar";
            this.btnAyarlar.Click += new System.EventHandler(this.btnAyarlar_Click);
            // 
            // btnCikis
            // 
            this.btnCikis.Name = "btnCikis";
            this.btnCikis.Size = new System.Drawing.Size(150, 22);
            this.btnCikis.Text = "Çıkış";
            this.btnCikis.Click += new System.EventHandler(this.btnCikis_Click);
            // 
            // label1
            // 
            this.label1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 19F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.label1.Location = new System.Drawing.Point(0, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(387, 324);
            this.label1.TabIndex = 2;
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.label1.Visible = false;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(387, 324);
            this.Controls.Add(this.label1);
            this.Name = "Form1";
            this.ShowInTaskbar = false;
            this.Text = "Era Yazılım B2B Uygulaması";
            this.WindowState = System.Windows.Forms.FormWindowState.Minimized;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ctxNotify.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.NotifyIcon notifyIcon1;
        private System.Windows.Forms.ContextMenuStrip ctxNotify;
        private System.Windows.Forms.ToolStripMenuItem btnBaslat;
        private System.Windows.Forms.ToolStripMenuItem btnDurdur;
        private System.Windows.Forms.ToolStripMenuItem btnYenidenBaslat;
        private System.Windows.Forms.ToolStripMenuItem btnGuncelle;
        private System.Windows.Forms.ToolStripMenuItem btnAyarlar;
        private System.Windows.Forms.ToolStripMenuItem btnCikis;
        private System.Windows.Forms.Label label1;
    }
}

