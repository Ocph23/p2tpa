using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Main.ViewModels;

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


    public enum KategoriInstansi
    {
        None , DPPPA, P2TP2A, Kepolisian, Kesehatan, Lapas, Sosial,  Lainnya
    }

    public enum TingakatInstansi
    {
        None, Propinsi, Kabupaten, Distrik
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

        internal static List<string> DataTempatKejadian()
        {
            return new List<string> { "Rumah Tangga", "Tempat Kerja", "Sekolah" ,"Fasilitas Umum","Lainnya"};
        }

        internal static List<string> DataPekerjaan()
        {
            return new List<string> { "NA", "Bekerja", "Pelajar", "Ibu Rumah Tangga", "Swasta/Buruh" , "PNS/TNI/POLRI", "Pedagang/Tani/Nelayan","Tidak Bekerja"};
        }

        internal static List<string> DataInstansi()
        {
            throw new NotImplementedException();
        }

        internal static List<Layanan> GetDataLayananKorban()
        {
            return new List<Layanan>()
           {
               new Layanan(){ Name = "Bantuan Humum"},
                 new Layanan(){ Name = "Kesehatan"},
                 new Layanan(){ Name = "Pemulangan"},
                 new Layanan(){ Name = "Pendampingan Tokoh Agama"},
                 new Layanan(){ Name = "Penegakan Hukum"},
                 new Layanan(){ Name = "Pengaduan"},
                 new Layanan(){ Name = "Rehabilitasi Sosial"},
                 new Layanan(){ Name = "Reintegrasi Sosial"}
           };
        }

        internal static List<Layanan> GetDataLayananTerlapor() => new List<Layanan>()
           {
               new Layanan(){ Name = "Pelaporan"},
                 new Layanan(){ Name = "Pemeriksaan"},
                 new Layanan(){ Name = "Penyidikan"},
                 new Layanan(){ Name = "Penyidikan"},
                 new Layanan(){ Name = "Penangkapan"},
                 new Layanan(){ Name = "Penanganan"},
                 new Layanan(){ Name = "Penggeledahan"},
                 new Layanan(){ Name = "Penyitaan"},
                 new Layanan(){ Name = "Pra Penuntutan"},
                 new Layanan(){ Name = "Penuntutan"},
                 new Layanan(){ Name = "Pengadilan Tingkat I"},
                 new Layanan(){ Name = "Kasasi"},
                 new Layanan(){ Name = "Penindaklanjutan Kembali"},
                 new Layanan(){ Name = "Diversi"},

           };
    }
}