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
using System.Web.Security;

namespace LapStore.Widget
{
    public partial class doanhThuNhomHangUserControl : System.Windows.Forms.UserControl
    {

        public doanhThuNhomHangUserControl()
        {
            InitializeComponent();
            cboDanhMuc.SelectedValue = "a";
        }

        void LoadingData()
        {
            
            List<DanhMuc> DanhMucs = DanhMucController.getAllDanhMucs();
            // cboDanhMuc.Items.Clear();
            Dictionary<string, string> DanhMucDict = new Dictionary<string, string>
            {
                { "a", "Tất cả danh mục" }
            };
            foreach (DanhMuc DanhMuc in DanhMucs)
            {
               DanhMucDict.Add(DanhMuc.id, DanhMuc.tenDanhMuc);
            }
            cboDanhMuc.DataSource = new BindingSource(DanhMucDict, null);
            cboDanhMuc.DisplayMember = "Value"; 
            cboDanhMuc.ValueMember = "Key";
        }

        private void cboDanhMuc_SelectedIndexChanged(object sender, EventArgs e)
        {
            var selectedValue = cboDanhMuc.SelectedValue.ToString();
            if (selectedValue == "a")
            {
                List<ThongKeDanhMuc> ThongKeDanhMucs = DoanhThuNhomHangController.getAllThongKeDanhMucs();
                dgv.Rows.Clear();
                var d = 0;
                foreach (ThongKeDanhMuc ThongKeDanhMuc in ThongKeDanhMucs)
                {
                    d++;
                    dgv.Rows.Add(d, ThongKeDanhMuc.DanhMucId, ThongKeDanhMuc.TenDanhMuc, ThongKeDanhMuc.TongSoLuong);
                }
                d = 0;
            }
            else
            {
                // MessageBox.Show("Đã chọn danh mục: " + selectedValue);
                List<ThongKeDanhMuc> ThongKeDanhMucs = DoanhThuNhomHangController.cboThongKeDanhMucs(selectedValue);
                dgv.Rows.Clear();
                var d = 0;
                foreach (ThongKeDanhMuc ThongKeDanhMuc in ThongKeDanhMucs)
                {
                    d++;
                    dgv.Rows.Add(d, ThongKeDanhMuc.DanhMucId, ThongKeDanhMuc.TenDanhMuc, ThongKeDanhMuc.TongSoLuong);
                }
                d = 0;
            }
           
            
        }

        private void doanhThuNhomHangUserControl_Load(object sender, EventArgs e)
        {
            LoadingData();
        }
    }
}