using LiveCharts;
using LiveCharts.Wpf;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Main.Charts.Dialogs
{
    /// <summary>
    /// Interaction logic for JumlahKasusDialog1.xaml
    /// </summary>
    public partial class JumlahKasusDialog1 : ChartMaster
    {
        public JumlahKasusDialog1()
        {
            InitializeComponent();
            this.RefreshChartCommand = new CommandHandler { CanExecuteAction = x => true, ExecuteAction = RefreshActionAsync };
            this.DataContext = this;
            this.RefreshChartCommand.Execute(null);
            Title = "% Korban menurut Jenis Kelamin";
            

        }

        private async void RefreshActionAsync(object obj)
        {
           await Task.Delay(500);
            var groupPengaduan = (from a in  DataAccess.DataBasic.DataPengaduan
                                 from korban in a.Korban select korban).GroupBy(x=>x.Gender);
            
            List<string> datagender = new List<string>() { "Laki-Laki", "Perempuan" };
            List<int> datas = new List<int>();
            foreach (var data in datagender)
            {
                double value = 0;
                if (data == "Laki-Laki")
                {
                    var resultPria = groupPengaduan.Where(x => x.Key == Gender.L).FirstOrDefault();
                    if (resultPria != null)
                        value = resultPria.Count();
                }
                else
                {
                    var resultWanita = groupPengaduan.Where(x => x.Key == Gender.P).FirstOrDefault();
                    if (resultWanita != null)
                        value = resultWanita.Count() ;
                }
                SeriesCollection.Add(new PieSeries { DataLabels = true, Title = data, Values = new ChartValues<double> { value } });
            }

            Labels = datagender.ToArray();
            //new[] { "Jan", "Feb", "Mar", "Apr", "May" };
            PointLabel = chartPoint =>
               string.Format("{0} ({1:P})", chartPoint.Y, chartPoint.Participation);

        }

    }
}
