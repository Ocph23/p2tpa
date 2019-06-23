using Main.Models;
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
        private DateTime? tanggal = DateTime.Now;
        private PelaporViewModel pelapor;
        private List<KorbanViewModel> korbans = new List<KorbanViewModel>();
        private List<TerlaporViewModel> terlapors= new List<TerlaporViewModel>();
        private KondisiKorban kondisi;
        private DampakKorban dampak;
        private string penanganan;
        private string uraian;
        private string catatan;
        private DateTime? waktu;
        private string tempat;
        private string penerima;
        private int? id;
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

        [DbColumn("nomor")]
        public string Nomor { get => nomor; set => SetProperty(ref nomor, value); }

        [DbColumn("rujukan")]

        public string Rujukan { get => rujukan; set => SetProperty(ref rujukan, value); }

        [DbColumn("penerima")]
        public string Penerima { get => penerima; set => SetProperty(ref penerima, value); }

        [DbColumn("harilapor")]
        public string JamLapor
        {
            get => hari;
            set
            {
                SetProperty(ref hari, value);
            }
        }
        [DbColumn("tanggallapor")]
        public DateTime? TanggalLapor { get => tanggal; set => SetProperty(ref tanggal, value); }

        [DbColumn("waktulapor")]
        public DateTime? WaktuLapor { get => waktu; set => SetProperty(ref waktu, value); }


        [DbColumn("tempat")]
        public string TempatLapor { get => tempat; set => SetProperty(ref tempat, value); }


        [DbColumn("uraian")]
        public string UraianKejadian { get => uraian; set => SetProperty(ref uraian, value); }

        [DbColumn("catatan")]
        public string Catatan { get => catatan; set => SetProperty(ref catatan, value); }


        [DbColumn("StatusPelapor")]
        public string StatusPelapor
        {
            get => statusLapor;
            set
            {
                SetProperty(ref statusLapor, value);

            }
        }

        [DbColumn("harikejadian")]
        public string JamKejadian
        {
            get => _jam;
            set
            {
                SetProperty(ref _jam, value);
            }
        }
        [DbColumn("tanggalkejadian")]
        public DateTime? TanggalKejadian{ get => _tanggal; set => SetProperty(ref _tanggal, value); }

        [DbColumn("waktukejadian")]
        public DateTime? WaktuKejadian{ get => _waktu; set => SetProperty(ref _waktu, value); }


        [DbColumn("tempatkejadian")]
        public string TempatKejadian{ get => _TempatKejadian; set => SetProperty(ref _TempatKejadian, value); }


        public string hkdt;
        private string statusLapor;
        private string statusLaporText;
        private string _TempatKejadian;
        private DateTime? _waktu;
        private DateTime? _tanggal;
        private string _jam;

      

        public PelaporViewModel Pelapor { get => pelapor; set => SetProperty(ref pelapor, value); }

        public List<KorbanViewModel> Korban { get => korbans; set => SetProperty(ref korbans, value); }

        public List<TerlaporViewModel> Terlapor { get => terlapors; set => SetProperty(ref terlapors, value); }

        public KondisiKorban Kondisi { get => kondisi; set => SetProperty(ref kondisi, value); }

        public DampakKorban Dampak { get => dampak; set => SetProperty(ref dampak, value); }


        public List<Kecamatan> Kecamatan { get; set; }


        public PackIcon Icon { get => _icon; set => SetProperty(ref _icon, value); }
        public List<string> StatusPelapors { get; }

        
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
                    me[GetPropertyName(() => WaktuLapor)] +
                    me[GetPropertyName(() => TanggalLapor)] +
                    me[GetPropertyName(() => Penerima)] +
                    me[GetPropertyName(() => TempatLapor)]
                    ;
                if (!string.IsNullOrEmpty(error))
                    return error;
                return null;
            }
        }

        public Pengaduan()
        {
            this.Icon = new PackIcon { Kind = PackIconKind.TimerSand };
            StatusPelapors = EnumSource.DataStatusPelapor();
            Kecamatan = DataAccess.DataBasic.GetKecamatan();
            this.Pelapor = new PelaporViewModel();
            this.Terlapor = new List<TerlaporViewModel>();
            this.Korban = new List<KorbanViewModel>();
            this.Kondisi = new KondisiKorban();
            this.Dampak = new DampakKorban();
        }

        private string Validate(string name)
        {
            if (name == "KodeDistrik" && string.IsNullOrEmpty(KodeDistrik))
                return "Distrik Tidak Boleh Kosong";

            if (name == "Nomor" && string.IsNullOrEmpty(Nomor))
             return "Nomor Tidak Boleh Kosong";

            if (name == "Rujukan" && string.IsNullOrEmpty(Rujukan))
                  return "Rujukan Tidak Boleh Kosong";

            if (name == "Hari" && string.IsNullOrEmpty(JamKejadian))
                  return "Hari Tidak Boleh Kosong";


            if (name == "Tanggal" && new DateTime() == TanggalLapor)
                  return "Tanggal Tidak Boleh Kosong";

            if (name == "Waktu" && WaktuLapor == new DateTime())
                  return "Waktu Tidak Boleh Kosong";

            if (name == "Penerima" && string.IsNullOrEmpty(Penerima))
                  return "Penerima Pengaduan Tidak Boleh Kosong";

            if (name == "Tempat" && string.IsNullOrEmpty(TempatLapor))
                  return "Tempat Tidak Boleh Kosong";

            if (name == "StatusPelapor" && string.IsNullOrEmpty(StatusPelapor))
                return "Status Pelapor Tidak Boleh Kosong";


            if (name == "UraianKejadian" && string.IsNullOrEmpty(UraianKejadian))
                  return "UraianKejadian Tidak Boleh Kosong";

            return null;
        }


        public string StatusPelaporText
        {
            get => statusLaporText;
            set
            {
                SetProperty(ref statusLaporText, value);
                if (StatusPelapor != null)
                {
                    return;
                }
                if (!string.IsNullOrEmpty(value))
                {
                    StatusPelapors.Add(value);
                    StatusPelapor = value;
                }

            }
        }



    }



}
