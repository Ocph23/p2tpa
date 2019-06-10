﻿using LiveCharts;
using LiveCharts.Defaults;
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
    /// Interaction logic for JenisKekerasanyangDialamiKorban.xaml
    /// </summary>
    public partial class JenisKekerasanyangDialamiKorban : UserControl
    {
        public JenisKekerasanyangDialamiKorban()
        {
            InitializeComponent();
            

            var source = DataAccess.DataBasic.DataPengaduan;

            List<string> labels = new List<string>();
            List<int> datas = new List<int>();

            labels.Add("Fisik");
            SeriesCollection = new SeriesCollection { new ColumnSeries { DataLabels = true, Title ="Fisik",
                Values = new ChartValues<ObservableValue> { new ObservableValue(source.Where(x => x.Kejadian.Fisik).Count()) } } };

            labels.Add("Psikis");
            SeriesCollection.Add(new ColumnSeries { DataLabels = true, Title = "Psikis",
                Values = new ChartValues<ObservableValue> { new ObservableValue(source.Where(x => x.Kejadian.Fisik).Count()) } });

            labels.Add("Penelataran");
            SeriesCollection.Add(new ColumnSeries { DataLabels = true, Title = "Penelataran", Values = new ChartValues<int> { source.Where(x => x.Kejadian.Penelantaran).Count() } });

            labels.Add("Seksual");
            SeriesCollection.Add(new ColumnSeries { DataLabels = true, Title = "Seksual", Values = new ChartValues<int> { source.Where(x => x.Kejadian.Seksual).Count() } });

            labels.Add("Penganiayaan");
            SeriesCollection.Add(new ColumnSeries { DataLabels = true, Title = "Penganiayaan", Values = new ChartValues<int> { source.Where(x => x.Kejadian.Penganiayaan).Count() } });

            labels.Add("Pencabulan");
            SeriesCollection.Add(new ColumnSeries { DataLabels = true, Title = "Pencabulan", Values = new ChartValues<int> { source.Where(x => x.Kejadian.Pencabulan).Count() } });

            labels.Add("Pemerkosaan");
            SeriesCollection.Add(new ColumnSeries { DataLabels = true, Title = "Pemerkosaan", Values = new ChartValues<int> { source.Where(x => x.Kejadian.Pemerkosaan).Count() } });

            labels.Add("Trafiking");
            SeriesCollection.Add(new ColumnSeries { DataLabels = true, Title = "Trafiking", Values = new ChartValues<int> { source.Where(x => x.Kejadian.Trafiking).Count() } });


            labels.Add("Lain");
            SeriesCollection.Add(new ColumnSeries {  DataLabels = true, Title = "Lain", Values = new ChartValues<int> { source.Where(x => !string.IsNullOrEmpty(x.Kejadian.Lain)).Count() } });




            Labels = labels.ToArray();
            //new[] { "Jan", "Feb", "Mar", "Apr", "May" };
            YFormatter = value => value.ToString("N");
            this.DataContext = this;
        }

        public SeriesCollection SeriesCollection { get; set; }
        public string[] Labels { get; private set; }
        public Func<int, string> YFormatter { get; set; }

    }
}