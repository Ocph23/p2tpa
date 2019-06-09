using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Main.Models
{

    public class Kecamatan
    {
        public string Id { get; set; }
        public string Nama { get; set; }
    }

    public class DataPenduduk : Kecamatan
    {
        public int Laki { get; set; }
        public int Perempuan { get; set; }

        public int Total
        {
            get
            {
                return Laki + Perempuan;
            }
        }


    }

    public class DataPekerja : Kecamatan
    {
        public int Kerja { get; set; }
        public int Menganggur { get; set; }
    }
}
