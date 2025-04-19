using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LapStore.Model
{
    internal class DanhMuc
    {
        public string id { get; set; }
        public string tenDanhMuc { get; set; }

        // Constructor mặc định
        public DanhMuc() { }

        // Constructor có tham số
        public DanhMuc(string id, string tenDanhMuc)
        {
            this.id = id;
            this.tenDanhMuc = tenDanhMuc;
        }
    }
}
