using Main.DataAccess;
using Main.Models;
using Main.Reports.Models;
using Microsoft.Reporting.WinForms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Main.Reports
{
    /// <summary>
    /// Interaction logic for ReportFilter.xaml
    /// </summary>
    public partial class ReportFilter : Window
    {
        public ReportFilter()
        {
            InitializeComponent();
            this.DataContext = new ReportVilterViewModel() { WindowClose=this.Close};
        }
    }


    public class ReportVilterViewModel:ReportModel
    {
        public ReportVilterViewModel()
        {

            ShowCommand = new CommandHandler { CanExecuteAction = ShowValidation, ExecuteAction = ShowAction };

            CancelCommand = new CommandHandler { CanExecuteAction =x=>true, ExecuteAction =x=> WindowClose() };

            JenisReports = new Dictionary<int, string>();
            JenisReports.Add(1, "Ciri Korban");
            JenisReports.Add(2, "Ciri Pelaku");
            JenisReports.Add(3, "Bentuk Kekerasan,Tempat Kejadian dan Pelayanan");
            JenisReports.Add(4, "Kasus & Korban Anak/Dewasa Terlayani");
            JenisReports.Add(5, "Kasus & Korban Laki-Laki/Perempuan Telayani");


            BasisPeriodes = new Dictionary<int, string>();
            BasisPeriodes.Add(1, "Tanggal Kejadian");
            BasisPeriodes.Add(2, "Tanggal Pelaporan");

            Triwulans = new List<int>() { 1,2,3,4};
            Semesters = new List<int>() { 1,2};
            Tahuns = new List<int>();
            for( int  i=Tahun; i>Tahun-4;i--)
            {
                Tahuns.Add(i);
            }


            Genders = new Dictionary<Gender, string>();
            Genders.Add(Gender.None, "");
            Genders.Add(Gender.L, "Laki-Laki");
            Genders.Add(Gender.P, "Perempuan");



            BaseReports = new Dictionary<int, string>();
            BaseReports.Add(1, "Wilayah");
            BaseReports.Add(2, "Instansi");
            DataKategoriInstansi = Enum.GetValues(typeof(KategoriInstansi));
            DataTingkat = Enum.GetValues(typeof(TingakatInstansi));
            DataInstansi = DataAccess.DataBasic.DataInstansi;


        }

        private bool ShowValidation(object obj)
        {

            if (JenisReport <= 0)
                return false;

            if (BasisPeriode <= 0)
                return false;


            switch (PeriodeLaporan)
            {
                case PeriodeLaporanType.Tanggal:
                    if (DariTanggal == null || SampaiTanggal == null || Tahun <= 0)
                        return false;
                    break;
                case PeriodeLaporanType.Triwulan:
                    if (DariTriwulan <=0 || SampaiTriwulan <=0 || Tahun <= 0)
                        return false;

                    if (DariTriwulan > SampaiTriwulan)
                        return false;


                    break;
                case PeriodeLaporanType.Semester:
                    if (DariSemester <= 0 || SampaiSemester<= 0 || Tahun <= 0)
                        return false;

                    if (DariSemester > SampaiSemester)
                        return false;
                    break;
                default:
                    break;
            }


            return true;
        }

        private void ShowAction(object obj)
        {

          

            switch (JenisReport)
            {
                case 1:
                    ProccesCiriKorban();
                    break;
                case 2:
                    ProccesCiriTerlapor();
                    break;

                case 3:
                    ProccesBentukKekerasan();
                    break;

                case 4:
                    ProccesAnakDewasaTerlayani();
                    break;

                case 5:
                    ProccesGenderTerlayani();
                    break;

                default:
                    break;
            }

        }


        private void ProccesCiriTerlapor()
        {
            var rangetanggal = GetTanggal();
            List<Pengaduan> pengaduans;
            if (this.BasisPeriode <= 1)
            {
                pengaduans = DataBasic.DataPengaduan.Where(x => x.TanggalKejadian >= rangetanggal.Item1 && x.TanggalKejadian <= rangetanggal.Item2).ToList();
            }
            else
            {
                pengaduans = DataBasic.DataPengaduan.Where(x => x.TanggalLapor >= rangetanggal.Item1 && x.TanggalLapor <= rangetanggal.Item2).ToList();
            }

            List<CiriTerlapor> listModel = new List<Models.CiriTerlapor>();

            var groupbyDistrik = pengaduans.GroupBy(x => x.KodeDistrik);
            foreach (var list in groupbyDistrik)
            {
                var genders = list.ToList().GroupByTerlaporGender();
                foreach (var gender in genders)
                {

                    var pekerjaans = gender.GroupPekerjaan().ToList();

                    var model = new CiriTerlapor();
                    model.Distrik = DataBasic.GetKecamatan().Where(x => x.Id == list.Key).FirstOrDefault().Nama;
                    model.Kasus = list.Count();
                    foreach (var pekerjaan in pekerjaans)
                    {
                        switch (pekerjaan.Key)
                        {
                            case "NA":
                                model.PekNa = pekerjaan.Count();
                                break;

                            case "Bekerja":
                                model.PekBekerja = pekerjaan.Count();
                                break;
                            case "Pelajar":
                                model.PekPelajar = pekerjaan.Count();
                                break;

                            case "Ibu Rumah Tangga":
                                model.PekIrt = pekerjaan.Count();
                                break;

                            case "Swasta/Buruh":
                                model.PekSwasta = pekerjaan.Count();
                                break;


                            case "PNS/TNI/POLRI":
                                model.PekPns = pekerjaan.Count();
                                break;


                            case "Pedagang/Tani/Nelayan":
                                model.PekPedagang = pekerjaan.Count();
                                break;


                            case "Tidak Bekerja":
                                model.PekTbekerja = pekerjaan.Count();
                                break;

                            default:
                                model.PekNa = pekerjaan.Count();
                                break;
                        }
                    }


                    var pendidkans = gender.GroupPendidikan();


                    foreach (var pendidikan in pendidkans)
                    {
                        switch (pendidikan.Key)
                        {
                            case "Tidak Sekolah":
                                model.PendTs = pendidikan.Count();
                                break;

                            case "Paud":
                                model.PendPaud = pendidikan.Count();
                                break;

                            case "TK":

                                model.PendTK = pendidikan.Count();
                                break;

                            case "SD":
                                model.PendSd = pendidikan.Count();
                                break;

                            case "SLTP":
                                model.PendSmp = pendidikan.Count();
                                break;

                            case "SLTA":
                                model.PendSlta = pendidikan.Count();
                                break;

                            case "Perguruan Tinggi":
                                model.PendPt = pendidikan.Count();
                                break;

                            default:
                                model.PendNa = pendidikan.Count();
                                break;
                        }
                    }



                    var ages = gender.GroupAgeOfKorban(BasisPeriode);
                    foreach (var item in ages)
                    {
                        model.Gender = gender.Key.ToString();

                        //    var count = model.Count();
                        switch (item.Key.Item2)
                        {
                            case 5:
                                model.usia5 = item.Count();
                                break;
                            case 12:
                                model.usia12 = item.Count();
                                break;
                            case 17:
                                model.usia17 = item.Count();
                                break;
                            case 24:
                                model.usia24 = item.Count();
                                break;
                            case 44:
                                model.usia44 = item.Count();
                                break;
                            case 49:
                                model.usia59 = item.Count();
                                break;
                            default:
                                model.usia60 = item.Count();
                                break;
                        }


                    }


                    var perkawinans = gender.GroupPernikahan();

                    foreach (var status in perkawinans)
                    {
                        model.Gender = gender.Key.ToString();

                        //    var count = model.Count();
                        switch (status.Key)
                        {
                            case "Tidak Ada Status Nikah":
                                model.KawinBelum = status.Count();
                                break;

                            case "Cerai":
                                model.KawinCerai = status.Count();
                                break;

                            default:
                                model.KawinKawin = status.Count();

                                break;

                        }
                    }




                    var hubungans = gender.GroupHubungan();
                    foreach (var hub in hubungans)
                    {
                        switch (hub.Key)
                        {

                            case "Pacar":
                                model.HubPacar = hub.Count();
                                break;

                            case "Saudara Kandung":
                                model.HubKelu += hub.Count();
                                break;

                            case "Paman":
                                model.HubKelu += hub.Count();
                                break;

                            case "Bibi":
                                model.HubKelu += hub.Count();
                                break;

                            case "Saudara Tiri":
                                model.HubKelu += hub.Count();
                                break;

                            case "Saudara Sepupu":
                                model.HubKelu += hub.Count();
                                break;


                            case "Istri":
                                model.HubSI += hub.Count();
                                break;


                            case "Suami":
                                model.HubSI += hub.Count();
                                break;


                            case "Tetangga":
                                model.HubTetangga = hub.Count();
                                break;

                            default:
                                model.HubLain += hub.Count();
                                break;
                        }
                    }


                    listModel.Add(model);

                }
                var header = new ReportHeader { Title = "REPORT CIRI TERLAPOR", Tahun = Tahun, BaseSelection = GetBaseSelection(), SubSelection1 = GetSub1() };
                HelperPrint.PrintWithFormActionTwoSource("Print Preview",
                      new ReportDataSource { Name = "Header", Value = new List<ReportHeader>() { header } },
                    new ReportDataSource { Name = "DataSet1", Value = listModel },
                        "Main.Reports.Layout.CiriKorban.rdlc", null);
            }


        }

        private void ProccesCiriKorban()
        {
            var rangetanggal = GetTanggal();
            List<Pengaduan> pengaduans;
            if (this.BasisPeriode <= 1)
            {
                pengaduans = DataBasic.DataPengaduan.Where(x => x.TanggalKejadian >= rangetanggal.Item1 && x.TanggalKejadian <= rangetanggal.Item2).ToList();
            }
            else
            {
                pengaduans = DataBasic.DataPengaduan.Where(x => x.TanggalLapor >= rangetanggal.Item1 && x.TanggalLapor <= rangetanggal.Item2).ToList();
            }

            List<CiriKorbanDanPelaku> listModel = new List<Models.CiriKorbanDanPelaku>();

            var groupbyDistrik = pengaduans.GroupBy(x => x.KodeDistrik);
            foreach (var list in groupbyDistrik)
            {
                var genders = list.ToList().GroupByKorbanGender();
                foreach (var gender in genders)
                {
                    var ages = gender.GroupAgeOfKorban(BasisPeriode);
                    var pendidkans = gender.GroupPendidikan();
                    var pekerjaans = gender.GroupPekerjaan();
                    var perkawinans = gender.GroupPernikahan();
                    var model = new CiriKorbanDanPelaku();
                    model.Distrik = DataBasic.GetKecamatan().Where(x => x.Id == list.Key).FirstOrDefault().Nama;
                    model.Kasus = list.Count();
                    foreach (var pekerjaan in pekerjaans)
                    {
                        switch (pekerjaan.Key)
                        {
                            case "NA":
                                model.PekNa = pekerjaan.Count();
                                break;

                            case "Bekerja":
                                model.PekBekerja = pekerjaan.Count();
                                break;
                            case "Pelajar":
                                model.PekPelajar = pekerjaan.Count();
                                break;

                            case "Ibu Rumah Tangga":
                                model.PekIrt = pekerjaan.Count();
                                break;

                            case "Swasta/Buruh":
                                model.PekSwasta = pekerjaan.Count();
                                break;


                            case "PNS/TNI/POLRI":
                                model.PekPns = pekerjaan.Count();
                                break;


                            case "Pedagang/Tani/Nelayan":
                                model.PekPedagang = pekerjaan.Count();
                                break;


                            case "Tidak Bekerja":
                                model.PekTbekerja = pekerjaan.Count();
                                break;

                            default:
                                model.PekNa = pekerjaan.Count();
                                break;
                        }
                    }

                    foreach (var pendidikan in pendidkans)
                    {
                        switch (pendidikan.Key)
                        {
                            case "Tidak Sekolah":
                                model.PendTs = pendidikan.Count();
                                break;

                            case "Paud":
                                model.PendPaud = pendidikan.Count();
                                break;

                            case "TK":

                                model.PendTK = pendidikan.Count();
                                break;

                            case "SD":
                                model.PendSd = pendidikan.Count();
                                break;

                            case "SLTP":
                                model.PendSmp = pendidikan.Count();
                                break;

                            case "SLTA":
                                model.PendSlta = pendidikan.Count();
                                break;

                            case "Perguruan Tinggi":
                                model.PendPt = pendidikan.Count();
                                break;

                            default:
                                model.PendNa = pendidikan.Count();
                                break;
                        }
                    }

                    foreach (var item in ages)
                    {
                        model.Gender = gender.Key.ToString();

                        //    var count = model.Count();
                        switch (item.Key.Item2)
                        {
                            case 5:
                                model.usia5 = item.Count();
                                break;
                            case 12:
                                model.usia12 = item.Count();
                                break;
                            case 17:
                                model.usia17 = item.Count();
                                break;
                            case 24:
                                model.usia24 = item.Count();
                                break;
                            case 44:
                                model.usia44 = item.Count();
                                break;
                            case 49:
                                model.usia59 = item.Count();
                                break;
                            default:
                                model.usia60 = item.Count();
                                break;
                        }


                    }

                    foreach (var status in perkawinans)
                    {
                        model.Gender = gender.Key.ToString();

                        //    var count = model.Count();
                        switch (status.Key)
                        {
                            case "Tidak Ada Status Nikah":
                                model.KawinBelum = status.Count();
                                break;

                            case "Cerai":
                                model.KawinCerai = status.Count();
                                break;

                            default:
                                model.KawinKawin = status.Count();

                                break;

                        }
                    }


                    listModel.Add(model);

                }
                var header = new ReportHeader { Title = "REPORT CIRI KORBAN", Tahun = Tahun, BaseSelection = GetBaseSelection(), SubSelection1 = GetSub1() };
                HelperPrint.PrintWithFormActionTwoSource("Print Preview",
                      new ReportDataSource { Name = "Header", Value = new List<ReportHeader>() { header } },
                    new ReportDataSource { Name = "DataSet1", Value = listModel },
                        "Main.Reports.Layout.CiriKorban.rdlc", null);
            }


        }


        private void ProccesGenderTerlayani()
        {
            var rangetanggal = GetTanggal();
            List<Pengaduan> pengaduans;
            if (this.BasisPeriode <= 1)
            {
                pengaduans = DataBasic.DataPengaduan.Where(x => x.TanggalKejadian >= rangetanggal.Item1 && x.TanggalKejadian <= rangetanggal.Item2).ToList();
            }
            else
            {
                pengaduans = DataBasic.DataPengaduan.Where(x => x.TanggalLapor >= rangetanggal.Item1 && x.TanggalLapor <= rangetanggal.Item2).ToList();
            }

            var groupbyDistrik = pengaduans.GroupBy(x => x.KodeDistrik);


            List<KorbanAnakDewasaTerlayani> source = new List<KorbanAnakDewasaTerlayani>();


            foreach (var distrik in groupbyDistrik)
            {
                var pengaduanTerlayani = distrik.Where(x => x.Korban.Count > 0);
                var agesTerlayani = pengaduanTerlayani.ToList().GroupByKorbanGender();


                var model = new KorbanAnakDewasaTerlayani();
                model.Kasus = distrik.Count();
                model.Distrik = DataBasic.GetKecamatan().Where(x => x.Id == distrik.Key).FirstOrDefault().Nama;
                foreach (var item in agesTerlayani)
                {
                    switch (item.Key)
                    {
                        case Gender.P:
                            model.TerlayaniAnak = item.Count();
                            break;
                        default:
                            model.TerlayaniDewasa = item.Count();
                            break;
                    }


                }

                var pengaduanTakTerlayani = distrik.Where(x => x.Korban.Count <= 0);
                var agesTakTerlayani = pengaduanTakTerlayani.ToList().GroupByKorbanGender();
                foreach (var item in agesTakTerlayani)
                {
                    switch (item.Key)
                    {
                        case Gender.P:
                            model.KorbanAnak = item.Count();
                            break;
                        default:
                            model.KorbanDewasa = item.Count();
                            break;
                    }


                }


                model.Nomor = source.Count + 1;
                source.Add(model);
            }

            var header = new ReportHeader { Title = "REPORT KASUS & KORBAN TERLAYANI", Tahun = Tahun, BaseSelection = GetBaseSelection(), SubSelection1 = GetSub1() };

            HelperPrint.PrintWithFormActionTwoSource("Print Preview",
                  new ReportDataSource { Name = "Header", Value = new List<ReportHeader>() { header } },
                new ReportDataSource { Name = "DataSet1", Value = source },
               "Main.Reports.Layout.GenderTelayani.rdlc", null);

        }

      
        private void ProccesAnakDewasaTerlayani()
        {
            var rangetanggal = GetTanggal();
            List<Pengaduan> pengaduans;
            if (this.BasisPeriode <= 1)
            {
                pengaduans = DataBasic.DataPengaduan.Where(x => x.TanggalKejadian >= rangetanggal.Item1 && x.TanggalKejadian <= rangetanggal.Item2).ToList();
            }
            else
            {
                pengaduans = DataBasic.DataPengaduan.Where(x => x.TanggalLapor >= rangetanggal.Item1 && x.TanggalLapor <= rangetanggal.Item2).ToList();
            }

            var groupbyDistrik = pengaduans.GroupBy(x => x.KodeDistrik);


            List<KorbanAnakDewasaTerlayani> source = new List<KorbanAnakDewasaTerlayani>();


            foreach (var distrik in groupbyDistrik)
            {
                var pengaduanTerlayani = distrik.Where(x => x.Korban.Count > 0);
                var agesTerlayani = pengaduanTerlayani.GroupByAge(BasisPeriode);
                var model = new KorbanAnakDewasaTerlayani();
                model.Kasus = distrik.Count();
                model.Distrik = DataBasic.GetKecamatan().Where(x => x.Id == distrik.Key).FirstOrDefault().Nama;
                foreach (var item in agesTerlayani)
                {
                    switch (item.Key)
                    {
                        case "Anak":
                            model.TerlayaniAnak = item.Count();
                            break;
                        default:
                            model.TerlayaniDewasa = item.Count();
                            break;
                    }


                }

                var pengaduanTakTerlayani = distrik.Where(x => x.Korban.Count <= 0);
                var agesTakTerlayani = pengaduanTakTerlayani.GroupByAge(BasisPeriode);
                foreach (var item in agesTakTerlayani)
                {
                    switch (item.Key)
                    {
                        case "Anak":
                            model.KorbanAnak = item.Count();
                            break;
                        default:
                            model.KorbanDewasa = item.Count();
                            break;
                    }


                }


                model.Nomor = source.Count + 1;
                source.Add(model);
            }
            var header = new ReportHeader { Title = "REPORT KASUS & KORBAN ANAK DEWASA TERLAYANI", Tahun = Tahun, BaseSelection = GetBaseSelection(), SubSelection1 = GetSub1() };

            HelperPrint.PrintWithFormActionTwoSource("Print Preview",
                  new ReportDataSource { Name = "Header", Value = new List<ReportHeader>() { header } },
                new ReportDataSource { Name = "DataSet1", Value = source },
               "Main.Reports.Layout.DewasaAnakTerlayani.rdlc", null);
        }

        private void ProccesBentukKekerasan()
        {
            var rangetanggal = GetTanggal();
            List<Pengaduan> pengaduans;
            if (this.BasisPeriode <= 1)
            {
                pengaduans = DataBasic.DataPengaduan.Where(x => x.TanggalKejadian >= rangetanggal.Item1 && x.TanggalKejadian <= rangetanggal.Item2).ToList();
            }
            else
            {
                pengaduans = DataBasic.DataPengaduan.Where(x => x.TanggalLapor >= rangetanggal.Item1 && x.TanggalLapor <= rangetanggal.Item2).ToList();
            }

            var groupbyDistrik = pengaduans.GroupBy(x => x.KodeDistrik);


            List<Kekerasandll> source = new List<Kekerasandll>();

            foreach (var distrik in groupbyDistrik)
            {
                var korbans = distrik.SelectMany(x => x.Korban).SelectMany(x => x.ListKekerasanDialami);
                var groupKekerasan = korbans.GroupBy(x => x);
                var model = new Kekerasandll();
                model.Kasus = distrik.Count();
                model.Unit = DataBasic.GetKecamatan().Where(x => x.Id == distrik.Key).FirstOrDefault().Nama;
                foreach (var item in groupKekerasan)
                {
                    switch (item.Key)
                    {
                        case "Fisik" :
                            model.KekerasanFisik = item.Count();
                            break;

                        case "Psikis":
                            model.KekerasanFisik = item.Count();
                            break;

                        case "Seksual":
                            model.KekerasanSeksual= item.Count();
                            break;

                        case "Eksploitasi":
                            model.KekerasanEksploitasi = item.Count();
                            break;

                        case "Trafficking":
                            model.KekerasanTrafficing= item.Count();
                            break;

                        case "Penelantaran":
                            model.KekerasanPenelantaran = item.Count();
                            break;

                        default:
                            model.KekerasanLainnya += item.Count();
                            break;
                    }
                }



                var pengananas = distrik.SelectMany(x => x.Korban).SelectMany(x => x.DataPenanganan).GroupBy(x=>x.Layanan);

                foreach (var item in pengananas)
                {
                    switch (item.Key)
                    {

                        case "Bantuan Humum":
                            model.JenisBantuanHukum= item.Count();
                            break;

                        case "Kesehatan":
                            model.JenisKesehatan= item.Count();
                            break;

                        case "Pemulangan":
                            model.JenisPemulangan = item.Count();
                            break;

                        case "Pendampingan Tokoh Agama":
                            model.JenisTokohAgama= item.Count();
                            break;

                        case "Penegakan Hukum":
                            model.JenisPenegakanHukum = item.Count();
                            break;


                        case "Pengaduan":
                            model.JenisPengaduan = item.Count();
                            break;

                        case "Rehabilitasi Sosial":
                            model.JenisRehabilitasi = item.Count();
                            break;

                        case "Reintegrasi Sosial":
                            model.JenisReintegrasi = item.Count();
                            break;

                    }
                }



                var tempats = distrik.GroupBy(x => x.TempatKejadian);
                foreach (var item in tempats)
                {
                    switch (item.Key)
                    {

                        case "Rumah Tangga":
                            model.TempatRumah = item.Count();
                            break;

                        case "Tempat Kerja":
                            model.TempatKerja = item.Count();
                            break;

                        case "Sekolah":
                            model.TempatSekolah = item.Count();
                            break;

                        case "Fasilitas Umum":
                            model.TempatUmum= item.Count();
                            break;

                        default:
                            model.TempatLainya += item.Count();
                            break;
                    }
                }

                model.Nomor = source.Count() + 1;
                source.Add(model);


            }
            var header = new ReportHeader { Title = "REPORT BENTUK KEKERASAN, TEMPAT KEJADIAN & PELAYANAN", Tahun = Tahun, BaseSelection = GetBaseSelection(), SubSelection1 = GetSub1() };

            HelperPrint.PrintWithFormActionTwoSource("Print Preview",
                  new ReportDataSource { Name = "Header", Value = new List<ReportHeader>() { header } },
                new ReportDataSource { Name = "DataSet1", Value = source },

                "Main.Reports.Layout.Kekerasandll.rdlc", null);
        }

        private string GetSub1()
        {
            switch (PeriodeLaporan)
            {
                case PeriodeLaporanType.Tanggal:
                    return $"Tanggal {DariTanggal.Value.ToShortDateString()} S/D TANGGAL {SampaiTanggal.Value.ToShortDateString()}";
                case PeriodeLaporanType.Triwulan:
                    var ta = GetTruwulan(DariTriwulan, Tahun);
                    var tw = GetTruwulan(SampaiTriwulan, Tahun);
                    return $"Triwulan {DariTriwulan} S/D Triwulan {SampaiTriwulan}";
                case PeriodeLaporanType.Semester:
                    var sa = GetSemester(DariSemester, Tahun);
                    var sw = GetSemester(SampaiSemester, Tahun);
                    return $"Semester {DariSemester} S/D Semester {SampaiSemester}";
                default:
                    return "";
            }
        }

        private string GetBaseSelection()
        {
            return BasisPeriodes.Where(x => x.Key == BasisPeriode).FirstOrDefault().Value;
        }

        private Tuple<DateTime?, DateTime?> GetTanggal()
        {
            DateTime? Dari = null;
            DateTime? Sampai = null;
            switch (PeriodeLaporan)
            {
                case PeriodeLaporanType.Tanggal:
                    Dari = DariTanggal;
                    Sampai = SampaiTanggal;
                    break;
                case PeriodeLaporanType.Triwulan:
                    var ta = GetTruwulan(DariTriwulan, Tahun);
                    var tw = GetTruwulan(SampaiTriwulan, Tahun);
                    Dari = ta.Item1;
                    Sampai = tw.Item2;
                    break;
                case PeriodeLaporanType.Semester:
                    var sa = GetSemester(DariSemester, Tahun);
                    var sw = GetSemester(SampaiSemester, Tahun);
                    Dari = sa.Item1;
                    Sampai = sw.Item2;
                    break;
                default:
                    break;
            }

            return Tuple.Create(Dari, Sampai);

        }

        private Tuple<DateTime,DateTime> GetTruwulan(int n, int tahun)
        {
            switch (n)
            {
                case 1:
                    return Tuple.Create(new DateTime(tahun, 1, 1), new DateTime(tahun, 3, 31));
                case 2:
                    return Tuple.Create(new DateTime(tahun, 4, 1), new DateTime(tahun, 6, 30));
                case 3:
                    return Tuple.Create(new DateTime(tahun, 7, 1), new DateTime(tahun, 9, 30));
                case 4:
                    return Tuple.Create(new DateTime(tahun, 10, 1), new DateTime(tahun, 12, 31));
                default:
                    return null;
            }
        }

        private Tuple<DateTime, DateTime> GetSemester(int n, int tahun)
        {
            switch (n)
            {
                case 1:
                    return Tuple.Create(new DateTime(tahun, 1, 1), new DateTime(tahun, 6, 30));
                case 2:
                    return Tuple.Create(new DateTime(tahun, 7, 1), new DateTime(tahun, 12, 31));
                default:
                    return null;
            }
        }

        public CommandHandler ShowCommand { get; }
        public CommandHandler CancelCommand { get; }
        public Dictionary<int, string> JenisReports { get; set; }

        public Dictionary<int, string> BasisPeriodes { get; set; }
        public List<int> Triwulans { get; }
        public List<int> Semesters { get; }
        public List<int> Tahuns { get; }
        public Dictionary<Gender, string> Genders { get; }
        public Dictionary<int, string> BaseReports { get; }
        public Array DataKategoriInstansi { get; }
        public Array DataTingkat { get; }
        public InstansiCollection DataInstansi { get; }
        public Action WindowClose { get;  set; }
    }





    public class ReportModel:BaseNotify
    {

        private int _jenisReport;

        public int JenisReport
        {
            get { return _jenisReport; }
            set { SetProperty(ref _jenisReport , value); }
        }

        private int basisPeriode;

        public int BasisPeriode
        {
            get { return basisPeriode; }
            set { SetProperty(ref basisPeriode , value); }
        }

        private DateTime? tanggalDari;
        private DateTime? tanggalsampai;

        public DateTime? DariTanggal
        {
            get { return tanggalDari; }
            set { SetProperty(ref tanggalDari, value); }
        }

        public DateTime? SampaiTanggal
        {
            get { return tanggalsampai; }
            set { SetProperty(ref tanggalsampai, value); }
        }




        private int tahun= DateTime.Now.Year;

        public int Tahun
        {
            get { return tahun; }
            set { SetProperty(ref tahun, value); }
        }

        private PeriodeLaporanType periodeLaporan;

        public PeriodeLaporanType PeriodeLaporan
        {
            get { return periodeLaporan; }
            set { SetProperty(ref periodeLaporan , value); }
        }

        private int triwulanDari;

        public int DariTriwulan
        {
            get { return triwulanDari; }
            set { SetProperty(ref triwulanDari , value); }
        }

        private int sampaiTriwulan;

        public int SampaiTriwulan
        {
            get { return sampaiTriwulan; }
            set { SetProperty(ref sampaiTriwulan , value); }
        }

        public int DariSemester
        {
            get { return semesterDari; }
            set { SetProperty(ref semesterDari, value); }
        }


        public int SampaiSemester
        {
            get { return sampaiSemester; }
            set { SetProperty(ref sampaiSemester, value); }
        }

        private Gender gender;

        public Gender Gender
        {
            get { return gender; }
            set {SetProperty(ref gender , value); }
        }

        private KategoriInstansi kategoriInstansi;

        public KategoriInstansi KategoriInstansi
        {
            get { return kategoriInstansi; }
            set { SetProperty(ref kategoriInstansi ,value); }
        }


        private Instansi instansi;

        public Instansi Instansi
        {
            get { return instansi; }
            set { SetProperty(ref instansi ,value); }
        }


        private int baseReport;
        private int semesterDari;
        private int sampaiSemester;

        public int BaseReport
        {
            get { return baseReport; }
            set {SetProperty(ref baseReport ,value); }
        }






    }


}
