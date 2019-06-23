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
            var groupPengaduan = DataAccess.DataBasic.DataPengaduan.GroupBy(x => x.Catatan);
            List<string> dataHubungan = EnumSource.HubunganKorbanDenganTerlapor();
            List<int> datas = new List<int>();
            foreach (var hubungan in dataHubungan)
            {
                var value = 0;
                var result = groupPengaduan.Where(x => x.Key == hubungan).FirstOrDefault();
                if (result != null)
                    value = result.Count();

                SeriesCollection.Add(new ColumnSeries { DataLabels = true, Title = hubungan, Values = new ChartValues<int> { value } });

            }

            Labels = dataHubungan.ToArray();
            //new[] { "Jan", "Feb", "Mar", "Apr", "May" };
            YFormatter = value => ((int)value).ToString("N");
            XFormatter = value => ((int)value) <= 0 ? "" : ((int)value).ToString("N");
        }
    }
}
