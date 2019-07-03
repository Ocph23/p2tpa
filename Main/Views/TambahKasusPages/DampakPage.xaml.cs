using Main.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Controls;

namespace Main.Views.TambahKasusPages
{
    /// <summary>
    /// Interaction logic for DampakPage.xaml
    /// </summary>
    public partial class DampakPage : Page
    {

        public DampakPage(Pengaduan vm)
        {
            InitializeComponent();
            this.DataContext = new DampakViewModel(vm);
        }

    }


    public class DampakViewModel : BaseNotify, IDataErrorInfo
    {
        private Pengaduan vm;
        private DateTime? waktu;
        private string tempat;
        private DateTime? tanggal;
        private string catatan;
        private string _jam;

        public DampakViewModel(Pengaduan vm)
        {
            this.vm = vm;
            Dampak = vm.Dampak;
            Tanggal = vm.TanggalKejadian;
            Waktu = vm.WaktuKejadian;
            Tempat = vm.TempatKejadian;
            Catatan = vm.Catatan;
        }

        public string this[string columnName] => Validate(columnName);
        public string Error
        {
            get
            {
                IDataErrorInfo me = (IDataErrorInfo)this;
                string error =
                    me[GetPropertyName(() => Waktu)] +
                    me[GetPropertyName(() => Tempat)] +
                      me[GetPropertyName(() => Catatan)] +
                      me[GetPropertyName(() => Tanggal)]; 

                if (!string.IsNullOrEmpty(error + Dampak.Error))
                    return "Please check inputted data.";
                //return null;
                return null;
            }
        }
        public string Validate(string name)
        {


            if (name == "Waktu" && Waktu == new DateTime())
                return "Waktu Tidak Boleh Kosong";

            if (name == "Tanggal" && Tanggal == new DateTime())
                return "Tanggal Tidak Boleh Kosong";

            if (name == "Tempat" && string.IsNullOrEmpty(Tempat))
                return "Tempat Tidak Boleh Kosong";

            if (name == "Catatan" && string.IsNullOrEmpty(Catatan))
                return "Catatan Tidak Boleh Kosong";

            return null;
        }


        public DampakKorban Dampak { get; set; }

        public DateTime? Tanggal {
            get => vm.TanggalKejadian;
            set {
                SetProperty(ref tanggal, value);
                vm.TanggalKejadian = value;
            } 
        }

        public DateTime? Waktu { get => waktu; set
            {
                SetProperty(ref waktu, value);
                vm.WaktuKejadian = value;
            }
        }

        public string Jam { get { return _jam; } set { SetProperty(ref _jam, value); } }

      

        public string Tempat { get => tempat; set
            {
                SetProperty(ref tempat, value);
                vm.TempatKejadian = value;
            }
        }
         public string Catatan { get => catatan; set
            {
                SetProperty(ref catatan, value);
                vm.Catatan = value;
            }
        }


        public List<string> DataTempat { get; set; } = EnumSource.DataTempatKejadian();

    }
}
