using Ocph.DAL;
using System;
using System.ComponentModel;

namespace Main.ViewModels
{

    [TableName("Pelapor")]
   public class Pelapor:Identitas ,IDataErrorInfo
    {

        private StatusPelapor statusLapor;
        private string statusLaporText;
      

        [DbColumn("StatusPelapor")]
        public StatusPelapor StatusPelapor
        {
            get => statusLapor;
            set
            {
                SetProperty(ref statusLapor, value);
                if (value == StatusPelapor.Sendiri)
                    StatusPelaporText = "Sendiri";
                if (value == StatusPelapor.OrangTua)
                    StatusPelaporText = "Orang Tua";
                if (value == StatusPelapor.Famili)
                    StatusPelaporText = "Famili";
                if (value == StatusPelapor.Lain)
                {
                    if (StatusPelaporText == "Sendiri" || StatusPelaporText == "Orang Tua" || StatusPelaporText == "Famili")
                        StatusPelaporText = string.Empty;
                }
            }
        }
        public string StatusPelaporText { get => statusLaporText; set => SetProperty(ref statusLaporText, value); }
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
                    me[GetPropertyName(() => StatusPelaporText)]
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


            if (name == "StatusPelaporText" && string.IsNullOrEmpty(StatusPelaporText))
                return "Jelaskan";

            if (name == "StatusPelapor" && StatusPelapor == StatusPelapor.None)
                return " ";

            return null;
        }

    }
}
