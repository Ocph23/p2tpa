using LiveCharts;
using LiveCharts.Wpf;
using Main.Reports;
using Main.Reports.Models;
using Microsoft.Reporting.WinForms;
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
            this.PrintCommand = new CommandHandler { CanExecuteAction = x => true, ExecuteAction = PrintAction };
            this.RefreshChartCommand.Execute(null);
            Title = "Proyeksi Terhadap Penduduk";
            this.DataContext = this;
        }
        public CommandHandler PrintCommand { get; }
        List<GrafikModel> datgrafirk = new List<GrafikModel>();

        private void PrintAction(object obj)
        {
            var header = new ReportHeader { Title = Title, Tahun = DateTime.Now.Year };
            HelperPrint.PrintWithFormActionTwoSource("Print Preview",
                  new ReportDataSource { Name = "Header", Value = new List<ReportHeader>() { header } },
                new ReportDataSource { Name = "DataSet1", Value = datgrafirk },
               "Main.Reports.Layout.GrafikMultiSeries.rdlc", null);
        }
        private void RefreshAction(object obj)
        {
            var dataKec = (from a in DataAccess.DataBasic.DataPendudukPerKecamatan() select a);
            var datapeng = (from a in DataAccess.DataBasic.DataPendudukKerjaPerKecamatan() select a);
            var groupPengaduan = DataAccess.DataBasic.DataPengaduan.GroupBy(x => x.KodeDistrik);
            List<double> listKasus = new List<double>();
            List<double> listPekerja = new List<double>();
            List<int> jumKasus = new List<int>();
            datgrafirk.Clear();
            SeriesCollection.Clear();
            int number = 0;
            foreach (var kec in dataKec)
            {
             
                var kasus = groupPengaduan.Where(x => x.Key == kec.Id).FirstOrDefault();
                var pekerja = datapeng.Where(x => x.Id == kec.Id).FirstOrDefault();




                var model = new GrafikModel { Kategori = kec.Nama, Series = kec.Nama };
                if (pekerja != null && kasus != null)
                {
                    var data = (Convert.ToDouble(pekerja.Menganggur) / kec.Total) * 100;
                    model.Nilai = data;
                    listPekerja.Add(data);
                }
                else
                {
                    model.Nilai = 0;
                    listPekerja.Add(0);
                }


                if (kasus != null)
                {
                    var data = (Convert.ToDouble(kasus.Count()) / kec.Total) * 100;
                   
                    listKasus.Add(data);
                    jumKasus.Add(kasus.Count());
                    model.Nilai2 = (double)data;
                    model.Nilai3 = kasus.Count();
                }
                else
                {
                    listKasus.Add(0);
                    jumKasus.Add(0);
                    model.Nilai2 = 0;
                    model.Nilai3 = 0;
                }
                number++;
              datgrafirk.Add(model);
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
