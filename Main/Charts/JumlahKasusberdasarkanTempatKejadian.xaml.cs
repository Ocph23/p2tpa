using LiveCharts;
using LiveCharts.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;

namespace Main.Charts
{
    /// <summary>
    /// Interaction logic for JumlahKasusberdasarkanTempatKejadian.xaml
    /// </summary>
    public partial class JumlahKasusberdasarkanTempatKejadian : ChartMaster
    {
        public JumlahKasusberdasarkanTempatKejadian()
        {
            InitializeComponent();
            this.RefreshChartCommand = new CommandHandler { CanExecuteAction = x => true, ExecuteAction = RefreshAction };
            this.RefreshChartCommand.Execute(null);
            Title = "Jumlah Kasus Berdasarkan Tempat Kejadian";
            this.DataContext = this;
        }

        private void RefreshAction(object obj)
        {
            var groupPengaduan = DataAccess.DataBasic.DataPengaduan.GroupBy(x => x.TempatKejadian);
            List<string> dataTempat = new List<string>();
            foreach (var tempat in EnumSource.DataTempatKejadian())
            {
                dataTempat.Add(tempat);
                var jumlah = groupPengaduan.Where(x => x.Key == tempat).GroupBy(x => x).Count();
                SeriesCollection.Add(new ColumnSeries { DataLabels = true, Title = tempat, Values = new ChartValues<int> { jumlah } });
            }

            Labels = dataTempat.ToArray();
            //new[] { "Jan", "Feb", "Mar", "Apr", "May" };
            YFormatter = value => ((int)value).ToString("N");
            XFormatter = value => ((int)value) <= 0 ? "" : ((int)value).ToString("N");
        }
       
    }
}
