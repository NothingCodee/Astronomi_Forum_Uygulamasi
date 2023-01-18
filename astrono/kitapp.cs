using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Threading;

using FireSharp.Config;
using FireSharp.Response;
using FireSharp.Interfaces;

namespace astrono
{
    public partial class kitapp : Form
    {
        public static string grup_id;
        public kitapp()
        {
            InitializeComponent();
            
        }
        IFirebaseConfig config = new FirebaseConfig
        {
            AuthSecret = "",
            BasePath = ""
        };
        IFirebaseClient client;
        private void kitapp_Load(object sender, EventArgs e)
        {
            button2.Enabled = false;
            button1.Enabled = true;
            try
            {
                client = new FireSharp.FirebaseClient(config);
            }
            catch
            {
                MessageBox.Show("Sunucuya bağlanırken bir sorun oluştu! Lütfen daha sonra tekrar deneyiniz.");
            }
        }
        int sayfa = 0;
        int asil_sayfa;
        private async void button1_Click(object sender, EventArgs e)
        {
            var _g = client.Get($"Gruplar/{grup_id}/Yayınlananlar/kitap0/Sayfalar/");
            grup_sinifi __g = _g.ResultAs<grup_sinifi>();
            asil_sayfa = Convert.ToInt32(__g.sayfa_sayisi);
            //
            if(sayfa < asil_sayfa)
            {
                var grupp = client.Get($"Gruplar/{grup_id}/Yayınlananlar/kitap0/Sayfalar/s{sayfa.ToString()}/");
                grup_sinifi _grup = grupp.ResultAs<grup_sinifi>();

                byte[] b = Convert.FromBase64String(_grup.img);

                MemoryStream ms = new MemoryStream();
                ms.Write(b, 0, Convert.ToInt32(b.Length));

                Bitmap bm = new Bitmap(ms, false);
                ms.Dispose();

                pictureBox1.Image = bm;

                sayfa++;

                if (sayfa != 1)
                {
                    button2.Enabled = true;
                }
                else if (sayfa == 0)
                {
                    button2.Enabled = false;
                }
            }
          else if(sayfa > asil_sayfa)
            {
                MessageBox.Show("Kitap Bitti!");
                this.Close();
            }
            else if (sayfa == asil_sayfa)
            {
                MessageBox.Show("Kitap Bitti!");
                this.Close();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
                button2.Enabled = true;
                sayfa--;
                var grupp = client.Get($"Gruplar/{grup_id}/Yayınlananlar/kitap0/Sayfalar/s{sayfa.ToString()}/");
                grup_sinifi _grup = grupp.ResultAs<grup_sinifi>();

                byte[] b = Convert.FromBase64String(_grup.img);

                MemoryStream ms = new MemoryStream();
                ms.Write(b, 0, Convert.ToInt32(b.Length));

                Bitmap bm = new Bitmap(ms, false);
                ms.Dispose();

                pictureBox1.Image = bm;

        }
    }
}
