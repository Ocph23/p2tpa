using LiveCharts;
using LiveCharts.Wpf;
using Main.Reports;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Main.Charts
{
    /// <summary>
    /// Interaction logic for PelakuBerdasarkanKewarganegaraan.xaml
    /// </summary>
    public partial class PelakuBerdasarkanKewarganegaraan : ChartMaster
    {
        public PelakuBerdasarkanKewarganegaraan()
        {
            InitializeComponent(); this.RefreshChartCommand = new CommandHandler { CanExecuteAction = x => true, ExecuteAction = RefreshAction };
            this.PrintCommand = new CommandHandler { CanExecuteAction = x => true, ExecuteAction = PrintAction };
            this.RefreshChartCommand.Execute(null);
            Title = "Pelaku Berdasarkan Suku";
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
            var groupPengaduan = (from a in DataAccess.DataBasic.DataPengaduan
                                  from terlapor in a.Terlapor
                                  select terlapor).GroupBy(x => x.Suku);
            List<string> datagender = new List<string>() { "Papua", "NonPapua" };
            List<int> datas = new List<int>();
            datgrafirk.Clear();
            int number = 0;
            foreach (var data in datagender)
            {
                var value = 0;
                if (data == "Papua")
                {
                    var resultPria = groupPengaduan.Where(x => x.Key == "Papua").FirstOrDefault();
                    if (resultPria != null)
                        value = resultPria.Count();
                }
                else
                {
                    var resultWanita = groupPengaduan.Where(x => x.Key == "NonPapua").FirstOrDefault();
                    if (resultWanita != null)
                        value = resultWanita.Count();
                }
                SeriesCollection.Add(new ColumnSeries { DataLabels = true, Title = data, Values = new ChartValues<int> { value } });
                number++;
                datgrafirk.Add(new GrafikModel { NilaiText = number.ToString(), Kategori = data, Series = data, Nilai = value, Title = Title });
            }

           // Labels = datagender.ToArray();
            //new[] { "Jan", "Feb", "Mar", "Apr", "May" };
            YFormatter = value => ((int)value).ToString("N");
           // XFormatter = value => ((int)value) <= 0 ? "" : ((int)value).ToString("N");
        }
    }
}
