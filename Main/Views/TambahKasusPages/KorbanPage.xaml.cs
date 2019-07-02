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

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var cmb = (ComboBox)sender;
            var cmbDataContext = (HubunganDenganKorban)cmb.DataContext;
            cmbDataContext.JenisHubungan = cmb.SelectedItem.ToString();
            viewmodel.Terlapors.Refresh();
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
            EditHubunganCommand = new CommandHandler { CanExecuteAction = x => true, ExecuteAction = EditHubunganAction };
        }

        private void EditHubunganAction(object obj)
        {
            var form = new EditHubunganView(obj);
            form.ShowDialog();
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
                var penanganan = new Penanganan(terlapor, "Terlapor") { IdentiasId=terlapor.Id, WindowClose = form.Close };
                form.DataContext = penanganan;
                form.ShowDialog();

                if(!string.IsNullOrEmpty( penanganan.Layanan))
                {
                    terlapor.DataPenanganan.Add(penanganan);
                    Terlapors.Refresh();
                }
                else
                {
                    MessageBox.Show("Data Tidak Valid", "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
                }

             
            }      else if(typeName.Contains("Korban"))
            {
                var korban = obj as Korban;
                var form = new PenangananView();
                var penanganan = new Penanganan(korban, "Korban") { IdentiasId=korban.Id, WindowClose = form.Close };
                form.DataContext = penanganan;
                form.ShowDialog();

                if (penanganan.IdentiasId != null && !string.IsNullOrEmpty(penanganan.Layanan))
                {
                    korban.DataPenanganan.Add(penanganan);
                    Korbans.Refresh();
                }
                else
                {
                    MessageBox.Show("Data Tidak Valid", "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                
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
            else if (formVM.DataValid && obj != null)
            {
                var terlapor = (Terlapor)obj;
                var model = (Terlapor)formVM;
                terlapor.Agama = model.Agama;
                terlapor.Alamat = model.Alamat;
                terlapor.Gender = model.Gender;
                terlapor.KekerasanDialami = model.KekerasanDialami;
                terlapor.Nama = model.Nama;
                terlapor.NamaPanggilan = model.NamaPanggilan;
                terlapor.NIK = model.NIK;
                terlapor.NoReq = model.NoReq;
                terlapor.Pekerjaan = model.Pekerjaan;
                terlapor.Pendidikan = model.Pendidikan;
                terlapor.Pernikahan = model.Pernikahan;
                terlapor.Suku = model.Suku;
                terlapor.TanggalLahir = model.TanggalLahir;
                terlapor.TempatLahir = model.TempatLahir;
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
            else if(korbanVM.DataValid && obj!=null)
            {
                var korban = (Korban)obj;
                var model = (Korban)korbanVM;
                korban.Agama = model.Agama;
                korban.Alamat = model.Alamat;
                korban.Gender = model.Gender;
                korban.KekerasanDialami = model.KekerasanDialami;
                korban.Nama = model.Nama;
                korban.NamaPanggilan = model.NamaPanggilan;
                korban.NIK = model.NIK;
                korban.NoReq = model.NoReq;
                korban.Pekerjaan = model.Pekerjaan;
                korban.Pendidikan = model.Pendidikan;
                korban.Pernikahan = model.Pernikahan;
                korban.Suku = model.Suku;
                korban.TanggalLahir = model.TanggalLahir;
                korban.TempatLahir = model.TempatLahir;
            }
            Korbans.Refresh();
        }

        public CollectionView Korbans { get; set; }

        public CollectionView Terlapors { get; set; }
        public CommandHandler AddPenangananCommand { get; }
        public CommandHandler EditPenangananCommand { get; }
        public CommandHandler DeletePenangananCommand { get; }
        public CommandHandler EditHubunganCommand { get; }
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
