using Main.Models;
using Ocph.DAL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Main.ViewModels
{

    [TableName("instansi")]
    public class Instansi : BaseNotify    ,IDataErrorInfo
    {
        private string name;
        private List<string> _data;
        private KategoriInstansi kategori;
        private TingakatInstansi tingkat;
        private Kecamatan kecamatan;
        private string alamat;
        private int? id;
        private string distrikName;

   

        [PrimaryKey("Id")]
        [DbColumn("Id")]
        public int? Id
        {
            get { return id; }
            set { SetProperty(ref id, value); }
        }


        [DbColumn("Nama")]
        public string Name
        {
            get { return name; }
            set { SetProperty(ref name, value); }
        }


        [DbColumn("Kategori")]
        public KategoriInstansi Kategori
        {
            get { return kategori; }
            set { SetProperty(ref kategori, value); }
        }


        [DbColumn("Tingkat")]
        public TingakatInstansi Tingkat
        {
            get { return tingkat; }
            set { SetProperty(ref tingkat, value);
                if (value != TingakatInstansi.Distrik)
                    DistrikName = string.Empty;
            }
        }

        [DbColumn("Distrik")]
        public string DistrikName
        {
            get { return distrikName; }
            set {

                SetProperty(ref distrikName, value);
                if (!string.IsNullOrEmpty(value) && Tingkat != TingakatInstansi.Distrik)
                    this.Tingkat = TingakatInstansi.Distrik;
              
            }
        }


        public Kecamatan Kecamatan
        {
            get { return kecamatan; }
            set { SetProperty(ref kecamatan, value);
            }
        }

        [DbColumn("Alamat")]
        public string Alamat
        {
            get { return alamat; }
            set { SetProperty(ref alamat, value); }
        }

        public string this[string columnName] => Validate(columnName);
        public string Error
        {
            get
            {
                IDataErrorInfo me = (IDataErrorInfo)this;
                string error =
                    me[GetPropertyName(() => Name)] +
                    me[GetPropertyName(() => DistrikName)] +
                    me[GetPropertyName(() => Kecamatan)] +
                    me[GetPropertyName(() => Tingkat)] +
                     me[GetPropertyName(() => Alamat)] +
                    me[GetPropertyName(() => Kategori)]
                    ;

                if (!string.IsNullOrEmpty(error))
                    return error;
                //return null;
                return null;
            }
        }
        public string Validate(string name)
        {

            if (name == "Name" && string.IsNullOrEmpty(Name))
                return $"{name} Tidak Boleh Kosong";

            if (name == "Kategori" && Kategori == KategoriInstansi.None)
                return $"{name} Tidak Boleh Kosong";


            if (name == "Kecamatan" && Tingkat== TingakatInstansi.Distrik && string.IsNullOrEmpty(DistrikName))
                return $"{name} Tidak Boleh Kosong";

            if (name == "Alamat" && string.IsNullOrEmpty(Alamat))
                return $"{name} Tidak Boleh Kosong";

            return null;
        }

    }


    public class Layanan : BaseNotify {
        private string name;
        private List<string> _data;
        private string _selectedDetail;

        public string Name
        {
            get { return name; }
            set { SetProperty(ref name ,value); }
        }

        public List<string> DataLayanan
        {
            get {
                if (_data == null && this.Name == "Kesehatan")
                    return new List<string>() { "Visum", "Medis", "Psikologi" };
                return _data;

            }
            set { SetProperty(ref _data, value); }
        }


        public string SelectedDetailLayanan { get { return _selectedDetail; } set { SetProperty(ref _selectedDetail, value); } }



    }


}
