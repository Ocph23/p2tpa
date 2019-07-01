using AutoMapper;
using Main.Models;
using Main.ViewModels;
using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace Main.Views.TambahKasusPages
{
    /// <summary>
    /// Interaction logic for KorbanPage.xaml
    /// </summary>
    public partial class KorbanPage : Page
    {
        KOrbanPageViewModel viewmodel;
        public KorbanPage(PengaduanViewModel vm)
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

        private PengaduanViewModel vm;

        public KOrbanPageViewModel(PengaduanViewModel model)
        {
            this.vm = model;
            this.Korbans = (CollectionView)CollectionViewSource.GetDefaultView(vm.Korban);
            this.Terlapors = (CollectionView)CollectionViewSource.GetDefaultView( vm.Terlapor);
            foreach (var korban in vm.Korban)
            {

                foreach (var terlapor in vm.Terlapor)
                {
                    bool isfound = false;
                    foreach (var hubungan in terlapor.Hubungan)
                    {
                        if (hubungan.Korban.Id == korban.Id)
                        {
                            isfound = true;
                            break;
                        }
                    }

                    if (!isfound)
                        terlapor.Hubungan.Add(new HubunganDenganKorban(terlapor.Id, korban));
                }
            }

            AddKorbanCommand = new CommandHandler { CanExecuteAction = x => true, ExecuteAction = AddKorbanAction };
            AddTerlaporCommand = new CommandHandler { CanExecuteAction = x => true, ExecuteAction = AddTerlaporAction };
            DeleteCommand = new CommandHandler { CanExecuteAction = x => true, ExecuteAction = DeleteAction };
            AddPenangananCommand = new CommandHandler { CanExecuteAction = x => true, ExecuteAction = AddPenangananAction };
            EditPenangananCommand = new CommandHandler { CanExecuteAction = x => true, ExecuteAction = EditPenanganAction };
            DeletePenangananCommand = new CommandHandler { CanExecuteAction = x => true, ExecuteAction = DeletePenanganAction};
        }

        private void DeletePenanganAction(object obj)
        {
            try
            {
                var data = obj as Penanganan;
                if (data.IdPenanganan > 0)
                {
                    using (var db = new DbContext())
                    {
                        if (!db.Penanganan.Delete(x => x.IdPenanganan == data.IdPenanganan))
                        {
                            throw new SystemException("Data Tidak Berhasil Dihapus");

                        }
                    }
                }

                if (data.IdentitasType == "Korban")
                {
                    var item = vm.Korban.Find(x => x.Nama == data.DataIdentias.Nama);
                    if (item != null)
                        item.DataPenanganan.Remove(data);
                }
                else
                {
                    var item = vm.Korban.Find(x => x.Nama == data.DataIdentias.Nama);
                    if (item != null)
                        item.DataPenanganan.Remove(data);
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }

            Korbans.Refresh();
        }

        private void EditPenanganAction(object obj)
        {
            var data = obj as Penanganan;
            var form = new PenangananView();
            data.WindowClose = form.Close;
            form.DataContext = data;
            form.ShowDialog();
        }

        private void AddPenangananAction(object obj)
        {
            var typeName = obj.GetType().Name;
            if (typeName.Contains("Terlapor"))
            {
                var terlapor = obj as TerlaporViewModel;
                var form = new PenangananView();
                var penanganan = new Penanganan(terlapor, "Terlapor") { WindowClose = form.Close };
                form.DataContext = penanganan;
                form.ShowDialog();
                terlapor.DataPenanganan.Add(penanganan);
                Terlapors.Refresh();
            }      else if(typeName.Contains("Korban"))
            {
                var korban = obj as Korban;
                var form = new PenangananView();
                var penanganan = new Penanganan(korban, "Korban") { WindowClose = form.Close };
                form.DataContext = penanganan;
                form.ShowDialog();
                korban.DataPenanganan.Add(penanganan);
                Korbans.Refresh();
            }
          
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
                    var data = obj as TerlaporViewModel;
                    vm.Terlapor.Remove(data);
                    this.Terlapors.Refresh();
                }
            }
        }

        private void AddTerlaporAction(object obj)
        {
            var form = new AddViewTerlaporView();
            if (obj != null)
            {
                var context = Mapper.Map<AddTerlaporViewModel>(obj as Terlapor);
                context.WindowClose = form.Close;
                form.DataContext = context;
            }
            else
            {
                form.DataContext = new AddTerlaporViewModel() { WindowClose = form.Close };
            }
               
            form.ShowDialog();

            var formVM = form.DataContext as AddTerlaporViewModel;
            if (formVM.DataValid && obj == null)
            {
                vm.AddTerlapor(formVM);
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
                vm.AddKorban((Korban)korbanVM);
            }
            Korbans.Refresh();
        }

        public CollectionView Korbans { get; set; }

        public CollectionView Terlapors { get; set; }
        public CommandHandler AddPenangananCommand { get; }
        public CommandHandler EditPenangananCommand { get; }
        public CommandHandler DeletePenangananCommand { get; }
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
