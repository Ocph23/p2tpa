using Main.DataAccess;
using Main.Models;
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
            DataKategori = Enum.GetValues(typeof(KategoriInstansi));
            DataTingkat = Enum.GetValues(typeof(TingakatInstansi));
            SaveCommand = new CommandHandler() { CanExecuteAction = x => string.IsNullOrEmpty(Error), ExecuteAction = SaveAction };
        }

        public AddInstansiViewModel(Instansi instansi)
        {
            DataKategori = Enum.GetValues(typeof(KategoriInstansi));
            DataTingkat = Enum.GetValues(typeof(TingakatInstansi));
            SaveCommand = new CommandHandler() { CanExecuteAction = x => string.IsNullOrEmpty(Error), ExecuteAction = SaveAction };
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

        public Array DataKategori { get; }
        public Array DataTingkat { get; }
        public CommandHandler SaveCommand { get; }
        public List<Kecamatan> DataKecamatan { get; set; } = DataBasic.GetKecamatan();
        public Action WindowClose { get; internal set; }
    }
}
