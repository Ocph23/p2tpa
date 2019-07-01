using Main.Models;
using Main.Utilities;
using Main.ViewModels;
using System.Threading.Tasks;
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
            Loaded += TambahPengaduan_LoadedAsync;
            this.IsEditable = Editable;
            //this.viewmodel = AutoMapper.Mapper.Map<PengaduanViewModel>(new Pengaduan());
            //this.DataContext = viewmodel; 
        }

        public bool IsEditable { get; }

        private void TambahPengaduan_LoadedAsync(object sender, RoutedEventArgs e)
        {
            var viewmodel = DataContext as PengaduanViewModel;
            nav.DataContext = new NavigationPageViewModel(this.MainFrame, viewmodel, IsEditable) { WindowClose = this.Close };

        }

      
    }

}
