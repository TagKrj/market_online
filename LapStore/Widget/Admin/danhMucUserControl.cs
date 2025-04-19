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

namespace LapStore.Widget
{
    public partial class danhMucUserControl : System.Windows.Forms.UserControl
    {
        public danhMucUserControl()
        {
            InitializeComponent();
            LoadingData();
        }

        public void LoadingData()
        {
            List<DanhMuc> DanhMucs = DanhMucController.getAllDanhMucs();
            dgvDanhMuc.Rows.Clear();
            foreach (DanhMuc DanhMuc in DanhMucs)
            {
                dgvDanhMuc.Rows.Add(DanhMuc.id, DanhMuc.tenDanhMuc); 
            }
        }

        private void btnThemDanhMuc_Click(object sender, EventArgs e)
        {
            var DanhMuc = new DanhMuc
            {
                id = txtIdDanhMuc.Text,
                tenDanhMuc = txtTenDanhMuc.Text,
            };

            // if (DanhMucController.CheckMa(DanhMuc.id))
            // {
            //     MessageBox.Show("Mã sản phẩm đã tồn tại!");
            //     return;
            // }

            // Use the static method directly with the class name
            DanhMucController.AddDanhMucs(DanhMuc);
            LoadingData();
        }

        private void btnSuaDanhMuc_Click(object sender, EventArgs e)
        {
            var DanhMuc = new DanhMuc
            {
                id = txtIdDanhMuc.Text,
                tenDanhMuc = txtTenDanhMuc.Text,
            };
            DanhMucController.UpdateDanhMucs(DanhMuc);
            LoadingData();
        }

        private void btnXoaDanhMuc_Click(object sender, EventArgs e)
        {
            var DanhMuc = new DanhMuc
            {
                id = txtIdDanhMuc.Text,
                // tenDanhMuc = txtTenDanhMuc.Text,
            };
            DanhMucController.DeleteDanhMucs(DanhMuc);
            LoadingData();
        }

        private void txtSearchDanhMuc_TextChanged(object sender, EventArgs e)
        {
            var text = txtSearchDanhMuc.Text;
            List<DanhMuc> DanhMucs = Controller.DanhMucController.SearchDanhMucs(text);
            dgvDanhMuc.Rows.Clear();
            foreach (DanhMuc DanhMuc in DanhMucs)
            {
                dgvDanhMuc.Rows.Add(DanhMuc.id, DanhMuc.tenDanhMuc);
            }
        }

        private void dgvDanhMuc_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dgvDanhMuc.Rows[e.RowIndex];
                txtIdDanhMuc.Text = row.Cells[0].Value?.ToString();
            }
        }
    }
}
