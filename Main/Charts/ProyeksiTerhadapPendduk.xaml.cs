using LiveCharts;
using LiveCharts.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Main.Charts
{
    /// <summary>
    /// Interaction logic for ProyeksiTerhadapPendduk.xaml
    /// </summary>
    public partial class ProyeksiTerhadapPendduk : ChartMaster
    {
        public ProyeksiTerhadapPendduk()
        {
            InitializeComponent();
            DataContext = this;
            this.RefreshChartCommand = new CommandHandler { CanExecuteAction = x => true, ExecuteAction = RefreshAction };
            this.RefreshChartCommand.Execute(null);
            Title = "ProyeksiTerhadapPendduk";
            this.DataContext = this;
        }

        private void RefreshAction(object obj)
        {
            var dataKec = (from a in DataAccess.DataBasic.DataPendudukPerKecamatan() select a);
            var datapeng = (from a in DataAccess.DataBasic.DataPendudukKerjaPerKecamatan() select a);
            var groupPengaduan = DataAccess.DataBasic.DataPengaduan.GroupBy(x => x.KodeDistrik);
            List<double> listKasus = new List<double>();
            List<double> listPekerja = new List<double>();
            List<int> jumKasus = new List<int>();
            foreach (var kec in dataKec)
            {
                var kasus = groupPengaduan.Where(x => x.Key == kec.Id).FirstOrDefault();
                var pekerja = datapeng.Where(x => x.Id == kec.Id).FirstOrDefault();


                if (pekerja != null && kasus != null)
                    listPekerja.Add((Convert.ToDouble(pekerja.Menganggur) / kec.Total) * 100);
                else
                {
                    listPekerja.Add(0);
                }

                if (kasus != null)
                {

                    listKasus.Add((Convert.ToDouble(kasus.Count()) / kec.Total) * 100);
                    jumKasus.Add(kasus.Count());
                }
                else
                {
                    listKasus.Add(0);
                    jumKasus.Add(0);
                }
            }

            SeriesCollection.Clear();
            SeriesCollection.Add(new LineSeries { Title = "Persen Penganguran", Values = new ChartValues<double>(listPekerja), });
            SeriesCollection.Add(new LineSeries { Title = "Persen Kasus Terhadap Penduduk", Values = new ChartValues<double>(listKasus) });
            SeriesCollection.Add(new ColumnSeries { Title = "Jumlah Kasus", Values = new ChartValues<int>(jumKasus) });

            Labels = (from a in DataAccess.DataBasic.GetKecamatan() select a.Nama).ToArray();
            //new[] { "Jan", "Feb", "Mar", "Apr", "May" };
            //YFormatter = value => value.ToString("C");
        }
    }
}
