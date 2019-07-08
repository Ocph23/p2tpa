using Main.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Main.DataAccess
{
    public static class DataExtention
    {


        public static IEnumerable<IGrouping<string, dynamic>> GroupByAge(this IEnumerable<Pengaduan> list, int baseLaporan)
        {

            if (baseLaporan == 2)
            {
                var groupsAge = from p in list.ToList()
                                from k in p.Korban
                                let age = p.TanggalLapor.Value.Year - k.TanggalLahir.Year
                                group p by age < 18 ? "Anak" : "Dewasa"
                             into ages
                                select ages ;


                return groupsAge;
            }
            else
            {
                var groupsAge = from p in list.ToList()
                                from k in p.Korban
                                let age = p.TanggalKejadian.Value.Year - k.TanggalLahir.Year
                                group p by age < 18 ? "Anak" : "Dewasa"
                            into ages
                                select ages;

                return groupsAge;
            }

        }



        public static IEnumerable<IGrouping<Tuple<string, int>, dynamic>> GroupAgeOfKorban(this IGrouping<Gender, dynamic> list, int baseLaporan)
        {
           
            if(baseLaporan==2)
            {
                var groupsAge = from p in list.ToList()
                                let age = p.TanggalLapor.Year - p.Data.TanggalLahir.Year

                                group p by age < 6 ? Tuple.Create<string, int>("0-5", 5) :
                                  age < 13 ? Tuple.Create<string, int>("6-12", 12) :
                                  age < 18 ? Tuple.Create<string, int>("13-17", 17) :
                                  age < 25 ? Tuple.Create<string, int>("18-24", 24) :
                                  age < 45 ? Tuple.Create<string, int>("25-44", 44) :
                                  age < 60 ? Tuple.Create<string, int>("45-59", 59) :
                                  Tuple.Create<string, int>("60+", 60)
                             into ages
                                select ages;


                return groupsAge;
            }else
            {
                var groupsAge = from p in list.ToList()
                                let age = p.TanggalKejadian.Year - p.Data.TanggalLahir.Year

                                group p by age < 6 ? Tuple.Create<string, int>("0-5", 5) :
                                  age < 13 ? Tuple.Create<string, int>("6-12", 12) :
                                  age < 18 ? Tuple.Create<string, int>("13-17", 17) :
                                  age < 25 ? Tuple.Create<string, int>("18-24", 24) :
                                  age < 45 ? Tuple.Create<string, int>("25-44", 44) :
                                  age < 60 ? Tuple.Create<string, int>("45-59", 59) :
                                  Tuple.Create<string, int>("60+", 60)
                             into ages
                                select ages;


                return groupsAge;
            }

        }

        public static IEnumerable<IGrouping<Gender, dynamic>> GroupByKorbanGender(this List<Pengaduan> list)
        {

            var korbans = from p in list
                          from korban in p.Korban
                          select new { TanggalLapor  = p.TanggalLapor, TanggalKejadian=p.TanggalKejadian, Data = korban };

            return korbans.GroupBy(x => x.Data.Gender);


        }


        public static IEnumerable<IGrouping<Gender, dynamic>> GroupByTerlaporGender(this List<Pengaduan> list)
        {
            var terlapors = from p in list
                          from terlapor in p.Terlapor
                          select new { TanggalLapor = p.TanggalLapor, TanggalKejadian = p.TanggalKejadian, Data = terlapor };

            return terlapors.GroupBy(x => x.Data.Gender);


        }




        public static IEnumerable<IGrouping<dynamic, dynamic>> GroupPendidikan(this IGrouping<Gender, dynamic> list)
        {

            var groupsPendidkan = list.ToList().GroupBy(x => x.Data.Pendidikan);

            return groupsPendidkan;

        }


        public static IEnumerable<IGrouping<dynamic, dynamic>> GroupPekerjaan(this IGrouping<Gender, dynamic> list)
        {

            var c = list.Count();

           return list.ToList().GroupBy(x => x.Data.Pekerjaan);

        }


        public static IEnumerable<IGrouping<dynamic, dynamic>> GroupPernikahan(this IGrouping<Gender, dynamic> list)
        {

            var groupsPernikahan= list.ToList().GroupBy(x => x.Data.Pernikahan);

            return groupsPernikahan;

        }

        public static IEnumerable<IGrouping<dynamic, dynamic>> GroupHubungan(this IGrouping<Gender, dynamic> list)
        {

            var groupsPernikahan = from a in list.ToList().Select(x => x.Data as Terlapor)
                                   from c in a.Hubungan
                                   select c;

            return groupsPernikahan.GroupBy(x=>x.JenisHubungan);

        }





        public static IEnumerable<IGrouping<dynamic, dynamic>> GroupTempatKejadian(this IGrouping<Gender, dynamic> list)
        {

            var c = list.Count();

            return list.ToList().GroupBy(x => x.Data.Pekerjaan);

        }



        public static IEnumerable<IGrouping<dynamic, dynamic>> GroupBentukKekerasan(this IGrouping<Gender, dynamic> list)
        {

            var c = list.Count();

            return list.ToList().GroupBy(x => x.Data.Pekerjaan);

        }


        public static IEnumerable<IGrouping<dynamic, dynamic>> GroupBentukLayanan(this IGrouping<Gender, dynamic> list)
        {

            var c = list.Count();

            return list.ToList().GroupBy(x => x.Data.Pekerjaan);

        }




    }
}
