using Ocph.DAL;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Main.Models
{
    [TableName("Korban")]
    public class Korban :Identitas
    {
        private string kekerasa;

        public string KekerasanDialami
        {
            get { return kekerasa; }
            set { SetProperty(ref kekerasa , value); }
        }

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
                    me[GetPropertyName(() => Gender)] + ValidatePenanganan();
                    ;



                if (!string.IsNullOrEmpty(error))
                    return "Please check inputted data.";
                return null;
            }
        }

        private string ValidatePenanganan()
        {
           if(this.DataPenanganan!=null && DataPenanganan.Count>0)
            {
                foreach(var item in DataPenanganan)
                {
                    if(!string.IsNullOrEmpty(item.Error))
                    {
                        return item.Error;  
                    }
                }
            }

            return string.Empty;
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
