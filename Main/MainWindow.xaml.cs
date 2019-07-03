using System.Windows;
using System.Collections.Generic;
using MaterialDesignThemes.Wpf;
using System.Threading.Tasks;
using System.Windows.Controls;
using Main.ViewModels;
using System;
using Main.Models;
using System.Linq;

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
            DataViewCommand = new CommandHandler() { CanExecuteAction = x => true, ExecuteAction = DataViewAction };
            TambahPengaduanCommand = new CommandHandler() { CanExecuteAction = x => true, ExecuteAction = TambahPengaduanAction };
            ImportCommand = new CommandHandler() { CanExecuteAction = x => true, ExecuteAction = ImportAction };
            InstansiCommand = new CommandHandler() { CanExecuteAction = x => true, ExecuteAction = InstansiCommandAction };
            DataContext = this;
        }

        private void InstansiCommandAction(object obj)
        {
            var form = new Views.InstansiView();
            form.Show();
        }

        private void DataViewAction(object obj)
        {
            var form = new Views.DataView();
            form.Show();
        }



        private void ImportAction(object obj)
        {
            var form = new Views.ImportView();
            form.Show();
        }

        private void Db_DataReseult(List<Pengaduan> data)
        {

        }

        public int JumlahKasus {
            get { return DataAccess.DataBasic.DataPengaduan.Count; }
        }

        public int KorbanLaki { get {
                var groupPengaduan = (from a in DataAccess.DataBasic.DataPengaduan
                                      from terlapor in a.Terlapor
                                      select terlapor).GroupBy(x => x.Gender);

                return groupPengaduan.Where(x => x.Key == Gender.L).Count();

            } }

        public int KorbanPerempuan { get {
                var groupPengaduan = (from a in DataAccess.DataBasic.DataPengaduan
                                      from terlapor in a.Terlapor
                                      select terlapor).GroupBy(x => x.Gender);

                return groupPengaduan.Where(x => x.Key == Gender.P).Count();
            } }


        public CommandHandler DataViewCommand { get; }
        public CommandHandler TambahPengaduanCommand { get; }
        public CommandHandler ImportCommand { get; private set; }
        public CommandHandler InstansiCommand { get; }

        private void TambahPengaduanAction(object obj)
        {
            var form = new Views.TambahPengaduan(false);
            form.DataContext = new PengaduanViewModel();
            form.Show();
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            var spiner = new ProgressBar() { Margin = new Thickness(30), IsIndeterminate = true };
            spiner.Style = (Style)FindResource("MaterialDesignCircularProgressBar");
            _ = DialogHost.Show(spiner, "RootDialog", ClosingEventHandler);

            await Task.Delay(500);
            DialogHost.CloseDialogCommand.Execute(null, spiner);

            var view = new Charts.Dialogs.JumlahKasusDialog();
            var result = await DialogHost.Show(view, "RootDialog", ClosingEventHandler);
        }

        private void ClosingEventHandler(object sender, DialogClosingEventArgs eventArgs)
        {

        }

        private async void lakiKasus(object sender, RoutedEventArgs e)
        {

            var spiner = new ProgressBar() { Margin = new Thickness(30), IsIndeterminate = true };
            spiner.Style = (Style)FindResource("MaterialDesignCircularProgressBar");
            _ = DialogHost.Show(spiner, "RootDialog", ClosingEventHandler);

            await Task.Delay(500);
            DialogHost.CloseDialogCommand.Execute(null, spiner);

            var view = new Charts.Dialogs.KorbanLakiDialog();
            var result = await DialogHost.Show(view, "RootDialog", ClosingEventHandler);
        }

        private async void perempuankasus(object sender, RoutedEventArgs e)
        {
            var spiner = new ProgressBar() { Margin = new Thickness(30), IsIndeterminate = true };
            spiner.Style = (Style)FindResource("MaterialDesignCircularProgressBar");
            _ = DialogHost.Show(spiner, "RootDialog", ClosingEventHandler);

            await Task.Delay(500);
            DialogHost.CloseDialogCommand.Execute(null, spiner);

            var view = new Charts.Dialogs.KorbanPerempuanDialog();
            var result = await DialogHost.Show(view, "RootDialog", ClosingEventHandler);
        }



    }

}
