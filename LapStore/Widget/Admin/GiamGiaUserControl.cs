using LapStore.Controller;
using LapStore.Model;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace LapStore.Widget
{
    public partial class GiamGiaUserControl : System.Windows.Forms.UserControl
    {
        public GiamGiaUserControl()
        {
            InitializeComponent();
            LoadingData();
        }

        public void LoadingData()
        {
            List<GiamGia> GiamGias = GiamGiaController.getAllGiamGias();
            dgvGiamGia.Rows.Clear();
            foreach (GiamGia GiamGia in GiamGias)
            {
                dgvGiamGia.Rows.Add(GiamGia.id, GiamGia.tenGiamGia,GiamGia.soGiamGia);
            }
        }

        private void btnThemGiamGia_Click(object sender, EventArgs e)
        {
            var GiamGia = new GiamGia
            {
                id = txtIdGiamGia.Text,
                tenGiamGia = txtTenGiamGia.Text,
                soGiamGia = txtSoGiamGia.Text,
            };

            // if (GiamGiaController.CheckMa(GiamGia.id))
            // {
            //     MessageBox.Show("Mã sản phẩm đã tồn tại!");
            //     return;
            // }

            // Use the static method directly with the class name
            GiamGiaController.AddGiamGias(GiamGia);
            LoadingData();
        }

        private void btnSuaGiamGia_Click(object sender, EventArgs e)
        {
            var GiamGia = new GiamGia
            {
                id = txtIdGiamGia.Text,
                tenGiamGia = txtTenGiamGia.Text,
                soGiamGia = txtSoGiamGia.Text,
            };
            GiamGiaController.UpdateGiamGias(GiamGia);
            LoadingData();
        }

        private void btnXoaGiamGia_Click(object sender, EventArgs e)
        {
            var GiamGia = new GiamGia
            {
                id = txtIdGiamGia.Text,
                // tenGiamGia = txtTenGiamGia.Text,
            };
            GiamGiaController.DeleteGiamGias(GiamGia);
            LoadingData();
        }

        private void txtSearchGiamGia_TextChanged(object sender, EventArgs e)
        {
            var text = txtSearchGiamGia.Text;
            List<GiamGia> GiamGias = Controller.GiamGiaController.SearchGiamGias(text);
            dgvGiamGia.Rows.Clear();
            foreach (GiamGia GiamGia in GiamGias)
            {
                dgvGiamGia.Rows.Add(GiamGia.id, GiamGia.tenGiamGia,GiamGia.soGiamGia);
            }
        }

        private void dgvGiamGia_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dgvGiamGia.Rows[e.RowIndex];
                txtIdGiamGia.Text = row.Cells[0].Value?.ToString();
                txtTenGiamGia.Text = row.Cells[1].Value?.ToString();
                txtSoGiamGia.Text = row.Cells[2].Value?.ToString();
            }
        }

    }
}
