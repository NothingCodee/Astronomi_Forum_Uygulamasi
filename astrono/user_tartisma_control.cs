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
using System.Threading;

namespace astrono
{
    public partial class user_tartisma_control : UserControl
    {
        public static string gonderen_ismi;
        public static string gonderen_tarih;
        public static string konu;
        public static string tartisma_id;
        public static string grup_id;
        public user_tartisma_control()
        {
            InitializeComponent();
        }
        IFirebaseConfig config = new FirebaseConfig
        {
            AuthSecret = "",
            BasePath = ""
        };
        IFirebaseClient client;
        private void user_tartisma_control_Load(object sender, EventArgs e)
        {
            try
            {
                client = new FireSharp.FirebaseClient(config);
            }
            catch
            {
                MessageBox.Show("Sunucuya bağlanırken bir sorun oluştu! Lütfen daha sonra tekrar deneyiniz.");
            }
            label1.Text = gonderen_ismi;
            label2.Text = gonderen_tarih;
            textBox1.Text = konu;
            Cevaplari_Yukle();
        }
        string tartisma_cevap_yol;
        private void Cevaplari_Yukle()
        {
            flowLayoutPanel1.Controls.Clear();
            var tartisma_sayisi = client.Get($"Gruplar/{grup_id}/Tartışmalar/t{tartisma_id}/Yorumlar/");
            tartisma_deger t_sayi = tartisma_sayisi.ResultAs<tartisma_deger>();
            int cevap_sayi = Convert.ToInt32(t_sayi.tartisma_cevap);

            for (int i = 0; i != cevap_sayi; i++)
            {
                var cevap_konu = client.Get($"Gruplar/{grup_id}/Tartışmalar/t{tartisma_id}/Yorumlar/y{i.ToString()}/");
                tartisma_icerik t_cevap = cevap_konu.ResultAs<tartisma_icerik>();
                tartisma_cevap_yol = $"Gruplar/{grup_id}/Tartışmalar/t{tartisma_id}/Yorumlar/";
                user_tartisma_yorum.gonderen = t_cevap.Gonderen;
                user_tartisma_yorum.tarih = t_cevap.Tarih;
                user_tartisma_yorum.konu = t_cevap.yorum;

                user_tartisma_yorum t_yorum = new user_tartisma_yorum();
                flowLayoutPanel1.Controls.Add(t_yorum);
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            var tartisma_sayisi = client.Get($"Gruplar/{grup_id}/Tartışmalar/t{tartisma_id}/Yorumlar/");
            tartisma_deger t_sayi = tartisma_sayisi.ResultAs<tartisma_deger>();
            cevap_sayi = Convert.ToInt32(t_sayi.tartisma_cevap);
            Thread.Sleep(50);
            cevap_girildi();
        }
        int cevap_sayi;
        int yeni_id;
        int id;
        private void cevap_girildi()
        {
            tartisma_deger t_sayi = new tartisma_deger
            {
                tartisma_cevap = (cevap_sayi + 1).ToString()
            };
            var ayarla = client.Update($"Gruplar/{grup_id}/Tartışmalar/t{tartisma_id}/Yorumlar/", t_sayi);
            id = Convert.ToInt32(t_sayi.tartisma_cevap);
            yeni_id = id - 1;
            //
            Thread.Sleep(1);
            //
            tartisma_icerik cevap_gir = new tartisma_icerik
            {
                yorum = textBox2.Text,
                Tarih = DateTime.Now.ToString(),
                Gonderen = uygulama.isim
            };
            var gonder = client.Set($"Gruplar/{grup_id}/Tartışmalar/t{tartisma_id}/Yorumlar/y{yeni_id}", cevap_gir);
            textBox2.Clear();
            Cevaplari_Yukle();
        }
    }
}
