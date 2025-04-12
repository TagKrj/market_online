using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Media;
using System.Windows.Forms;

namespace LapStore.View
{
    public partial class QuangCao : Form
    {

        public QuangCao()
        {
            InitializeComponent();
        }

        private void thoat_Click(object sender, EventArgs e)
        {
            this.Close(); // Đóng form
        }
    }
}
