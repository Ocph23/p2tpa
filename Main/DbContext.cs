using Main.ViewModels;
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
            catch (Exception)
            {

            }
        }

        public IRepository<Pengaduan> DataPengaduan { get { return new Repository<Pengaduan>(this); } }
        public IRepository<PelaporViewModel> DataPelapor{ get { return new Repository<PelaporViewModel>(this); } }
        public IRepository<KorbanViewModel> DataKorban{ get { return new Repository<KorbanViewModel>(this); } }
        public IRepository<TerlaporViewModel> DataTerlapor { get { return new Repository<TerlaporViewModel>(this); } }
        public IRepository<DampakKorban> DataDampak { get { return new Repository<DampakKorban>(this); } }
        public IRepository<KondisiKorban> DataKondisiKorban { get { return new Repository<KondisiKorban>(this); } }
        public IRepository<Instansi> Instansi { get { return new Repository<Instansi>(this); } }

        public IRepository<Penanganan> Penanganan { get { return new Repository<Penanganan>(this); } }


    }
}
