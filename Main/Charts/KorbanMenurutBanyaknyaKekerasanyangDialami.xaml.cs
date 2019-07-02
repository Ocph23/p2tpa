using LiveCharts;
using LiveCharts.Wpf;
using Main.Models;
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
            this.RefreshChartCommand.Execute(null);
            Title = "Korban Menurut Banyaknya Kekerasan Yang Dialami";
            this.DataContext = this;
        }

        private void RefreshAction(object obj)
        {

            var result = from p in DataAccess.DataBasic.DataPengaduan
                         from korban in p.Korban
                         where korban.ListKekerasanDialami != null
                         let c = korban.ListKekerasanDialami.Count
                         group p by
                            c <=1  ? "1" :
                            c == 2 ? "2" :
                            c == 3? "3" : ">3" into counts
                         select new {  Key = counts.Key, data = counts.Count() };





            List<string> labels = new List<string>();

            foreach(var item in result)
            {
                labels.Add(item.Key);

                SeriesCollection.Add(new ColumnSeries
                {
                    DataLabels = true,
                    Title = $"{item.Key} Jenis",
                    Values = new ChartValues<int> { item.data}
                });


            }

            Labels = labels.ToArray();
            //new[] { "Jan", "Feb", "Mar", "Apr", "May" };
            YFormatter = value => value + "Jenis";
            this.DataContext = this;
        }

    }
}
