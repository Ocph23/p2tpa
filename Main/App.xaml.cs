using AutoMapper;
using Main.Models;
using Main.ViewModels;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows;

namespace Main
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        [Obsolete]
        public App()
        {
            Load();
        }

        [Obsolete]
        private void Load()
        {
            Mapper.Initialize(x => {
                x.CreateMap<PengaduanViewModel, Pengaduan>().ReverseMap();
                x.CreateMap<Pengaduan, PengaduanViewModel>();

            });

        }
    }
}
