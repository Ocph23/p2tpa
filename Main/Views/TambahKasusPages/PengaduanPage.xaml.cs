using Main.Utilities;
using Main.ViewModels;
using MaterialDesignThemes.Wpf;
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

namespace Main.Views.TambahKasusPages
{
    /// <summary>
    /// Interaction logic for PengaduanPage.xaml
    /// </summary>
    public partial class PengaduanPage : Page
    {
        private Pengaduan viewmodel;

        public PengaduanPage(Pengaduan vm)
        {
            InitializeComponent();
           this.DataContext= this.viewmodel = vm;
            this.Loaded += PengaduanPage_Loaded;
        }

        private void PengaduanPage_Loaded(object sender, RoutedEventArgs e)
        {
         
        }

    }
}
