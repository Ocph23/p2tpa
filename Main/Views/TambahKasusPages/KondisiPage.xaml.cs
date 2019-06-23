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
    /// Interaction logic for KondisiPage.xaml
    /// </summary>
    public partial class KondisiPage : Page
    {

        public KondisiPage(Pengaduan vm)
        {
            InitializeComponent();
            this.DataContext = new KondisiPageViewModel(vm);
        }
    }


    public class KondisiPageViewModel :BaseNotify, IDataErrorInfo
    {
        private Pengaduan vm;
        private string _uraian;

        public KondisiPageViewModel(Pengaduan vm)
        {
            this.vm = vm;
            Kondisi = vm.Kondisi;
        }

        public string this[string columnName] => Validate(columnName);

        private string Validate(string columnName)
        {
            if (columnName == "UraianKejadian" && string.IsNullOrEmpty(UraianKejadian))
                return "Uraian Singkat tidak Boleh Kosong";
            return null;
        }

        public string Error
        {
            get
            {
                IDataErrorInfo me = (IDataErrorInfo)this;
                string error =
                    me[GetPropertyName(() => UraianKejadian)] ;
                if (!string.IsNullOrEmpty(error+Kondisi.Error))
                    return "Please check inputted data.";
                //return null;
                return null;
            }
        }


        public KondisiKorban Kondisi { get; set; }


        public string UraianKejadian {
            get { return vm.UraianKejadian; }
            set { SetProperty(ref _uraian, value);
                vm.UraianKejadian = value;
            } }
    }
}
