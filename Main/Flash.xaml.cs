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
using System.Windows.Shapes;

namespace Main
{
    /// <summary>
    /// Interaction logic for Flash.xaml
    /// </summary>
    public partial class Flash : Window
    {
        public Flash()
        {
            InitializeComponent();
            this.Loaded += Flash_Loaded;
        }

        private async void Flash_Loaded(object sender, RoutedEventArgs e)
        {
            await Task.Delay(5000);
            CloseFlashAsync();
        }

        private void CloseFlashAsync()
        {
            var form = new MainWindow();
            form.Show();
            this.Close();
        }
    }
}
