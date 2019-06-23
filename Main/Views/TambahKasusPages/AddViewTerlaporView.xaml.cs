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

namespace Main.Views.TambahKasusPages
{
    /// <summary>
    /// Interaction logic for AddViewTerlaporView.xaml
    /// </summary>
    public partial class AddViewTerlaporView : Window
    {
        public AddViewTerlaporView()
        {
            InitializeComponent();
            this.DataContext = new AddTerlaporViewModel() { WindowClose = this.Close };
        }
    }

    public class AddTerlaporViewModel : TerlaporViewModel
    {
        public CommandHandler SaveCommand { get; }
        public CommandHandler CancelCommand { get; }
        public Action WindowClose { get; internal set; }

        public AddTerlaporViewModel()
        {
            SaveCommand = new CommandHandler { CanExecuteAction = ValidateSave, ExecuteAction = SaveAction };
            CancelCommand = new CommandHandler { CanExecuteAction = x => true, ExecuteAction =CancelAction };
        }
        public AddTerlaporViewModel(TerlaporViewModel data)
        {
            SaveCommand = new CommandHandler { CanExecuteAction = ValidateSave, ExecuteAction = SaveAction };
            CancelCommand = new CommandHandler { CanExecuteAction = x => true, ExecuteAction = CancelAction };
            this.Nama = data.Nama;
            this.Alamat = data.Alamat;
            this.Agama = data.Agama;
            this.Gender = data.Gender;
            this.Id = data.Id;
            this.NamaPanggilan = data.NamaPanggilan;
            this.NIK = data.NIK;
            this.NoReq = data.NoReq;
            this.Pekerjaan = data.Pekerjaan;
            this.Pendidikan = data.Pendidikan;
            this.PengaduanId = data.PengaduanId;
            this.Pernikahan = data.Pernikahan;
            this.Suku = data.Suku;
            this.TanggalLahir = data.TanggalLahir;
            this.TempatLahir = data.TempatLahir;
        }


        private void CancelAction(object obj)
        {
            this.Nama = string.Empty;
            WindowClose();
        }

        private bool ValidateSave(object obj)
        {
            if (string.IsNullOrEmpty(this.Error))
                return true;
            return false;
        }

        private void SaveAction(object obj)
        {
            WindowClose();
        }

        public bool DataValid => ValidateSave(null);
    }
}
