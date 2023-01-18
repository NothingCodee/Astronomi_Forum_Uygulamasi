using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace astrono
{
    public partial class grup_uyeleri : UserControl
    {
        public static string isim;
        public static string kategori;
        public static string durum;
        public grup_uyeleri()
        {
            InitializeComponent();
        }

        private void grup_uyeleri_Load(object sender, EventArgs e)
        {
            label2.Text = durum;
            label1.Text = isim;
        }
    }
}
