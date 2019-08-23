using Main.DataAccess;
using Main.Models;
using Main.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    /// Interaction logic for AddInstansi.xaml
    /// </summary>
    public partial class AddInstansi : Window
    {
        public AddInstansi()
        {
            InitializeComponent();
        }
    }


    public class AddInstansiViewModel : Instansi
    {
        public AddInstansiViewModel()
        {
            Load();
        }

        private void Load()
        {
            DataTingkat = Enum.GetValues(typeof(TingakatInstansi));
            SaveCommand = new CommandHandler() { CanExecuteAction = x => string.IsNullOrEmpty(Error), ExecuteAction = SaveAction };
            AddCategoryCommand = new CommandHandler() { CanExecuteAction = x => true, ExecuteAction = AddCategoryAction };
            SourceKategori = GetDataKategori();
            DataKategori = (CollectionView)CollectionViewSource.GetDefaultView(SourceKategori);
        }

        private void AddCategoryAction(object obj)
        {
            var form = new KategoriView(this.Kategori);
            form.ShowDialog();
            var vm = form.DataContext as KategoriViewModel;
            if(vm!=null && vm.SelectedModel!=null)
            {
                var data = SourceKategori.Where(x => x.Id == vm.SelectedModel.Id).FirstOrDefault();
                if(data!=null)
                {
                    data.Name = vm.SelectedModel.Name;
                    data.Code = vm.SelectedModel.Code;
                }
                else
                {
                    SourceKategori.Add(vm.SelectedModel);
                }
            }

            DataKategori.Refresh();
        }

        private ObservableCollection<KategoriInstansi> GetDataKategori()
        {
            using (var db = new DbContext())
            {
                var result= db.KategoriInstansi.Select();
                return new ObservableCollection<KategoriInstansi>(result);
            }
        }

        public AddInstansiViewModel(Instansi instansi)
        {
            Load();
            this.Id = instansi.Id;
            this.Name = instansi.Name;
            this.DistrikName = instansi.DistrikName;
            this.Kategori = instansi.Kategori;
            this.Kecamatan = instansi.Kecamatan;
            this.Alamat = instansi.Alamat;
        }

        private async void SaveAction(object obj)
        {
            await Task.Delay(200);
            try
            {
                if (this.Id==null)
                    DataBasic.DataInstansi.Add((Instansi)this);
                else
                    DataBasic.DataInstansi.Update((Instansi)this);
                this.WindowClose();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public CollectionView DataKategori { get; set; }
        public ObservableCollection<KategoriInstansi> SourceKategori { get; set; }
        public Array DataTingkat { get; set; }
        public CommandHandler SaveCommand { get; set; }
        public CommandHandler AddCategoryCommand { get; set; }
        public List<Kecamatan> DataKecamatan { get; set; } = DataBasic.GetKecamatan();
        public Action WindowClose { get;  set; }
    }
}
