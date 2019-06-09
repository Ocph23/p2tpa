using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using LiveCharts;
using LiveCharts.Wpf;
using System.Linq;
using Main.DataAccess;
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
            var db = new ImportFromExcel()  ;
            db.DataReseult += Db_DataReseult;
            db.Start();
            DataContext = this;
        }

        private void Db_DataReseult(List<ViewModels.Pengaduan> data)
        {
          
        }

        public CommandHandler TambahPengaduanCommand { get; }

        private void TambahPengaduanAction(object obj)
        {
            var form = new Views.TambahPengaduan();
            form.Show();
        }
    }

}
