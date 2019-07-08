using Main.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Main.Reports.Models
{
   public class CiriKorbanDanPelaku
    {
        public int Nomor { get; set; }
        public int Kasus { get; set; }
        public string Distrik { get; set; }
        public int usia5 { get; set; }
        public int usia12 { get; set; }

        public int usia17 { get; set; }
        public int usia24 { get; set; }
        public int usia44 { get; set; }
        public int usia59 { get; set; }
        public int usia60 { get; set; }
        public int usiaanak { get { return usia5 + usia12 + usia17; } }

        public int usiadewasa{ get { { return usia24 + usia44 + usia59 +usia60; } } }


        public int PendNa { get; set; }
        public int PendTs { get; set; }
        public int PendSd { get; set; }
        public int PendSmp { get; set; }
        public int PendSlta { get; set; }
        public int PendPt { get; set; }
        public int PendTK { get; set; }
        public int PendPaud { get; set; }


        public int PekNa { get; set; }
        public int PekTbekerja { get; set; }
        public int PekBekerja { get; set; }
        public int PekIrt { get; set; }
        public int PekSwasta { get; set; }
        public int PekPns { get; set; }

        public int PekPedagang { get; set; }

        public int KawinNA { get; set; }

        public int KawinBelum{ get; set; }
        public int KawinKawin { get; set; }
        public int KawinCerai { get; set; }

        public int Total { get; set; }
        public string Gender { get;  set; }
        public int PekPelajar { get; internal set; }
    }
}
