using LiveCharts;
using LiveCharts.Wpf;
using System.Collections.Generic;
using System.Linq;

namespace Main.Charts.Dialogs
{
    /// <summary>
    /// Interaction logic for KorbanLakiPendidikan.xaml
    /// </summary>
    public partial class KorbanLakiPendidikan : ChartMaster
    {
        public KorbanLakiPendidikan()
        {
            InitializeComponent();
            this.RefreshChartCommand = new CommandHandler { CanExecuteAction = x => true, ExecuteAction = RefreshAction};
            this.RefreshChartCommand.Execute(null);
            Title = "% Korban Laki-Laki Berdasarkan Pendidikan";
            this.DataContext = this;
        }

        private void RefreshAction(object obj)
        {


            var groupPengaduan = from p in DataAccess.DataBasic.DataPengaduan
                                 from korban in p.Korban where korban.Gender== Gender.L select korban;
            List<string> labels = new List<string>();
            foreach (var pendidikan in groupPengaduan.GroupBy(x=>x.Pendidikan))
            {

                labels.Add(pendidikan.Key);
                int value = pendidikan.Count();
                
                SeriesCollection.Add(new PieSeries { DataLabels = true, Title = pendidikan.Key, Values = new ChartValues<double> { value } });
            }

            Labels = labels.ToArray();
            PointLabel = chartPoint =>
                 string.Format("{0} ({1:P})", chartPoint.Y, chartPoint.Participation);
        }
    }
}
