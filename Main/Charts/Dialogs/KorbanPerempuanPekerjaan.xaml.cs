using LiveCharts;
using LiveCharts.Wpf;
using System.Linq;

namespace Main.Charts.Dialogs
{
    /// <summary>
    /// Interaction logic for KorbanPerempuanPekerjaan.xaml
    /// </summary>
    public partial class KorbanPerempuanPekerjaan : ChartMaster
    {
        public KorbanPerempuanPekerjaan()
        {
            InitializeComponent();
            this.RefreshChartCommand = new CommandHandler { CanExecuteAction = x => true, ExecuteAction = RefreshAction};
            this.RefreshChartCommand.Execute(null);
            Title = "% Korban Perempuan Berdasarkan Pekerjaan";
            this.DataContext = this;
        }

        private void RefreshAction(object obj)
        {
            var groupPengaduan = (from p in  DataAccess.DataBasic.DataPengaduan
                                  from korban in p.Korban select korban).GroupBy(x => x.Pekerjaan);

            foreach (var pekerjaan in groupPengaduan)
            {
                int value = 0;
                var data = pekerjaan.Where(x => x.Gender == Gender.P);

                if (data != null)
                {
                    value = data.Count();
                }

                SeriesCollection.Add(new PieSeries { DataLabels = true, Title = pekerjaan.Key, Values = new ChartValues<double> { value } });
            }

            Labels = EnumSource.DataPendidikan().ToArray();
            PointLabel = chartPoint =>
                 string.Format("{0} ({1:P})", chartPoint.Y, chartPoint.Participation);
        }
    }
}
