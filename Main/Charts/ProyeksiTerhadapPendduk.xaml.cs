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
    /// Interaction logic for ProyeksiTerhadapPendduk.xaml
    /// </summary>
    public partial class ProyeksiTerhadapPendduk : UserControl
    {
        public ProyeksiTerhadapPendduk()
        {
            InitializeComponent();
            var dataPenduduk = (from a in DataAccess.DataBasic.DataPendudukPerKecamatan() select a.Total);
            SeriesCollection = new SeriesCollection
            {
                new ColumnSeries
                {
                    Title = "Penduduk",
                    Values = new ChartValues<int>(dataPenduduk)
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
