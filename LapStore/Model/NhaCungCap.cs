using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LapStore.Model
{
    internal class NhaCungCap
    {
        public string id { get; set; }
        public string tenNhaCungCap { get; set; }
        public string diaChi { get; set; }

        // Constructor mặc định
        public NhaCungCap() { }

        // Constructor có tham số
        public NhaCungCap(string id, string tenNhaCungCap, string soGiamGia)
        {
            this.id = id;
            this.tenNhaCungCap = tenNhaCungCap;
            this.diaChi = soGiamGia;
        }
    }
}
