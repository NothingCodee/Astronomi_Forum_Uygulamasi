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
    public partial class user_tartisma_yorum : UserControl
    {
        public static string gonderen;
        public static string konu;
        public static string tarih;
        public user_tartisma_yorum()
        {
            InitializeComponent();
        }

        private void user_tartisma_yorum_Load(object sender, EventArgs e)
        {
            label1.Text = gonderen;
            label2.Text = tarih;
            textBox1.Text = konu;
        }
    }
}
