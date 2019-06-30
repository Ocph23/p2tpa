using Ocph.DAL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Main.Models
{

    [TableName("dampak")]
    public class DampakKorban : BaseNotify, IDataErrorInfo
    {

        private string fisik;
        private string psikis;
        private string sex;
        private string ekonomi;
        private string lain;
        private string kesehatan;
        private int? id;
        private int pengaduanid;

        [PrimaryKey("Id")]
        [DbColumn("Id")]
        public int? Id { get => id; set => SetProperty(ref id, value); }

        [DbColumn("PengaduanId")]
        public int PengaduanId { get => pengaduanid; set => SetProperty(ref pengaduanid, value); }


        [DbColumn("Fisik")]

        public string Fisik { get => fisik; set => SetProperty(ref fisik, value); }

        [DbColumn("Psikis")]
        public string Psikis { get => psikis; set => SetProperty(ref psikis, value); }

        [DbColumn("Seksual")]
        public string Seksual { get => sex; set => SetProperty(ref sex, value); }

        [DbColumn("Ekonomi")]
        public string Ekonomi { get => ekonomi; set => SetProperty(ref ekonomi, value); }

        [DbColumn("Kesehatan")]
        public string Kesehatan { get => kesehatan; set => SetProperty(ref kesehatan, value); }

        [DbColumn("Lainnya")]
        public string Lain { get => lain; set => SetProperty(ref lain, value); }


        public string this[string columnName] => Validate(columnName);
        public string Error
        {
            get
            {
                return null;
            }
        }
        public string Validate(string name)
        {
            return null;
        }

    }


}
