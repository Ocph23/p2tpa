using Main.ViewModels;
using System.ComponentModel;
using System.Text;
using System.Windows.Controls;

namespace Main.Views.TambahKasusPages
{
    /// <summary>
    /// Interaction logic for PenanganDanUraianPage.xaml
    /// </summary>
    public partial class PenanganDanUraianPage : Page
    {

        public PenanganDanUraianPage(Pengaduan vm)
        {
            InitializeComponent();
            this.DataContext = new PenangananKasus(vm);
        }
    }


    public class PenangananKasus : BaseNotify, IDataErrorInfo
    {
        private Pengaduan vm;
        private bool _hukum;
        private bool _nonHukum;
        private bool _konseling;
        private bool _rujukan;
        private bool _lain;
        private string _lainText;

        public PenangananKasus(Pengaduan vm)
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

                    if (item == "Pendampingan Hukum")
                        this.Hukum = true;
                    else if (item == "Konsultasi")
                        this.Konsultasi = true;
                    else if (item == "Pendampingan Non Hukum")
                        this.NonHukum = true;
                    else if (item =="Rujukan")
                        this.Rujukan = true;
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
                sb.Append("Pendampingan Hukum" + "#");
            if (NonHukum)
                sb.Append("Pendampingan Non Hukum" + "#");
            if (Konsultasi)
                sb.Append("Konsultasi" + "#");
            if (Rujukan)
                sb.Append("Rujukan" + "#");
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
