using LiveCharts;
using LiveCharts.Defaults;
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
    /// Interaction logic for JenisKekerasanyangDialamiKorban.xaml
    /// </summary>
    public partial class JenisKekerasanyangDialamiKorban : ChartMaster
    {
        public JenisKekerasanyangDialamiKorban()
        {
            InitializeComponent();
            this.RefreshChartCommand = new CommandHandler { CanExecuteAction = x => true, ExecuteAction = RefreshAction };
            this.RefreshChartCommand.Execute(null);
            Title = "Jenis Kekerasan yang Dialami Korban";
            this.DataContext = this;
        }

        private void RefreshAction(object obj)
        {
            var result = from p in DataAccess.DataBasic.DataPengaduan
                         from b in EnumSource.DaftarKekerasan()
                         from korban in p.Korban.Where(x=>x.KekerasanDialami.Contains(b)).DefaultIfEmpty()
                         group p by b into counts
                            select new { Key = counts.Key, data = counts.Count() };

            List<string> labels = new List<string>();

            foreach (var item in result)
            {
                labels.Add(item.Key);

                SeriesCollection.Add(new ColumnSeries
                {
                    DataLabels = true,
                    Title = $"{item.Key}",
                    Values = new ChartValues<int> { item.data }
                });
            }

            Labels = labels.ToArray();
            //new[] { "Jan", "Feb", "Mar", "Apr", "May" };
            YFormatter = value => ((int)value).ToString("N");
            XFormatter = value => ((int)value) <= 0 ? "" : ((int)value).ToString("N");
        }

      

    }
}
