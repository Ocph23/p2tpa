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
    /// Interaction logic for LayananYangDiberikanKeKorban.xaml
    /// </summary>
    public partial class LayananYangDiberikanKeKorban : ChartMaster
    {
        public LayananYangDiberikanKeKorban()
        {
            InitializeComponent();
            this.RefreshChartCommand = new CommandHandler { CanExecuteAction = x => true, ExecuteAction = RefreshAction };
            this.PrintCommand = new CommandHandler { CanExecuteAction = x => true, ExecuteAction = PrintAction };
            this.RefreshChartCommand.Execute(null);
            Title = "Jenis Layanan Yang Diberikan Kepada Korban";
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
            var result = from p in DataAccess.DataBasic.DataPengaduan
                         from b in EnumSource.GetDataLayananKorban()
                         from korban in p.Korban
                         from layanan in korban.DataPenanganan where layanan.IdentitasType=="Korban"
                         group layanan by b.Name into counts
                         select new { Key = counts.Key, value = counts };

            List<string> labels = new List<string>();
            datgrafirk.Clear();
            int number = 0;
            foreach (var item in EnumSource.GetDataLayananKorban())
            {
                labels.Add("");
                var value = 0;
                var data = result.Where(x => x.Key == item.Name).FirstOrDefault();
                if (data != null)
                    value = data.value.Where(x=>x.Layanan==item.Name).Count();

                SeriesCollection.Add(new ColumnSeries
                {
                    DataLabels = true,
                    Title = $"{item.Name}",
                    Values = new ChartValues<int> { value }
                });

                number++;
                datgrafirk.Add(new GrafikModel { NilaiText = number.ToString(), Kategori = item.Name, Series = item.Name, Nilai = value, Title = Title });

            }

            Labels = labels.ToArray();
            //new[] { "Jan", "Feb", "Mar", "Apr", "May" };
            YFormatter = value => ((int)value).ToString("N");
            XFormatter = value => ((int)value) <= 0 ? "" : ((int)value).ToString("N");

        }


    }
}
