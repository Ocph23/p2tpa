using LiveCharts;
using LiveCharts.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;

namespace Main.Charts
{
    /// <summary>
    /// Interaction logic for RasioPerempuanKorbanKekerasan.xaml
    /// </summary>
    public partial class RasioPerempuanKorbanKekerasan : ChartMaster
    {
        public RasioPerempuanKorbanKekerasan()
        {
            InitializeComponent();
            this.RefreshChartCommand = new CommandHandler { CanExecuteAction = x => true, ExecuteAction = RefreshAction };
            this.RefreshChartCommand.Execute(null);
            Title = "Rasio Perempuan Korban Kekerasan";
            this.DataContext = this;
        }

        private void RefreshAction(object obj)
        {
            var dataKec = (from a in DataAccess.DataBasic.DataPendudukPerKecamatan() select a);
            var groupPengaduan = DataAccess.DataBasic.DataPengaduan.GroupBy(x => x.KodeDistrik);

            List<double> rasio = new List<double>();
            List<int> jumlahKorbanPerempuan = new List<int>();
            foreach (var kec in dataKec)
            {
                var kasus = groupPengaduan.Where(x => x.Key == kec.Id).FirstOrDefault();
                if (kasus != null)
                {
                    var kasusPerem = (from a in kasus
                                     from korban in a.Korban where korban.Gender == Gender.P select korban).Count();
                    rasio.Add((Convert.ToDouble(kasusPerem)*100000)/kec.Perempuan);
                    jumlahKorbanPerempuan.Add(kasusPerem);
                }
                else
                {
                    rasio.Add(0);
                    jumlahKorbanPerempuan.Add(0);
                }
            }

            SeriesCollection = new SeriesCollection
            {
                new LineSeries
                {
                    Title = "Rasio",
                    Values = new ChartValues<double>(rasio),
                },
                  new ColumnSeries
                {
                    Title = "Jumlah Korban Perempuan",
                    Values = new ChartValues<int>(jumlahKorbanPerempuan)
                }
            };

            Labels = (from a in DataAccess.DataBasic.GetKecamatan() select a.Nama).ToArray();
            //new[] { "Jan", "Feb", "Mar", "Apr", "May" };
            //YFormatter = value => value.ToString("C");
        }

    }
}
