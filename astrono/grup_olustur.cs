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
using FireSharp.Interfaces;
using FireSharp.Response;

namespace astrono
{
    public partial class grup_olustur : Form
    {
        public static string parola;
        public static string kategori;
        public grup_olustur()
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
            var g = client.Get("Gruplar/");
            grup_sinifi _g = g.ResultAs<grup_sinifi>();
            grup_sayisi = Convert.ToInt32(_g.grup_sayisi);
            MessageBox.Show("Bu işlem biraz uzun sürebilir lütfen 'Tamam'a bastıktan sonra işlem yapmayınız!");
            olustur();
        }
        int grup_sayisi;
        int ggrup_id;
        private void olustur()
        {
            grup_sinifi _g = new grup_sinifi
            {
                grup_ismi = textBox1.Text,
                uye_sayisi = "1",
            };
            var olustur = client.Set($"Gruplar/grup{grup_sayisi}", _g);
            ggrup_id = grup_sayisi;
            grup_sayisi++;
            grup_sayisi_artir();
        }

        private void grup_sayisi_artir()
        {
            grup_sinifi _g = new grup_sinifi
            {
                grup_sayisi = grup_sayisi.ToString()
            };
            var olustur = client.Update($"Gruplar/", _g);
            grup_tartismalar_ornek();
        }

        private void grup_tartismalar_ornek()
        {
            tartisma_deger _g = new tartisma_deger
            {
                tartisma_sayisi = "0"
            };
            var olustur = client.Set($"Gruplar/grup{ggrup_id}/Tartışmalar", _g);
            kitaplar();
        }

        private void kitaplar()
        {
            grup_sinifi _g = new grup_sinifi
            {
                kitap_ismi = "Kulüp Kitabı"
            };
            var olustur = client.Set($"Gruplar/grup{ggrup_id}/Yayınlananlar/kitap0", _g);
            kitaplar_sayfa();
        }

        private void kitaplar_sayfa()
        {
            grup_sinifi _g = new grup_sinifi
            {
                sayfa_sayisi = "0"
            };
            var olustur = client.Set($"Gruplar/grup{ggrup_id}/Yayınlananlar/kitap0/Sayfalar", _g);
            uye_eklke();
        }
        private void uye_eklke()
        {
            grup_sinifi _g = new grup_sinifi
            {
                ismi = uygulama.isim,
                Durum = "Çevrimdışı"
            };
            var olustur = client.Set($"Gruplar/grup{ggrup_id}/Uyeler/uye0", _g);
            kullanici_ayarla();
        }

        private void kullanici_ayarla()
        {
            uyelerr _g = new uyelerr
            {
                Grup_id = $"grup{ggrup_id.ToString()}"
            };
            var olustur = client.Update($"Üyeler/{uygulama.isim}", _g);
            MessageBox.Show("Grubunuz oluşturuldu! Uygulama kapandıktan sonra tekrar giriş yaparak grubunuz katılabilirsiniz!");
            this.Close();
            Environment.Exit(0);
        }
        private void grup_olustur_Load(object sender, EventArgs e)
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
