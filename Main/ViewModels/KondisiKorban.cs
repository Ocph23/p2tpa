using Ocph.DAL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Main.ViewModels
{

    [TableName("Kondisi")]
    public class KondisiKorban : BaseNotify ,IDataErrorInfo
    {
        private KondisiFisik fisik;
        private KondisiPsikis psikis;
        private KondisiSex sex;
        private int? id;
        private int pengaduanid;
        private string sextText;
        private string psikisText;

        [PrimaryKey("Id")]
        [DbColumn("Id")]
        public int? Id { get => id; set => SetProperty(ref id, value); }

        [DbColumn("PengaduanId")]
        public int PengaduanId { get => pengaduanid; set => SetProperty(ref pengaduanid, value); }

        [DbColumn("Fisik")]
        public KondisiFisik Fisik { get => fisik; set => SetProperty(ref fisik, value); }

        [DbColumn("Psikis")]
        public KondisiPsikis Psikis { get => psikis; set => SetProperty(ref psikis, value); }

        [DbColumn("Sex")]
        public KondisiSex Sex { get => sex; set => SetProperty(ref sex, value); }


        [DbColumn("PsikisText")]
        public string PsikisText { get => psikisText; set => SetProperty(ref psikisText, value); }

        [DbColumn("SexText")]
        public string SexText { get => sextText; set => SetProperty(ref sextText, value); }


        public string Error
        {
            get
            {
                IDataErrorInfo me = (IDataErrorInfo)this;
                string error =
                    me[GetPropertyName(() => PsikisText)] +
                    me[GetPropertyName(() => SexText)] 
                 
                    ;
                if (!string.IsNullOrEmpty(error))
                    //return "Please check inputted data.";
                    return null;
                return null;
            }
        }


        public string this[string columnName] => Validate(columnName);
        public string Validate(string name)
        {

            if (name == "Psikis" && Psikis == KondisiPsikis.Lain && string.IsNullOrEmpty(PsikisText))
                return " "; 

            if (name == "PsikisText" && Psikis == KondisiPsikis.Lain && string.IsNullOrEmpty(PsikisText))
                return "Jelaskan Tidak Boleh Kosong";

            if (name == "SexText" && Sex == KondisiSex.Lain && string.IsNullOrEmpty(SexText))
                return "Jelaskan Tidak Boleh Kosong";

            if (name == "Sex" && Sex == KondisiSex.Lain && string.IsNullOrEmpty(SexText))
                return " ";

            return null;
        }

    }
}
