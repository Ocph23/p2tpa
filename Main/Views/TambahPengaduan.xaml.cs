using Main.Utilities;
using Main.ViewModels;
using System.Windows;

namespace Main.Views
{
    /// <summary>
    /// Interaction logic for TambahPengaduan.xaml
    /// </summary>
    public partial class TambahPengaduan : Window
    {
       
        public TambahPengaduan()
        {
            InitializeComponent();
            Loaded += TambahPengaduan_Loaded;
           // this.DataContext = vm = new Pengaduan();
            this.Loaded += TambahPengaduan_Loaded1;
        }

        private void TambahPengaduan_Loaded1(object sender, RoutedEventArgs e)
        {
            var vm = DataContext as Pengaduan;
            nav.DataContext =  new NavigationPageViewModel(this.MainFrame, vm);
           
        }

       // public Identitas Model { get; set; }

        private void TambahPengaduan_Loaded(object sender, RoutedEventArgs e)
        {
          //  this.Model = new Identitas();
        }
    }

}
