using Main.DataAccess;
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
using System.Windows.Shapes;

namespace Main.Views
{
    /// <summary>
    /// Interaction logic for InstansiView.xaml
    /// </summary>
    public partial class InstansiView : Window
    {
        public InstansiView()
        {
            InitializeComponent();
            DataInstansi =(CollectionView)CollectionViewSource.GetDefaultView(DataBasic.DataInstansi);
            this.DataContext = this;
            AddItemCommand = new CommandHandler() { CanExecuteAction = x => true, ExecuteAction = AddItemCommandAction };
            EditItemCommand = new CommandHandler() { CanExecuteAction = x => true, ExecuteAction = EditItemCommandAction };
            DeleteItemCommand = new CommandHandler() { CanExecuteAction = x => true, ExecuteAction = DeleteItemCommandAction };

        }

        private void DeleteItemCommandAction(object obj)
        {
            DataBasic.DataInstansi.Remove(obj as Instansi);
            DataInstansi.Refresh();
        }

        private void EditItemCommandAction(object obj)
        {
            var form = new AddInstansi();
            form.DataContext = new AddInstansiViewModel(obj as Instansi) { WindowClose = form.Close };
            form.ShowDialog();
            DataInstansi.Refresh();
        }



        private void AddItemCommandAction(object obj)
        {
            var form = new AddInstansi();
            form.DataContext = new AddInstansiViewModel() { WindowClose=form.Close};
            form.ShowDialog();
            DataInstansi.Refresh();
        }

        public CollectionView DataInstansi { get; }
        public CommandHandler AddItemCommand { get; }
        public CommandHandler EditItemCommand { get; }
        public CommandHandler DeleteItemCommand { get; }
    }
}
