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
            this.DataContext = new ReportVilterViewModel();
        }
    }


    public class ReportVilterViewModel:ReportModel
    {
        public ReportVilterViewModel()
        {

            ShowCommand = new CommandHandler { CanExecuteAction = ShowValidation, ExecuteAction = ShowAction };

            JenisReports = new Dictionary<int, string>();
            JenisReports.Add(1, "Ciri Korban & Pelaku");
            JenisReports.Add(2, "Bentuk Kekerasan,Tempat Kejadian dan Pelayanan");
            JenisReports.Add(3, "Kasus & Korban Anak/Dewasa Terlayani");
            JenisReports.Add(4, "Kasus & Korban Laki-Laki/Perempuan Telayani");


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
            return true;
        }

        private void ShowAction(object obj)
        {

            var groupPengaduan = (from a in DataAccess.DataBasic.DataPengaduan
                                  from korban in a.Korban
                                  select korban);

            //var list = groupPengaduan.ToList();

            List<CiriKorbanDanPelaku> list = new List<Models.CiriKorbanDanPelaku>();
            list.Add(new CiriKorbanDanPelaku() { Distrik="Distrik A", Nomor=1}); 

            HelperPrint.PrintPreviewWithFormAction("Print Preview", new ReportDataSource { Name = "DataSet1", Value = list },
                "Main.Reports.Layout.CiriKorban.rdlc", null);

        }

        public CommandHandler ShowCommand { get; }
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

        public DateTime? TanggalDari
        {
            get { return tanggalDari; }
            set { SetProperty(ref tanggalDari , value); }
        }

        public DateTime? TanggalSampai
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

        public int BaseReport
        {
            get { return baseReport; }
            set {SetProperty(ref baseReport ,value); }
        }






    }


}
