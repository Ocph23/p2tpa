using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Main.Reports.Models
{
   public class Kekerasandll
    {
        public int Nomor { get; set; }
        public string Unit { get; set; }
        public int Kasus { get; set; }
        public int KekerasanFisik { get; set; }
        public int KekerasanPsikis{ get; set; }
        public int KekerasanSeksual { get; set; }
        public int KekerasanEksploitasi { get; set; }
        public int KekerasanTrafficing { get; set; }
        public int KekerasanPenelantaran { get; set; }
        public int KekerasanLainnya { get; set; }
        public int TempatRumah { get; set; }
        public int TempatKerja { get; set; }
        public int TempatLainya { get; set; }
        public int TempatSekolah { get; set; }
        public int TempatUmum { get; set; }
        public int TempatLembaga { get; set; }

        public int JenisPengaduan { get; set; }
        public int JenisKesehatan { get; set; }
        public int JenisBantuanHukum { get; set; }
        public int JenisPenegakanHukum { get; set; }

        public int JenisRehabilitasi { get; set; }
        public int JenisReintegrasi { get; set; }
        public int JenisPemulangan { get; set; }
        public int JenisTokohAgama { get; set; }
      
    }
}
