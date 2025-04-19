using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LapStore.Model
{
    internal class GiamGia
    {
        public string id { get; set; }
        public string tenGiamGia { get; set; }
        public string soGiamGia { get; set; }

        // Constructor mặc định
        public GiamGia() { }

        // Constructor có tham số
        public GiamGia(string id, string tenGiamGia, string soGiamGia)
        {
            this.id = id;
            this.tenGiamGia = tenGiamGia;
            this.soGiamGia = soGiamGia;
        }
    }
}
