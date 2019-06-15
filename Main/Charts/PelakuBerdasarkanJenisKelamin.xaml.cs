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
    /// Interaction logic for PelakuBerdasarkanJenisKelamin.xaml
    /// </summary>
    public partial class PelakuBerdasarkanJenisKelamin : ChartMaster
    {
        public PelakuBerdasarkanJenisKelamin()
        {
            InitializeComponent();
            this.RefreshChartCommand = new CommandHandler { CanExecuteAction = x => true, ExecuteAction = RefreshAction };
            this.RefreshChartCommand.Execute(null);
            Title = "Pelaku Berdasarkan Jenis Kelamin";
            this.DataContext = this;
        }

        private void RefreshAction(object obj)
        {
            var groupPengaduan = DataAccess.DataBasic.DataPengaduan.GroupBy(x => x.Terlapor.Gender);
            List<string> datagender = new List<string>() { "Laki-Laki","Perempuan"};
            List<int> datas = new List<int>();
            foreach (var data in datagender)
            {
                var value = 0;
                if (data == "Laki-Laki")
                {
                   var resultPria = groupPengaduan.Where(x => x.Key == Gender.L).FirstOrDefault();
                    if (resultPria != null)
                        value = resultPria.Count();
                }
                else
                {
                    var resultWanita= groupPengaduan.Where(x => x.Key == Gender.P).FirstOrDefault();
                    if (resultWanita != null)
                        value = resultWanita.Count();
                }
                SeriesCollection.Add(new ColumnSeries { DataLabels = true, Title = data, Values = new ChartValues<int> { value } });
            }

            Labels = datagender.ToArray();
            //new[] { "Jan", "Feb", "Mar", "Apr", "May" };
            YFormatter = value => ((int)value).ToString("N");
            XFormatter = value => ((int)value) <= 0 ? "" : ((int)value).ToString("N");
        }
    }
}
