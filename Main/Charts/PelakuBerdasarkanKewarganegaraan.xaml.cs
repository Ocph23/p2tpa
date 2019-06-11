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
    /// Interaction logic for PelakuBerdasarkanKewarganegaraan.xaml
    /// </summary>
    public partial class PelakuBerdasarkanKewarganegaraan : ChartMaster
    {
        public PelakuBerdasarkanKewarganegaraan()
        {
            InitializeComponent(); this.RefreshChartCommand = new CommandHandler { CanExecuteAction = x => true, ExecuteAction = RefreshAction };
            this.RefreshChartCommand.Execute(null);
            Title = "Pelaku Berdasarkan Kewarganegaraan";
            this.DataContext = this;
        }

        private void RefreshAction(object obj)
        {
        }
    }
}
