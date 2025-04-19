using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using LapStore.Controller;
using LapStore.Model;

namespace LapStore.Widget.Admin
{
    public partial class nhaCungCapUserControl : System.Windows.Forms.UserControl
    {
        public nhaCungCapUserControl()
        {
            InitializeComponent();
            LoadingData();
        }
        public void LoadingData()
        {
            List<NhaCungCap> NhaCungCaps = NhaCungCapController.getAllNhaCungCaps();
            dgvNhaCungCap.Rows.Clear();
            foreach (NhaCungCap NhaCungCap in NhaCungCaps)
            {
                dgvNhaCungCap.Rows.Add(NhaCungCap.id, NhaCungCap.tenNhaCungCap, NhaCungCap.diaChi);
            }
        }

        private void btnThemNhaCungCap_Click(object sender, EventArgs e)
        {
            var NhaCungCap = new NhaCungCap
            {
                id = txtIdNhaCungCap.Text,
                tenNhaCungCap = txtTenNhaCungCap.Text,
                diaChi = txtSoNhaCungCap.Text,
            };

            // if (NhaCungCapController.CheckMa(NhaCungCap.id))
            // {
            //     MessageBox.Show("Mã sản phẩm đã tồn tại!");
            //     return;
            // }

            // Use the static method directly with the class name
            NhaCungCapController.AddNhaCungCaps(NhaCungCap);
            LoadingData();
        }

        private void btnSuaNhaCungCap_Click(object sender, EventArgs e)
        {
            var NhaCungCap = new NhaCungCap
            {
                id = txtIdNhaCungCap.Text,
                tenNhaCungCap = txtTenNhaCungCap.Text,
                diaChi = txtSoNhaCungCap.Text,
            };
            NhaCungCapController.UpdateNhaCungCaps(NhaCungCap);
            LoadingData();
        }

        private void btnXoaNhaCungCap_Click(object sender, EventArgs e)
        {
            var NhaCungCap = new NhaCungCap
            {
                id = txtIdNhaCungCap.Text,
                // tenNhaCungCap = txtTenNhaCungCap.Text,
            };
            NhaCungCapController.DeleteNhaCungCaps(NhaCungCap);
            LoadingData();
        }

        private void txtSearchNhaCungCap_TextChanged(object sender, EventArgs e)
        {
            var text = txtSearchNhaCungCap.Text;
            List<NhaCungCap> NhaCungCaps = Controller.NhaCungCapController.SearchNhaCungCaps(text);
            dgvNhaCungCap.Rows.Clear();
            foreach (NhaCungCap NhaCungCap in NhaCungCaps)
            {
                dgvNhaCungCap.Rows.Add(NhaCungCap.id, NhaCungCap.tenNhaCungCap, NhaCungCap.diaChi);
            }
        }

        private void dgvNhaCungCap_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dgvNhaCungCap.Rows[e.RowIndex];
                txtIdNhaCungCap.Text = row.Cells[0].Value?.ToString();
                txtTenNhaCungCap.Text = row.Cells[1].Value?.ToString();
                txtSoNhaCungCap.Text = row.Cells[2].Value?.ToString();
            }
        }
    }
}

