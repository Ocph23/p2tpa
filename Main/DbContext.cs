﻿using Main.ViewModels;
using Ocph.DAL.Provider.SQLite;
using Ocph.DAL.Repository;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Main
{
    public class DbContext  :SQLiteDbConnection
    {
        private string path = AppDomain.CurrentDomain.BaseDirectory;
        public DbContext()
        {
            this.ConnectionString = "Data Source=database.db;Version=3;New=True;";
            OpenConnection();
        }

        private void OpenConnection()
        {
            try
            {

                this.Open();
            }
            catch (Exception ex)
            {

            }
        }

        public IRepository<Pengaduan> DataPengaduan { get { return new Repository<Pengaduan>(this); } }
        public IRepository<Pelapor> DataPelapor{ get { return new Repository<Pelapor>(this); } }
        public IRepository<Korban> DataKorban{ get { return new Repository<Korban>(this); } }
        public IRepository<Terlapor> DataTerlapor { get { return new Repository<Terlapor>(this); } }
        public IRepository<Kejadian> DataKejadian { get { return new Repository<Kejadian>(this); } }
        public IRepository<DampakKorban> DataDampak { get { return new Repository<DampakKorban>(this); } }
        public IRepository<KondisiKorban> DataKondisiKorban { get { return new Repository<KondisiKorban>(this); } }
        public IRepository<TahapanPerkembangan> DataTahapanPerkembangan { get { return new Repository<TahapanPerkembangan>(this); } }


    }
}
