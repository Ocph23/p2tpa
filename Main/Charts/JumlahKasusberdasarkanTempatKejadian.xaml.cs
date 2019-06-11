﻿using LiveCharts;
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
            var groupPengaduan = DataAccess.DataBasic.DataPengaduan.GroupBy(x => x.Kejadian.Tempat);
            List<string> dataTempat = new List<string>();
            List<int> datas = new List<int>();
            foreach (var kasus in groupPengaduan)
            {
                dataTempat.Add(kasus.Key);
                if (kasus != null)
                {
                    datas.Add(kasus.Count());
                    SeriesCollection.Add(new ColumnSeries { DataLabels = true, Title = kasus.Key, Values = new ChartValues<int> { kasus.Count() } });
                }
            }

            Labels = dataTempat.ToArray();
            //new[] { "Jan", "Feb", "Mar", "Apr", "May" };
            YFormatter = value => value.ToString("N");

        }
       
    }
}
