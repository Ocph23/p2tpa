using System;
using System.Windows;
using Main.Utilities;
using System.Linq;

namespace Main.Charts.Dialogs
{
    /// <summary>
    /// Interaction logic for KorbanLakiDialog.xaml
    /// </summary>
    public partial class KorbanLakiDialog : ChartMaster
    {
        public KorbanLakiDialog()
        {
            InitializeComponent();


            var win= WindowHelpers.GetWindow();
            var result = win.Width;
            this.Width = result * 80/100;
            this.Height = win.Height *80/100;
            var data = DataAccess.DataBasic.DataPengaduan.Where(x => x.Korban.Gender == Gender.L).Count();
            this.Title = $"Jumlah korban kekerasan dengan gender Laki-Laki tahun {DateTime.Now.Year} adalah {data} jiwa ";
            this.DataContext = this;

        }
    }
}
