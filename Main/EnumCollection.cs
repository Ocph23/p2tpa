using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Main
{
    public enum StatusPelapor
    {
       None, Sendiri, OrangTua, Famili , Lain
    }


    public enum Gender
    {
        L,P,None
    }


    public enum StatusPernikahan
    {
        Tidak,Adat, Catatan_Sipil,Agama,Cerai
    }



    public enum KondisiFisik {
          Sehat,Sakit,Luka
    }


    public enum KondisiPsikis
    {
        Ketakutan,Cemas,Emosi, Lain
    }


    public enum KDRT
    {
        Fisik,Psikis,Penelataran,Seksual
    }

    public enum KekerasanPulik
    {
        Penganiayaan,Pencabulan,Pemerkosaan,Trafiking
    }


    public enum KondisiSex
    {
        Lain, Pendarahan
    }


    public enum Penanganan
    {
        Hukum, Non_Hukum, Konsultasi,Rujukan, Lain
    }

    public enum Suku
    {
        Papua, Non , None
    }



    public class EnumSource
    {
        public static List<string> HubunganKorbanDenganTerlapor()
        {
            return new List<string> { "Suami","Istri", "Mantan Suami", "Mantan Istri", "Pacar", "Teman", "Saudara Kandung", "Saudara Tiri", "Saudara Sepupu",
                "Paman", "Bibi", "Tetangga", "Lain-Lain"};
        }


        public static List<string> DataAgama()
        {
            return new List<string> { "Islam","Kristen","Katolik","Hindu","Budha","Kong Hu Chu"};
        }

        internal static List<string> DataPendidikan()
        {
            return new List<string> { "Tidak Sekolah", "Paud", "TK", "SD", "SLTP", "SLTA", "Perguruan Tinggi" };
        }
    }
}
