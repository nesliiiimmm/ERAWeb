
namespace EraWinPanel
{
    partial class AyarForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.chkexec = new System.Windows.Forms.CheckBox();
            this.btnKaydet = new System.Windows.Forms.Button();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.rdbDBSQLAuth = new System.Windows.Forms.RadioButton();
            this.rdbDBWinAuth = new System.Windows.Forms.RadioButton();
            this.txtDBConStr = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.txtDBPassword = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.txtDBUser = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.txtDBDataBase = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.txtDBServer = new System.Windows.Forms.TextBox();
            this.label12 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.rdbGenelSqlAuth = new System.Windows.Forms.RadioButton();
            this.rdbGenelWinAuth = new System.Windows.Forms.RadioButton();
            this.txtGenelConStr = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.txtGenelPassword = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtGenelUser = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.txtGenelDatabase = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtGenelServer = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.txtLisansDurum = new System.Windows.Forms.TextBox();
            this.label19 = new System.Windows.Forms.Label();
            this.txtLisansKontrol = new System.Windows.Forms.TextBox();
            this.label18 = new System.Windows.Forms.Label();
            this.txtLisans = new System.Windows.Forms.TextBox();
            this.label17 = new System.Windows.Forms.Label();
            this.txtVergiNo = new System.Windows.Forms.TextBox();
            this.label16 = new System.Windows.Forms.Label();
            this.txtSehir = new System.Windows.Forms.TextBox();
            this.label15 = new System.Windows.Forms.Label();
            this.txtFirmaAdi = new System.Windows.Forms.TextBox();
            this.label14 = new System.Windows.Forms.Label();
            this.txtPort = new System.Windows.Forms.TextBox();
            this.label13 = new System.Windows.Forms.Label();
            this.groupBox3.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // chkexec
            // 
            this.chkexec.AutoSize = true;
            this.chkexec.Location = new System.Drawing.Point(428, 494);
            this.chkexec.Name = "chkexec";
            this.chkexec.Size = new System.Drawing.Size(109, 19);
            this.chkexec.TabIndex = 10;
            this.chkexec.Text = "Geliştirici Ekranı";
            this.chkexec.UseVisualStyleBackColor = true;
            // 
            // btnKaydet
            // 
            this.btnKaydet.Location = new System.Drawing.Point(543, 486);
            this.btnKaydet.Name = "btnKaydet";
            this.btnKaydet.Size = new System.Drawing.Size(93, 33);
            this.btnKaydet.TabIndex = 9;
            this.btnKaydet.Text = "Kaydet";
            this.btnKaydet.UseVisualStyleBackColor = true;
            this.btnKaydet.Click += new System.EventHandler(this.btnKaydet_Click);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.rdbDBSQLAuth);
            this.groupBox3.Controls.Add(this.rdbDBWinAuth);
            this.groupBox3.Controls.Add(this.txtDBConStr);
            this.groupBox3.Controls.Add(this.label7);
            this.groupBox3.Controls.Add(this.txtDBPassword);
            this.groupBox3.Controls.Add(this.label8);
            this.groupBox3.Controls.Add(this.txtDBUser);
            this.groupBox3.Controls.Add(this.label9);
            this.groupBox3.Controls.Add(this.label10);
            this.groupBox3.Controls.Add(this.txtDBDataBase);
            this.groupBox3.Controls.Add(this.label11);
            this.groupBox3.Controls.Add(this.txtDBServer);
            this.groupBox3.Controls.Add(this.label12);
            this.groupBox3.Location = new System.Drawing.Point(333, 205);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(309, 275);
            this.groupBox3.TabIndex = 8;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "DB Veritabanı";
            // 
            // rdbDBSQLAuth
            // 
            this.rdbDBSQLAuth.AutoSize = true;
            this.rdbDBSQLAuth.Checked = true;
            this.rdbDBSQLAuth.Location = new System.Drawing.Point(176, 94);
            this.rdbDBSQLAuth.Name = "rdbDBSQLAuth";
            this.rdbDBSQLAuth.Size = new System.Drawing.Size(46, 19);
            this.rdbDBSQLAuth.TabIndex = 3;
            this.rdbDBSQLAuth.TabStop = true;
            this.rdbDBSQLAuth.Text = "SQL";
            this.rdbDBSQLAuth.UseVisualStyleBackColor = true;
            this.rdbDBSQLAuth.CheckedChanged += new System.EventHandler(this.txtDBServer_TextChanged);
            this.rdbDBSQLAuth.TextChanged += new System.EventHandler(this.txtDBServer_TextChanged);
            // 
            // rdbDBWinAuth
            // 
            this.rdbDBWinAuth.AutoSize = true;
            this.rdbDBWinAuth.Location = new System.Drawing.Point(90, 94);
            this.rdbDBWinAuth.Name = "rdbDBWinAuth";
            this.rdbDBWinAuth.Size = new System.Drawing.Size(74, 19);
            this.rdbDBWinAuth.TabIndex = 2;
            this.rdbDBWinAuth.Text = "Windows";
            this.rdbDBWinAuth.UseVisualStyleBackColor = true;
            this.rdbDBWinAuth.CheckedChanged += new System.EventHandler(this.txtDBServer_TextChanged);
            this.rdbDBWinAuth.TextChanged += new System.EventHandler(this.txtDBServer_TextChanged);
            // 
            // txtDBConStr
            // 
            this.txtDBConStr.Location = new System.Drawing.Point(90, 180);
            this.txtDBConStr.Multiline = true;
            this.txtDBConStr.Name = "txtDBConStr";
            this.txtDBConStr.Size = new System.Drawing.Size(213, 89);
            this.txtDBConStr.TabIndex = 6;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(21, 182);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(63, 15);
            this.label7.TabIndex = 0;
            this.label7.Text = "Con String";
            // 
            // txtDBPassword
            // 
            this.txtDBPassword.Location = new System.Drawing.Point(90, 151);
            this.txtDBPassword.Name = "txtDBPassword";
            this.txtDBPassword.Size = new System.Drawing.Size(213, 23);
            this.txtDBPassword.TabIndex = 5;
            this.txtDBPassword.Text = "123";
            this.txtDBPassword.Leave += new System.EventHandler(this.txtDBServer_TextChanged);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(21, 153);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(63, 15);
            this.label8.TabIndex = 0;
            this.label8.Text = "Password :";
            // 
            // txtDBUser
            // 
            this.txtDBUser.Location = new System.Drawing.Point(90, 122);
            this.txtDBUser.Name = "txtDBUser";
            this.txtDBUser.Size = new System.Drawing.Size(213, 23);
            this.txtDBUser.TabIndex = 4;
            this.txtDBUser.Text = "sa";
            this.txtDBUser.Leave += new System.EventHandler(this.txtDBServer_TextChanged);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(21, 96);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(39, 15);
            this.label9.TabIndex = 0;
            this.label9.Text = "Auth :";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(21, 124);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(65, 15);
            this.label10.TabIndex = 0;
            this.label10.Text = "User Name";
            // 
            // txtDBDataBase
            // 
            this.txtDBDataBase.Location = new System.Drawing.Point(90, 65);
            this.txtDBDataBase.Name = "txtDBDataBase";
            this.txtDBDataBase.Size = new System.Drawing.Size(213, 23);
            this.txtDBDataBase.TabIndex = 1;
            this.txtDBDataBase.Text = "db_100";
            this.txtDBDataBase.Leave += new System.EventHandler(this.txtDBServer_TextChanged);
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(21, 67);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(55, 15);
            this.label11.TabIndex = 0;
            this.label11.Text = "Database";
            // 
            // txtDBServer
            // 
            this.txtDBServer.Location = new System.Drawing.Point(90, 36);
            this.txtDBServer.Name = "txtDBServer";
            this.txtDBServer.Size = new System.Drawing.Size(213, 23);
            this.txtDBServer.TabIndex = 0;
            this.txtDBServer.Text = "localhost";
            this.txtDBServer.TextChanged += new System.EventHandler(this.txtDBServer_TextChanged);
            this.txtDBServer.Leave += new System.EventHandler(this.txtDBServer_TextChanged);
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(21, 38);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(39, 15);
            this.label12.TabIndex = 0;
            this.label12.Text = "Server";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.rdbGenelSqlAuth);
            this.groupBox2.Controls.Add(this.rdbGenelWinAuth);
            this.groupBox2.Controls.Add(this.txtGenelConStr);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.txtGenelPassword);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.txtGenelUser);
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.txtGenelDatabase);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.txtGenelServer);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Location = new System.Drawing.Point(18, 205);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(309, 275);
            this.groupBox2.TabIndex = 7;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Genel Veritabanı";
            // 
            // rdbGenelSqlAuth
            // 
            this.rdbGenelSqlAuth.AutoSize = true;
            this.rdbGenelSqlAuth.Checked = true;
            this.rdbGenelSqlAuth.Location = new System.Drawing.Point(176, 94);
            this.rdbGenelSqlAuth.Name = "rdbGenelSqlAuth";
            this.rdbGenelSqlAuth.Size = new System.Drawing.Size(46, 19);
            this.rdbGenelSqlAuth.TabIndex = 3;
            this.rdbGenelSqlAuth.TabStop = true;
            this.rdbGenelSqlAuth.Text = "SQL";
            this.rdbGenelSqlAuth.UseVisualStyleBackColor = true;
            this.rdbGenelSqlAuth.CheckedChanged += new System.EventHandler(this.txtGenelServer_TextChanged);
            this.rdbGenelSqlAuth.TextChanged += new System.EventHandler(this.txtGenelServer_TextChanged);
            // 
            // rdbGenelWinAuth
            // 
            this.rdbGenelWinAuth.AutoSize = true;
            this.rdbGenelWinAuth.Location = new System.Drawing.Point(90, 94);
            this.rdbGenelWinAuth.Name = "rdbGenelWinAuth";
            this.rdbGenelWinAuth.Size = new System.Drawing.Size(74, 19);
            this.rdbGenelWinAuth.TabIndex = 2;
            this.rdbGenelWinAuth.Text = "Windows";
            this.rdbGenelWinAuth.UseVisualStyleBackColor = true;
            this.rdbGenelWinAuth.CheckedChanged += new System.EventHandler(this.txtGenelServer_TextChanged);
            this.rdbGenelWinAuth.TextChanged += new System.EventHandler(this.txtGenelServer_TextChanged);
            // 
            // txtGenelConStr
            // 
            this.txtGenelConStr.Location = new System.Drawing.Point(90, 180);
            this.txtGenelConStr.Multiline = true;
            this.txtGenelConStr.Name = "txtGenelConStr";
            this.txtGenelConStr.Size = new System.Drawing.Size(213, 89);
            this.txtGenelConStr.TabIndex = 6;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(21, 182);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(63, 15);
            this.label5.TabIndex = 0;
            this.label5.Text = "Con String";
            // 
            // txtGenelPassword
            // 
            this.txtGenelPassword.Location = new System.Drawing.Point(90, 151);
            this.txtGenelPassword.Name = "txtGenelPassword";
            this.txtGenelPassword.Size = new System.Drawing.Size(213, 23);
            this.txtGenelPassword.TabIndex = 5;
            this.txtGenelPassword.Text = "123";
            this.txtGenelPassword.Leave += new System.EventHandler(this.txtGenelServer_TextChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(21, 153);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(63, 15);
            this.label4.TabIndex = 0;
            this.label4.Text = "Password :";
            // 
            // txtGenelUser
            // 
            this.txtGenelUser.Location = new System.Drawing.Point(90, 122);
            this.txtGenelUser.Name = "txtGenelUser";
            this.txtGenelUser.Size = new System.Drawing.Size(213, 23);
            this.txtGenelUser.TabIndex = 4;
            this.txtGenelUser.Text = "sa";
            this.txtGenelUser.Leave += new System.EventHandler(this.txtGenelServer_TextChanged);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(21, 96);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(39, 15);
            this.label6.TabIndex = 0;
            this.label6.Text = "Auth :";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(21, 124);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(65, 15);
            this.label3.TabIndex = 0;
            this.label3.Text = "User Name";
            // 
            // txtGenelDatabase
            // 
            this.txtGenelDatabase.Location = new System.Drawing.Point(90, 65);
            this.txtGenelDatabase.Name = "txtGenelDatabase";
            this.txtGenelDatabase.Size = new System.Drawing.Size(213, 23);
            this.txtGenelDatabase.TabIndex = 1;
            this.txtGenelDatabase.Text = "GENEL";
            this.txtGenelDatabase.Leave += new System.EventHandler(this.txtGenelServer_TextChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(21, 67);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(55, 15);
            this.label2.TabIndex = 0;
            this.label2.Text = "Database";
            // 
            // txtGenelServer
            // 
            this.txtGenelServer.Location = new System.Drawing.Point(90, 36);
            this.txtGenelServer.Name = "txtGenelServer";
            this.txtGenelServer.Size = new System.Drawing.Size(213, 23);
            this.txtGenelServer.TabIndex = 0;
            this.txtGenelServer.Text = "localhost";
            this.txtGenelServer.TextChanged += new System.EventHandler(this.txtGenelServer_TextChanged);
            this.txtGenelServer.Leave += new System.EventHandler(this.txtGenelServer_TextChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(21, 38);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(39, 15);
            this.label1.TabIndex = 0;
            this.label1.Text = "Server";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.txtLisansDurum);
            this.groupBox1.Controls.Add(this.label19);
            this.groupBox1.Controls.Add(this.txtLisansKontrol);
            this.groupBox1.Controls.Add(this.label18);
            this.groupBox1.Controls.Add(this.txtLisans);
            this.groupBox1.Controls.Add(this.label17);
            this.groupBox1.Controls.Add(this.txtVergiNo);
            this.groupBox1.Controls.Add(this.label16);
            this.groupBox1.Controls.Add(this.txtSehir);
            this.groupBox1.Controls.Add(this.label15);
            this.groupBox1.Controls.Add(this.txtFirmaAdi);
            this.groupBox1.Controls.Add(this.label14);
            this.groupBox1.Controls.Add(this.txtPort);
            this.groupBox1.Controls.Add(this.label13);
            this.groupBox1.Location = new System.Drawing.Point(18, 28);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(618, 171);
            this.groupBox1.TabIndex = 6;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Site Seçenekleri";
            // 
            // txtLisansDurum
            // 
            this.txtLisansDurum.Enabled = false;
            this.txtLisansDurum.Location = new System.Drawing.Point(408, 82);
            this.txtLisansDurum.Name = "txtLisansDurum";
            this.txtLisansDurum.Size = new System.Drawing.Size(200, 23);
            this.txtLisansDurum.TabIndex = 3;
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.Location = new System.Drawing.Point(316, 85);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(85, 15);
            this.label19.TabIndex = 2;
            this.label19.Text = "Lisans Durum :";
            // 
            // txtLisansKontrol
            // 
            this.txtLisansKontrol.Enabled = false;
            this.txtLisansKontrol.Location = new System.Drawing.Point(408, 51);
            this.txtLisansKontrol.Name = "txtLisansKontrol";
            this.txtLisansKontrol.Size = new System.Drawing.Size(200, 23);
            this.txtLisansKontrol.TabIndex = 3;
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Location = new System.Drawing.Point(316, 54);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(87, 15);
            this.label18.TabIndex = 2;
            this.label18.Text = "Lisans Kontrol :";
            // 
            // txtLisans
            // 
            this.txtLisans.Location = new System.Drawing.Point(408, 22);
            this.txtLisans.Name = "txtLisans";
            this.txtLisans.Size = new System.Drawing.Size(200, 23);
            this.txtLisans.TabIndex = 3;
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Location = new System.Drawing.Point(316, 25);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(93, 15);
            this.label17.TabIndex = 2;
            this.label17.Text = "Lisans Anahtarı :";
            // 
            // txtVergiNo
            // 
            this.txtVergiNo.Location = new System.Drawing.Point(108, 80);
            this.txtVergiNo.Name = "txtVergiNo";
            this.txtVergiNo.Size = new System.Drawing.Size(195, 23);
            this.txtVergiNo.TabIndex = 2;
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(19, 83);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(58, 15);
            this.label16.TabIndex = 2;
            this.label16.Text = "Vergi No :";
            // 
            // txtSehir
            // 
            this.txtSehir.Location = new System.Drawing.Point(109, 51);
            this.txtSehir.Name = "txtSehir";
            this.txtSehir.Size = new System.Drawing.Size(195, 23);
            this.txtSehir.TabIndex = 1;
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(20, 54);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(39, 15);
            this.label15.TabIndex = 2;
            this.label15.Text = "Şehir :";
            // 
            // txtFirmaAdi
            // 
            this.txtFirmaAdi.Location = new System.Drawing.Point(109, 22);
            this.txtFirmaAdi.Name = "txtFirmaAdi";
            this.txtFirmaAdi.Size = new System.Drawing.Size(195, 23);
            this.txtFirmaAdi.TabIndex = 0;
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(20, 25);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(64, 15);
            this.label14.TabIndex = 2;
            this.label14.Text = "Firma Adı :";
            // 
            // txtPort
            // 
            this.txtPort.Location = new System.Drawing.Point(553, 142);
            this.txtPort.Name = "txtPort";
            this.txtPort.Size = new System.Drawing.Size(59, 23);
            this.txtPort.TabIndex = 4;
            this.txtPort.Text = "1970";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(499, 145);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(35, 15);
            this.label13.TabIndex = 0;
            this.label13.Text = "Port :";
            // 
            // AyarForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(661, 547);
            this.Controls.Add(this.chkexec);
            this.Controls.Add(this.btnKaydet);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Name = "AyarForm";
            this.Text = "AyarForm";
            this.Load += new System.EventHandler(this.AyarForm_Load);
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox chkexec;
        private System.Windows.Forms.Button btnKaydet;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.RadioButton rdbDBSQLAuth;
        private System.Windows.Forms.RadioButton rdbDBWinAuth;
        private System.Windows.Forms.TextBox txtDBConStr;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox txtDBPassword;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox txtDBUser;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox txtDBDataBase;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.TextBox txtDBServer;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.RadioButton rdbGenelSqlAuth;
        private System.Windows.Forms.RadioButton rdbGenelWinAuth;
        private System.Windows.Forms.TextBox txtGenelConStr;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtGenelPassword;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtGenelUser;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtGenelDatabase;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtGenelServer;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox txtLisansDurum;
        private System.Windows.Forms.Label label19;
        private System.Windows.Forms.TextBox txtLisansKontrol;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.TextBox txtLisans;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.TextBox txtVergiNo;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.TextBox txtSehir;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.TextBox txtFirmaAdi;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.TextBox txtPort;
        private System.Windows.Forms.Label label13;
    }
}