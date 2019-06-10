using System.Windows;
using System.Collections.Generic;

namespace Main
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        
            TambahPengaduanCommand = new CommandHandler() { CanExecuteAction = x => true, ExecuteAction = TambahPengaduanAction };
            ImportCommand = new CommandHandler() { CanExecuteAction = x => true, ExecuteAction = ImportAction };
            DataContext = this;
        }

        private void ImportAction(object obj)
        {
            var form = new Views.ImportView();
            form.Show();
        }

        private void Db_DataReseult(List<ViewModels.Pengaduan> data)
        {
         
        }

        public CommandHandler TambahPengaduanCommand { get; }
        public CommandHandler ImportCommand { get; private set; }

        private void TambahPengaduanAction(object obj)
        {
            var form = new Views.TambahPengaduan();
            form.Show();
        }
    }

}
