using LiveCharts;
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
    public partial class JenisKekerasanyangDialamiKorban : ChartMaster
    {
        public JenisKekerasanyangDialamiKorban()
        {
            InitializeComponent();
            this.RefreshChartCommand = new CommandHandler { CanExecuteAction = x => true, ExecuteAction = RefreshAction };
            this.RefreshChartCommand.Execute(null);
            Title = "Jenis Kekerasan yang Dialami Korban";
            this.DataContext = this;
        }

        private void RefreshAction(object obj)
        {
            var source = DataAccess.DataBasic.DataPengaduan;
            List<string> labels = new List<string>();
            List<int> datas = new List<int>();
            labels.Add("Fisik");
           
            Labels = labels.ToArray();
            //new[] { "Jan", "Feb", "Mar", "Apr", "May" };
            YFormatter = value => ((int)value).ToString("N");
            XFormatter = value => ((int)value) <= 0 ? "" : ((int)value).ToString("N");
        }

      

    }
}
