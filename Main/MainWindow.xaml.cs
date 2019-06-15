using System.Windows;
using System.Collections.Generic;
using MaterialDesignThemes.Wpf;
using System;
using System.Threading.Tasks;
using System.Windows.Controls;

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
            DataViewCommand    =      new CommandHandler() { CanExecuteAction = x => true, ExecuteAction = DataViewAction };
            TambahPengaduanCommand = new CommandHandler() { CanExecuteAction = x => true, ExecuteAction = TambahPengaduanAction };
            ImportCommand = new CommandHandler() { CanExecuteAction = x => true, ExecuteAction = ImportAction };
            DataContext = this;
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

        private void Db_DataReseult(List<ViewModels.Pengaduan> data)
        {
         
        }

        public CommandHandler DataViewCommand { get; }
        public CommandHandler TambahPengaduanCommand { get; }
        public CommandHandler ImportCommand { get; private set; }

        private void TambahPengaduanAction(object obj)
        {
            var form = new Views.TambahPengaduan();
            form.Show();
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            var spiner = new ProgressBar() { Margin= new Thickness(30), IsIndeterminate=true};
            spiner.Style = (Style)FindResource("MaterialDesignCircularProgressBar");
            DialogHost.Show(spiner, "RootDialog", ClosingEventHandler);
           
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
            DialogHost.Show(spiner, "RootDialog", ClosingEventHandler);

            await Task.Delay(500);
            DialogHost.CloseDialogCommand.Execute(null, spiner);

            var view = new Charts.Dialogs.KorbanLakiDialog();
            var result = await DialogHost.Show(view, "RootDialog", ClosingEventHandler);
        }

        private async void perempuankasus(object sender, RoutedEventArgs e)
        {
            var spiner = new ProgressBar() { Margin = new Thickness(30), IsIndeterminate = true };
            spiner.Style = (Style)FindResource("MaterialDesignCircularProgressBar");
            DialogHost.Show(spiner, "RootDialog", ClosingEventHandler);

            await Task.Delay(500);
            DialogHost.CloseDialogCommand.Execute(null, spiner);

            var view = new Charts.Dialogs.KorbanPerempuanDialog();
            var result = await DialogHost.Show(view, "RootDialog", ClosingEventHandler);
        }
    }

}
