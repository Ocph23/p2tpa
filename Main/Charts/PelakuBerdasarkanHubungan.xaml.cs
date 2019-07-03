using LiveCharts;
using LiveCharts.Wpf;
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
            this.RefreshChartCommand.Execute(null);
            Title = "PelakuBerdasarkanHubungan";
            this.DataContext = this;
        }

        private void RefreshAction(object obj)
        {
            var groupPengaduan = from a in DataAccess.DataBasic.DataPengaduan
                                 from b in a.Terlapor
                                 from d in b.Hubungan.DefaultIfEmpty()
                                 group d by d.JenisHubungan into dGroup
                                 select new { Key = dGroup.Key, Values = dGroup };


            foreach(var item in EnumSource.HubunganKorbanDenganTerlapor())
            {
                var result = groupPengaduan.Where(x => x.Key == item).FirstOrDefault();
                var value = 0;
                if(result !=null)
                {
                    value = result.Values.Count();
                }
                SeriesCollection.Add(new ColumnSeries { DataLabels = true, Title = item, Values = new ChartValues<double> { value } });
            }

            Labels = EnumSource.HubunganKorbanDenganTerlapor().ToArray();
            //new[] { "Jan", "Feb", "Mar", "Apr", "May" };
            YFormatter = value => ((int)value).ToString("N");
            XFormatter = value => ((int)value) <= 0 ? "" : ((int)value).ToString("N");
        }
    }
}
