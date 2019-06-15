using LiveCharts;
using LiveCharts.Defaults;
using LiveCharts.Wpf;
using Main.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Main.Charts.Dialogs
{
    /// <summary>
    /// Interaction logic for JumlahKasusDialog.xaml
    /// </summary>
    public partial class JumlahKasusDialog : ChartMaster
    {
        public JumlahKasusDialog()
        {
            InitializeComponent();

            var win = WindowHelpers.GetWindow();
            var result = win.Width;
            this.Width = result * 80 / 100;
            this.Height = win.Height * 80 / 100;
            var data = DataAccess.DataBasic.DataPengaduan.Count;
            this.Title = $"Jumlah kasus kekerasan terhadap perempuan dan anak tahun {DateTime.Now.Year} adalah {data} kasus ";
            this.DataContext = this;
        }


    }
}
