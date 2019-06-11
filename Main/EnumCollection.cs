﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Main
{



    public enum Gender
    {
        L, P, None
    }



    public enum KondisiFisik {
        Sehat, Sakit, Luka
    }


    public enum KondisiPsikis
    {
        Ketakutan, Cemas, Emosi, Lain
    }


    public enum KDRT
    {
        Fisik, Psikis, Penelataran, Seksual
    }

    public enum KekerasanPulik
    {
        Penganiayaan, Pencabulan, Pemerkosaan, Trafiking
    }


    public enum KondisiSex
    {
        Lain, Pendarahan
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
            return new List<string> { "Islam", "Kristen", "Katolik", "Hindu", "Budha", "Kong Hu Chu" };
        }

        internal static List<string> DataPendidikan()
        {
            return new List<string> { "Tidak Sekolah", "Paud", "TK", "SD", "SLTP", "SLTA", "Perguruan Tinggi" };
        }

        internal static List<string> DataStatusPernikahan()
        {
            return new List<string> { "Nikah Adat", "Nikah Agama", "Nikah Catatan Sipil", "Tidak Ada Status Nikah", "Cerai" };
        }

        internal static List<string> DataSuku()
        {
            return new List<string> { "Papua", "Non Papua" };
        }
        internal static List<string> DataEnumPenganan()
        {
            return new List<string> { "Pendampingan Hukum", "Pendampingan Non Hukum", "Konsultasi", "Rujukan" };
        }

        internal static List<string> DataStatusPelapor()
        {
            return new List<string> { "Sendiri", "OrangTua", "Famili" };
        }
    }
}