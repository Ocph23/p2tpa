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
    /// Interaction logic for PelakuBerdasarkanHubungan.xaml
    /// </summary>
    public partial class PelakuBerdasarkanHubungan : ChartMaster
    {
        public PelakuBerdasarkanHubungan()
        {
            InitializeComponent();
            this.RefreshChartCommand = new CommandHandler { CanExecuteAction = x => true, ExecuteAction = RefreshAction };
            this.PrintCommand = new CommandHandler { CanExecuteAction = x => true, ExecuteAction = PrintAction };
            this.RefreshChartCommand.Execute(null);
            Title = "PelakuBerdasarkanHubungan";
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
            var groupPengaduan = from a in DataAccess.DataBasic.DataPengaduan
                                 from b in a.Terlapor.Where(x=>x.Hubungan.Count()>0)
                                 from d in b.Hubungan.DefaultIfEmpty()
                                 group d by d.JenisHubungan into dGroup
                                 select new { Key = dGroup.Key, Values = dGroup };
            datgrafirk.Clear();
            int number = 0;
            foreach(var item in EnumSource.HubunganKorbanDenganTerlapor())
            {
                var result = groupPengaduan.Where(x => x.Key == item).FirstOrDefault();
                var value = 0;
                if(result !=null)
                {
                    value = result.Values.Count();
                }
                SeriesCollection.Add(new ColumnSeries { DataLabels = true, Title = item, Values = new ChartValues<double> { value } });
                number++;
                datgrafirk.Add(new GrafikModel { NilaiText = number.ToString(), Kategori = item, Series = item, Nilai = value, Title = Title });
            }

          //  Labels = EnumSource.HubunganKorbanDenganTerlapor().ToArray();
            //new[] { "Jan", "Feb", "Mar", "Apr", "May" };
            YFormatter = value => ((int)value).ToString("N");
            XFormatter = value => ((int)value) <= 0 ? "" : ((int)value).ToString("N");
        }
    }
}
