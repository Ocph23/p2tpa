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
    /// Interaction logic for JumlahKasusberdasarkanTempatKejadian.xaml
    /// </summary>
    public partial class JumlahKasusberdasarkanTempatKejadian : ChartMaster
    {
        public JumlahKasusberdasarkanTempatKejadian()
        {
            InitializeComponent();
            this.RefreshChartCommand = new CommandHandler { CanExecuteAction = x => true, ExecuteAction = RefreshAction };
            this.PrintCommand = new CommandHandler { CanExecuteAction = x => true, ExecuteAction = PrintAction };
            this.RefreshChartCommand.Execute(null);
            Title = "Jumlah Kasus Berdasarkan Tempat Kejadian";
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
               "Main.Reports.Layout.GrafikBarLayout.rdlc", null);
        }


        private void RefreshAction(object obj)
        {
            SeriesCollection.Clear();
            var groupPengaduan = DataAccess.DataBasic.DataPengaduan.GroupBy(x => x.TempatKejadian);
            List<string> dataTempat = new List<string>();
            datgrafirk.Clear();
            int number = 0;
            foreach (var tempat in EnumSource.DataTempatKejadian())
            {
                dataTempat.Add("");
                var jumlah = groupPengaduan.Where(x => x.Key == tempat).GroupBy(x => x).Count();
                SeriesCollection.Add(new ColumnSeries { DataLabels = true, Title = tempat, Values = new ChartValues<int> { jumlah } });
                number++;
                datgrafirk.Add(new GrafikModel { NilaiText = number.ToString(), Kategori = tempat, Series = tempat, Nilai = jumlah, Title = Title });
            }

            Labels = dataTempat.ToArray();
            YFormatter = value => ((int)value).ToString("N");
            XFormatter = value => ((int)value) <= 0 ? " " : " ";
        }
       
    }
}
