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
    /// Interaction logic for LaporLangsung.xaml
    /// </summary>
    public partial class LaporLangsung : ChartMaster
    {
        public LaporLangsung()
        {
            InitializeComponent();
            this.RefreshChartCommand = new CommandHandler { CanExecuteAction = x => true, ExecuteAction = RefreshAction };
            this.PrintCommand = new CommandHandler { CanExecuteAction = x => true, ExecuteAction = PrintAction };
            this.RefreshChartCommand.Execute(null);

            Title = "Jumlah Kasus Lapor Langsung dan Rujukan";
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
                                  group a by
                                    string.IsNullOrEmpty(a.Rujukan) ? "Langsung" : "Rujukan" into items
                                  select new { Key = items.Key, Datas = items });

            List<string> dataTempat = new List<string>() { "Langsung","Rujukan"};
            List<int> datas = new List<int>();
            datgrafirk.Clear();
            int number = 0;
            foreach (var tempat in dataTempat)
            {
                var g  = groupPengaduan.Where(x => x.Key == tempat).FirstOrDefault();
                var jumlah = 0;
                if(g!=null)
                {
                    jumlah = g.Datas.Count();
                }
                SeriesCollection.Add(new ColumnSeries { DataLabels = true, Title = tempat, Values = new ChartValues<int> { jumlah } });
                number++;
                datgrafirk.Add(new GrafikModel { NilaiText = number.ToString(), Kategori = tempat, Series = tempat, Nilai = jumlah, Title = Title });
            }

            Labels = dataTempat.ToArray();
            //new[] { "Jan", "Feb", "Mar", "Apr", "May" };
            YFormatter = value => ((int)value).ToString("N");
            XFormatter = value => ((int)value) <= 0 ? "" : ((int)value).ToString("N");
        }
    }
}
