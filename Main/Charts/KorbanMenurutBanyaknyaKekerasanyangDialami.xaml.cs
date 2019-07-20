using LiveCharts;
using LiveCharts.Wpf;
using Main.Models;
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
    /// Interaction logic for KorbanMenurutBanyaknyaKekerasanyangDialami.xaml
    /// </summary>
    public partial class KorbanMenurutBanyaknyaKekerasanyangDialami : ChartMaster
    {
        public KorbanMenurutBanyaknyaKekerasanyangDialami()
        {
            InitializeComponent();

            this.RefreshChartCommand = new CommandHandler { CanExecuteAction = x => true, ExecuteAction = RefreshAction };
            this.PrintCommand = new CommandHandler { CanExecuteAction = x => true, ExecuteAction = PrintAction };
            this.RefreshChartCommand.Execute(null);
            Title = "Korban Menurut Banyaknya Kekerasan Yang Dialami";
            this.DataContext = this;
        }
        public CommandHandler PrintCommand { get; }
        List<GrafikModel> datgrafirk = new List<GrafikModel>();

        private void PrintAction(object obj)
        {

            var header = new ReportHeader { Title = Title, Tahun = DateTime.Now.Year };

            HelperPrint.PrintWithFormActionTwoSource("Print Preview",
                  new ReportDataSource { Name = "Header", Value = new List<ReportHeader>() { header } },
                new ReportDataSource { Name = "DataSet1", Value = datgrafirk.OrderBy(O=>O.NilaiText) },
               "Main.Reports.Layout.GrafikBarLayout.rdlc", null);
        }
        private void RefreshAction(object obj)
        {


            var result = from p in DataAccess.DataBasic.DataPengaduan
                         from korban in p.Korban
                         where korban.ListKekerasanDialami != null
                         let c = korban.ListKekerasanDialami.Count
                         group p by
                            c <=1  ? "1 Jenis" :
                            c == 2 ? "2 Jenis" :
                            c == 3? "3 Jenis" : ">3 Jenis" into counts
                         select new {  Key = counts.Key, data = counts.Count() };





            List<string> labels = new List<string>();
            SeriesCollection.Clear();
            datgrafirk.Clear();
            int number = 0;

            var listS = new List<ColumnSeries>();
            foreach(var item in result.OrderBy(x=>x.Key))
            {
                number++;
                datgrafirk.Add(new GrafikModel { NilaiText = item.Key.Contains(">")?"9":number.ToString(), Kategori = item.Key, Series = item.Key, Nilai = item.data, Title = Title });

            }

            foreach(var item in datgrafirk.OrderBy(x=>x.NilaiText))
            {

                labels.Add(item.Series);
                SeriesCollection.Add(new ColumnSeries
                {
                    DataLabels = true,
                    Title = $"{item.Series}",
                    Values = new ChartValues<int> { (int)item.Nilai}
                });
                
            }



           // Labels = labels.ToArray();
            //new[] { "Jan", "Feb", "Mar", "Apr", "May" };
            YFormatter = value => value + "Jenis";
            this.DataContext = this;
        }

    }
}
