using Ocph.DAL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Main.ViewModels
{

    [TableName("Kejadian")]
    public class Kejadian : BaseNotify  , IDataErrorInfo
    {

        [PrimaryKey("Id")]
        [DbColumn("Id")]
        public int? Id { get => id; set => SetProperty(ref id, value); }

        [DbColumn("PengaduanId")]
        public int PengaduanId { get => pengaduanid; set => SetProperty(ref pengaduanid, value); }


        [DbColumn("Waktu")]
        public DateTime Waktu { get => waktu; set => SetProperty(ref waktu, value); }

        [DbColumn("Tempat")]
        public string Tempat { get => tempat; set => SetProperty(ref tempat, value); }

        [DbColumn("KDRT")]
        public string KDRTData
        {
            get
            {
                StringBuilder sb = new StringBuilder();

                if (this.Fisik)
                    sb.Append(KDRT.Fisik + "#");
                if (this.Psikis)
                    sb.Append(KDRT.Psikis + "#");
                if (this.Seksual)
                    sb.Append(KDRT.Seksual+ "#");
                if (this.Penelantaran)
                    sb.Append(KDRT.Penelataran+ "#");

                return sb.ToString();

            }
            set {
                SetProperty(ref kdrtData, value);
                var result = value.Split('#');
                foreach(var item in result)
                {
                    if(!string.IsNullOrEmpty(item))
                    {
                        KDRT MyStatus = (KDRT)Enum.Parse(typeof(KDRT), item, true);
                        if (MyStatus == KDRT.Fisik)
                            this.Fisik = true;
                        if (MyStatus == KDRT.Penelataran)
                            this.Penelantaran = true;
                        if (MyStatus == KDRT.Psikis)
                            this.Psikis = true;
                        if (MyStatus == KDRT.Seksual)
                            this.Seksual = true;
                    }
                }
            }
        }

        [DbColumn("publik")]
        public string PublikData
        {
            get
            {
                StringBuilder sb = new StringBuilder();

                if (this.Penganiayaan)
                    sb.Append(KekerasanPulik.Penganiayaan+ "#");
                if (this.Pencabulan)
                    sb.Append(KekerasanPulik.Pencabulan + "#");
                if (this.Pemerkosaan)
                    sb.Append(KekerasanPulik.Pemerkosaan + "#");
                if (this.Trafiking)
                    sb.Append(KekerasanPulik.Trafiking+ "#");
                return sb.ToString();
            }
            set
            {
                SetProperty(ref publickData, value);
                var result = value.Split('#');
                foreach (var item in result)
                {
                    if (!string.IsNullOrEmpty(item))
                    {
                        KekerasanPulik MyStatus = (KekerasanPulik)Enum.Parse(typeof(KekerasanPulik), item, true);
                        if (MyStatus == KekerasanPulik.Pemerkosaan)
                            this.Pemerkosaan= true;
                        if (MyStatus == KekerasanPulik.Pencabulan)
                            this.Pencabulan = true;
                        if (MyStatus == KekerasanPulik.Penganiayaan)
                            this.Penganiayaan= true;
                        if (MyStatus == KekerasanPulik.Trafiking)
                            this.trafiking= true;
                    }
                }
            }
        }

        [DbColumn("Lainnya")]
        public string Lain { get => lain; set => SetProperty(ref lain, value); }

        public string this[string columnName] => Validate(columnName);


        public string Error
        {
            get
            {
                IDataErrorInfo me = (IDataErrorInfo)this;
                string error =
                    me[GetPropertyName(() => Waktu)] +
                    me[GetPropertyName(() => Tempat)] +
                    me[GetPropertyName(() => KDRTData)] +
                    me[GetPropertyName(() => PublikData)] +
                    me[GetPropertyName(() => Lain)] 
                    ;


                string dataError = me[GetPropertyName(() => Penganiayaan)] +
                                                  me[GetPropertyName(() => Pencabulan)] +
                                                  me[GetPropertyName(() => Pemerkosaan)] +
                                                  me[GetPropertyName(() => Trafiking)]+
                                                  me[GetPropertyName(() => Fisik)] +
                me[GetPropertyName(() => Psikis)] +
                me[GetPropertyName(() => Penelantaran)] +
                me[GetPropertyName(() => Seksual)];

                KDRTMessage = !string.IsNullOrEmpty(dataError) ? "Minimal Pilih Satu" : null;
                if (!string.IsNullOrEmpty(error+dataError))
                    return "Please check inputted data.";
                    //return null;
                return null;
            }
        }
        public string Validate(string name)
        {

            if (name == "Fisik" && CheckSelectedChoice())
                return "Minimal Pilih Salah Satu";


            if (name == "Psikis" && CheckSelectedChoice())
                return "Minimal Pilih Salah Satu";


            if (name == "Penelantaran" && CheckSelectedChoice())
                return "Minimal Pilih Salah Satu";


            if (name == "Seksual" && CheckSelectedChoice())
                return "Minimal Pilih Salah Satu";

            if (name == "Penganiayaan" && CheckSelectedChoice())
                return "Minimal Pilih Salah Satu";

            if (name == "Pencabulan" && CheckSelectedChoice())
                return "Minimal Pilih Salah Satu";

            if (name == "Pemerkosaan" && CheckSelectedChoice())
                return "Minimal Pilih Salah Satu";

            if (name == "Trafiking" && CheckSelectedChoice())
                return "Minimal Pilih Salah Satu";

            if (name == "Waktu" && Waktu == new DateTime())
                return "Waktu Tidak Boleh Kosong";

            if (name == "Tempat" && string.IsNullOrEmpty(Tempat))
                return "Tempat Tidak Boleh Kosong";

            return null;
        }

        private bool CheckSelectedChoice()
        {
            if (!Fisik && !Psikis && !Penelantaran && !Seksual && !Penganiayaan && !Pencabulan && !Pemerkosaan && !Trafiking && string.IsNullOrEmpty(Lain))
                return true;
            return false;
        }


        private bool fisik;
        private bool psikis;
        private bool penelantaran;
        private bool seksual;

        public bool Fisik { get => fisik; set => SetProperty(ref fisik, value); }
        public bool Psikis { get => psikis; set => SetProperty(ref psikis, value); }
        public bool Penelantaran { get => penelantaran; set => SetProperty(ref penelantaran, value); }
        public bool Seksual { get => seksual; set => SetProperty(ref seksual, value); }

        private DateTime waktu;
        private string lain;
        private string tempat;
        private int? id;
        private int pengaduanid;


        private bool penganiayaan;
        private bool pencabulan;
        private bool pemerkosaan;
        private bool trafiking;
        private string kdrtMessage;
        private string publikMessage;
        private string kdrtData;
        private string publickData;

        public bool Penganiayaan{ get => penganiayaan; set => SetProperty(ref penganiayaan, value); }
        public bool Pencabulan{ get => pencabulan; set => SetProperty(ref pencabulan, value); }
        public bool Pemerkosaan { get => pemerkosaan; set => SetProperty(ref pemerkosaan, value); }
        public bool Trafiking{ get => trafiking; set => SetProperty(ref trafiking, value); }

        public string KDRTMessage { get => kdrtMessage; set => SetProperty(ref kdrtMessage, value); }

        public string PublikMessage { get => publikMessage; set => SetProperty(ref publikMessage, value); }


    }


}
