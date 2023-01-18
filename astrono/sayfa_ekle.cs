using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing.Imaging;
using System.IO;

using FireSharp.Config;
using FireSharp.Response;
using FireSharp.Interfaces;

namespace astrono
{
    public partial class sayfa_ekle : Form
    {
        public static string grup_id;
       
        public sayfa_ekle()
        {
            InitializeComponent();
        }
        IFirebaseConfig config = new FirebaseConfig
        {
            AuthSecret = "",
            BasePath = ""
        };
        IFirebaseClient client;

        private void sayfa_ekle_Load(object sender, EventArgs e)
        {
            try
            {
                client = new FireSharp.FirebaseClient(config);
            }
            catch
            {
                MessageBox.Show("Sunucuya bağlanırken bir sorun oluştu! Lütfen daha sonra tekrar deneyiniz.");
            }
            gruplar();
        }

        private void gruplar()
        {
            var yayinlana_sayi = client.Get($"Gruplar/{grup_id}/Yayınlananlar/");
            ozel_grup_id_kismi yayinlanan = yayinlana_sayi.ResultAs<ozel_grup_id_kismi>();
            int kitap_sayisi = Convert.ToInt32(yayinlanan.kitap_sayisi);

            for(int i = 0; i != kitap_sayisi; i++)
            {
                var uyeler = client.Get($"Gruplar/{grup_id}/Yayınlananlar/kitap{i.ToString()}/");
                grup_sinifi grup = uyeler.ResultAs<grup_sinifi>();
                comboBox1.Items.Add(grup.kitap_ismi);
                kitap_ismi = $"kitap{i.ToString()}";
            }
        }
        string kitap_ismi;
        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Title = "Select Image";

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                Image img = new Bitmap(ofd.FileName);
                pictureBox1.Image = img;
            }
        }
        int yeni_sayfa = 0;
        
        private void button2_Click(object sender, EventArgs e)
        {
            if(pictureBox1.Image != null)
            {
                var grupp = client.Get($"Gruplar/{grup_id}/Yayınlananlar/kitap0/Sayfalar/");
                grup_sinifi _grup = grupp.ResultAs<grup_sinifi>();
                yeni_sayfa = Convert.ToInt32(_grup.sayfa_sayisi);

                    MemoryStream ms = new MemoryStream();
                    pictureBox1.Image.Save(ms, ImageFormat.Jpeg);

                    byte[] a = ms.GetBuffer();

                    string output = Convert.ToBase64String(a);

                    var grrup = new grup_sinifi
                    {
                        img = output,
                    };
                    var setter = client.Set($"Gruplar/{grup_id}/Yayınlananlar/kitap0/Sayfalar/s{yeni_sayfa.ToString()}", grrup);
                yeni_sayfa++;
                sayfa_sayisi_arttir();
                    MessageBox.Show("Öge eklendi");

            }
            else if(pictureBox1.Image == null)
            {
                MessageBox.Show("Lütfen JPEG Dosya Seçiniz!");
            }
        }

        private void sayfa_sayisi_arttir()
        {
            var grrup = new grup_sinifi
            {
                sayfa_sayisi = yeni_sayfa.ToString()
            };
            var setter = client.Update($"Gruplar/{grup_id}/Yayınlananlar/kitap0/Sayfalar", grrup);
        }
    }
}
