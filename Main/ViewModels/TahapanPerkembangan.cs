using Ocph.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Main.ViewModels
{

    [TableName("Perkembangan")]
    public class TahapanPerkembangan : BaseNotify
    {
        private DateTime tgl;
        private string bentuk;
        private string keterangan;
        private int? id;
        private int pengaduanid;

        [PrimaryKey("Id")]
        [DbColumn("Id")]
        public int? Id { get => id; set => SetProperty(ref id, value); }

        [DbColumn("PengaduanId")]
        public int PengaduanId { get => pengaduanid; set => SetProperty(ref pengaduanid, value); }

        [DbColumn("Tanggal")]
        public DateTime Tanggal { get => tgl; set => SetProperty(ref tgl, value); }

        [DbColumn("Penanganan")]
        public string BentukPenanganan { get => bentuk; set => SetProperty(ref bentuk, value); }

        [DbColumn("Keterangan")]
        public string Keterangan { get => keterangan; set => SetProperty(ref keterangan, value); }

        public string NoReg { get; set; }
    }




}
