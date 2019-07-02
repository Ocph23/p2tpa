using AutoMapper;
using Main.Models;
using Main.Utilities;
using Main.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Linq;

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
            cmb.ItemSource= EnumSource.DaftarKekerasan();
        }
      

        private void statusPernikahanLosfocs(object sender, RoutedEventArgs e)
        {

            var dataContext = DataContext as AddKorbanViewModel;
            var comboBox = (ComboChips)sender;
            dataContext.KekerasanDialami = dataContext.GetListKekerasanFromStringKekerasan(comboBox.Result);
            comboBox.SelectedItem = null;

            
            
        }
    }

    public class AddKorbanViewModel  :Korban
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

        public AddKorbanViewModel(Korban korban)
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
