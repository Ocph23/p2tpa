using LiveCharts;
using LiveCharts.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Main.Charts
{
    /// <summary>
    /// Interaction logic for KorbanBerdasarkanPendidikan.xaml
    /// </summary>
    public partial class KorbanBerdasarkanPendidikan : ChartMaster
    {
        public KorbanBerdasarkanPendidikan()
        {
            InitializeComponent();
            this.RefreshChartCommand = new CommandHandler { CanExecuteAction = x => true, ExecuteAction = RefreshAction };
            this.RefreshChartCommand.Execute(null);
            Title = "Korban Berdasarkan Pendidikan";
            this.DataContext = this;
        }

        private void RefreshAction(object obj)
        {
            var groupPengaduan = (from p in  DataAccess.DataBasic.DataPengaduan
                                 from korban in p.Korban select korban).GroupBy(x => x.Pendidikan);
          
            foreach (var pendidikan in EnumSource.DataPendidikan())
            {
                int value = 0;
                var data = groupPengaduan.Where(x => x.Key == pendidikan).FirstOrDefault();
            
                if (data != null)
                {
                    value = data.Count();
                }

                SeriesCollection.Add(new ColumnSeries
                {
                    DataLabels = true,
                    Title = pendidikan,
                    Values = new ChartValues<int> { value }
                });
            }

            Labels = EnumSource.DataPendidikan().ToArray();
            //new[] { "Jan", "Feb", "Mar", "Apr", "May" };
            YFormatter = value => ((int)value).ToString("N");
            XFormatter = value => ((int)value)<=0 ? "": ((int)value).ToString("N");
        }
    }
}
