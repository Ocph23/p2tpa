using Ocph.DAL;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Main.ViewModels
{
    public abstract class Identitas : BaseNotify
    {
        public Identitas()
        {
            ListAgama = EnumSource.DataAgama();
            ListHubunganKorban = EnumSource.HubunganKorbanDenganTerlapor();
            ListStatusPernikahan = EnumSource.DataStatusPernikahan();
            ListPendidikan = EnumSource.DataPendidikan();
            ListPekerjaan = EnumSource.DataPekerjaan();

        }
        private string nik;
        private string nama;
        private string tl;
        private string pekerjaan;
        private DateTime tgll = new DateTime(1988,1,1);
        private string agama;
        private string pendidikan;
        private Gender gender;
       
        private string pernikahan;
        private string suku;
        private string alamat;
        private string panggilan;
        private int pengaduanid;
        private int? id;



        [PrimaryKey("Id")]
        [DbColumn("Id")]
        public int? Id { get => id; set => SetProperty(ref id, value); }
     

        [DbColumn("PengaduanId")]
        public int PengaduanId { get => pengaduanid; set => SetProperty(ref pengaduanid, value); }


        [DbColumn("NIK")]
        public string NIK { get => nik; set => SetProperty(ref nik, value); }

        [DbColumn("Panggilan")]
        public string NamaPanggilan { get => panggilan; set => SetProperty(ref panggilan, value); }

        [DbColumn("TempatLahir")]
        public string TempatLahir { get => tl; set => SetProperty(ref tl, value); }

        [DbColumn("TanggalLahir")]
        public DateTime TanggalLahir { get => tgll; set => SetProperty(ref tgll, value); }

        [DbColumn("Pekerjaan")]
        public string Pekerjaan { get => pekerjaan; set => SetProperty(ref pekerjaan, value); }

        [DbColumn("Agama")]
        public string Agama { get => agama; set => SetProperty(ref agama, value); }

        [DbColumn("Pendidikan")]
        public string Pendidikan { get => pendidikan; set => SetProperty(ref pendidikan, value); }

        [DbColumn("Nama")]
        public string Nama { get => nama; set => SetProperty(ref nama, value); }

        [DbColumn("Alamat")]
        public string Alamat { get => alamat; set => SetProperty(ref alamat, value); }

        [DbColumn("Suku")]
        public string Suku { get => suku; set => SetProperty(ref suku, value); }

        [DbColumn("Pernikahan")]
        public string Pernikahan { get => pernikahan; set => SetProperty(ref pernikahan, value); }

        [DbColumn("Gender")]
        public Gender Gender { get => gender; set => SetProperty(ref gender, value); }


        public List<Penanganan> DataPenanganan { get; set; } = new List<Penanganan>();


        public string NoReq { get; set; }
        public List<string> ListAgama { get; set; }
        public List<string> ListHubunganKorban { get; set; }
        public List<string> ListStatusPernikahan { get; }
        public List<string> ListPendidikan { get;  set; }

        public List<string> ListPekerjaan { get; set; }




        public string StatusPelaporText
        {
            set
            {
                if (Pekerjaan != null)
                {
                    return;
                }
                if (!string.IsNullOrEmpty(value))
                {
                    ListPekerjaan.Add(value);
                    Pekerjaan = value;
                }

            }
        }
    }





}
