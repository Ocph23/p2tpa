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
    /// Interaction logic for PenanganDanUraianPage.xaml
    /// </summary>
    public partial class PenanganDanUraianPage : Page
    {

        public PenanganDanUraianPage(TambahViewModel vm)
        {
            InitializeComponent();
            this.DataContext = new PenangananKasus(vm);
        }
    }


    public class PenangananKasus : BaseNotify, IDataErrorInfo
    {
        private TambahViewModel vm;
        private bool _hukum;
        private bool _nonHukum;
        private bool _konseling;
        private bool _rujukan;
        private bool _lain;
        private string _lainText;

        public PenangananKasus(TambahViewModel vm)
        {
            this.vm = vm;
        }


        public bool Hukum { get => _hukum;
            set => SetProperty(ref _hukum, value); }

        public bool NonHukum{ get => _nonHukum;
            set => SetProperty(ref _nonHukum, value); }


        public bool Konsultasi{ get => _konseling;
            set => SetProperty(ref _konseling, value); }


        public bool Rujukan
        {
            get => _rujukan;
            set {
                SetProperty(ref _rujukan, value);
            }
        }

        private string uraian;
        private string _errorMessage;

        public string Uraian
        {
            get => vm.UraianKejadian;
            set
            {
                SetProperty(ref uraian, value);
                vm.UraianKejadian = value;
            }
        }



        public bool Lain { get => _lain;
            set
            {
                SetProperty(ref _lain, value);
            }
        }

        private void SetDataValue(string data)
        {
            var list = data.Split('#');
            foreach(var item in list)
            {
                if(string.IsNullOrEmpty(item))
                {
                    Penanganan penanganan;
                    var converted=Enum.TryParse<Penanganan>(item ,out penanganan);
                    if(converted)
                    {
                        if (penanganan == Penanganan.Hukum)
                            this.Hukum = true;
                        if (penanganan == Penanganan.Konsultasi)
                            this.Konsultasi = true;
                       
                        if (penanganan == Penanganan.Non_Hukum)
                            this.NonHukum = true;
                        if (penanganan == Penanganan.Rujukan)
                            this.Rujukan = true;
                    }
                    else
                    {
                        LainText = item;
                        this.Lain = true;
                    }
               
                  
                }
            }
        }

        public string LainText
        {
            get => _lainText;
            set
            {
                SetProperty(ref _lainText, value);
                //SetDataValue();
            }
        }

        private void GetDataValue()
        {
           
            StringBuilder sb = new StringBuilder();
            if (Hukum)
                sb.Append(Penanganan.Hukum + "#");
            if (NonHukum)
                sb.Append(Penanganan.Non_Hukum+ "#");
            if (Konsultasi)
                sb.Append(Penanganan.Konsultasi+ "#");
            if (Rujukan)
                sb.Append(Penanganan.Rujukan+ "#");
            if (Lain)
                sb.Append(LainText+"#");
            vm.Penanganan = sb.ToString();
        }

        public string this[string columnName] => Validate(columnName);

        private string Validate(string columnName)
        {
            if (columnName == "Hukum" && !Hukum && !NonHukum && !Konsultasi && !Rujukan && !Lain)
                return "Minimal Pilih Satu";

            if (columnName == "NonHukum" && !Hukum && !NonHukum && !Konsultasi && !Rujukan && !Lain)
                return "Minimal Pilih Satu";

            if (columnName == "Konsultasi" && !Hukum && !NonHukum && !Konsultasi && !Rujukan && !Lain)
                return "Minimal Pilih Satu";

            if (columnName == "Rujukan" && !Hukum && !NonHukum && !Konsultasi && !Rujukan && !Lain)
                return "Minimal Pilih Satu";

            if (columnName == "Lain" && !Hukum && !NonHukum && !Konsultasi && !Rujukan && !Lain)
                return "Minimal Pilih Satu";


            if (columnName == "LainText" && Lain && string.IsNullOrEmpty(LainText))
                return "Jelaskan";

            if (columnName == "Uraian" && string.IsNullOrEmpty(Uraian))
                return "Jelaskan";
            return null;
        }

        public string Error
        {
            get
            {
                IDataErrorInfo me = (IDataErrorInfo)this;
                string error = me[GetPropertyName(() => Uraian)]+
                    me[GetPropertyName(()=>LainText)];

                string errorMessage = me[GetPropertyName(() => Hukum)] +
                                                   me[GetPropertyName(() => NonHukum)] +
                                                   me[GetPropertyName(() => Konsultasi)] +
                                                   me[GetPropertyName(() => Rujukan)] +
                                                   me[GetPropertyName(() => Lain)] ;

                ErrorMessage = (string.IsNullOrEmpty(errorMessage)?"":"Minimal Pilih Satu");
                GetDataValue();

                if (!string.IsNullOrEmpty(error+errorMessage))
                    return "Please check inputted data.";
                    //return null;
                return null;
            }
        }

        public string ErrorMessage
        {
            get => _errorMessage;
            set
            {
                SetProperty(ref _errorMessage, value);
            }
        }

    }
}
