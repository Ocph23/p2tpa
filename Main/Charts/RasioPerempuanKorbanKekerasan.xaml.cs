using LiveCharts;
using LiveCharts.Wpf;
using Main.Reports;
using Main.Reports.Models;
using Microsoft.Reporting.WinForms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;

namespace Main.Charts
{
    /// <summary>
    /// Interaction logic for RasioPerempuanKorbanKekerasan.xaml
    /// </summary>
    public partial class RasioPerempuanKorbanKekerasan : ChartMaster
    {
        public RasioPerempuanKorbanKekerasan()
        {
            InitializeComponent();
            this.RefreshChartCommand = new CommandHandler { CanExecuteAction = x => true, ExecuteAction = RefreshAction };
            this.PrintCommand = new CommandHandler { CanExecuteAction = x => true, ExecuteAction = PrintAction };
            this.RefreshChartCommand.Execute(null);
            Title = "Rasio Perempuan Korban Kekerasan";
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
            List<int> jumlahKorbanPerempuan = new List<int>();
            datgrafirk.Clear();
            foreach (var kec in dataKec)
            {
                var model = new GrafikModel { Kategori = kec.Nama, Series = kec.Nama };
                var kasus = groupPengaduan.Where(x => x.Key == kec.Id).FirstOrDefault();
                if (kasus != null)
                {
                    var kasusPerem = (from a in kasus
                                     from korban in a.Korban where korban.Gender == Gender.P select korban).Count();
                    rasio.Add((Convert.ToDouble(100000/kec.Perempuan)/ kasusPerem));
                    jumlahKorbanPerempuan.Add(kasusPerem);
                    model.Nilai = (Convert.ToDouble(100000 / kec.Perempuan) / kasusPerem);
                    model.Nilai2 = kasusPerem;
                }
                else
                {
                    rasio.Add(0);
                    jumlahKorbanPerempuan.Add(0);
                    model.Nilai =0;
                    model.Nilai2 =0;
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
                    Title = "Jumlah Korban Perempuan",
                    Values = new ChartValues<int>(jumlahKorbanPerempuan)
                }
            };

            Labels = (from a in DataAccess.DataBasic.GetKecamatan() select a.Nama).ToArray();
            //new[] { "Jan", "Feb", "Mar", "Apr", "May" };
            //YFormatter = value => value.ToString("C");
        }

    }
}
