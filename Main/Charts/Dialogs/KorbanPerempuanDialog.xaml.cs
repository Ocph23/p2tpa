using Main.Utilities;
using System;
using System.Linq;

namespace Main.Charts.Dialogs
{
    /// <summary>
    /// Interaction logic for KorbanPerempuanDialog.xaml
    /// </summary>
    public partial class KorbanPerempuanDialog : ChartMaster
    {
        public KorbanPerempuanDialog()
        {
            InitializeComponent();

            var win = WindowHelpers.GetWindow();
            var result = win.Width;
            this.Width = result * 80 / 100;
            this.Height = win.Height * 80 / 100;
            var data = DataAccess.DataBasic.DataPengaduan.Where(x => x.Korban.Gender == Gender.P).Count();
            this.Title = $"Jumlah korban kekerasan dengan gender Perempuan tahun {DateTime.Now.Year} adalah {data} jiwa ";
            this.DataContext = this;
        }
    }
}
