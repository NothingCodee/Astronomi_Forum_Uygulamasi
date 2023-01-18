using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using System.Drawing.Imaging;
using System.IO;

using FireSharp.Config;
using FireSharp.Response;
using FireSharp.Interfaces;
using Firebase.Storage;


namespace astrono
{
    public partial class uygulama : Form
    {
        public static string isim;
        public static string grup_id;
        public uygulama()
        {
            InitializeComponent();
        }
        IFirebaseConfig config = new FirebaseConfig
        {
            AuthSecret = "",
            BasePath = ""
        };
        IFirebaseClient client;
        private void durum()
        {
            var uyeler = client.Get($"Üyeler/{isim}/");
            uyelerr uye = uyeler.ResultAs<uyelerr>();

         
            if (uye.Grup_id != "yok")
            {
                for (int i = 0; i != uye_sayisi; i++)
                {
                    var uyes = client.Get($"Gruplar/{grup_id}/Uyeler/uye{i.ToString()}");
                    grup_sinifi u = uyes.ResultAs<grup_sinifi>();
                    if (u.ismi == isim)
                    {
                        grup_sinifi sss = new grup_sinifi
                        {
                            Durum = "Çevrimiçi"
                        };
                        var setter = client.Update($"Gruplar/{grup_id}/Uyeler/", sss);
                    }
                }

            }
        }
        private void durum_kontrol()
        {
            var uyeler = client.Get($"Üyeler/{isim}/");
            uyelerr uye = uyeler.ResultAs<uyelerr>();
            grup_id = uye.Grup_id;
            user_tartisma_control.grup_id = uye.Grup_id;
            if (uye.Grup_id == "yok")
            {
                button2.Hide();
                panel6.Hide();
                button3.Hide();
                button7.Hide();
                button5.Show();
                button10.Hide();
                button11.Hide();
                button5.Show();
                panel5.Hide();
                panel4.Show();
            }
            else if(uye.Grup_id != "yok")
            {
                Tartismalar_yukle();
                panel5.Hide();
                panel4.Hide();
                button5.Hide();
                button2.Show();
                button3.Show();
                button7.Show();
                var tartismaa_konu = client.Get($"Gruplar/{grup_id}/");
                tartisma_icerik t_icerik = tartismaa_konu.ResultAs<tartisma_icerik>();
                button7.Text = t_icerik.grup_ismi;
            }
            grup_olustur.kategori = uye.Kategori;
            if(uye.Kategori == "Yetki Sahibi" || uye.Kategori == "Developer")
            {
                button10.Show();
                button11.Show();
                button9.Show();
            }
            else
            {
                button9.Hide();
                button10.Hide();
                button11.Hide();
            }
        }
        private void uygulama_Load(object sender, EventArgs e)
        {
            try
            {
                client = new FireSharp.FirebaseClient(config);
            }
            catch
            {
                MessageBox.Show("Sunucuya bağlanırken bir sorun oluştu! Lütfen daha sonra tekrar deneyiniz.");
            }
            label1.Text = isim;
            durum();
            durum_kontrol();
        }
        
        private void button1_Click(object sender, EventArgs e)
        {
            Form1 fr1 = new Form1();
            fr1.Show();
            this.Close();

        }
        int tt_sayi;
        private void Tartismalar_yukle()
        {
            panel8.Show();
            flowLayoutPanel1.Controls.Clear();
            panel_tartismalar.Show();
            var uyeler = client.Get($"Üyeler/{isim}/");
            uyelerr uye = uyeler.ResultAs<uyelerr>();
            grup_id = uye.Grup_id;
            //
            var tartisma_sayisi = client.Get($"Gruplar/{grup_id}/Tartışmalar/");
            tartisma_deger t_sayi = tartisma_sayisi.ResultAs<tartisma_deger>();
            if (t_sayi != null)
            {
                tt_sayi = Convert.ToInt32(t_sayi.tartisma_sayisi);
                for (int i = 0; i != tt_sayi; i++)
                {
                    var tartismaa_konu = client.Get($"Gruplar/{grup_id}/Tartışmalar/t{i.ToString()}/");
                    tartisma_icerik t_icerik = tartismaa_konu.ResultAs<tartisma_icerik>();
                    user_tartisma_control.gonderen_ismi = t_icerik.Gonderen;
                    user_tartisma_control.gonderen_tarih = t_icerik.Tarih;
                    user_tartisma_control.konu = t_icerik.Konu;
                    user_tartisma_control.tartisma_id = i.ToString();
                    tartismaa_id = i;
                    user_tartisma_control t = new user_tartisma_control();
                    flowLayoutPanel1.Controls.Add(t);
                }
            }
            Thread.Sleep(2000);
            panel8.Hide();
            panel_tartismalar.Show();
        }
        int tartismaa_id;
        private void panel_tartismalar_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            Tartismalar_yukle();
            panel6.Hide();
            panel4.Hide();
            panel5.Hide();
        }
        int tartisma_sayisii;
        int yeni_id;
        int id;
        private void button4_Click(object sender, EventArgs e)
        {
            tartisma_basladi();
        }
        int tartisma_cevap_sayisii;
        int cevap_sayi;
        private void tartisma_basladi()
        {
            var tartisma_sayisi = client.Get($"Gruplar/{grup_id}/Tartışmalar/");
            tartisma_deger t_sayi = tartisma_sayisi.ResultAs<tartisma_deger>();
            tt_sayi = Convert.ToInt32(t_sayi.tartisma_sayisi);
            tartisma_icerik t_icerik = new tartisma_icerik()
            {
                Gonderen = isim,
                Konu = textBox1.Text,
                Tarih = DateTime.Now.ToString()
            };
            var olustur = client.Set($"Gruplar/{grup_id}/Tartışmalar/t{tt_sayi}/", t_icerik);
            cevap_sayi = tt_sayi;
            tt_sayi++;
            tartisma_cevap_deger();
        }
        private void tartisma_cevap_deger()
        {
            tartisma_deger t_sayi = new tartisma_deger()
            {
                tartisma_sayisi = Convert.ToString(tt_sayi)
            };
            var update = client.Update($"Gruplar/{grup_id}/Tartışmalar/", t_sayi);
            Yorum_olayi();
        }
        private void Yorum_olayi()
        {
            tartisma_deger t_sayi = new tartisma_deger()
            {
                tartisma_cevap = 0.ToString()
            };
            var update = client.Update($"Gruplar/{grup_id}/Tartışmalar/t{cevap_sayi}/Yorumlar/", t_sayi);
            Thread.Sleep(50);
            Tartismalar_yukle();
        }

        private void panel4_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button5_Click(object sender, EventArgs e)
        {
            gruplar_yukle();

        }
        int grup_sayisii;
        int uye_sayisi;
        private void gruplar_yukle()
        {
            panel8.Show();
            flowLayoutPanel2.Controls.Clear();
            var grupp = client.Get($"Gruplar/");
            grup_sinifi _grup = grupp.ResultAs<grup_sinifi>();
            grup_sayisii = Convert.ToInt32(_grup.grup_sayisi);
            //
            for (int i = 0; i != grup_sayisii; i++)
            {
                var grupllar = client.Get($"Gruplar/grup{i.ToString()}/");
                gruplar.grup_id = i.ToString();
                grup_sinifi g_sinif = grupllar.ResultAs<grup_sinifi>();
                gruplar.grup_ismi = g_sinif.grup_ismi;
                gruplar.uye_sayisi = g_sinif.uye_sayisi;
                gruplar.isim = isim;

                gruplar g = new gruplar();
                flowLayoutPanel2.Controls.Add(g);
            }
            Thread.Sleep(2000);
            panel8.Hide();
            panel4.Show();
        }

        public void yenile()
        {
            panel5.Hide();
            panel4.Hide();
            button5.Hide();
            button2.Show();
            button3.Show();
            button7.Show();
            var tartismaa_konu = client.Get($"Gruplar/{grup_id}/");
            tartisma_icerik t_icerik = tartismaa_konu.ResultAs<tartisma_icerik>();
            button7.Text = t_icerik.grup_ismi;
            Tartismalar_yukle();
        }
        private void grrrrrup()
        {
            panel5.Show();
            flowLayoutPanel3.Controls.Clear();
            for (int i = 0; i != uye_sayisi; i++)
            {
                var grupp = client.Get($"Gruplar/{grup_id}/Uyeler/uye{i.ToString()}");
                grup_sinifi _grup = grupp.ResultAs<grup_sinifi>();
                grup_uyeleri.isim = _grup.ismi;
                grup_uyeleri.durum = _grup.Durum;

                grup_uyeleri g_u = new grup_uyeleri();
                flowLayoutPanel3.Controls.Add(g_u);
            }
        }
        private void button7_Click(object sender, EventArgs e)
        {
            panel8.Show();
            panel6.Hide();
            panel_tartismalar.Hide();
            panel4.Hide();
            var grupp = client.Get($"Gruplar/{grup_id}/");
            grup_sinifi _grup = grupp.ResultAs<grup_sinifi>();
            uye_sayisi = Convert.ToInt32(_grup.uye_sayisi);
            grrrrrup();
            Thread.Sleep(2000);
            panel8.Hide();
            panel5.Show();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            
        }
        private void button10_Click(object sender, EventArgs e)
        {
            ogrenci_ekle ogrenci = new ogrenci_ekle();
            ogrenci.Show();
        }

        private void button11_Click(object sender, EventArgs e)
        {
            sayfa_ekle.grup_id = grup_id;
            sayfa_ekle s = new sayfa_ekle();
            s.Show();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            panel8.Show();
            panel_tartismalar.Hide();
            panel4.Hide();
            panel5.Hide();
            var grupp = client.Get($"Gruplar/{grup_id}/Yayınlananlar/kitap0/");
            grup_sinifi _grup = grupp.ResultAs<grup_sinifi>();
            label7.Text = _grup.kitap_ismi;
            label9.Text = _grup.sayfa_sayisi;
            Thread.Sleep(2000);
            panel8.Hide();
            panel6.Show();
        }

        private void button8_Click_1(object sender, EventArgs e)
        {
            kitapp.grup_id = grup_id;
            kitapp kitap = new kitapp();
            kitap.Show();
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button9_Click(object sender, EventArgs e)
        {
            grup_olustur g = new grup_olustur();
            g.Show();
        }


    }
    }





    

