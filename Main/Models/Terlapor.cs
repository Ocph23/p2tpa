using Ocph.DAL;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Main.ViewModels
{

    [TableName("Terlapor")]
   public class Terlapor : Identitas
    {
        private List<HubunganViewModel> _hubungan = new List<HubunganViewModel>();
        
        public List<HubunganViewModel> Hubungan
        {
            get { return _hubungan; }
            set { _hubungan = value; }
        }



    }
}
