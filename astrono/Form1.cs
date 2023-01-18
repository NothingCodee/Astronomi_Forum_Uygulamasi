using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using FireSharp.Config;
using FireSharp.Response;
using FireSharp.Interfaces;

namespace astrono
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        IFirebaseConfig config = new FirebaseConfig
        {
            AuthSecret = "",
            BasePath = ""
        };
        IFirebaseClient client;
        private void Giris_Yap()
        {
            var uyeler = client.Get("Üyeler/" + textBox1.Text);
            uyelerr uye = uyeler.ResultAs<uyelerr>();

            if (textBox2.Text == uye.Parola)
            {
                uygulama.isim = textBox1.Text;
                this.Hide();
                uygulama fr2 = new uygulama();
                fr2.Show();
            }
            else
            {
                MessageBox.Show("Parola Yanlış");
            }
        }



        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                client = new FireSharp.FirebaseClient(config);
            }
            catch
            {
                MessageBox.Show("Sunucuya bağlanırken bir sorun oluştu! Lütfen daha sonra tekrar deneyiniz.");
            }
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Environment.Exit(0);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            uygulama.isim = textBox1.Text;
            Giris_Yap();
            grup_olustur.parola = textBox2.Text;
        }
    }
}
