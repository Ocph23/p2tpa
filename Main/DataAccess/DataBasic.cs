using Main.Models;
using Main.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Main.DataAccess
{


    public class DataBasic
    {
        private static PengaduanServices masterPengaduan;
        private static List<Pengaduan> pengaduans;
        private static InstansiCollection _instansi;

        public static List<DataPenduduk> DataPendudukPerKecamatan()
        {
            List<DataPenduduk> list = new List<DataPenduduk>();
            var data = @"010 Jair 9990 7484 17474
 #011 Subur 644 580 1224
 #013 Kia 848 853 1701
 #020 Mindiptana 1851 1683 3534
 #021 Iniyandit 445 388 833
 #022 Kombut 337 354 691
 #023 Sesnuk 1198 904 2102
 #030 Mandobo 7006 5766 12772
 #031 Fofi 990 956 1946
 #032 Arimop 659 611 1270
 #040 Kouh 591 595 1186
 #041 Bomakia 1116 1080 2196
 #042 Firiwage 530 558 1088
 #043 Manggelum 609 579 1188
 #044 Yaniruma 327 273 600
 #045 Kawagit 462 539 1001
 #046 Kombay 507 413 920
 #050 Waropko 894 838 1732
 #051 Ambatkwi 398 345 743
 #052 Ninati 293 298 591";


            var row = data.Split('#');
            foreach (string item in row)
            {
                var col = item.Trim().Split(' ');
                list.Add(new DataPenduduk
                {
                    Id = col[0],
                    Nama = col[1],
                    Laki = Convert.ToInt32(col[2]),
                    Perempuan = Convert.ToInt32(col[3])
                });

            }
            return list;
        }

        public static List<DataPekerja> DataPendudukKerjaPerKecamatan()
        {
            List<DataPekerja> list = new List<DataPekerja>();
            var data = @"010 Jair 8399 5 70
 #011 Subur 605 0 4
 #013 Kia 836 0 0
 #020 Mindiptana 1689 4 14
 #021 Iniyandit 438 0 0
 #022 Kombut 384 0 0
 #023 Sesnuk 995 0 1
 #030 Mandobo 5037 391 876
 #031 Fofi 652 0 6
 #032 Arimop 525 0 0
 #040 Kouh 556 0 0
 #041 Bomakia 732 1 8
 #042 Firiwage 529 0 0
 #043 Manggelum 671 0 0
 #044 Yaniruma 468 2 0
 #045 Kawagit 464 0 2
 #046 Kombay 666 0 0
 #050 Waropko 896 0 0
 #051 Ambatkwi 409 0 1
 #052 Ninati 294 0 1";


            var row = data.Split('#');
            foreach (string item in row)
            {
                var col = item.Trim().Split(' ');
                list.Add(new DataPekerja
                {
                    Id = col[0],
                    Nama = col[1],
                    Kerja = Convert.ToInt32(col[2]),
                    Menganggur = Convert.ToInt32(col[3]) + Convert.ToInt32(col[4])
                });

            }
            return list;
        }

        public static List<Kecamatan> GetKecamatan()
        {
          return (from kec in DataPendudukPerKecamatan() select new Kecamatan { Id=kec.Id, Nama=kec.Nama }).ToList();
        }


        public static List<Pengaduan> DataPengaduan
        {
            get
            {
                return MasterPengaduan.Where(x => x.TanggalLapor.Value.Year == DateTime.Now.Year).ToList(); ;
            }
        }


        public static PengaduanServices MasterPengaduan
        {
            get
            {
                if (masterPengaduan == null)
                {
                    masterPengaduan = new PengaduanServices();

                }

                return masterPengaduan;
            }
        }



        public static InstansiCollection DataInstansi
        {
            get
            {
                if(_instansi==null)
                {
                    _instansi = new InstansiCollection();
                }

                return _instansi;
            }
        }

        public static List<Instansi> GetDataInstansi
        {
            get
            {
                return DataInstansi.ToList();
            }
        }

    }


    


}
