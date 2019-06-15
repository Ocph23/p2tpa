using LiveCharts;
using LiveCharts.Wpf;
using System.Linq;

namespace Main.Charts.Dialogs
{
    /// <summary>
    /// Interaction logic for KorbanLakiPekerjaan.xaml
    /// </summary>
    public partial class KorbanLakiPekerjaan : ChartMaster
    {
        public KorbanLakiPekerjaan()
        {
            InitializeComponent(); this.RefreshChartCommand = new CommandHandler { CanExecuteAction = x => true, ExecuteAction = RefreshAction };
            this.RefreshChartCommand.Execute(null);
            Title = "% Korban Laki-Laki Berdasarkan Pekerjaan";
            this.DataContext = this;
        }

        private void RefreshAction(object obj)
        {
            var groupPengaduan = from pengaduan in DataAccess.DataBasic.DataPengaduan 
                                 from korban in pengaduan.Korban
                                 select korban ;

            foreach (var pekerjaan in groupPengaduan.GroupBy(x=>x.Pekerjaan))
            {
                int value = 0;
                var data = pekerjaan.Where(x => x.Gender == Gender.L);

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
