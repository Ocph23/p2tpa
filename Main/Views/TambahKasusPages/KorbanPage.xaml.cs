using Main.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
        KOrbanPageViewModel viewmodel;
        public KorbanPage(Pengaduan vm)
        {
            InitializeComponent();
            this.DataContext= viewmodel = new KOrbanPageViewModel(vm);
        }

        private void statusPernikahanLosfocs(object sender, RoutedEventArgs e)
        {
        
        }

        private void addkorban(object sender, RoutedEventArgs e)
        {
            viewmodel.AddKorbanCommand.Execute(null);
        }

        private void addpelaku_Click(object sender, RoutedEventArgs e)
        {
            viewmodel.AddTerlaporCommand.Execute(null);
        }
    }


    public class KOrbanPageViewModel :BaseNotify  , IDataErrorInfo
    {

        private Pengaduan vm;

        public KOrbanPageViewModel(Pengaduan vm)
        {
            this.vm = vm;
            this.Korbans = (CollectionView)CollectionViewSource.GetDefaultView(vm.Korban);
            this.Terlapors = (CollectionView)CollectionViewSource.GetDefaultView( vm.Terlapor);
            AddKorbanCommand = new CommandHandler { CanExecuteAction = x => true, ExecuteAction = AddKorbanAction };
            AddTerlaporCommand = new CommandHandler { CanExecuteAction = x => true, ExecuteAction = AddTerlaporAction };
            DeleteCommand = new CommandHandler { CanExecuteAction = x => true, ExecuteAction = DeleteAction };
        }

        private void DeleteAction(object obj)
        {
            if(obj!=null)
            {
                if(obj.GetType()==typeof(AddKorbanViewModel))
                {
                    var data = obj as Korban;
                    vm.Korban.Remove(data);
                    this.Korbans.Refresh();
                }

                if (obj.GetType() == typeof(AddTerlaporViewModel))
                {
                    var data = obj as Terlapor;
                    vm.Terlapor.Remove(data);
                    this.Terlapors.Refresh();
                }
            }
        }

        private void AddTerlaporAction(object obj)
        {
            var form = new AddViewTerlaporView();
            if (obj != null)
                form.DataContext = new AddTerlaporViewModel(obj as Terlapor) { WindowClose = form.Close };
            else
                form.DataContext = new AddTerlaporViewModel() { WindowClose = form.Close };
            form.ShowDialog();

            var formVM = form.DataContext as AddTerlaporViewModel;
            if (formVM.DataValid && obj == null)
            {
                vm.Terlapor.Add((Terlapor)formVM);
            }
            Terlapors.Refresh();
        }

        private void AddKorbanAction(object obj)
        {
            AddKorbanView form = new AddKorbanView();
            if (obj != null)
                form.DataContext = new AddKorbanViewModel(obj as Korban) { WindowClose = form.Close };
            else
                form.DataContext = new AddKorbanViewModel() { WindowClose = form.Close };
            form.ShowDialog();

            var korbanVM = form.DataContext as AddKorbanViewModel;
            if (korbanVM.DataValid && obj==null)
            {
                vm.Korban.Add((Korban)korbanVM);
            }
            Korbans.Refresh();
        }

        public CollectionView Korbans { get; set; }

        public CollectionView Terlapors { get; set; }


        public CommandHandler AddKorbanCommand { get; }
        public CommandHandler AddTerlaporCommand { get; }
        public CommandHandler DeleteCommand { get; }

        public string this[string columnName] => ValidateError(columnName);

        private string ValidateError(string columnName)
        {
            if (columnName == "Korbans" && (Korbans == null || Korbans.Count <= 0))
                return "Korban Harus Minimal  Satu";

            if (columnName == "Terlapors" && (Terlapors == null || Terlapors.Count <= 0))
                return "Terlapor  Harus Minimal  Satu";

            return null;
        }

        public string Error
        {
            get
            {
                IDataErrorInfo me = (IDataErrorInfo)this;
                string error =
                    me[GetPropertyName(() => Korbans)] +
                    me[GetPropertyName(() => Terlapors)] 
                    ;

                if (!string.IsNullOrEmpty(error))
                    return "Korban atau Pelaku masing-masing minimal satu";
                //return null;
                return null;
            }
        }
    }
}
