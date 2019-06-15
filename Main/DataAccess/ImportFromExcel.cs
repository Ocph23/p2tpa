using Main.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;
using Excel = Microsoft.Office.Interop.Excel;

namespace Main.DataAccess
{

    public delegate void onResult(List<Pengaduan> data);
    public class ImportFromExcel : IDisposable
    {

        Excel.Application xlApp = new Excel.Application();
        Excel.Workbook xlWorkbook = null;

        public event onResult DataReseult;
        public ImportFromExcel()
        {
            Excel.Application xlApp = new Excel.Application();
            var path = Environment.CurrentDirectory + "\\ImportPengaduan.xlsx";
            xlWorkbook = xlApp.Workbooks.Open(path);
            Pengaduans = new ObservableCollection<Pengaduan>();
            PengaduanViews = (CollectionView)CollectionViewSource.GetDefaultView(Pengaduans);
            SaveCommand = new CommandHandler { CanExecuteAction = x => true, ExecuteAction = SaveToDatabaseAsync };
            this.StartAsync();
        }
        public async void StartAsync()
        {
            await Task.Delay(500);
            await ProccessPengaduan().ContinueWith(taskPengaduanCompleteAsync);
            await ProccessKorban().ContinueWith(taskKorbanCompleteAsync);
            await ProccessPelapor().ContinueWith(taskPelaporCompleteAsync);
            await ProccessTerlapor().ContinueWith(taskTerlaporCompleteAsync);
            await ProccessPerkembangan().ContinueWith(taskPerkembanganCompleteAsync);

            await ProccessUraian().ContinueWith(taskUraianCompleteAsync);
            //  SaveToDatabaseAsync(Pengaduans.ToList());
            PengaduanViews.Refresh();
        }

        private async Task taskPengaduanCompleteAsync(Task<List<Pengaduan>> obj)
        {
            var result = await obj;

            foreach (var item in result)
            {
                App.Current.Dispatcher.Invoke((System.Action)delegate
                {
                    this.Pengaduans.Add(item);
                });
            }
        }

        private void SaveToDatabaseAsync(object obj)
        {
            foreach (var item in this.Pengaduans)
            {
                SaveDataAsync(item).ContinueWith(completeSaveAsync);
            }

        }

        public ObservableCollection<Pengaduan> Pengaduans { get; set; }
        public CollectionView PengaduanViews { get; set; }
        public CommandHandler SaveCommand { get; }


        private async Task completeSaveAsync(Task<Tuple<string, Pengaduan, bool>> obj)
        {
            var result = await obj;
            App.Current.Dispatcher.Invoke((System.Action)delegate
            {

                result.Item2.Icon.ToolTip = result.Item1;

                if (result.Item3)
                {
                    result.Item2.Icon.Kind = MaterialDesignThemes.Wpf.PackIconKind.DoneAll;
                    result.Item2.Icon.Foreground = Brushes.Green;

                }
                else
                {
                    if (result.Item1 == "Data Sudah Ada")
                    {
                        result.Item2.Icon.Kind = MaterialDesignThemes.Wpf.PackIconKind.ContentDuplicate;
                        result.Item2.Icon.Foreground = Brushes.Orange;
                    }
                    else
                    {
                        result.Item2.Icon.Kind = MaterialDesignThemes.Wpf.PackIconKind.Error;
                        result.Item2.Icon.Foreground = Brushes.Red;
                    }

                }
            });


        }

        private async Task<Tuple<string, Pengaduan, bool>> SaveDataAsync(Pengaduan item)
        {
            using (var db = new DbContext())
            {
                var trans = db.BeginTransaction();
                try
                {
                    var pengaduanFound = db.DataPengaduan.Where(O => O.KodeDistrik == item.KodeDistrik && O.Nomor == item.Nomor).FirstOrDefault();
                    if (pengaduanFound != null)
                        throw new SystemException("Data Sudah Ada");

                    var idPelapor = await GetIdIdentitas(item.Pelapor, db);
                    if (idPelapor <= 0)
                        throw new SystemException("Data Pelapor Tidak Ditemukan/Tidak Lengkap");


                    item.IdPelapor = idPelapor;

                    item.Id = db.DataPengaduan.InsertAndGetLastID(item);
                    if (item.Id <= 0)
                        throw new SystemException("Data Pengaduan Tidak Tersimpan");

                    item.Kejadian.PengaduanId = item.Id.Value;
                    item.Dampak.PengaduanId = item.Id.Value;
                    item.Kondisi.PengaduanId = item.Id.Value;

                    item.Kondisi.Id = db.DataKondisiKorban.InsertAndGetLastID(item.Kondisi);
                    if (item.Kondisi.Id <= 0)
                        throw new SystemException("Data Kondisi Korban Tidak Tersimpan");

                    item.Dampak.Id = db.DataDampak.InsertAndGetLastID(item.Dampak);
                    if (item.Dampak.Id <= 0)
                        throw new SystemException("Data  Dampak Yang Dialami Tidak Tersimpan");

                    item.Kejadian.Id = db.DataKejadian.InsertAndGetLastID(item.Kejadian);
                    if (item.Kejadian.Id <= 0)
                        throw new SystemException("Data  Kejadian Tidak Tersimpan");
                    if (item.Perkembangan != null)
                    {
                        foreach (var data in item.Perkembangan)
                        {
                            data.PengaduanId = item.Id.Value;
                            data.Id = db.DataTahapanPerkembangan.InsertAndGetLastID(data);
                            if (data.Id <= 0)
                                throw new SystemException("Data Tahapan  Perkembangan Kejadian Tidak Tersimpan");
                        }
                    }

                    trans.Commit();
                    return Tuple.Create("Berhasil", item, true);
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    return Tuple.Create(ex.Message, item, false);
                }


            }


        }

        private Task<int> GetIdIdentitas(Pelapor pelapor, DbContext db)
        {
            if (!string.IsNullOrEmpty(pelapor.Error))
                throw new SystemException("Data pelapor tidak Valid");
            Gender gender = pelapor.Gender;
            var data = db.DataPelapor.Select().Where(O => O.Nama == pelapor.Nama).FirstOrDefault();
            if (data != null)
                return Task.FromResult(data.Id.Value);
            var id = db.DataPelapor.InsertAndGetLastID(pelapor);
            return Task.FromResult(id);
        }

        private Task<int> GetIdIdentitas(Korban item, DbContext db)
        {
            if (!string.IsNullOrEmpty(item.Error))
                throw new SystemException("Data Korban tidak Valid");
            Gender gender = item.Gender;
            var data = db.DataKorban.Select().Where(O => O.Nama == item.Nama && O.Gender == gender).FirstOrDefault();
            if (data != null)
                return Task.FromResult(data.Id.Value);
            var id = db.DataKorban.InsertAndGetLastID(item);
            return Task.FromResult(id);
        }

        private Task<int> GetIdIdentitas(Terlapor item, DbContext db)
        {
            if (!string.IsNullOrEmpty(item.Error))
                throw new SystemException("Data Terlapor tidak Valid");
            Gender gender = item.Gender;
            var data = db.DataTerlapor.Select().Where(O => O.Nama == item.Nama && O.Gender == gender).FirstOrDefault();
            if (data != null)
                return Task.FromResult(data.Id.Value);
            var id = db.DataTerlapor.InsertAndGetLastID(item);
            return Task.FromResult(id);
        }

        private async Task taskUraianCompleteAsync(Task<List<Tuple<string, string, string>>> obj)
        {
            var result = await obj;

            App.Current.Dispatcher.Invoke((System.Action)async delegate
            {
                if (Pengaduans.Count <= 0)
                    await Task.Delay(2000);
                foreach (var item in result)
                {
                    var data = Pengaduans.Where(O => O.Nomor == item.Item1).FirstOrDefault();
                    if (data != null)
                    {
                        data.UraianKejadian = item.Item2;
                        data.Catatan = item.Item3;
                    }
                }
            });

        }

        private async Task taskPerkembanganCompleteAsync(Task<List<TahapanPerkembangan>> obj)
        {
            var result = await obj;
            App.Current.Dispatcher.Invoke((System.Action)async delegate
            {
                if (Pengaduans.Count <= 0)
                    await Task.Delay(10000);
                foreach (var item in result.GroupBy(O => O.NoReg))
                {
                    var data = Pengaduans.Where(O => O.Nomor == item.Key).FirstOrDefault();
                    data.Perkembangan = new List<TahapanPerkembangan>();
                    foreach (var tahap in item)
                    {
                        data.Perkembangan.Add(tahap);
                    }
                }
            });
        }

        private async void taskKorbanCompleteAsync(Task<List<Korban>> obj)
        {
            var datas = await obj;
            App.Current.Dispatcher.Invoke((System.Action)async delegate
            {
                if (Pengaduans.Count <= 0)
                    await Task.Delay(10000);
                foreach (var item in datas)
                {
                    var pengaduans = Pengaduans.Where(O => O.Id == item.PengaduanId).FirstOrDefault();
                    foreach (var data in pengaduans.Terlapor.Where(x=>x.Nama==item.Nama))
                    {
                        data.Nama = item.Nama;
                        data.NamaPanggilan = item.NamaPanggilan;
                        data.TempatLahir = item.TempatLahir;
                        data.Pendidikan = item.Pendidikan;
                        data.Agama = item.Agama;
                        data.Alamat = item.Alamat;
                        data.TanggalLahir = item.TanggalLahir;
                        data.Gender = item.Gender;
                        data.NIK = item.NIK;
                    }
                }
            });

        }

        private async void taskTerlaporCompleteAsync(Task<List<Terlapor>> obj)
        {
            var datas = await obj;
            App.Current.Dispatcher.Invoke((System.Action)async delegate
            {
                if (Pengaduans.Count <= 0)
                    await Task.Delay(10000);
                foreach (var item in datas)
                {
                    var pengaduans = Pengaduans.Where(O => O.Id == item.PengaduanId).FirstOrDefault();
                    foreach (var data in pengaduans.Terlapor.Where(x=>x.Nama==item.Nama))
                    {
                        data.Nama = item.Nama;
                        data.NamaPanggilan = item.NamaPanggilan;
                        data.TempatLahir = item.TempatLahir;
                        data.Pendidikan = item.Pendidikan;
                        data.Agama = item.Agama;
                        data.Alamat = item.Alamat;
                        data.TanggalLahir = item.TanggalLahir;
                        data.Gender = item.Gender;
                        data.NIK = item.NIK;
                    }
                }
            });


        }

        private async void taskPelaporCompleteAsync(Task<List<Pelapor>> obj)
        {
            var datas = await obj;
            App.Current.Dispatcher.Invoke((System.Action)async delegate
            {
                if (Pengaduans.Count <= 0)
                    await Task.Delay(10000);
                foreach (var item in datas)
                {
                    var pengaduans = Pengaduans.Where(O => O.Pelapor.Nama == item.Nama);
                    foreach (var data in pengaduans)
                    {
                        data.Pelapor.Nama = item.Nama;
                        data.Pelapor.Alamat = item.Alamat;
                        data.Pelapor.Gender = item.Gender;
                    }
                }
            });
        }


        private Task<List<Pelapor>> ProccessPelapor()
        {
            Excel._Worksheet xlWorksheet = xlWorkbook.Sheets["Pelapor"];

            List<Pelapor> listPelapor = new List<Pelapor>();

            //read pengaduan
            Excel.Range rngPengaduan = (Excel.Range)xlWorksheet.Range["A3", "C100"];
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

                pengaduan.Tanggal = DateTime.FromOADate(Convert.ToInt64(rngPengaduan.Cells[row, "C"].Value2));
                pengaduan.Waktu = DateTime.FromOADate(Convert.ToDouble(rngPengaduan.Cells[row, "D"].Value2)).TimeOfDay;
                pengaduan.Rujukan = rngPengaduan.Cells[row, "E"].Value2;
                pengaduan.Penerima = rngPengaduan.Cells[row, "F"].Value2;
                pengaduan.Tempat = rngPengaduan.Cells[row, "G"].Value2;
                pengaduan.StatusPelapor = rngPengaduan.Cells[row, "H"].Value2;

                pengaduan.HubunganKorbanDenganTerlapor = rngPengaduan.Cells[row, "M"].Value2;

                pengaduan.Pelapor = new Pelapor();
                pengaduan.Pelapor.Nama = rngPengaduan.Cells[row, "I"].Value2;


               // pengaduan.Korban = new Korban();
              //  pengaduan.Korban.Nama = rngPengaduan.Cells[row, "K"].Value2;

             //   pengaduan.Terlapor = new Terlapor();
             //   pengaduan.Terlapor.Nama = rngPengaduan.Cells[row, "N"].Value2;


                pengaduan.Kondisi = new KondisiKorban();
                pengaduan.Kondisi.Fisik = ConvertEnum<KondisiFisik>(rngPengaduan.Cells[row, "P"].Value2);
                pengaduan.Kondisi.Psikis = ConvertEnum<KondisiPsikis>(rngPengaduan.Cells[row, "Q"].Value2);
                pengaduan.Kondisi.Sex = ConvertEnum<KondisiSex>(rngPengaduan.Cells[row, "R"].Value2);
                if (pengaduan.Kondisi.Sex != KondisiSex.Pendarahan)
                    pengaduan.Kondisi.SexText = rngPengaduan.Cells[row, "S"].Value2;

                //Kondisi Korban
                pengaduan.Dampak = new DampakKorban();
                pengaduan.Dampak.Fisik = rngPengaduan.Cells[row, "T"].Value2;
                pengaduan.Dampak.Psikis = rngPengaduan.Cells[row, "U"].Value2;
                pengaduan.Dampak.Seksual = rngPengaduan.Cells[row, "V"].Value2;
                pengaduan.Dampak.Ekonomi = rngPengaduan.Cells[row, "W"].Value2;
                pengaduan.Dampak.Kesehatan = rngPengaduan.Cells[row, "X"].Value2;
                pengaduan.Dampak.Lain = rngPengaduan.Cells[row, "Y"].Value2;

                pengaduan.Kejadian = new Kejadian();
                pengaduan.Kejadian.Waktu = DateTime.FromOADate(Convert.ToInt64(rngPengaduan.Cells[row, "Z"].Value2));
                pengaduan.Kejadian.Tempat = rngPengaduan.Cells[row, "AA"].Value2;
                pengaduan.Kejadian.Fisik = rngPengaduan.Cells[row, "AB"].Value2;
                pengaduan.Kejadian.Psikis = rngPengaduan.Cells[row, "AC"].Value2;
                pengaduan.Kejadian.Penelantaran = rngPengaduan.Cells[row, "AD"].Value2;
                pengaduan.Kejadian.Seksual = rngPengaduan.Cells[row, "AE"].Value2;
                pengaduan.Kejadian.Penganiayaan = rngPengaduan.Cells[row, "AF"].Value2;
                pengaduan.Kejadian.Pencabulan = rngPengaduan.Cells[row, "AG"].Value2;
                pengaduan.Kejadian.Pemerkosaan = rngPengaduan.Cells[row, "AH"].Value2;
                pengaduan.Kejadian.Trafiking = rngPengaduan.Cells[row, "AI"].Value2;
                pengaduan.Kejadian.Lain = rngPengaduan.Cells[row, "AJ"].Value2;
                pengaduan.Penanganan = rngPengaduan.Cells[row, "AK"].Value2;
                if (pengaduan.Penanganan == "Lain")
                    pengaduan.Penanganan = rngPengaduan.Cells[row, "AL"].Value2;
                row++;


                list.Add(pengaduan);
            }
            return Task.FromResult(list);

        }

        private Task<List<Korban>> ProccessKorban()
        {

            List<Korban> listKorban = new List<Korban>();
            Excel._Worksheet xlWorksheet = xlWorkbook.Sheets["Korban"];
            Excel.Range rngPengaduan = (Excel.Range)xlWorksheet.Range["A2", "M100"];
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
                data.Gender = ConvertEnum<Gender>(rngPengaduan.Cells[row, "D"].Value2);
                data.TempatLahir = rngPengaduan.Cells[row, "E"].Value2;
                data.TanggalLahir = DateTime.FromOADate(Convert.ToInt64(rngPengaduan.Cells[row, "F"].Value2));
                data.Alamat = rngPengaduan.Cells[row, "G"].Value2;
                data.NIK = rngPengaduan.Cells[row, "H"].Value2;
                data.Pekerjaan = rngPengaduan.Cells[row, "I"].Value2;
                data.Pendidikan = rngPengaduan.Cells[row, "J"].Value2;
                data.Agama = rngPengaduan.Cells[row, "K"].Value2;
                data.Suku = rngPengaduan.Cells[row, "L"].Value2;
                data.Pernikahan = rngPengaduan.Cells[row, "M"].Value2;

                row++;

                listKorban.Add(data);


            }

            return Task.FromResult(listKorban);

        }

        private Task<List<Terlapor>> ProccessTerlapor()
        {
            List<Terlapor> listTerlapor = new List<Terlapor>();
            Excel._Worksheet xlWorksheet = xlWorkbook.Sheets["Terlapor"];
            Excel.Range rngPengaduan = (Excel.Range)xlWorksheet.Range["A2", "L100"];
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
                data.Gender = ConvertEnum<Gender>(rngPengaduan.Cells[row, "D"].Value2);
                data.TempatLahir = rngPengaduan.Cells[row, "E"].Value2;
                data.TanggalLahir = DateTime.FromOADate(Convert.ToInt64(rngPengaduan.Cells[row, "F"].Value2));
                data.Alamat = rngPengaduan.Cells[row, "G"].Value2;
                data.NIK = rngPengaduan.Cells[row, "H"].Value2;
                data.Pekerjaan = rngPengaduan.Cells[row, "I"].Value2;
                data.Pendidikan = rngPengaduan.Cells[row, "J"].Value2;
                data.Agama = rngPengaduan.Cells[row, "K"].Value2;
                data.Suku = rngPengaduan.Cells[row, "L"].Value2;
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
                data.Tanggal = new DateTime(Convert.ToInt64(rngPengaduan.Cells[row, "B"].Value2));
                data.BentukPenanganan = rngPengaduan.Cells[row, "C"].Value2;
                data.Keterangan = rngPengaduan.Cells[row, "D"].Value2;
                row++;

                list.Add(data);


            }

            return Task.FromResult(list);

        }


        private Task<List<Tuple<string, string, string>>> ProccessUraian()
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
                var uraian = rngPengaduan.Cells[row, "B"].Value2 == null ? string.Empty : rngPengaduan.Cells[row, "B"].Value2;
                var catatan = rngPengaduan.Cells[row, "C"].Value2 == null ? string.Empty : rngPengaduan.Cells[row, "C"].Value2;
                row++;

                var data = Tuple.Create(NoReg, uraian, catatan);
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
}
