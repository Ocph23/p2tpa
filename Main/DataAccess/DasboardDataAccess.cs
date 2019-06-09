using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Main.DataAccess
{
    public class DasboardDataAccess
    {
        private PengaduanServices pengaduan = new PengaduanServices();
        public int JumlahKasus()
        {
            return pengaduan.Count;
        }

    }
}
