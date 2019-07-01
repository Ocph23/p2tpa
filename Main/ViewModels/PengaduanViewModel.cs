using Main.Models;
using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Main.ViewModels
{
    public class PengaduanViewModel:Pengaduan
    {
        public PengaduanViewModel()
        {
            this.Icon = new PackIcon { Kind = PackIconKind.TimerSand };
          
            Kecamatan = DataAccess.DataBasic.GetKecamatan();
            this.Pelapor = new Pelapor();
            this.Terlapor = new List<Terlapor>();
            this.Korban = new List<Korban>();
            this.Kondisi = new KondisiKorban();
            this.Dampak = new DampakKorban();
        }
       

        public List<Kecamatan> Kecamatan { get; set; }



        public void AddKorban(Korban korban)
        {
            foreach (var item in Terlapor)
            {
                item.Hubungan.Add(new HubunganDenganKorban(item.Id,korban));
            }
            this.Korban.Add(korban);
        }

        public void AddTerlapor(Terlapor terlapor)
        {
            foreach (var item in Korban)
            {
                terlapor.Hubungan.Add(new HubunganDenganKorban(terlapor.Id, item));
            }
            this.Terlapor.Add(terlapor);
        }


    }
}
