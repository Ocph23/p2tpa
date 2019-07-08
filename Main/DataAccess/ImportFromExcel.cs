using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using Main.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Collections.ObjectModel;
using System.Windows.Data;
using System.Windows.Media;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Text;
using System.Globalization;
using AutoMapper;
using Main.ViewModels;
using System.Windows;

namespace Main.DataAccess
{

    public delegate void onResult(List<Pengaduan> data);
    public class ImportFromExcel : BaseNotify,IDisposable
    {
        private string fileName;
        private Pengaduan _selected;
        private ExcelContext excel;

        public ImportFromExcel(string resultFile)
        {
            fileName = resultFile;
            Pengaduans = new ObservableCollection<Pengaduan>();
            PengaduanViews = (CollectionView)CollectionViewSource.GetDefaultView(Pengaduans);
            SaveCommand = new CommandHandler { CanExecuteAction = x => true, ExecuteAction = SaveToDatabaseAsync };
            EditCommand = new CommandHandler { CanExecuteAction = x => this.SelectedItem != null, ExecuteAction = EditCommandAction };
            ValidateCommand = new CommandHandler { CanExecuteAction = x => this.SelectedItem != null, ExecuteAction = ValidateAction };
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
                data.Icon.ToolTip = "Valid";
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
                form.DataContext = Mapper.Map<PengaduanViewModel>(data);
                form.ShowDialog();
            }
        }

        public async void StartAsync()
        {
            try
            {
                await Task.Delay(2000);
                excel = new ExcelContext(fileName);
                await Task.Delay(500);
                await ProccessInstasi();
                await ProccessPengaduan().ContinueWith(taskPengaduanCompleteAsync);
                await ProccessPelapor().ContinueWith(taskPelaporCompleteAsync);
                await ProccessKorban().ContinueWith(taskKorbanCompleteAsync);
                await ProccessTerlapor().ContinueWith(taskTerlaporCompleteAsync);
                PengaduanViews.Refresh();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                await Task.Delay(1000);
                WindowClose();
            }
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
            try
            {
                var btn = obj as Button;
                if (btn.Content.ToString() == "Validasi")
                {

                    foreach (var item in this.Pengaduans)
                    {
                        ValidateAction(item);
                    }
                    btn.Content = "Simpan";

                }
                else
                {

                    var onerror = (this.Pengaduans.Where(x => !string.IsNullOrEmpty(x.Error)).Count() > 0);
                   var result = System.Windows.MessageBox.Show("Simpan Data Valid ?", "?", System.Windows.MessageBoxButton.YesNo, System.Windows.MessageBoxImage.Question);
                    if(result== System.Windows.MessageBoxResult.Yes)
                    {
                        foreach (var item in this.Pengaduans)
                        {
                            SaveDataAsync(item).ContinueWith(completeSaveAsync);
                        }
                    }
                }
            }
            catch (Exception)
            {
               
            }



        }

        public ObservableCollection<Pengaduan> Pengaduans { get; set; }
        public CollectionView PengaduanViews { get; set; }
        public CommandHandler SaveCommand { get; }
        public CommandHandler EditCommand { get; }
        public CommandHandler ValidateCommand { get; }
        public bool Restart { get; internal set; }
        public Action WindowClose { get; internal set; }

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

                    this.Restart = true;

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
            try
            {
                await Task.Delay(500);
                var pengaduanFound = DataBasic.DataPengaduan.Where(O => O.KodeDistrik == item.KodeDistrik && O.Nomor == item.Nomor).FirstOrDefault();
                if (pengaduanFound != null)
                    throw new SystemException("Data Sudah Ada");

                ValidatePengaduan(item);
                DataBasic.MasterPengaduan.Add(item);

                return System.Tuple.Create("Berhasil", item, true);
            }
            catch (Exception ex)
            {
                return System.Tuple.Create(ex.Message, item, false);
            }

        }

        private void ValidatePengaduan(Pengaduan data)
        {
            try
            {
                StringBuilder sb = new StringBuilder();
                var pengaduanVM = Mapper.Map<PengaduanViewModel>(data);
                if (!string.IsNullOrEmpty(pengaduanVM.Error))
                    sb.AppendLine(pengaduanVM.Error);

                if (!string.IsNullOrEmpty(pengaduanVM.Pelapor.Error))
                    sb.AppendLine(pengaduanVM.Pelapor.Error);

                if (pengaduanVM.Korban != null && pengaduanVM.Korban.Count > 0)
                {
                    foreach (var korban in pengaduanVM.Korban)
                    {
                        if (!string.IsNullOrEmpty(korban.Error))
                            sb.AppendLine($"Data Korban '{korban.Nama}' - {korban.Error}");
                    }
                }
                else
                    sb.AppendLine("data korban belum ada");

                if (pengaduanVM.Terlapor != null && pengaduanVM.Terlapor.Count > 0)
                {
                    foreach (var terlapor in pengaduanVM.Terlapor)
                    {
                        if (!string.IsNullOrEmpty(data.Error))
                            sb.AppendLine($"Data Terlapor '{terlapor.Nama}' - {terlapor.Error}");
                    }
                }
                else
                    sb.AppendLine("data Terlapor belum ada");


                if (!string.IsNullOrEmpty(pengaduanVM.Dampak.Error))
                    sb.AppendLine(pengaduanVM.Dampak.Error);

                if (!string.IsNullOrEmpty(pengaduanVM.Kondisi.Error))
                    sb.AppendLine(pengaduanVM.Kondisi.Error);

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


        private bool progressVisible =true;

        public bool ProgressVisible
        {
            get { return progressVisible; }
            set { SetProperty(ref progressVisible ,value); }
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
                    foreach (var data in datas.Where(x => x.NoReq == item.Nomor))
                    {
                        item.Terlapor.Add(data);
                    }
                }

                ProgressVisible = false;
            });
        }

        private async void taskPelaporCompleteAsync(Task<List<Pelapor>> obj)
        {
            var datas = await obj;
                App.Current.Dispatcher.Invoke((System.Action)async delegate
            {
                if (Pengaduans.Count <= 0)
                    await Task.Delay(10000);
                foreach (var item in Pengaduans)
                {
                    foreach (var data in datas.Where(x=>x.NoReq==item.Nomor))
                    {
                        item.Pelapor.Nama = data.Nama;
                        item.Pelapor.Alamat = data.Alamat;
                        item.Pelapor.Gender = data.Gender;
                    }
                }
            });
        }


        private Task ProccessInstasi()
        {
            try
            {
                var rngPengaduan = excel.GetRange("Instansi", "A3:D100");

                for (var row = 3; row <= rngPengaduan.Count; row++)
                {
                    Instansi instansi = new Instansi();
                    var nama = rngPengaduan.Cell(row, "A");
                    if (string.IsNullOrEmpty(nama))
                        break;
                    instansi.Name = nama;

                    KategoriInstansi kategori;
                    var success = Enum.TryParse<KategoriInstansi>(rngPengaduan.Cell(row, "B"), out kategori);
                    if (!success)
                        break;
                    instansi.Kategori = kategori;


                    TingakatInstansi tingkat;
                    success = Enum.TryParse<TingakatInstansi>(rngPengaduan.Cell(row, "C"), out tingkat);
                    if (!success)
                        break;

                    instansi.Tingkat = tingkat;
                    instansi.Alamat = rngPengaduan.Cell(row, "D");

                    var isFound = DataAccess.DataBasic.DataInstansi.Where(x => x.Name.ToLower() == nama.ToLower()).FirstOrDefault();
                    if (isFound == null)
                        DataAccess.DataBasic.DataInstansi.Add(instansi);
                }

                return Task.FromResult(0);
            }
            catch (Exception)
            {
                throw new SystemException("Format Template Salah, Hubungi Admin");
            }
        }


        private Task<List<Pelapor>> ProccessPelapor()
        {
            var rngPengaduan = excel.GetRange("Pelapor", "A1:D500");
            List<Pelapor> listPelapor = new List<Pelapor>();
            for (var row = 3; row <= rngPengaduan.Count; row++)
            {
                Pelapor pelaport = new Pelapor();
                var nomor = rngPengaduan.Cell(row, "A");
                if (string.IsNullOrEmpty(nomor))
                    break;
                pelaport.NoReq = nomor;
                pelaport.Nama = rngPengaduan.Cell(row, "B");

                Gender data;
                var success = Enum.TryParse<Gender>(rngPengaduan.Cell(row, "C"), out data);
                if (!success)
                    break;
                pelaport.Gender = data;
                pelaport.Alamat = rngPengaduan.Cell(row, "D");

                listPelapor.Add(pelaport);
            }

            return Task.FromResult(listPelapor);
        }

        private Task<List<Pengaduan>> ProccessPengaduan()
        {

            var rngPengaduan = excel.GetRange("Pengaduan", "A1:AG500");

            var kodeDistrik = rngPengaduan.Cell(2, "C");
            if (string.IsNullOrEmpty(kodeDistrik))
                throw new SystemException("Kode Distrik Tidak Ditemukan");

            List<Pengaduan> list = new List<Pengaduan>();

            for (var row = 7; row <= rngPengaduan.Count; row++)
            {
                var pengaduan = new Pengaduan();
                pengaduan.KodeDistrik = kodeDistrik;
                pengaduan.Nomor = rngPengaduan.Cell(row, "B");
                if (string.IsNullOrEmpty(pengaduan.Nomor))
                    break;

                pengaduan.TanggalLapor = DateTime.FromOADate(Double.Parse(rngPengaduan.Cell(row, "C"), NumberStyles.Any, CultureInfo.InvariantCulture));
                pengaduan.WaktuLapor = DateTime.FromOADate(Double.Parse(rngPengaduan.Cell(row, "D"), NumberStyles.Any, CultureInfo.InvariantCulture));
                pengaduan.Rujukan = rngPengaduan.Cell(row, "E");
                pengaduan.Penerima = rngPengaduan.Cell(row, "F");
                pengaduan.TempatLapor = rngPengaduan.Cell(row, "G");
                pengaduan.StatusPelapor = rngPengaduan.Cell(row, "H");

                //  pengaduan.HubunganKorbanDenganTerlapor = rngPengaduan.Cell(row, "M");

                pengaduan.Pelapor = new Pelapor();
                pengaduan.Pelapor.Nama = rngPengaduan.Cell(row, "I");

                pengaduan.TanggalKejadian = DateTime.FromOADate(Double.Parse(rngPengaduan.Cell(row, "U"), NumberStyles.Any, CultureInfo.InvariantCulture));
                pengaduan.WaktuKejadian = new DateTime(pengaduan.TanggalKejadian.Value.Ticks);
                pengaduan.TempatKejadian = rngPengaduan.Cell(row, "V");
                pengaduan.UraianKejadian= rngPengaduan.Cell(row, "W");
                pengaduan.Catatan = rngPengaduan.Cell(row, "X");


                // pengaduan.Korban = new Korban();
                //  pengaduan.Korban.Nama = rngPengaduan.Cell(row, "K");

                //   pengaduan.Terlapor = new Terlapor();
                //   pengaduan.Terlapor.Nama = rngPengaduan.Cell(row, "N");


                pengaduan.Kondisi = new KondisiKorban();
                pengaduan.Kondisi.Fisik = ConvertEnum<KondisiFisik>(rngPengaduan.Cell(row, "K"));
                pengaduan.Kondisi.Psikis = ConvertEnum<KondisiPsikis>(rngPengaduan.Cell(row, "L"));
                pengaduan.Kondisi.Sex = ConvertEnum<KondisiSex>(rngPengaduan.Cell(row, "M"));
                if (pengaduan.Kondisi.Sex != KondisiSex.Pendarahan)
                    pengaduan.Kondisi.SexText = rngPengaduan.Cell(row, "N");

                //Kondisi Korban
                pengaduan.Dampak = new DampakKorban();
                pengaduan.Dampak.Fisik = rngPengaduan.Cell(row, "O");
                pengaduan.Dampak.Psikis = rngPengaduan.Cell(row, "P");
                pengaduan.Dampak.Seksual = rngPengaduan.Cell(row, "Q");
                pengaduan.Dampak.Ekonomi = rngPengaduan.Cell(row, "R");
                pengaduan.Dampak.Kesehatan = rngPengaduan.Cell(row, "S");
                pengaduan.Dampak.Lain = rngPengaduan.Cell(row, "T");



                list.Add(pengaduan);
            }
            return Task.FromResult(list);
        }

        private Task<List<Korban>> ProccessKorban()
        {
            var rngPengaduan = excel.GetRange("Korban", "A1:M500");
          
            List<Korban> listKorban = new List<Korban>();
            for (var row = 4; row <= rngPengaduan.Count; row++)
            {
                Korban data = new Korban();
                var nomor = rngPengaduan.Cell(row, "A");
                if (string.IsNullOrEmpty(nomor))
                    break;
                data.NoReq = nomor;
                data.Nama = rngPengaduan.Cell(row, "B");
                data.NamaPanggilan = rngPengaduan.Cell(row, "C");
                Gender gender;
                var success = Enum.TryParse<Gender>(rngPengaduan.Cell(row, "D"), out gender);
                if (!success)
                    break;
                data.Gender = ConvertEnum<Gender>(rngPengaduan.Cell(row, "D"));
                data.TempatLahir = rngPengaduan.Cell(row, "E");
                data.TanggalLahir = DateTime.FromOADate(Double.Parse(rngPengaduan.Cell(row, "F"), NumberStyles.Any, CultureInfo.InvariantCulture));
                data.Alamat = rngPengaduan.Cell(row, "G");
                data.NIK = rngPengaduan.Cell(row, "H");
                data.Pekerjaan = rngPengaduan.Cell(row, "I");
                data.Pendidikan = rngPengaduan.Cell(row, "J");
                data.Agama = rngPengaduan.Cell(row, "K");
                data.Suku = rngPengaduan.Cell(row, "L");
                data.Pernikahan = rngPengaduan.Cell(row, "M");
                StringBuilder sb = new StringBuilder();
              
                if (rngPengaduan.Cell(row, "N").ToLower() == "1")
                    sb.Append($"Fisik#");

                if (rngPengaduan.Cell(row, "O").ToLower() == "1")
                    sb.Append($"Psikis#");

                if (rngPengaduan.Cell(row, "P").ToLower() == "1")
                    sb.Append($"Seksual#");

                if (rngPengaduan.Cell(row, "Q").ToLower() == "1")
                    sb.Append($"Eksploitasi#");

                if (rngPengaduan.Cell(row, "R").ToLower() == "1")
                    sb.Append($"Trafficking#");

                if (rngPengaduan.Cell(row, "S").ToLower() == "1")
                    sb.Append($"Penelantaran#");

                if (rngPengaduan.Cell(row, "T").ToLower() == "1")
                    sb.Append($"Lainnya#");

                data.KekerasanDialami = sb.ToString().Substring(0,sb.Length-1);
                listKorban.Add(data);
            }



            var pengangans = excel.GetRange("Penanganan", "A4:G500");

            for (var row = 4; row <= pengangans.Count; row++)
            {
                Penanganan data = new  Penanganan();
                var nama = pengangans.Cell(row, "B");
                if (string.IsNullOrEmpty(nama))
                    break;
                data.Nama = nama;
                data.Tanggal = DateTime.FromOADate(Double.Parse(pengangans.Cell(row, "C"), NumberStyles.Any, CultureInfo.InvariantCulture));

                string instansiName = pengangans.Cell(row, "D");
                data.Instansi = new Instansi { Name = instansiName };
                data.Layanan = pengangans.Cell(row, "E");
                data.DetailLayanan= pengangans.Cell(row, "F");
                data.Deskripsi= pengangans.Cell(row, "G");
                var dataKorban = listKorban.Where(x => x.Nama == nama).FirstOrDefault();
                if(dataKorban!=null)
                {

                    var instansi = DataAccess.DataBasic.DataInstansi.Where(x => x.Name.ToLower() == instansiName.ToLower()).FirstOrDefault();
                    if(instansi!=null)
                    {
                        data.DataIdentias = dataKorban;
                        data.InstansiId = instansi.Id;
                        data.Instansi = instansi;
                        data.IdentitasType = "Korban";
                    }
                    dataKorban.DataPenanganan.Add(data);
                }
            }
            return Task.FromResult(listKorban);
        }

        private Task<List<Terlapor>> ProccessTerlapor()
        {
            var rngPengaduan = excel.GetRange("Terlapor", "A1:T500");
            List<Terlapor> listTerlapor = new List<Terlapor>();
            for (var row = 4; row <= rngPengaduan.Count; row++)
            {
                Terlapor data = new Terlapor();
                var nomor = rngPengaduan.Cell(row, "A");
                if (string.IsNullOrEmpty(nomor))
                    break;
                data.NoReq = nomor;
                data.Nama = rngPengaduan.Cell(row, "B");
                data.NamaPanggilan = rngPengaduan.Cell(row, "C");
                Gender gender;
                var success = Enum.TryParse<Gender>(rngPengaduan.Cell(row, "D"), out gender);
                data.Gender = ConvertEnum<Gender>(rngPengaduan.Cell(row, "D"));
                data.TempatLahir = rngPengaduan.Cell(row, "E");
                data.TanggalLahir = DateTime.FromOADate(Double.Parse(rngPengaduan.Cell(row, "F"), NumberStyles.Any, CultureInfo.InvariantCulture));
                data.Alamat = rngPengaduan.Cell(row, "G");
                data.NIK = rngPengaduan.Cell(row, "H");
                data.Pekerjaan = rngPengaduan.Cell(row, "I");
                data.Pendidikan = rngPengaduan.Cell(row, "J");
                data.Agama = rngPengaduan.Cell(row, "K");
                data.Suku = rngPengaduan.Cell(row, "L");


                var hub1Name = rngPengaduan.Cell(row, "M");
                var hub1 = rngPengaduan.Cell(row, "N");
                if (!string.IsNullOrEmpty(hub1) && !string.IsNullOrEmpty(hub1Name))
                {
                    data.Hubungan.Add(new HubunganDenganKorban(0, new Korban() { Nama = hub1Name }) {  JenisHubungan=hub1});
                }

                var hub2Name = rngPengaduan.Cell(row, "O");
                var hub2 = rngPengaduan.Cell(row, "P");
                if (!string.IsNullOrEmpty(hub2) && !string.IsNullOrEmpty(hub2Name))
                {
                    data.Hubungan.Add(new HubunganDenganKorban(0, new Korban() { Nama = hub2Name }) { JenisHubungan = hub2 });
                }


                var hub3Name = rngPengaduan.Cell(row, "Q");
                var hub3 = rngPengaduan.Cell(row, "R");
                if (!string.IsNullOrEmpty(hub3) && !string.IsNullOrEmpty(hub3Name))
                {
                    data.Hubungan.Add(new HubunganDenganKorban(0, new Korban() { Nama = hub3Name }) { JenisHubungan = hub3 });
                }

                var hub4Name = rngPengaduan.Cell(row, "S");
                var hub4 = rngPengaduan.Cell(row, "T");
                if (!string.IsNullOrEmpty(hub4) && !string.IsNullOrEmpty(hub4Name))
                {
                    data.Hubungan.Add(new HubunganDenganKorban(0, new Korban() { Nama = hub4Name }) { JenisHubungan = hub4 });
                }
                listTerlapor.Add(data);
            }




            var pengangans = excel.GetRange("Penanganan", "H4:M500");

            for (var row = 4; row <= pengangans.Count; row++)
            {
                Penanganan data = new Penanganan();
                var nama = pengangans.Cell(row, "I");
                if (string.IsNullOrEmpty(nama))
                    break;
                data.Nama = nama;
                data.Tanggal = DateTime.FromOADate(Double.Parse(pengangans.Cell(row, "J"), NumberStyles.Any, CultureInfo.InvariantCulture));

                string instansiName = pengangans.Cell(row, "K");
                data.Instansi = new Instansi { Name = instansiName };
                data.Layanan = pengangans.Cell(row, "L");
                data.Deskripsi = pengangans.Cell(row, "M");
                var dataTerlapor = listTerlapor.Where(x => x.Nama == nama).FirstOrDefault();
                if (dataTerlapor != null)
                {
                    var instansi = DataAccess.DataBasic.DataInstansi.Where(x => x.Name.ToLower() == instansiName.ToLower()).FirstOrDefault();
                    if (instansi != null)
                    {
                        data.DataIdentias = dataTerlapor;
                        data.InstansiId = instansi.Id;
                        data.Instansi = instansi;
                        data.IdentitasType = "Terlapor";
                    }
                    dataTerlapor.DataPenanganan.Add(data);
                }
            }

            return Task.FromResult(listTerlapor);
        }

        private T ConvertEnum<T>(string value2)
        {
            if (string.IsNullOrEmpty(value2))
                return Activator.CreateInstance<T>();
            return (T)Enum.Parse(typeof(T), value2);
        }

        public void Dispose()
        {
            excel.Dispose();
        }
    }


}
