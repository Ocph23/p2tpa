using Ocph.DAL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Main.Models
{
    [TableName("Korban")]
    public class Korban :Identitas  ,IDataErrorInfo
    {
        private string kekerasa;


        [DbColumn("kekerasan")]
        public string KekerasanDialami
        {
            get {

                return GetListKekerasanFromStringKekerasan(ListKekerasanDialami);
            }
            set { SetProperty(ref kekerasa , value);
                ListKekerasanDialami.Clear();
                foreach(var item in value.Split('#'))
                {
                    ListKekerasanDialami.Add(item);
                }

            }
        }


        public List<string> ListKekerasanDialami { get; set; } = new List<string>();


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
                    return error;
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



        public string GetListKekerasanFromStringKekerasan(IEnumerable<string> result)
        {
            if (result != null)
            {
                StringBuilder sb = new StringBuilder();
                var i = 0;
                foreach (var item in result)
                {
                    sb.Append($"{item}");
                    if (i + 1 < result.Count())
                    {
                        sb.Append("#");
                    }
                    i++;
                }
                return sb.ToString();
            }
            return string.Empty;
        }


    }
}
