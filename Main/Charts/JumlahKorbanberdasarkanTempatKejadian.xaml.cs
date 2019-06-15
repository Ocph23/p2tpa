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
    /// Interaction logic for JumlahKorbanberdasarkanTempatKejadian.xaml
    /// </summary>
    public partial class JumlahKorbanberdasarkanTempatKejadian : ChartMaster
    {
        public JumlahKorbanberdasarkanTempatKejadian()
        {
            InitializeComponent();
            this.RefreshChartCommand = new CommandHandler { CanExecuteAction = x => true, ExecuteAction = RefreshAction };
            this.RefreshChartCommand.Execute(null);

            Title = "Jumlah Korban Berdasarkan Tempat Kejadian";
            this.DataContext = this;
        }

        private void RefreshAction(object obj)
        {
            var groupPengaduan = DataAccess.DataBasic.DataPengaduan.GroupBy(x => x.Kejadian.Tempat);
            List<string> dataTempat = new List<string>();
            List<int> datas = new List<int>();
            foreach (var kasus in groupPengaduan)
            {
                dataTempat.Add(kasus.Key);
                if (kasus != null)
                {
                    datas.Add(kasus.GroupBy(x => x.Korban.Id).Count());
                    SeriesCollection.Add(new ColumnSeries { DataLabels = true, Title = kasus.Key, Values = new ChartValues<int> { kasus.Count() } });
                }
            }

            Labels = dataTempat.ToArray();
            //new[] { "Jan", "Feb", "Mar", "Apr", "May" };
            YFormatter = value => ((int)value).ToString("N");
            XFormatter = value => ((int)value) <= 0 ? "" : ((int)value).ToString("N");
        }
    }
}
