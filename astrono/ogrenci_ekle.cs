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
    public partial class ogrenci_ekle : Form
    {
        public ogrenci_ekle()
        {
            InitializeComponent();
        }
        IFirebaseConfig config = new FirebaseConfig
        {
            AuthSecret = "",
            BasePath = ""
        };
        IFirebaseClient client;

        private void button1_Click(object sender, EventArgs e)
        {
            uyelerr uye = new uyelerr
            {
                Parola = textBox2.Text,
                Kategori = comboBox1.Text,
                Grup_id = "yok"
            };
            var kayit_et = client.Set($"Üyeler/{textBox1.Text}", uye);
        }

        private void ogrenci_ekle_Load(object sender, EventArgs e)
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
    }
}
