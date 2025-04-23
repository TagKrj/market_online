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

namespace LapStore.Widget
{
    public partial class doanhThuLaiLoUserControl : UserControl
    {
        public doanhThuLaiLoUserControl()
        {
            InitializeComponent();
            LoadingData();
        }

        private void LoadingData()
        {
            List<ThongKeDoanhThuLaiLo> ThongKeDoanhThuLaiLos = DoanhThuLaiLoController.getAllThongKeDoanhThuLaiLos();
            dgv.Rows.Clear();
            var d = 0;
            foreach (ThongKeDoanhThuLaiLo ThongKeDoanhThuLaiLo in ThongKeDoanhThuLaiLos)
            {
                d++;
                dgv.Rows.Add(d,ThongKeDoanhThuLaiLo.MaHD, ThongKeDoanhThuLaiLo.TenKH, ThongKeDoanhThuLaiLo.TienVon, ThongKeDoanhThuLaiLo.TienBan,ThongKeDoanhThuLaiLo.LoiNhuan);
            }
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            var text = txtSearch.Text;
            List<ThongKeDoanhThuLaiLo> ThongKeDoanhThuLaiLos = DoanhThuLaiLoController.searchThongKeDoanhThuLaiLos(text);
            dgv.Rows.Clear();
            var d = 0;
            foreach (ThongKeDoanhThuLaiLo ThongKeDoanhThuLaiLo in ThongKeDoanhThuLaiLos)
            {
                d++;
                dgv.Rows.Add(d, ThongKeDoanhThuLaiLo.MaHD, ThongKeDoanhThuLaiLo.TenKH, ThongKeDoanhThuLaiLo.TienVon, ThongKeDoanhThuLaiLo.TienBan, ThongKeDoanhThuLaiLo.LoiNhuan);
            }
        }
    }
}