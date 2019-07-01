using Main.Models;
using Main.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using Excel = Microsoft.Office.Interop.Excel;

namespace Main.DataAccess
{

    public delegate void onResult(List<Pengaduan> data);
    public class ImportFromExcel : BaseNotify, IDisposable
    {

        Excel.Application xlApp = new Excel.Application();
        Excel.Workbook xlWorkbook = null;
        private Pengaduan _selected;

      //  public event onResult DataReseult;
        public ImportFromExcel()
        {
            Excel.Application xlApp = new Excel.Application();
            var path = Environment.CurrentDirectory + "\\ImportPengaduan.xlsx";
            xlWorkbook = xlApp.Workbooks.Open(path);
            Pengaduans = new ObservableCollection<Pengaduan>();
            PengaduanViews = (CollectionView)CollectionViewSource.GetDefaultView(Pengaduans);
            SaveCommand = new CommandHandler { CanExecuteAction = x => true, ExecuteAction = SaveToDatabaseAsync };
            EditCommand = new CommandHandler { CanExecuteAction = x => this.SelectedItem!=null, ExecuteAction = EditCommandAction };
            ValidateCommand = new CommandHandler { CanExecuteAction = x => this.SelectedItem!=null, ExecuteAction = ValidateAction };
            this.StartAsync();
        }

        private void ValidateAction(object obj)
        {
            var data = obj as Pengaduan;
           
            try
            {
                using (var db = new DbContext())
                {

                    var pengaduanFound = db.DataPengaduan.Where(O => O.KodeDistrik == data.KodeDistrik && O.Nomor == data.Nomor).FirstOrDefault();
                    if (pengaduanFound != null)
                        throw new SystemException("Data Sudah Ada");

                }
                ValidatePengaduan(data);
                data.Icon.ToolTip ="Valid";
                data.Icon.Kind = MaterialDesignThemes.Wpf.PackIconKind.Read;
                data.Icon.Foreground = Brushes.LightSeaGreen;
            }
            catch (Exception ex)
            {
                data.Icon.ToolTip = ex.Message;
                if (ex.Message == "Data Sudah Ada")
                {
                    data.Icon.Kind = MaterialDesignThemes.Wpf.PackIconKind.ContentDuplicate;
                   data.Icon.Foreground = Brushes.Orange;
                }
                else
                {
                   data.Icon.Kind = MaterialDesignThemes.Wpf.PackIconKind.Error;
                    data.Icon.Foreground = Brushes.Red;
                }

            }
        }

        public Pengaduan SelectedItem
        {
            get { return _selected; }
            set
            {
                SetProperty(ref _selected, value);
            }
        }
        private void EditCommandAction(object obj)
        {
            var data = obj as Pengaduan;
            if (data != null)
            {
                var form = new Views.TambahPengaduan(true);
                form.DataContext = data;
                form.ShowDialog();
            }
        }

        public async void StartAsync()
        {
            await Task.Delay(500);
            await ProccessPengaduan().ContinueWith(taskPengaduanCompleteAsync);
            await ProccessKorban().ContinueWith(taskKorbanCompleteAsync);
            await ProccessPelapor().ContinueWith(taskPelaporCompleteAsync);
            await ProccessTerlapor().ContinueWith(taskTerlaporCompleteAsync);

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
            var btn = obj as Button;
            foreach (var item in this.Pengaduans)
            {
                if (btn.Content.ToString() == "Validasi")
                {
                    ValidateAction(item);
                    btn.Content = "Simpan";
                }
                else
                    SaveDataAsync(item).ContinueWith(completeSaveAsync);
            }
        }

        public ObservableCollection<Pengaduan> Pengaduans { get; set; }
        public CollectionView PengaduanViews { get; set; }
        public CommandHandler SaveCommand { get; }
        public CommandHandler EditCommand { get; }
        public CommandHandler ValidateCommand { get; }

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

                    ValidatePengaduan(item);
                    item.Id = db.DataPengaduan.InsertAndGetLastID(item);
                    if (item.Id <= 0)
                        throw new SystemException("Data Pengaduan Tidak Tersimpan");

                    item.Pelapor.PengaduanId = item.Id.Value;
                    item.Pelapor.Id = await GetIdIdentitas(item.Pelapor, db);
                    if (item.Pelapor.Id <= 0)
                        throw new SystemException("Data Pelapor Tidak Ditemukan/Tidak Lengkap");


                    foreach(var data in item.Korban)
                    {
                        data.PengaduanId = item.Id.Value;
                        data.Id = await GetIdIdentitas(data, db);
                    }

                    foreach (var data in item.Terlapor)
                    {
                        data.PengaduanId = item.Id.Value;
                        data.Id = await GetIdIdentitas(data, db);
                    }

                    item.Dampak.PengaduanId = item.Id.Value;
                    item.Kondisi.PengaduanId = item.Id.Value;

                    item.Kondisi.Id = db.DataKondisiKorban.InsertAndGetLastID(item.Kondisi);
                    if (item.Kondisi.Id <= 0)
                        throw new SystemException("Data Kondisi Korban Tidak Tersimpan");

                    item.Dampak.Id = db.DataDampak.InsertAndGetLastID(item.Dampak);
                    if (item.Dampak.Id <= 0)
                        throw new SystemException("Data  Dampak Yang Dialami Tidak Tersimpan");

                  

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

        private void ValidatePengaduan(Pengaduan item)
        {
            try
            {
                StringBuilder sb = new StringBuilder();
                if (!string.IsNullOrEmpty(item.Error))
                    sb.AppendLine(item.Error);

                if (!string.IsNullOrEmpty(item.Pelapor.Error))
                    sb.AppendLine(item.Pelapor.Error);

                if (item.Korban != null && item.Korban.Count > 0)
                {
                    foreach (var data in item.Korban)
                    {
                        if (!string.IsNullOrEmpty(data.Error))
                            sb.AppendLine($"Data Korban '{data.Nama}' - {data.Error}");
                    }
                }
                else
                    sb.AppendLine("data korban belum ada");

                if (item.Terlapor != null && item.Terlapor.Count > 0)
                {
                    foreach (var data in item.Terlapor)
                    {
                        if (!string.IsNullOrEmpty(data.Error))
                            sb.AppendLine($"Data Terlapor '{data.Nama}' - {data.Error}");
                    }
                }
                else
                    sb.AppendLine("data Terlapor belum ada");

             
                if (!string.IsNullOrEmpty(item.Dampak.Error))
                    sb.AppendLine(item.Dampak.Error);

                if (!string.IsNullOrEmpty(item.Kondisi.Error))
                    sb.AppendLine(item.Kondisi.Error);

                if (!string.IsNullOrEmpty(sb.ToString()))
                    throw new SystemException(sb.ToString());

            }
            catch (Exception ex)
            {
                throw new SystemException(ex.Message);
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


        private async void taskKorbanCompleteAsync(Task<List<Korban>> obj)
        {
            var datas = await obj;
            App.Current.Dispatcher.Invoke((System.Action)async delegate
            {
                if (Pengaduans.Count <= 0)
                    await Task.Delay(10000);
                foreach (var item in Pengaduans)
                {
                    foreach (var data in datas.Where(x => x.NoReq == item.Nomor))
                    {
                        item.Korban.Add(data);
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
                foreach (var item in Pengaduans)
                {
                    foreach(var data in datas.Where(x => x.NoReq == item.Nomor))
                    {
                        item.Terlapor.Add(data);
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
            Excel.Range rngPengaduan = (Excel.Range)xlWorksheet.Range["A3", "D500"];
            int row = 1;
            for (var i = 1; i <= rngPengaduan.Rows.Count; i++)
            {
                Pelapor pelaport = new Pelapor();
                var nomor = rngPengaduan.Cells[row, "A"].Value2;
                if (string.IsNullOrEmpty(nomor))
                    break;
                pelaport.NoReq = nomor;
                pelaport.Nama = rngPengaduan.Cells[row, "B"].Value2;

                Gender data;
                var success = Enum.TryParse<Gender>(rngPengaduan.Cells[row, "C"].Value2, out data);
                if (!success)
                    break;
                pelaport.Gender = data;
                pelaport.Alamat = rngPengaduan.Cells[row, "D"].Value2;
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
            Excel.Range rngPengaduan = (Excel.Range)xlWorksheet.Range["A7", "AG500"];
            if (string.IsNullOrEmpty(kodeDistrik))
                throw new SystemException("Kode Distrik Tidak Ditemukan");

            List<Pengaduan> list = new List<Pengaduan>();

            for (var i = 1; i <= rngPengaduan.Rows.Count; i++)
            {
                var pengaduan = new Pengaduan();
                pengaduan.KodeDistrik = kodeDistrik;
                pengaduan.Nomor = rngPengaduan.Cells[row, "B"].Value2;
                if (string.IsNullOrEmpty(pengaduan.Nomor))
                    break;

                pengaduan.TanggalLapor = DateTime.FromOADate(Convert.ToInt64(rngPengaduan.Cells[row, "C"].Value2));
                TimeSpan waktu = DateTime.FromOADate(Convert.ToDouble(rngPengaduan.Cells[row, "D"].Value2)).TimeOfDay;
                pengaduan.WaktuLapor = new DateTime(pengaduan.TanggalLapor.Value.Year, pengaduan.TanggalLapor.Value.Month,
                    pengaduan.TanggalLapor.Value.Day, waktu.Hours, waktu.Minutes,0);
                pengaduan.Rujukan = rngPengaduan.Cells[row, "E"].Value2;
                pengaduan.Penerima = rngPengaduan.Cells[row, "F"].Value2;
                pengaduan.TempatLapor = rngPengaduan.Cells[row, "G"].Value2;
                pengaduan.StatusPelapor = rngPengaduan.Cells[row, "H"].Value2;

              //  pengaduan.HubunganKorbanDenganTerlapor = rngPengaduan.Cells[row, "M"].Value2;

                pengaduan.Pelapor = new Pelapor();
                pengaduan.Pelapor.Nama = rngPengaduan.Cells[row, "I"].Value2;


               // pengaduan.Korban = new Korban();
              //  pengaduan.Korban.Nama = rngPengaduan.Cells[row, "K"].Value2;

             //   pengaduan.Terlapor = new Terlapor();
             //   pengaduan.Terlapor.Nama = rngPengaduan.Cells[row, "N"].Value2;


                pengaduan.Kondisi = new KondisiKorban();
                pengaduan.Kondisi.Fisik = ConvertEnum<KondisiFisik>(rngPengaduan.Cells[row, "K"].Value2);
                pengaduan.Kondisi.Psikis = ConvertEnum<KondisiPsikis>(rngPengaduan.Cells[row, "L"].Value2);
                pengaduan.Kondisi.Sex = ConvertEnum<KondisiSex>(rngPengaduan.Cells[row, "M"].Value2);
                if (pengaduan.Kondisi.Sex != KondisiSex.Pendarahan)
                    pengaduan.Kondisi.SexText = rngPengaduan.Cells[row, "N"].Value2;

                //Kondisi Korban
                pengaduan.Dampak = new DampakKorban();
                pengaduan.Dampak.Fisik = rngPengaduan.Cells[row, "O"].Value2;
                pengaduan.Dampak.Psikis = rngPengaduan.Cells[row, "P"].Value2;
                pengaduan.Dampak.Seksual = rngPengaduan.Cells[row, "Q"].Value2;
                pengaduan.Dampak.Ekonomi = rngPengaduan.Cells[row, "R"].Value2;
                pengaduan.Dampak.Kesehatan = rngPengaduan.Cells[row, "S"].Value2;
                pengaduan.Dampak.Lain = rngPengaduan.Cells[row, "T"].Value2;

               

                list.Add(pengaduan);
            }
            return Task.FromResult(list);

        }

        private Task<List<Korban>> ProccessKorban()
        {

            List<Korban> listKorban = new List<Korban>();
            Excel._Worksheet xlWorksheet = xlWorkbook.Sheets["Korban"];
            Excel.Range rngPengaduan = (Excel.Range)xlWorksheet.Range["A3", "M500"];
            int row = 1;
            for (var i = 1; i <= rngPengaduan.Rows.Count; i++)
            {
                Korban data = new Korban();
                var nomor = rngPengaduan.Cells[row, "A"].Value2;
                if (string.IsNullOrEmpty(nomor))
                    break;
                data.NoReq = nomor;
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
            
            Excel.Range rngPengaduan = (Excel.Range)xlWorksheet.Range["A3", "L500"];
            int row = 1;
            for (var i = 1; i <= rngPengaduan.Rows.Count; i++)
            {
                Terlapor data = new Terlapor();

                var nomor= rngPengaduan.Cells[row, "A"].Value2;
                if (string.IsNullOrEmpty(nomor))
                    break;
                data.NoReq = nomor;
                data.Nama = rngPengaduan.Cells[row, "B"].Value2;
                data.NamaPanggilan = rngPengaduan.Cells[row, "C"].Value2;

                Gender gender;
                var success = Enum.TryParse<Gender>(rngPengaduan.Cells[row, "D"].Value2, out gender);
               
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


        private Task<List<Tuple<string, string, string>>> ProccessUraian()
        {
            List<Tuple<string, string, string>> list = new List<Tuple<string, string, string>>();
            Excel._Worksheet xlWorksheet = xlWorkbook.Sheets["Uraian&Catatan"];
            Excel.Range rngPengaduan = (Excel.Range)xlWorksheet.Range["A3", "C500"];
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
