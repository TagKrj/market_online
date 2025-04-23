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
    public partial class doanhthuThoiGianUserControl : System.Windows.Forms.UserControl
    {
      
        public doanhthuThoiGianUserControl()
        {
            InitializeComponent();
        }

        private void dateEndAt_ValueChanged(object sender, EventArgs e)
        {
            var createAt = dateCreateAt.Text;
            var endAt = dateEndAt.Text;
            List<DoanhThuTheoNgay> DoanhThuTheoNgays = DoanhThuThoiGianController.getAllDoanhThuThoiGians(createAt,endAt);
            dgv.Rows.Clear();
            var d = 0;
            foreach (DoanhThuTheoNgay DoanhThuTheoNgay in DoanhThuTheoNgays)
            {
                dgv.Rows.Add(DoanhThuTheoNgay.Id, DoanhThuTheoNgay.CreatedAt, DoanhThuTheoNgay.MaDonHang, DoanhThuTheoNgay.DoanhThu);
                d++;
            }

            txtTongHoaDon.Text = d.ToString();
            txtTongTien.Text = DoanhThuTheoNgays.Sum(x => x.DoanhThu).ToString();
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            var createAt = dateCreateAt.Text;
            var endAt = dateEndAt.Text;
            var text = txtSearch.Text;
            var d = 0;
            List<DoanhThuTheoNgay> DoanhThuTheoNgays = DoanhThuThoiGianController.searchDoanhThuThoiGians(createAt, endAt, text);
            dgv.Rows.Clear();
            foreach (DoanhThuTheoNgay DoanhThuTheoNgay in DoanhThuTheoNgays)
            {
                dgv.Rows.Add(DoanhThuTheoNgay.Id, DoanhThuTheoNgay.CreatedAt, DoanhThuTheoNgay.MaDonHang, DoanhThuTheoNgay.DoanhThu);
            }
            txtTongHoaDon.Text = d.ToString();
            txtTongTien.Text = DoanhThuTheoNgays.Sum(x => x.DoanhThu).ToString();
        }
    }
}
