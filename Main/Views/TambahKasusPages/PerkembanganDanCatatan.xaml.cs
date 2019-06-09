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
    /// Interaction logic for PerkembanganDanCatatan.xaml
    /// </summary>
    public partial class PerkembanganDanCatatan : Page
    {

        public PerkembanganDanCatatan(TambahViewModel vm)
        {
            InitializeComponent();
            this.DataContext = new PerkembanganViewModel(vm);
        }
    }


    public class PerkembanganViewModel : BaseNotify, IDataErrorInfo
    {
        public PerkembanganViewModel(TambahViewModel vm)
        {
            this.viewmodel = vm;
            Tahapans = vm.Perkembangan;
        }

        private string catatan;
        private TambahViewModel viewmodel;

        public string CatatanAkhir
        {
            get { return viewmodel.Catatan; }
            set {SetProperty(ref catatan ,value); viewmodel.Catatan = value; }
        }

       public List<TahapanPerkembangan> Tahapans { get; set; }



        public string this[string columnName] =>this.Validate(columnName);

        private string Validate(string columnName)
        {
            if (columnName == "CatatanAkhir" && string.IsNullOrEmpty(CatatanAkhir))
                return "Catatan Akhir Tidak Boleh Kosong";
            return null;
        }

        public string Error
        {
            get
            {
                IDataErrorInfo me = (IDataErrorInfo)this;
                string error =
                    me[GetPropertyName(() => CatatanAkhir)] 
                  
                    ;
                if (!string.IsNullOrEmpty(error))
                    return "Please check inputted data.";
                return null;
            }
        }
    }
}
