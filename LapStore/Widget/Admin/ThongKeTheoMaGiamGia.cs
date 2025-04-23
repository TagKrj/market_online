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
    public partial class ThongKeTheoMaGiamGia : System.Windows.Forms.UserControl
    {

        public ThongKeTheoMaGiamGia()
        {
            InitializeComponent();
        }
        void LoadingData()
        {

            List<GiamGia> GiamGias = GiamGiaController.getAllGiamGias();
            // cboGiamGia.Items.Clear();
            Dictionary<string, string> GiamGiaDict = new Dictionary<string, string>
            {
                { "a", "Tất cả mã giảm giá" }
            };
            foreach (GiamGia GiamGia in GiamGias)
            {
                GiamGiaDict.Add(GiamGia.id, GiamGia.tenGiamGia);
            }
            cboGiamGia.DataSource = new BindingSource(GiamGiaDict, null);
            cboGiamGia.DisplayMember = "Value";
            cboGiamGia.ValueMember = "Key";
        }

        private void cboGiamGia_SelectedIndexChanged(object sender, EventArgs e)
        {
            var selectedValue = cboGiamGia.SelectedValue.ToString();
            if (selectedValue == "a")
            {
                List<ThongKeGiamGia> ThongKeGiamGias = ThongKeTheoMaGiamGiaController.getAllThongKeGiamGias();
                dgv.Rows.Clear();
                var d = 0;
                foreach (ThongKeGiamGia ThongKeGiamGia in ThongKeGiamGias)
                {
                    d++;
                    dgv.Rows.Add(d, ThongKeGiamGia.GiamGiaId, ThongKeGiamGia.TenGiamGia, ThongKeGiamGia.TongSoLuong);
                }
                d = 0;
            }
            else
            {
                // MessageBox.Show("Đã chọn danh mục: " + selectedValue);
                List<ThongKeGiamGia> ThongKeGiamGias = ThongKeTheoMaGiamGiaController.cboThongKeGiamGias(selectedValue);
                dgv.Rows.Clear();
                var d = 0;
                foreach (ThongKeGiamGia ThongKeGiamGia in ThongKeGiamGias)
                {
                    d++;
                    dgv.Rows.Add(d, ThongKeGiamGia.GiamGiaId, ThongKeGiamGia.TenGiamGia, ThongKeGiamGia.TongSoLuong);
                }
                d = 0;
            }


        }

        private void ThongKeTheoMaGiamGia_Load(object sender, EventArgs e)
        {
            LoadingData();
        }

    }
}