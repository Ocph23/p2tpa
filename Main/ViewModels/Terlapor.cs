using Ocph.DAL;
using System;
using System.ComponentModel;

namespace Main.ViewModels
{

    [TableName("Terlapor")]
   public class Terlapor : Identitas   ,IDataErrorInfo
    {
      
        public string this[string columnName] => Validate(columnName);
        public string Error
        {
            get
            {
                IDataErrorInfo me = (IDataErrorInfo)this;
                string error =
                    me[GetPropertyName(() => Nama)] +
                    me[GetPropertyName(() => TempatLahir)] +
                    me[GetPropertyName(() => TanggalLahir)] +
                    me[GetPropertyName(() => Agama)] +
                    me[GetPropertyName(() => Alamat)] +
                    me[GetPropertyName(() => Pendidikan)] +
                    me[GetPropertyName(() => Suku)] +
                    me[GetPropertyName(() => Gender)] 
                    ;
                if (!string.IsNullOrEmpty(error))
                    // return "Please check inputted data.";
                    return null;
                return null;
            }
        }
        public string Validate(string name)
        {

            if (name == "Nama" && string.IsNullOrEmpty(Nama))
                return "Nama Tidak Boleh Kosong";

           

            if (name == "TempatLahir" && string.IsNullOrEmpty(TempatLahir))
                return "TempatLahir Tidak Boleh Kosong";

            if (name == "TanggalLahir" && new DateTime() == TanggalLahir)
                return "TanggalLahir Tidak Boleh Kosong";

            if (name == "Agama" && string.IsNullOrEmpty(Agama))
                return "Agama Tidak Boleh Kosong";

            if (name == "Alamat" && string.IsNullOrEmpty(Alamat))
                return "Alamat Tidak Boleh Kosong";

            if (name == "Pendidikan" && string.IsNullOrEmpty(Pendidikan))
                return "Pendidikan Tidak Boleh Kosong";

            if (name == "Suku" && string.IsNullOrEmpty(Suku))
                return "Suku Tidak Boleh Kosong";

            if (name == "Gender" && Gender == Gender.None)
                return "Jenis Kelamin Tidak Boleh Kosong";


            return null;
        }

    }
}
