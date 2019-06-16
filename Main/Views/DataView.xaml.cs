using System.Collections.Generic;
using System.Windows;
using Main.ViewModels;

namespace Main.Views
{
    public partial class DataView : Window
    {
        public DataView()
        {
            InitializeComponent();
           
            this.DataContext = new DataViewModel();
        }
    }


    public class DataViewModel
    {
        public DataViewModel()
        {
            Datas = DataAccess.DataBasic.DataPengaduan;
            EditCommand = new CommandHandler { CanExecuteAction = x => true, ExecuteAction = EditAction };
            DeleteCommand = new CommandHandler { CanExecuteAction = x => true, ExecuteAction = DeleteAction };
        }

        private void DeleteAction(object obj)
        {
            MessageBoxResult dialog = MessageBox.Show("Yakin Hapus Data Ini ? ", "Perhatian", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if(dialog== MessageBoxResult.Yes)
            {
                //Delete
            }
        }

        private void EditAction(object obj)
        {
            if(obj!=null)
            {
                var form = new TambahPengaduan();
                form.DataContext = obj as Pengaduan;
                form.ShowDialog();
            }
          
        }

        public List<Pengaduan> Datas { get; }
        public CommandHandler EditCommand { get; }
        public CommandHandler DeleteCommand { get; }
    }
}
