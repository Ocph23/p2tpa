using Ocph.DAL;
using System;
using System.ComponentModel;

namespace Main.ViewModels
{

   public class PelaporViewModel:Pelapor ,IDataErrorInfo
    {

         public string Error
        {
            get
            {
                IDataErrorInfo me = (IDataErrorInfo)this;
                string error =
                    me[GetPropertyName(() => Nama)] +
                   
                    me[GetPropertyName(() => Alamat)] +
                    me[GetPropertyName(() => Gender)]
                    ;
                if (!string.IsNullOrEmpty(error))
                    return "Please check inputted data.";
                return null;
            }
        }
        public string this[string columnName] => Validate(columnName);
        public string Validate(string name)
        {

            if (name == "Nama" && string.IsNullOrEmpty(Nama))
                return "Nama Tidak Boleh Kosong";

            if (name == "Gender" && Gender == Gender.None)
                return "Jenis Kelamin Tidak Boleh Kosong";

            if (name == "Agama" && string.IsNullOrEmpty(Agama))
                return "Agama Tidak Boleh Kosong";

            if (name == "Alamat" && string.IsNullOrEmpty(Alamat))
                return "Alamat Tidak Boleh Kosong";

            if (name == "Pendidikan" && string.IsNullOrEmpty(Pendidikan))
                return "Pendidikan Tidak Boleh Kosong";


            return null;
        }

    }
}
