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
            var groupPengaduan = DataAccess.DataBasic.DataPengaduan.GroupBy(x => x.Korban.Pendidikan);
            List<string> labels = new List<string>();
            foreach (var pendidikan in groupPengaduan)
            {

                labels.Add(pendidikan.Key);
                int value = 0;
                var data = pendidikan.Where(x => x.Korban.Gender == Gender.L);

                if (data != null)
                {
                    value = data.Count();
                }

                SeriesCollection.Add(new PieSeries { DataLabels = true, Title = pendidikan.Key, Values = new ChartValues<double> { value } });
            }

            Labels = labels.ToArray();
            PointLabel = chartPoint =>
                 string.Format("{0} ({1:P})", chartPoint.Y, chartPoint.Participation);
        }
    }
}
