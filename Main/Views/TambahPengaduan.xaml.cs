using Main.DataAccess;
using Main.Models;
using Main.Utilities;
using Main.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Main.Views
{
    /// <summary>
    /// Interaction logic for TambahPengaduan.xaml
    /// </summary>
    public partial class TambahPengaduan : Window
    {
        private TambahViewModel vm;

        public TambahPengaduan()
        {
            InitializeComponent();
            Loaded += TambahPengaduan_Loaded;
            this.DataContext = vm = new TambahViewModel();
            this.Loaded += TambahPengaduan_Loaded1;
        }

        private void TambahPengaduan_Loaded1(object sender, RoutedEventArgs e)
        {
            nav.DataContext =  new NavigationPageViewModel(this.MainFrame, vm);
            var service = new PengaduanServices();
           
        }

        public Identitas Model { get; set; }

        private void TambahPengaduan_Loaded(object sender, RoutedEventArgs e)
        {
            this.Model = new Identitas();
        }
    }


    public class TambahViewModel:Pengaduan
    {
        public TambahViewModel()
        {
            Kecamatan = DataAccess.DataBasic.GetKecamatan();
            this.Pelapor = new Pelapor();
            this.Terlapor = new List<Terlapor>();
            this.Korban = new List<Korban>();
            this.Kondisi = new KondisiKorban();
            this.Dampak = new DampakKorban();
            this.Kejadian = new Kejadian();
            this.Perkembangan = new List<TahapanPerkembangan>();
        }

        public List<Kecamatan> Kecamatan { get; set; }

    }
}
