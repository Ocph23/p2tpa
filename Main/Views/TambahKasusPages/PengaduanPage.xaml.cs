using Main.Models;
using System.Windows;
using System.Windows.Controls;

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
