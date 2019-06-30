using Main.ViewModels;
using Ocph.DAL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Main.Models
{
    [TableName("Hubungan")]
   public class HubunganDenganKorban:BaseNotify,IDataErrorInfo
    {
        public HubunganDenganKorban()
        {
        }

        public HubunganDenganKorban(Korban korban)
        {
            this.Korban = korban;
        }
        private string nama;

        private int? id;

        [PrimaryKey("Id")]
        [DbColumn("Id")]
        public int? Id
        {
            get { return id; }
            set { id = value; }
        }

        [DbColumn("JenisHubungan")]
        public string JenisHubungan
        {
            get { return nama; }
            set { SetProperty(ref nama, value); }
        }

        [DbColumn("KorbanId")]
        public int? KorbanId { get { return _korbanId; } set { SetProperty(ref _korbanId, value); } }

        private Korban _korban;

        public Korban Korban
        {
            get { return _korban; }
            set
            {
                SetProperty(ref _korban, value);
            }
        }

        private List<string> jenisHubungans;
        private int? _korbanId;

        public List<string> DataJenisHubungans
        {
            get
            {
                if (jenisHubungans == null)
                    jenisHubungans = EnumSource.HubunganKorbanDenganTerlapor();
                return jenisHubungans;
            }
            set { jenisHubungans = value; }
        }
        public string Error
        {
            get
            {
                IDataErrorInfo me = (IDataErrorInfo)this;
                string error =
                    me[GetPropertyName(() => JenisHubungan)] +
                    me[GetPropertyName(() => KorbanId)] 
                ;
                if (!string.IsNullOrEmpty(error))
                    // return "Please check inputted data.";
                    return null;
                return null;
            }
        }

        public string this[string columnName] =>Validate(columnName);


        public string Validate(string name)
        {

            if (name == "JenisHubungan" && string.IsNullOrEmpty(JenisHubungan))
                return "Nama Tidak Boleh Kosong";

            if (name == "KorbanId" && KorbanId==null)
                return "Nama Korban Belum Ada";

            return null;
        }


    }
}
