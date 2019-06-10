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
    public partial class RasioPerempuanKorbanKekerasan : UserControl
    {
        public RasioPerempuanKorbanKekerasan()
        {
            InitializeComponent();
            var dataKec = (from a in DataAccess.DataBasic.DataPendudukPerKecamatan() select a);
            var groupPengaduan = DataAccess.DataBasic.DataPengaduan.GroupBy(x => x.KodeDistrik);

            List<double> rasio = new List<double>();
            List<int> jumlahKorbanPerempuan = new List<int>();
            foreach (var kec in dataKec)
            {
                var kasus = groupPengaduan.Where(x => x.Key == kec.Id).FirstOrDefault();


                if (kasus != null)
                {
                    var kasusPerem = kasus.Where(x => x.Korban.Gender == Gender.P).Count();
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
            this.DataContext = this;
        }

        public SeriesCollection SeriesCollection { get; private set; }
        public string[] Labels { get; private set; }
    }
}
