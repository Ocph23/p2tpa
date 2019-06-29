using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Main.ViewModels
{
  public  class HubunganViewModel    :BaseNotify
    {
        private string nama;

        public string Nama
        {
            get { return nama; }
            set { SetProperty(ref nama ,value); }
        }

        private Korban _korban;

        public Korban Korban {
            get { return _korban; }
            set
            {
                SetProperty(ref _korban, value);
            }
        }
            
        public HubunganViewModel(Korban korban)
        {
            this.Korban = korban;
        }

        private List<string> jenisHubungans;
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


    }
}
