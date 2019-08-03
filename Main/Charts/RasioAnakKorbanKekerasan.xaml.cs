using LiveCharts;
using LiveCharts.Wpf;
using Main.Reports;
using Main.Reports.Models;
using Microsoft.Reporting.WinForms;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Main.Charts
{
    /// <summary>
    /// Interaction logic for RasioAnakKorbanKekerasan.xaml
    /// </summary>
    public partial class RasioAnakKorbanKekerasan : ChartMaster
    {
        public RasioAnakKorbanKekerasan()
        {
            InitializeComponent();
            this.RefreshChartCommand = new CommandHandler { CanExecuteAction = x => true, ExecuteAction = RefreshAction };
            this.PrintCommand = new CommandHandler { CanExecuteAction = x => true, ExecuteAction = PrintAction };
            this.RefreshChartCommand.Execute(null);
            Title = "Rasio Anak Korban Kekerasan";
            this.DataContext = this;
        }
        public CommandHandler PrintCommand { get; }
        List<GrafikModel> datgrafirk = new List<GrafikModel>();

        private void PrintAction(object obj)
        {
            var header = new ReportHeader { Title = Title, Tahun = DateTime.Now.Year };
            HelperPrint.PrintWithFormActionTwoSource("Print Preview",
                  new ReportDataSource { Name = "Header", Value = new List<ReportHeader>() { header } },
                new ReportDataSource { Name = "DataSet1", Value = datgrafirk },
               "Main.Reports.Layout.GrafikRatioSeries.rdlc", null);
        }
        private void RefreshAction(object obj)
        {
            var dataKec = (from a in DataAccess.DataBasic.DataPendudukPerKecamatan() select a);
            var groupPengaduan = DataAccess.DataBasic.DataPengaduan.GroupBy(x => x.KodeDistrik);

            List<double> rasio = new List<double>();
            List<double> jumlahKorbanAnak = new List<double>();
            datgrafirk.Clear();
            foreach (var kec in dataKec)
            {
                var model = new GrafikModel { Kategori = kec.Nama, Series = kec.Nama };
                var kasus = groupPengaduan.Where(x => x.Key == kec.Id).FirstOrDefault();

                if (kasus != null)
                {
                    var listkasusAnak = (from p in kasus
                                     from korban in p.Korban
                                     where korban.TanggalLahir != null
                                     let age = p.TanggalLapor.Value.Year - korban.TanggalLahir.Year
                                     group p by
                                        age < 18 ? "Anak" :"Dewasa" into ages
                                     select new { Age = ages.Key, Persons = ages });


                    Double kasusAnak = listkasusAnak.Where(x => x.Age == "Anak").Count();
                    var total = kasusAnak + listkasusAnak.Where(x => x.Age == "Dewasa").Count();
                    double data = (Convert.ToDouble((kasusAnak / kec.Total)*1000));
                    rasio.Add(Math.Round(data, 2));
                    jumlahKorbanAnak.Add(kasusAnak);
                    model.Nilai = Math.Round(data, 2);
                    model.Nilai2 = kasusAnak;
                }
                else
                {
                    rasio.Add(0);
                    jumlahKorbanAnak.Add(0);
                    model.Nilai = 0;
                    model.Nilai2 = 0;
                }

                datgrafirk.Add(model);

            }
            SeriesCollection = new SeriesCollection
            {
                new LineSeries
                {
                    Title = "Rasio",
                    Values = new ChartValues<double>(rasio),
                },
                  new ColumnSeries
                {
                    Title = "Jumlah Korban Anak",
                    Values = new ChartValues<double>(jumlahKorbanAnak)
                }
            };

            Labels = (from a in DataAccess.DataBasic.GetKecamatan() select a.Nama).ToArray();
            //new[] { "Jan", "Feb", "Mar", "Apr", "May" };
            //YFormatter = value => value.ToString("C");
        }
    }
}
