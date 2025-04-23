using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using LapStore.Model;
using LapStore.Controller;
using System.Globalization;

namespace LapStore.Widget
{
    public partial class lichSuKhachHangUserControl : UserControl
    {
       
        public lichSuKhachHangUserControl()
        {
            InitializeComponent();
            
        }

        private void btnTimKiem_Click_1(object sender, EventArgs e)
        {
            var text = txtTimKiem.Text.Trim();
            if (string.IsNullOrEmpty(text))
            {
                MessageBox.Show("Vui lòng nhập mã đơn hàng để tìm kiếm.");
                return;
            }
            List<LichSuDonHangInfo> LichSuDonHangInfos = LichSuKhachHangController.searchLichSuDonHangInfos(text);
            dgvKhach.Rows.Clear();
            foreach (LichSuDonHangInfo LichSuDonHangInfo in LichSuDonHangInfos)
            {
                dgvKhach.Rows.Add(LichSuDonHangInfo.IdDonHang, LichSuDonHangInfo.CreatedAtDonHang, LichSuDonHangInfo.HoTenUsers);
            }
            label2.Text = LichSuKhachHangController.searchTenKH(text);
        }

        private string maDonHangText = null;
        private void dgvKhach_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.RowIndex < dgvKhach.Rows.Count)
            {
                DataGridViewRow row = dgvKhach.Rows[e.RowIndex];
                maDonHangText = row.Cells["maDonHang"].Value?.ToString();
            }
            if (maDonHangText != null)
            {
                List<ChiTietDonHang> ChiTietDonHangs = LichSuKhachHangController.SearchChiTietDonHangs(maDonHangText);
                dgvChiTiet.Rows.Clear();
                foreach (ChiTietDonHang ChiTietDonHang in ChiTietDonHangs)
                {
                    dgvChiTiet.Rows.Add(ChiTietDonHang.TenSp, ChiTietDonHang.SoLuong, ChiTietDonHang.GiaBan.ToString("N0") + "đ");
                }
            }
        }
    }
}