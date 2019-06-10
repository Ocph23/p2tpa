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
    /// Interaction logic for KorbanMenurutBanyaknyaKekerasanyangDialami.xaml
    /// </summary>
    public partial class KorbanMenurutBanyaknyaKekerasanyangDialami : UserControl
    {
        public KorbanMenurutBanyaknyaKekerasanyangDialami()
        {
            InitializeComponent();
            List<string> labels = new List<string>();
            int satu = 0;
            int dua = 0;
            int tiga = 0;

            foreach (var data in DataAccess.DataBasic.DataPengaduan)
            {
                int found = 0;
                if (data.Kejadian.Fisik)
                    found++;
                if (data.Kejadian.Psikis)
                    found++;
                if (data.Kejadian.Penelantaran)
                    found++;
                if (data.Kejadian.Seksual)
                    found++;
                if (data.Kejadian.Penganiayaan)
                    found++;
                if (data.Kejadian.Pencabulan)
                    found++;
                if (data.Kejadian.Pemerkosaan)
                    found++;
                if (data.Kejadian.Trafiking)
                    found++;
                if (!string.IsNullOrEmpty(data.Kejadian.Lain))
                    found++;

                if (found == 1)
                    satu++;
                if (found == 2)
                    dua++;
                if (found >= 3)
                    tiga++;

            }



            labels.Add("1 ");

            labels.Add("2 ");

            labels.Add(">3 ");

            SeriesCollection = new SeriesCollection{new ColumnSeries { DataLabels = true, Title = "1 Jenis",
                Values = new ChartValues<int> { satu } } };

            SeriesCollection.Add(new ColumnSeries
            {
                DataLabels = true,
                Title = "2 Jenis",
                Values = new ChartValues<int> { dua }
            });

            SeriesCollection.Add(new ColumnSeries
            {
                DataLabels = true,
                Title = "> 3 Jenis",
                Values = new ChartValues<int> { tiga }
            });





            Labels = labels.ToArray();
            //new[] { "Jan", "Feb", "Mar", "Apr", "May" };
            YFormatter = value => value + "Jenis";
            this.DataContext = this;
        }

        public SeriesCollection SeriesCollection { get; set; }
        public string[] Labels { get; private set; }
        public Func<int, string> YFormatter { get; set; }
    }
}
