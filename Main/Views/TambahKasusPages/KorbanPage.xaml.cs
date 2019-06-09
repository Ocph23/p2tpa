using Main.ViewModels;
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
    /// Interaction logic for KorbanPage.xaml
    /// </summary>
    public partial class KorbanPage : Page
    {

        public KorbanPage(TambahViewModel vm)
        {
            InitializeComponent();
            this.DataContext =  vm.Korban;
        }


        private void statusPernikahanLosfocs(object sender, RoutedEventArgs e)
        {
            var comboBox = (ComboBox)sender;
            if (comboBox.SelectedItem != null)
                return;
            var newItem = comboBox.Text;
            var viewmodel = this.DataContext as Korban;
            viewmodel.ListHubunganKorban.Add(newItem);
            comboBox.SelectedItem = newItem;
        }

      
    }
}
