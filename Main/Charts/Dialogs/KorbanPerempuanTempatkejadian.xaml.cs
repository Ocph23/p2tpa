using LiveCharts;
using LiveCharts.Wpf;
using System.Collections.Generic;
using System.Linq;

namespace Main.Charts.Dialogs
{
    /// <summary>
    /// Interaction logic for KorbanPerempuanTempatkejadian.xaml
    /// </summary>
    public partial class KorbanPerempuanTempatkejadian : ChartMaster
    {
        public KorbanPerempuanTempatkejadian()
        {
            InitializeComponent();
            this.RefreshChartCommand = new CommandHandler { CanExecuteAction = x => true, ExecuteAction = RefreshAction};
            this.DataContext = this;
            this.RefreshChartCommand.Execute(null);
            Title = "% Korban Perempuan Berdasarkan Tempat Kejadian";
        }

        private void RefreshAction(object obj)
        {
            var groupPengaduan = from p in DataAccess.DataBasic.DataPengaduan
                                 from korban in p.Korban
                                 where korban.Gender == Gender.P
                                 select new { Pengaduan = p, Korban = korban };


            List<string> labels = new List<string>();
            foreach (var tempatKejadidan in groupPengaduan.GroupBy(x => x.Pengaduan.Kejadian.Tempat))
            {

                labels.Add(tempatKejadidan.Key);
                int value = tempatKejadidan.Count();

                SeriesCollection.Add(new PieSeries { DataLabels = true, Title = tempatKejadidan.Key, Values = new ChartValues<double> { value } });
            }

            Labels = labels.ToArray();
            PointLabel = chartPoint =>
                 string.Format("{0} ({1:P})", chartPoint.Y, chartPoint.Participation);
        }
    }
}
