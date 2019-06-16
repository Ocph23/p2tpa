using System.Windows.Controls;
using Main.ViewModels;

namespace Main.Views.TambahKasusPages
{
    /// <summary>
    /// Interaction logic for KejadianPage.xaml
    /// </summary>
    public partial class KejadianPage : Page
    {

        public KejadianPage(Pengaduan vm)
        {
            InitializeComponent();
            this.DataContext = vm.Kejadian;
        }
    }


}
