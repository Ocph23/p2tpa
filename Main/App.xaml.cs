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

        public App()
        {
            this.LoadCompleted();
        }

        private async void LoadCompleted()
        {
            Mapper.Initialize(x => {
                x.CreateMap<Pengaduan, PengaduanViewModel>();
                x.CreateMap<PengaduanViewModel, Pengaduan>();
            });
        }
    }
}
