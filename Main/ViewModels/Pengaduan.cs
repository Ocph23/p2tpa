using MaterialDesignThemes.Wpf;
using Ocph.DAL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;

namespace Main.ViewModels
{

    [TableName("pengaduan")]
    public class Pengaduan : BaseNotify ,IDataErrorInfo
    {
        private string nomor;
        private string rujukan;
        private string hari;
        private DateTime tanggal;
        private Pelapor pelapor;
        private Korban korban;
        private Terlapor terlapor;
        private KondisiKorban kondisi;
        private DampakKorban dampak;
        private Kejadian kejadian;
        private string penanganan;
        private string uraian;
        private List<TahapanPerkembangan> perkembangan;
        private string catatan;
        private TimeSpan waktu;
        private string tempat;
        private string penerima;
        private int? id;
        private int idTerlapor;
        private int idPelapor;
        private int idKorban;
        private string kodeDistrik;
        private PackIcon _icon;

        [PrimaryKey("id")]
        [DbColumn("id")]
        public int? Id { get => id; set => SetProperty(ref id, value); }

       
        [DbColumn("KodeDistrik")]
        public string KodeDistrik
        {
            get { return kodeDistrik; }
            set { SetProperty(ref kodeDistrik, value); }
        }


        [DbColumn("idPelapor")]
        public int IdPelapor { get => idPelapor; set => SetProperty(ref idPelapor, value); }

        [DbColumn("idKorban")]
        public int IdKorban { get => idKorban; set => SetProperty(ref idKorban, value); }

        [DbColumn("idTerlapor")]
        public int IdTerlapor { get => idTerlapor; set => SetProperty(ref idTerlapor, value); }




        [DbColumn("nomor")]
        public string Nomor { get => nomor; set => SetProperty(ref nomor, value); }

        [DbColumn("rujukan")]

        public string Rujukan { get => rujukan; set => SetProperty(ref rujukan, value); }


        [DbColumn("hari")]
        public string Hari { get => hari; set => SetProperty(ref hari, value); }


        [DbColumn("penerima")]
        public string Penerima { get => penerima; set => SetProperty(ref penerima, value); }


        [DbColumn("tanggal")]
        public DateTime Tanggal { get => tanggal; set => SetProperty(ref tanggal, value); }

        public TimeSpan Waktu { get => waktu; set => SetProperty(ref waktu, value); }


        [DbColumn("tempat")]
        public string Tempat { get => tempat; set => SetProperty(ref tempat, value); }


        [DbColumn("uraian")]
        public string UraianKejadian { get => uraian; set => SetProperty(ref uraian, value); }

        [DbColumn("catatan")]
        public string Catatan { get => catatan; set => SetProperty(ref catatan, value); }

        [DbColumn("penanganan")]
        public string Penanganan { get => penanganan; set => SetProperty(ref penanganan, value); }


        public string hkdt;

        [DbColumn("HubunganKorbanDenganTerlapor")]
        public string HubunganKorbanDenganTerlapor
        {
            get => hkdt;
            set => SetProperty(ref hkdt, value);
        }


        public Pelapor Pelapor { get => pelapor; set => SetProperty(ref pelapor, value); }

        public Korban Korban { get => korban; set => SetProperty(ref korban, value); }

        public Terlapor Terlapor { get => terlapor; set => SetProperty(ref terlapor, value); }

        public KondisiKorban Kondisi { get => kondisi; set => SetProperty(ref kondisi, value); }

        public DampakKorban Dampak { get => dampak; set => SetProperty(ref dampak, value); }

        public Kejadian Kejadian { get => kejadian; set => SetProperty(ref kejadian, value); }



        public List<TahapanPerkembangan> Perkembangan { get => perkembangan; set => SetProperty(ref perkembangan, value); }

        public PackIcon Icon { get => _icon; set => SetProperty(ref _icon, value); } 


        private string GetPropertyName(string nomor)
        {
            throw new NotImplementedException();
        }

        public string this[string columnName] => Validate(columnName);

        public string Error
        {
            get
            {
                IDataErrorInfo me = (IDataErrorInfo)this;
                string error =
                    me[GetPropertyName(() => KodeDistrik)] +
                    me[GetPropertyName(() => Nomor)] +
                    me[GetPropertyName(() => Rujukan)] +
                    me[GetPropertyName(() => Hari)] +
                    me[GetPropertyName(() => Waktu)] +
                    me[GetPropertyName(() => Tanggal)] +
                    me[GetPropertyName(() => Penerima)] +
                    me[GetPropertyName(() => Tempat)]
                    ;
                if (!string.IsNullOrEmpty(error))
                    return null;
                return null;
            }
        }

        public Pengaduan()
        {
            this.Icon = new PackIcon { Kind = PackIconKind.TimerSand };
        }

        private string Validate(string name)
        {
            if (name == "KodeDistrik" && string.IsNullOrEmpty(KodeDistrik))
                return "Distrik Tidak Boleh Kosong";

            if (name == "Nomor" && string.IsNullOrEmpty(Nomor))
             return "Nomor Tidak Boleh Kosong";

            if (name == "Rujukan" && string.IsNullOrEmpty(Rujukan))
                  return "Rujukan Tidak Boleh Kosong";

            if (name == "Hari" && string.IsNullOrEmpty(Hari))
                  return "Hari Tidak Boleh Kosong";


            if (name == "Tanggal" && new DateTime() == Tanggal)
                  return "Tanggal Tidak Boleh Kosong";

            if (name == "Waktu" && Waktu == new TimeSpan())
                  return "Waktu Tidak Boleh Kosong";

            if (name == "Penerima" && string.IsNullOrEmpty(Penerima))
                  return "Penerima Pengaduan Tidak Boleh Kosong";

            if (name == "Tempat" && string.IsNullOrEmpty(Tempat))
                  return "Tempat Tidak Boleh Kosong";


            if (name == "UraianKejadian" && string.IsNullOrEmpty(UraianKejadian))
                  return "UraianKejadian Tidak Boleh Kosong";

            return null;
        }

    }




}
