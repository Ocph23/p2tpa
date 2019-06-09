using Ocph.DAL;
using System;
using System.ComponentModel;

namespace Main.ViewModels
{
    [TableName("Korban")]
    public class Korban :Identitas,IDataErrorInfo
    {
      
        public string hkdt;

        [DbColumn("HubunganKorbanDenganTerlapor")]
        public string HubunganKorbanDenganTerlapor
        {
            get => hkdt;
            set => SetProperty(ref hkdt, value);
        }


        public string NoReg { get; set; }
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
                    me[GetPropertyName(() => Gender)] +
                    me[GetPropertyName(() => HubunganKorbanDenganTerlapor)]
                    ;
                if (!string.IsNullOrEmpty(error))
                    //return "Please check inputted data.";
                    return null;
                return null;
            }
        }
        public string Validate(string name)
        {

            if (name == "Nama" && string.IsNullOrEmpty(Nama))
                return "Nama Tidak Boleh Kosong";

            if (name == "HubunganKorbanDenganTerlapor" && string.IsNullOrEmpty(HubunganKorbanDenganTerlapor))
                return "HubunganKorbanDenganTerlapor Tidak Boleh Kosong";

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

            if (name == "Suku" && Suku== Suku.None)
                return "Suku Tidak Boleh Kosong";

            if (name == "Gender" && Gender == Gender.None)
                return "Jenis Kelamin Tidak Boleh Kosong";


            return null;
        }

    }
}
