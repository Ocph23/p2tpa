using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Main.Reports.Models
{
   public class KorbanAnakDewasaTerlayani
    {

        public int Nomor { get; set; }
        public int Kasus { get; set; }
        public string Distrik { get; set; }

        public double KorbanNa { get; set; }

        public double KorbanAnak { get; set; }
        public double KorbanDewasa { get; set; }

        public double TerlayaniNa { get; set; }
        public double TerlayaniAnak { get; set; }
        public double TerlayaniDewasa { get; set; }



    }



    public class KasusDanKorbanTerlayani
    {

        public int Nomor { get; set; }
        public int Kasus { get; set; }
        public string Distrik { get; set; }

        public double KorbanLaki { get; set; }
        public double KorbanPerempuan { get; set; }
        public double TerlayaniLaki { get; set; }
        public double TerlayaniPerempuan { get; set; }
        public double TerlayaniLakiPersen { get; set; }
        public double TerlayaniPerempuanPersen { get; set; }

        public double Total { get; set; }




    }
}
