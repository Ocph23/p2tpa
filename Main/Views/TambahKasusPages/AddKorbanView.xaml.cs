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
    /// Interaction logic for AddKorbanView.xaml
    /// </summary>
    public partial class AddKorbanView : Window
    {
        public AddKorbanView()
        {
            InitializeComponent();
            cmb.ItemSource= EnumSource.DataPendidikan();
        }
      

        private void statusPernikahanLosfocs(object sender, RoutedEventArgs e)
        {
            var comboBox = (ComboBox)sender;
            if (comboBox.SelectedItem != null)
                return;
            var newItem = comboBox.Text;
            var viewmodel = this.DataContext as KorbanViewModel;
            viewmodel.ListHubunganKorban.Add(newItem);
            comboBox.SelectedItem = newItem;
        }
    }

    public class AddKorbanViewModel  :KorbanViewModel
    {
        public CommandHandler SaveCommand { get; }
        public CommandHandler CancelCommand { get; }
        public Action WindowClose { get; internal set; }

        public AddKorbanViewModel()
        {
            SaveCommand = new CommandHandler { CanExecuteAction = ValidateSave, ExecuteAction = SaveAction };
            CancelCommand = new CommandHandler { CanExecuteAction = x => true, ExecuteAction = CancelAction };
        }

        public List<string> Kekerasans { get; set; } = EnumSource.DataPendidikan();

        public AddKorbanViewModel(KorbanViewModel korban)
        {
            SaveCommand = new CommandHandler { CanExecuteAction = ValidateSave, ExecuteAction = SaveAction };
            CancelCommand = new CommandHandler { CanExecuteAction = x => true, ExecuteAction = CancelAction };
            this.Nama = korban.Nama;
            this.Alamat = korban.Alamat;
            this.Agama = korban.Agama;
            this.Gender = korban.Gender;
            this.Id = korban.Id;
            this.NamaPanggilan = korban.NamaPanggilan;
            this.NIK = korban.NIK;
            this.NoReq = korban.NoReq;
            this.Pekerjaan = korban.Pekerjaan;
            this.Pendidikan = korban.Pendidikan;
            this.PengaduanId = korban.PengaduanId;
            this.Pernikahan = korban.Pernikahan;
            this.Suku = korban.Suku;
            this.TanggalLahir = korban.TanggalLahir;
            this.TempatLahir = korban.TempatLahir;
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

        public bool DataValid =>ValidateSave(null);
    }
}
