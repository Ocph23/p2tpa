using Main.Views;
using Ocph.DAL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Main.Models
{
    [TableName("Penanganan")]
    public class Penanganan : BaseNotify, IDataErrorInfo
    {
        public Penanganan(Identitas identitas, string type)
        {
            this.DataIdentias = identitas;
            this.IdentitasType = type;
            this.IdentiasId = identitas.Id;
           
            SaveCommand = new CommandHandler() { CanExecuteAction = x => string.IsNullOrEmpty(Error), ExecuteAction = SaveAction };
            CancelCommand = new CommandHandler() { CanExecuteAction = x => true, ExecuteAction =x=> WindowClose() };
            AddInstansiCommand = new CommandHandler() { CanExecuteAction=x=> true, ExecuteAction = AddInstansiView };
            ListInstansi = (CollectionView)CollectionViewSource.GetDefaultView(DataAccess.DataBasic.GetDataInstansi);
        }

        private void AddInstansiView(object obj)
        {
            var form = new AddInstansi();
            form.DataContext = new AddInstansiViewModel() { WindowClose=form.Close};
            form.ShowDialog();
            ListInstansi.Refresh();
        }

        public string Title {
            get  {

                return $"Penanganan {IdentitasType}";
            }
            set { SetProperty(ref _title, value); }
        }

        public Penanganan()
        {

            SaveCommand = new CommandHandler() { CanExecuteAction = x => string.IsNullOrEmpty(Error), ExecuteAction = SaveAction };
            CancelCommand = new CommandHandler() { CanExecuteAction = x => true, ExecuteAction = x => WindowClose() };
            ListInstansi = (CollectionView)CollectionViewSource.GetDefaultView(DataAccess.DataBasic.GetDataInstansi);
            Title = $"Penanganan {IdentitasType}";
        }
        private void SaveAction(object obj)
        {
            WindowClose();
        }

        [PrimaryKey("IdPenanganan")]
        [DbColumn("IdPenanganan")]
        public int? IdPenanganan { get => id; set => SetProperty(ref id, value); }

        [DbColumn("IdentiasId")]
        public int? IdentiasId { get => identiasId ; set => SetProperty(ref identiasId, value); }

        [DbColumn("Tanggal")]
        public DateTime Tanggal { get => tanggal; set => SetProperty(ref tanggal, value); }


        [DbColumn("IdentitasType")]
        public string IdentitasType { get => identitasType; set => SetProperty(ref identitasType, value); }

        [DbColumn("InstansiId")]
        public int InstansiId {
            get => instansiId;
            set => SetProperty(ref instansiId, value); }

        [DbColumn("Layanan")]
        public string Layanan { get => layanan; set => SetProperty(ref layanan, value); }


        [DbColumn("DetailLayanan")]
        public string DetailLayanan { get => detailLayanan; set => SetProperty(ref detailLayanan, value); }

        [DbColumn("Deskripsi")]
        public string Deskripsi { get => deskripsi; set => SetProperty(ref deskripsi, value); }

        public Identitas DataIdentias { get => dataIdentitas; set => SetProperty(ref dataIdentitas, value); }


        public string TTL
        {
            get {
                if(DataIdentias!=null)
                     return $"{DataIdentias.TempatLahir}, {DataIdentias.TanggalLahir.ToShortDateString()}";
                return string.Empty;
            }
            set
            {
                SetProperty(ref ttl, value);
            }
        }


        public Instansi Instansi
        {
            get {  return _instansiSelected;  }
            set
            {
                SetProperty(ref _instansiSelected, value);
            }
        }

        public string this[string columnName] => Validate(columnName);


        public string Error
        {
            get
            {
                IDataErrorInfo me = (IDataErrorInfo)this;
                string error =
                    me[GetPropertyName(() => IdentiasId)] +
                    me[GetPropertyName(() => Tanggal)] +
                    me[GetPropertyName(() => IdentitasType)] +
                    me[GetPropertyName(() => Deskripsi)] 
                    ;

                if (!string.IsNullOrEmpty(error))
                    return error;
                //return null;
                return null;
            }
        }
        public string Validate(string name)
        {


            if (name == "IdentiasId" && IdentiasId<0)
                return "Identitas Type Tidak Boleh 0";


            if (name == "Tanggal" && Tanggal== new DateTime())
                return "Minimal Pilih Salah Satu";


            if (name == "IdentitasType" && string.IsNullOrEmpty(IdentitasType))
                return "Pilih Jenis Identitas ";


            if (name == "Deskripsi" && string.IsNullOrEmpty(Deskripsi))
                return "Deskripsi Tidak Boleh Kosong";

            return null;
        }


        private int? identiasId;
        private string identitasType;
        private DateTime tanggal= DateTime.Now;
        private string detailLayanan;
        private string ttl;
        private int? id;

        private string deskripsi;
        private string layanan;
        private Identitas dataIdentitas;
        private Layanan _selectedLayanan;
        private int instansiId;
        private Instansi _instansiSelected;
        private string _title;

        public List<Layanan> ListLayanan {
            get
            {
                if (IdentitasType == "Korban")
                   return EnumSource.GetDataLayananKorban();
                else if (IdentitasType == "Terlapor")
                    return EnumSource.GetDataLayananTerlapor();
                return null;
            }
        }

        public CollectionView ListInstansi { get; set; }

        public Layanan SelectedLayanan {
            get {

                return _selectedLayanan; }
            set
            {
                this.Layanan = value.Name.ToString();
                SetProperty(ref _selectedLayanan, value);
            }
        }

        public CommandHandler SaveCommand { get; }
        public CommandHandler CancelCommand { get; }
        public CommandHandler AddInstansiCommand { get; }
        public Action WindowClose { get; internal set; }
    }

}
