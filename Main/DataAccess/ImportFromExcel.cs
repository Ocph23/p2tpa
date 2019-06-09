using Main.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Excel = Microsoft.Office.Interop.Excel;

namespace Main.DataAccess
{


    public delegate void onResult(List<Pengaduan> data);
   public class ImportFromExcel      :IDisposable
    {
       
        Excel.Application xlApp = new Excel.Application();
        Excel.Workbook xlWorkbook = null;

        public event onResult DataReseult;
        public ImportFromExcel()
        {
            Excel.Application xlApp = new Excel.Application();
            var path = Environment.CurrentDirectory + "\\ImportPengaduan.xlsx";
            xlWorkbook = xlApp.Workbooks.Open(path);
        }


        public List<Pengaduan> Pengaduans { get; private set; }

        public async void Start()
        {
            Pengaduans= await ProccessPengaduan();
            await ProccessKorban().ContinueWith(taskKorbanCompleteAsync);
           await ProccessPelapor().ContinueWith(taskPelaporCompleteAsync);
           await ProccessPerkembangan().ContinueWith(taskPerkembanganCompleteAsync);
            await ProccessTerlapor().ContinueWith(taskTerlaporCompleteAsync);
            await ProccessUraian().ContinueWith(taskUraianCompleteAsync);
            SaveToDatabase(Pengaduans);




           
        }

        private void SaveToDatabase(List<Pengaduan> pengaduans)
        {
            using (var db = new DbContext())
            {
                try
                {
                    foreach (var item in pengaduans)
                    {
                        var pengaduan = db.DataPengaduan.Where(O => O.KodeDistrik == item.KodeDistrik && O.Tanggal == item.Tanggal && O.Nomor == item.Nomor).FirstOrDefault();
                        if (pengaduan != null)
                            return;
                        int idPelapor = await GetPelapor(pengaduan.Pelapor);
                    }
                }
                catch (Exception)
                {

                    throw;
                }

              
            }
         

            if (DataReseult != null)
                DataReseult(Pengaduans);
        }

        private Task<int> GetPelapor(Pelapor pelapor)
        {
            using (var db = new DbContext())
            {

            }
        }

        private async Task taskUraianCompleteAsync(Task<List<Tuple<string, string, string>>> obj)
        {
            var result = await obj;
            if (Pengaduans == null)
                await Task.Delay(2000);

            foreach(var item in result)
            {
                var data = Pengaduans.Where(O => O.Nomor == item.Item1).FirstOrDefault();
                if (data != null)
                {
                    data.UraianKejadian = item.Item2;
                    data.Catatan = item.Item3;
                }
            }
        }

        private async Task taskPerkembanganCompleteAsync(Task<List<TahapanPerkembangan>> obj)
        {
            var result = await obj;
            if (Pengaduans == null)
                await Task.Delay(2000);

            
            foreach (var item in result.GroupBy(O=>O.NoReg))
            {
                var data = Pengaduans.Where(O => O.Nomor == item.Key).FirstOrDefault();
                data.Perkembangan = new List<TahapanPerkembangan>();
                foreach(var tahap in item)
                {
                    data.Perkembangan.Add(tahap);
                }
            }
        }

        private async Task taskKorbanCompleteAsync(Task<List<Korban>> obj)
        {
            var result = await obj;
            if (Pengaduans == null)
                await Task.Delay(2000);

            foreach (var item in result)
            {
                var data = Pengaduans.Where(O => O.Korban.Nama == item.Nama).FirstOrDefault();
                if (data != null)
                {
                    data.Korban.Nama = item.Nama;
                    data.Korban.NamaPanggilan = item.NamaPanggilan;
                    data.Korban.TempatLahir= item.TempatLahir;
                    data.Korban.Pendidikan = item.Pendidikan;
                    data.Korban.Agama= item.Agama;
                    data.Korban.Alamat = item.Alamat ;
                    data.Korban.TanggalLahir= item.TanggalLahir;
                    data.Korban.Gender= item.Gender;
                    data.Korban.NIK= item.NIK;
                    data.Korban.HubunganKorbanDenganTerlapor= item.HubunganKorbanDenganTerlapor;
                }
            }
        }

        private async Task taskTerlaporCompleteAsync(Task<List<Terlapor>> obj)
        {
            var result = await obj;
            if (Pengaduans == null)
                await Task.Delay(2000);

            foreach (var item in result)
            {
                var data = Pengaduans.Where(O => O.Terlapor.Nama == item.Nama).FirstOrDefault();
                if (data != null)
                {
                    data.Terlapor.Nama = item.Nama;
                    data.Terlapor.NamaPanggilan = item.NamaPanggilan;
                    data.Terlapor.TempatLahir = item.TempatLahir;
                    data.Terlapor.Pendidikan = item.Pendidikan;
                    data.Terlapor.Agama = item.Agama;
                    data.Terlapor.Alamat = item.Alamat;
                    data.Terlapor.TanggalLahir = item.TanggalLahir;
                    data.Terlapor.Gender = item.Gender;
                    data.Terlapor.NIK = item.NIK;
                }
            }
        }

        private async Task taskPelaporCompleteAsync(Task<List<Pelapor>> obj)
        {
            var result = await obj;
            if (Pengaduans == null)
                await Task.Delay(2000);

            foreach (var item in result)
            {
                var data = Pengaduans.Where(O => O.Pelapor.Nama == item.Nama).FirstOrDefault();
                if (data != null)
                {
                    data.Pelapor.Nama = item.Nama;
                    data.Pelapor.Alamat = item.Alamat;
                    data.Pelapor.Gender = item.Gender;
                }
            }
        }

        private async Task taskPengaduanComplete(Task<List<Pengaduan>> obj)
        {
            Pengaduans = await obj;

        }

        private Task<List<Pelapor>> ProccessPelapor()
        {
          Excel._Worksheet  xlWorksheet = xlWorkbook.Sheets["Pelapor"];
           
            List<Pelapor> listPelapor = new List<Pelapor>();

            //read pengaduan
          Excel.Range  rngPengaduan = (Excel.Range)xlWorksheet.Range["A3", "C100"];
           int row = 1;
            for (var i = 1; i <= 100; i++)
            {
                Pelapor pelaport = new Pelapor();
                pelaport.Nama = rngPengaduan.Cells[row, "A"].Value2;

                Gender data;
                var success = Enum.TryParse<Gender>(rngPengaduan.Cells[row, "B"].Value2, out data);
                if (!success)
                    break;
                pelaport.Gender = data;
                pelaport.Alamat = rngPengaduan.Cells[row, "C"].Value2;
                row++;

                listPelapor.Add(pelaport);


            }

            return Task.FromResult(listPelapor);

        }

        private Task<List<Pengaduan>> ProccessPengaduan()
        {
            Excel._Worksheet xlWorksheet = xlWorkbook.Sheets["Pengaduan"];
            Excel.Range xlRange = xlWorksheet.UsedRange;

            //read pengaduan
          
            int row = 1;
            var kodeDistrik = xlRange.Cells[2, 3].Value2;
            Excel.Range rngPengaduan = (Excel.Range)xlWorksheet.Range["A7", "AH100"];
            if (string.IsNullOrEmpty(kodeDistrik))
                throw new SystemException("Kode Distrik Tidak Ditemukan");

            List<Pengaduan> list = new List<Pengaduan>();

            for (var i = 1; i <= 100; i++)
            {
                var pengaduan = new Pengaduan();
                pengaduan.KodeDistrik = kodeDistrik;
                pengaduan.Nomor = rngPengaduan.Cells[row, "B"].Value2;
                if (string.IsNullOrEmpty(pengaduan.Nomor))
                    break;
                pengaduan.Tanggal = new DateTime(Convert.ToInt64(rngPengaduan.Cells[row, "C"].Value2));
                pengaduan.Waktu = new TimeSpan(Convert.ToInt64(rngPengaduan.Cells[row, "D"].Value2));
                pengaduan.Tempat = rngPengaduan.Cells[row, "E"].Value2;


                pengaduan.Pelapor = new Pelapor();
                pengaduan.Pelapor.StatusPelapor = ConvertEnum<StatusPelapor>(rngPengaduan.Cells[row, "F"].Value2);
                pengaduan.Pelapor.Nama = rngPengaduan.Cells[row, "G"].Value2;

                pengaduan.Terlapor = new Terlapor();
                pengaduan.Terlapor.Nama = rngPengaduan.Cells[row, "K"].Value2;

                pengaduan.Korban = new Korban();
                pengaduan.Korban.Nama = rngPengaduan.Cells[row, "I"].Value2;


                pengaduan.Kondisi = new KondisiKorban();
                pengaduan.Kondisi.Fisik = ConvertEnum<KondisiFisik>(rngPengaduan.Cells[row, "M"].Value2);
                pengaduan.Kondisi.Psikis = ConvertEnum<KondisiPsikis>( rngPengaduan.Cells[row, "N"].Value2);
                pengaduan.Kondisi.Sex = ConvertEnum<KondisiSex>(rngPengaduan.Cells[row, "O"].Value2);
                if (pengaduan.Kondisi.Sex != KondisiSex.Pendarahan)
                    pengaduan.Kondisi.SexText = rngPengaduan.Cells[row, "P"].Value2;

                //Kondisi Korban
                pengaduan.Dampak = new DampakKorban();
                pengaduan.Dampak.Fisik = rngPengaduan.Cells[row, "Q"].Value2;
                pengaduan.Dampak.Psikis = rngPengaduan.Cells[row, "R"].Value2;
                pengaduan.Dampak.Seksual= rngPengaduan.Cells[row, "S"].Value2;
                pengaduan.Dampak.Ekonomi= rngPengaduan.Cells[row, "T"].Value2;
                pengaduan.Dampak.Kesehatan= rngPengaduan.Cells[row, "U"].Value2;
                pengaduan.Dampak.Lain= rngPengaduan.Cells[row, "V"].Value2;

                pengaduan.Kejadian = new Kejadian();
                pengaduan.Kejadian.Waktu = new DateTime(Convert.ToInt64(rngPengaduan.Cells[row, "W"].Value2));
                pengaduan.Kejadian.Tempat= rngPengaduan.Cells[row, "X"].Value2;
                pengaduan.Kejadian.Fisik = rngPengaduan.Cells[row, "Y"].Value2;
                pengaduan.Kejadian.Psikis = rngPengaduan.Cells[row, "Z"].Value2;
                pengaduan.Kejadian.Penelantaran= rngPengaduan.Cells[row, "AA"].Value2;
                pengaduan.Kejadian.Seksual= rngPengaduan.Cells[row, "AB"].Value2;
                pengaduan.Kejadian.Penganiayaan = rngPengaduan.Cells[row, "AC"].Value2;
                pengaduan.Kejadian.Pencabulan= rngPengaduan.Cells[row, "AD"].Value2;
                pengaduan.Kejadian.Pemerkosaan = rngPengaduan.Cells[row, "AE"].Value2;
                pengaduan.Kejadian.Trafiking= rngPengaduan.Cells[row, "AF"].Value2;
                pengaduan.Kejadian.Lain= rngPengaduan.Cells[row, "AG"].Value2;
                pengaduan.Penanganan = rngPengaduan.Cells[row, "AH"].Value2;
                if(pengaduan.Penanganan=="Lain")
                    pengaduan.Penanganan = rngPengaduan.Cells[row, "AI"].Value2;









                row++;
               

                list.Add(pengaduan);
            }
            return  Task.FromResult(list);

        }

        private Task<List<Korban>> ProccessKorban()
        {
         
            List<Korban> listKorban = new List<Korban>();
            Excel._Worksheet xlWorksheet = xlWorkbook.Sheets["Korban"];
           Excel.Range rngPengaduan = (Excel.Range)xlWorksheet.Range["A2", "N100"];
           int row = 1;
            for (var i = 1; i <= 100; i++)
            {
                Korban data = new Korban();
                data.Nama = rngPengaduan.Cells[row, "B"].Value2;
                data.NamaPanggilan = rngPengaduan.Cells[row, "C"].Value2;

                Gender gender;
                var success = Enum.TryParse<Gender>(rngPengaduan.Cells[row, "D"].Value2, out gender);
                if (!success)
                    break;
                data.Gender = ConvertEnum<Gender>( rngPengaduan.Cells[row, "D"].Value2);
                data.TempatLahir = rngPengaduan.Cells[row, "E"].Value2;
                data.TanggalLahir = new DateTime(Convert.ToInt64(rngPengaduan.Cells[row, "F"].Value2));
                data.Alamat = rngPengaduan.Cells[row, "G"].Value2;
                data.NIK = rngPengaduan.Cells[row, "H"].Value2;
                data.Pekerjaan = rngPengaduan.Cells[row, "I"].Value2;
                data.Pendidikan = rngPengaduan.Cells[row, "J"].Value2;
                data.Agama = rngPengaduan.Cells[row, "K"].Value2;
                data.HubunganKorbanDenganTerlapor = rngPengaduan.Cells[row, "L"].Value2;
                data.Suku = ConvertEnum<Suku>(rngPengaduan.Cells[row, "M"].Value2);
                data.Pernikahan = ConvertEnum<StatusPernikahan>( rngPengaduan.Cells[row, "N"].Value2);

                row++;

                listKorban.Add(data);


            }

            return Task.FromResult(listKorban);

        }

        private Task<List<Terlapor>> ProccessTerlapor()
        {
            List<Terlapor> listTerlapor = new List<Terlapor>();
           Excel._Worksheet xlWorksheet = xlWorkbook.Sheets["Terlapor"];
          Excel.Range  rngPengaduan = (Excel.Range)xlWorksheet.Range["A2", "L100"];
            int row = 1;
            for (var i = 1; i <= 100; i++)
            {
                Terlapor data = new Terlapor();
                data.Nama = rngPengaduan.Cells[row, "B"].Value2;
                data.NamaPanggilan = rngPengaduan.Cells[row, "C"].Value2;

                Gender gender;
                var success = Enum.TryParse<Gender>(rngPengaduan.Cells[row, "D"].Value2, out gender);
                if (!success)
                    break;
                data.Gender = ConvertEnum<Gender>( rngPengaduan.Cells[row, "D"].Value2);
                data.TempatLahir = rngPengaduan.Cells[row, "E"].Value2;
                data.TanggalLahir = new DateTime(Convert.ToInt64(rngPengaduan.Cells[row, "F"].Value2));
                data.Alamat = rngPengaduan.Cells[row, "G"].Value2;
                data.NIK = rngPengaduan.Cells[row, "H"].Value2;
                data.Pekerjaan = rngPengaduan.Cells[row, "I"].Value2;
                data.Pendidikan = rngPengaduan.Cells[row, "J"].Value2;
                data.Agama = rngPengaduan.Cells[row, "K"].Value2;
                data.Suku = ConvertEnum<Suku>(rngPengaduan.Cells[row, "L"].Value2);
                row++;

                listTerlapor.Add(data);


            }

            return Task.FromResult(listTerlapor);

        }


        private Task<List<TahapanPerkembangan>> ProccessPerkembangan()
        {
            List<TahapanPerkembangan> list = new List<TahapanPerkembangan>();
            Excel._Worksheet xlWorksheet = xlWorkbook.Sheets["Perkembangan"];
            Excel.Range rngPengaduan = (Excel.Range)xlWorksheet.Range["A2", "D200"];
            int row = 1;
            for (var i = 1; i <= 100; i++)
            {
                TahapanPerkembangan data = new TahapanPerkembangan();
                data.NoReg = rngPengaduan.Cells[row, "A"].Value2;
                if (string.IsNullOrEmpty(data.NoReg))
                    break;
                data.Tanggal= new DateTime(Convert.ToInt64(rngPengaduan.Cells[row, "B"].Value2));
                data.BentukPenanganan = rngPengaduan.Cells[row, "C"].Value2;
                data.Keterangan= rngPengaduan.Cells[row, "D"].Value2;
                row++;

                list.Add(data);


            }

            return Task.FromResult(list);

        }


        private Task<List<Tuple<string,string,string>>> ProccessUraian()
        {
            List<Tuple<string, string, string>> list = new List<Tuple<string, string, string>>();
            Excel._Worksheet xlWorksheet = xlWorkbook.Sheets["Uraian&Catatan"];
            Excel.Range rngPengaduan = (Excel.Range)xlWorksheet.Range["A2", "C200"];
            int row = 1;
            for (var i = 1; i <= rngPengaduan.Rows.Count; i++)
            {
             
                var NoReg = rngPengaduan.Cells[row, "A"].Value2;
                if (string.IsNullOrEmpty(NoReg))
                    break;
                var uraian= rngPengaduan.Cells[row, "B"].Value2 ==null?string.Empty: rngPengaduan.Cells[row, "B"].Value2;
              var catatan = rngPengaduan.Cells[row, "C"].Value2 == null ? string.Empty : rngPengaduan.Cells[row, "C"].Value2;
                row++;

                var data = Tuple.Create(NoReg,uraian,catatan);
                list.Add(data);


            }

            return Task.FromResult(list);

        }




        private T ConvertEnum<T>(string value2)
        {
            if (string.IsNullOrEmpty(value2))
                return Activator.CreateInstance<T>();
            return (T)Enum.Parse(typeof(T), value2);
        }

        public void Dispose()
        {
            this.xlWorkbook.Close();
            
        }
    }




    public class DataCell
    {
       
    }
}
