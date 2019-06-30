using Main.Models;
using Ocph.DAL.Provider.SQLite;
using Ocph.DAL.Repository;
using System;


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
            catch (Exception)
            {

            }
        }

        public IRepository<Pengaduan> DataPengaduan { get { return new Repository<Pengaduan>(this); } }
        public IRepository<Pelapor> DataPelapor{ get { return new Repository<Pelapor>(this); } }
        public IRepository<Korban> DataKorban{ get { return new Repository<Korban>(this); } }
        public IRepository<Terlapor> DataTerlapor { get { return new Repository<Terlapor>(this); } }
        public IRepository<DampakKorban> DataDampak { get { return new Repository<DampakKorban>(this); } }
        public IRepository<KondisiKorban> DataKondisiKorban { get { return new Repository<KondisiKorban>(this); } }
        public IRepository<Instansi> Instansi { get { return new Repository<Instansi>(this); } }
        public IRepository<Penanganan> Penanganan { get { return new Repository<Penanganan>(this); } }


    }
}
