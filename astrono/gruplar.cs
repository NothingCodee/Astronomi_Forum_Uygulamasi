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
    public partial class gruplar : UserControl
    {
        public static string grup_ismi;
        public static string uye_sayisi;
        public static string grup_id;
        public static string isim;
        public gruplar()
        {
            InitializeComponent();
        }
        IFirebaseConfig config = new FirebaseConfig
        {
            AuthSecret = "",
            BasePath = ""
        };
        IFirebaseClient client;
        private void gruplar_Load(object sender, EventArgs e)
        {
            label1.Text = grup_ismi;
            label3.Text = uye_sayisi;
            label4.Text = $"grup{grup_id}";
            try
            {
                client = new FireSharp.FirebaseClient(config);
            }
            catch
            {
                MessageBox.Show("Sunucuya bağlanırken bir sorun oluştu! Lütfen daha sonra tekrar deneyiniz.");
            }
            label1.Text = grup_ismi;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ozel_grup_id_kismi sss = new ozel_grup_id_kismi
            {
                Grup_id = label4.Text            
            };
            var setter = client.Update($"Üyeler/{isim}", sss);
            uygulama fr2 = new uygulama();
            gruup_id = $"grup{grup_id}";
            uye_kaydi_ekle();
        }
        string gruup_id;
        string uye_saayisi;
        private void uye_kaydi_ekle()
        {
            var grupp = client.Get($"Gruplar/{gruup_id}/");
            ozel_grup_id_kismi __grup = grupp.ResultAs<ozel_grup_id_kismi>();
            uye_saayisi = __grup.uye_sayisi;
            grup_sinifi _grup = new grup_sinifi
            {
                ismi = isim,
                Durum = "Çevrimiçi"
            };
            var setter = client.Set($"Gruplar/{gruup_id}/Uyeler/uye{uye_saayisi}", _grup);
            MessageBox.Show("Bir Gruba katıldığınız için uygulama kapatılcak. Uygulamayı tekrar açarak gruba tamamen katılmış olursunuz.");
            Environment.Exit(0);
        }
    }
}
