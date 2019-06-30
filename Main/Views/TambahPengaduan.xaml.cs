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

        public TambahPengaduan(bool Editable)
        {
            InitializeComponent();
            Loaded += TambahPengaduan_Loaded;
            this.IsEditable = Editable;
            // this.DataContext = vm = new Pengaduan();
        }

        public bool IsEditable { get; }

        private void TambahPengaduan_Loaded(object sender, RoutedEventArgs e)
        {
            var vm = DataContext as Pengaduan;
            nav.DataContext = new NavigationPageViewModel(this.MainFrame, vm, IsEditable) { WindowClose = this.Close };

        }

      
    }

}
