﻿using Ocph.DAL;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Main.ViewModels
{

    public class Identitas : BaseNotify
    {
        public Identitas()
        {

            ListAgama = EnumSource.DataAgama();
            ListHubunganKorban = EnumSource.HubunganKorbanDenganTerlapor();
            ListStatusPernikahan = Enum.GetValues(typeof(StatusPernikahan)).Cast<StatusPernikahan>().ToList();
            ListPendidikan = EnumSource.DataPendidikan();

        }
        private string nik;
        private string nama;
        private string tl;
        private string pekerjaan;
        private DateTime tgll;
        private string agama;
        private string pendidikan;
        private Gender gender;
       
        private StatusPernikahan pernikahan;
        private Suku suku;
        private string alamat;
        private string panggilan;

        private int? id;

        [PrimaryKey("Id")]
        [DbColumn("Id")]
        public int? Id { get => id; set => SetProperty(ref id, value); }

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
        public Suku Suku { get => suku; set => SetProperty(ref suku, value); }

        [DbColumn("Pernikahan")]
        public StatusPernikahan Pernikahan { get => pernikahan; set => SetProperty(ref pernikahan, value); }

        
        public Gender Gender { get => gender; set => SetProperty(ref gender, value); }

        public List<string> ListAgama { get; set; }
        public List<string> ListHubunganKorban { get; set; }
        public List<StatusPernikahan> ListStatusPernikahan { get; }
        public List<string> ListPendidikan { get;  set; }
    }





}