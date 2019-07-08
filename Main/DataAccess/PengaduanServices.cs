using Main.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Main.DataAccess
{
    public class PengaduanServices : ICollection<Pengaduan>
    {
        ArrayList list = new ArrayList();
        public PengaduanServices()
        {
            using (var db = new DbContext())
            {
                try
                {
                    var aaa = db.DataPengaduan.Select();

                    var data = from pengaduan in db.DataPengaduan.Select()
                               join dis in  DataBasic.GetKecamatan() on pengaduan.KodeDistrik equals dis.Id
                               join pelapor in db.DataPelapor.Select().DefaultIfEmpty() on pengaduan.Id equals pelapor.PengaduanId
                               join dampak in db.DataDampak.Select().DefaultIfEmpty() on pengaduan.Id equals dampak.PengaduanId
                               join kondisi in db.DataKondisiKorban.Select().DefaultIfEmpty() on pengaduan.Id equals kondisi.PengaduanId
                               select new Pengaduan()
                               {         
                                   StatusPelapor = pengaduan.StatusPelapor,
                                   StatusPelaporText = pengaduan.StatusPelaporText,
                                   TanggalKejadian = pengaduan.TanggalKejadian,
                                   TempatKejadian = pengaduan.TempatKejadian,
                                   WaktuKejadian = pengaduan.WaktuKejadian,
                                   KodeDistrik = pengaduan.KodeDistrik, 
                                   Distrik = dis,
                                   Catatan = pengaduan.Catatan,
                                   Id = pengaduan.Id,
                                   Nomor = pengaduan.Nomor, 
                                   Penerima = pengaduan.Penerima,
                                   Rujukan = pengaduan.Rujukan,
                                   TanggalLapor = pengaduan.TanggalLapor,
                                   WaktuLapor = pengaduan.WaktuLapor,
                                   UraianKejadian = pengaduan.UraianKejadian,
                                   TempatLapor = pengaduan.TempatLapor,
                                   Dampak = dampak,
                                   Kondisi = kondisi,
                                   Pelapor = pelapor,
                               };

                    if (data.Count() >0)
                    {
                        foreach (var item in data)
                        {
                            var id = item.Id.Value;
                            item.Korban = db.DataKorban.Where(x => x.PengaduanId == id).ToList();
                            foreach (var korban in item.Korban)
                            {
                                var penangananKorban = from a in db.Penanganan.Where(x => x.IdentiasId == korban.Id && x.IdentitasType == "Korban")
                                                       join i in db.Instansi.Select().DefaultIfEmpty() on a.InstansiId equals i.Id
                                                       select new Penanganan
                                                       {

                                                           DataIdentias = korban,
                                                           Deskripsi = a.Deskripsi,
                                                           DetailLayanan = a.DetailLayanan,
                                                           IdPenanganan = a.IdPenanganan,
                                                           IdentiasId = a.IdentiasId,
                                                           IdentitasType = a.IdentitasType,
                                                           InstansiId = a.InstansiId,
                                                           Instansi = i, 
                                                           Layanan = a.Layanan,
                                                           Tanggal = a.Tanggal
                                                       };

                                korban.DataPenanganan = penangananKorban.ToList();
                            }
                            item.Terlapor = db.DataTerlapor.Where(x => x.PengaduanId == id).ToList();


                            foreach (var terlapor in item.Terlapor)
                            {
                                var penangananKorban = from a in db.Penanganan.Where(x => x.IdentiasId == terlapor.Id &&
                               x.IdentitasType == "Terlapor")
                                                       join i in db.Instansi.Select().DefaultIfEmpty() on a.InstansiId equals i.Id
                                                       select new Penanganan
                                                       {
                                                           DataIdentias = terlapor,
                                                           Deskripsi = a.Deskripsi,
                                                           DetailLayanan = a.DetailLayanan,
                                                           IdPenanganan = a.IdPenanganan,
                                                           IdentiasId = a.IdentiasId,
                                                           IdentitasType = a.IdentitasType,
                                                           InstansiId = a.InstansiId,
                                                           Instansi = i,
                                                           Layanan = a.Layanan,
                                                           Tanggal = a.Tanggal
                                                       };

                                terlapor.DataPenanganan = penangananKorban.ToList();


                                var hubs = from a in db.DataHubungan.Where(x => x.TerlaporId == terlapor.Id)
                                           join b in item.Korban on a.KorbanId equals b.Id
                                           select new HubunganDenganKorban(terlapor.Id, b)
                                           { Id = a.Id, Korban = b, KorbanId = a.Id, JenisHubungan = a.JenisHubungan, TerlaporId = a.TerlaporId };
                                terlapor.Hubungan = hubs.ToList();



                            }
                            list.Add(item);
                        }
                    }
                        
                }
                catch (Exception ex)
                {

                    Console.WriteLine(ex.Message);
                }
            }
        }

      
        public int Count => list.Count;

        public bool IsReadOnly => false;

        public void Add(Pengaduan item)
        {
            using (var db = new DbContext())
            {
                var trans = db.BeginTransaction();
                try
                {
                    if(item.Id==null)
                    {
                        item.Id = db.DataPengaduan.InsertAndGetLastID(item);
                        if (item.Id <= 0)
                        {
                            item.Id = null;
                            throw new SystemException("Data Pengaduan Tidak Tersimpan");
                        }
                            
                    }
                    else
                    {
                        if(!db.DataPengaduan.Update(x=> new { x.Catatan,x.KodeDistrik,x.Nomor,x.Penerima,x.Rujukan,x.StatusPelapor,
                        x.TanggalKejadian,x.TanggalLapor,x.TempatKejadian,x.TempatLapor,x.UraianKejadian,x.WaktuKejadian,x.WaktuLapor},item,x=>x.Id==item.Id))
                            throw new SystemException("Data Pengaduan Tidak Tersimpan");
                    }
                    

                     if(item.Korban!=null)
                        foreach(var data in item.Korban)
                        {
                            if (data.Id==null)
                            {
                                data.PengaduanId = item.Id.Value;
                                data.Id = db.DataKorban.InsertAndGetLastID(data);
                            }
                            else
                            {
                                db.DataKorban.Update(x => new
                                {
                                    x.Agama,
                                    x.Alamat,
                                    x.Gender,
                                    x.Nama,
                                    x.NamaPanggilan,
                                    x.NIK,
                                    x.Pekerjaan,
                                    x.Pendidikan,
                                    x.Pernikahan,
                                    x.Suku,
                                    x.TanggalLahir,
                                    x.TempatLahir
                                }, data, x => x.Id == data.Id);
                            }

                             foreach(var penanganan in data.DataPenanganan)
                            {

                                penanganan.IdentiasId = data.Id;

                                if(penanganan.IdPenanganan==null)
                                {
                                    penanganan.IdPenanganan=db.Penanganan.InsertAndGetLastID(penanganan);
                                }
                                else
                                {
                                    db.Penanganan.Update(x=> new {x.Deskripsi,x.DetailLayanan,x.InstansiId,x.Layanan,x.Tanggal},penanganan,x=>x.IdPenanganan==penanganan.IdPenanganan);
                                }
                            }


                        }


                    if (item.Terlapor != null)
                        foreach (var data in item.Terlapor)
                        {
                            if (data.Id ==null)
                            {
                                data.PengaduanId = item.Id.Value;
                                data.Id = db.DataTerlapor.InsertAndGetLastID(data);
                            }
                            else
                            {
                                db.DataTerlapor.Update(x => new
                                {
                                    x.Agama,
                                    x.Alamat,
                                    x.Gender,
                                    x.Nama,
                                    x.NamaPanggilan,
                                    x.NIK,
                                    x.Pekerjaan,
                                    x.Pendidikan,
                                    x.Pernikahan,
                                    x.Suku,
                                    x.TanggalLahir,
                                    x.TempatLahir
                                }, data, x => x.Id == data.Id);
                            }

                            foreach( var hub in data.Hubungan)
                            {
                                if(hub.Id==null)
                                {
                                    hub.TerlaporId = data.Id;
                                    if(hub.KorbanId==null)
                                    {
                                        var korban = item.Korban.Where(x => x.Nama == hub.Korban.Nama).FirstOrDefault();
                                        if(korban!=null)
                                        {
                                            hub.KorbanId = korban.Id;
                                        }
                                    }
                                   int? hubId= db.DataHubungan.InsertAndGetLastID(hub);
                                    if (hubId > 0)
                                        hub.Id = hubId;
                                    else
                                        throw new SystemException("Data Hubungan Tidak Tersimpan");

                                }
                                else
                                {
                                    if(!db.DataHubungan.Update(x=> new {x.JenisHubungan, x.KorbanId},hub, x=>x.Id==hub.Id && x.TerlaporId==data.Id))
                                        throw new SystemException("Data Hubungan Tidak Tersimpan");
                                }
                            }

                            foreach (var penanganan in data.DataPenanganan)
                            {
                                penanganan.IdentiasId = data.Id;
                                if (penanganan.IdPenanganan == null)
                                {
                                    penanganan.IdPenanganan = db.Penanganan.InsertAndGetLastID(penanganan);
                                }
                                else
                                {
                                    db.Penanganan.Update(x => new { x.Deskripsi, x.DetailLayanan, x.InstansiId, x.Layanan, x.Tanggal }, penanganan, x => x.IdPenanganan == penanganan.IdPenanganan);
                                }
                            }
                        }


                    if(item.Pelapor.Id==null)
                    {
                        item.Pelapor.PengaduanId = item.Id.Value;
                        item.Pelapor.Id = db.DataPelapor.InsertAndGetLastID(item.Pelapor);
                        if (item.Pelapor.Id <= 0)
                            throw new SystemException("Data Pelapor Tidak Tersimpan");
                    }
                    else
                    {
                        if(!db.DataPelapor.Update(x => new { x.Alamat, x.Gender, x.Nama }, item.Pelapor, x => x.Id == item.Pelapor.Id))
                            throw new SystemException("Data Pelapor Tidak Tersimpan");
                    }
                    
                    
                    if(item.Kondisi.Id==null)
                    {
                        item.Kondisi.PengaduanId = item.Id.Value;
                        item.Kondisi.Id = db.DataKondisiKorban.InsertAndGetLastID(item.Kondisi);
                        if (item.Kondisi.Id <= 0)
                            throw new SystemException("Data Kondisi Korban Tidak Tersimpan");

                    }
                    else
                    {
                        if(!db.DataKondisiKorban.Update(x => new {x.Fisik,x.Psikis,x.PsikisText,x.Sex,x.SexText },item.Kondisi,x=>x.Id==item.Kondisi.Id))
                            throw new SystemException("Data Kondisi Korban Tidak Tersimpan");

                    }

                    if(item.Dampak.Id==null)
                    {
                        item.Dampak.PengaduanId = item.Id.Value;
                        item.Dampak.Id = db.DataDampak.InsertAndGetLastID(item.Dampak);
                        if (item.Dampak.Id <= 0)
                            throw new SystemException("Data  Dampak Yang Dialami Tidak Tersimpan");

                    }
                    else
                    {

                        if (!db.DataDampak.Update(x => new { x.Fisik, x.Psikis, x.Ekonomi, x.Kesehatan, x.Lain,x.Seksual }, item.Dampak, x => x.Id == item.Dampak.Id))
                            throw new SystemException("Data Dampak Yang Dialami Korban Tidak Tersimpan");

                    }


                    trans.Commit();
                    this.list.Add(item);
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    throw new SystemException(ex.Message);
                }

            }

        }



        public bool Update(Pengaduan item)
        {
            using (var db = new DbContext())
            {
                var trans = db.BeginTransaction();
                try
                {

                    if (item.Id == null)
                    {
                        item.Id = db.DataPengaduan.InsertAndGetLastID(item);
                        if (item.Id <= 0)
                            throw new SystemException("Data Pengaduan Tidak Tersimpan");
                    }
                    else
                    {
                        if (!db.DataPengaduan.Update(x => new {
                            x.Catatan,
                            x.KodeDistrik,
                            x.Nomor,
                            x.Penerima,
                            x.Rujukan,
                            x.StatusPelapor,
                            x.TanggalKejadian,
                            x.TanggalLapor,
                            x.TempatKejadian,
                            x.TempatLapor,
                            x.UraianKejadian,
                            x.WaktuKejadian,
                            x.WaktuLapor, x.StatusPelaporText
                        }, item, x => x.Id == item.Id))
                            throw new SystemException("Data Pengaduan Tidak Tersimpan");
                    }


                    if (item.Korban != null)
                        foreach (var data in item.Korban)
                        {
                            if (data.Id == null)
                            {
                                data.PengaduanId = item.Id.Value;
                                data.Id = db.DataKorban.InsertAndGetLastID(data);
                            }
                            else
                            {
                                db.DataKorban.Update(x => new
                                {
                                    x.Agama,
                                    x.Alamat,
                                    x.Gender,
                                    x.Nama,
                                    x.NamaPanggilan,
                                    x.NIK,
                                    x.Pekerjaan,
                                    x.Pendidikan,
                                    x.Pernikahan,
                                    x.Suku,
                                    x.TanggalLahir,
                                    x.TempatLahir
                                }, data, x => x.Id == data.Id);
                            }

                            foreach (var penanganan in data.DataPenanganan)
                            {
                                if (penanganan.IdPenanganan == null)
                                {
                                    penanganan.IdPenanganan = db.Penanganan.InsertAndGetLastID(penanganan);
                                }
                                else
                                {
                                    db.Penanganan.Update(x => new { x.Deskripsi, x.DetailLayanan, x.InstansiId, x.Layanan, x.Tanggal }, penanganan, x => x.IdPenanganan == penanganan.IdPenanganan);
                                }
                            }


                        }


                    if (item.Terlapor != null)
                        foreach (var data in item.Terlapor)
                        {
                            if (data.Id == null)
                            {
                                data.PengaduanId = item.Id.Value;
                                data.Id = db.DataTerlapor.InsertAndGetLastID(data);
                            }
                            else
                            {
                                db.DataTerlapor.Update(x => new
                                {
                                    x.Agama,
                                    x.Alamat,
                                    x.Gender,
                                    x.Nama,
                                    x.NamaPanggilan,
                                    x.NIK,
                                    x.Pekerjaan,
                                    x.Pendidikan,
                                    x.Pernikahan,
                                    x.Suku,
                                    x.TanggalLahir,
                                    x.TempatLahir
                                }, data, x => x.Id == data.Id);
                            }
                            foreach (var penanganan in data.DataPenanganan)
                            {
                                if (penanganan.IdPenanganan == null)
                                {
                                    penanganan.IdPenanganan = db.Penanganan.InsertAndGetLastID(penanganan);
                                }
                                else
                                {
                                    db.Penanganan.Update(x => new { x.Deskripsi, x.DetailLayanan, x.InstansiId, x.Layanan, x.Tanggal }, penanganan, x => x.IdPenanganan == penanganan.IdPenanganan);
                                }
                            }
                        }


                    if (item.Pelapor.Id == null)
                    {
                        item.Pelapor.PengaduanId = item.Id.Value;
                        item.Pelapor.Id = db.DataPelapor.InsertAndGetLastID(item.Pelapor);
                        if (item.Pelapor.Id <= 0)
                            throw new SystemException("Data Pelapor Tidak Tersimpan");
                    }
                    else
                    {
                        if (!db.DataPelapor.Update(x => new { x.Alamat, x.Gender, x.Nama }, item.Pelapor, x => x.Id == item.Pelapor.Id))
                            throw new SystemException("Data Pelapor Tidak Tersimpan");
                    }


                    if (item.Kondisi.Id == null)
                    {
                        item.Kondisi.PengaduanId = item.Id.Value;
                        item.Kondisi.Id = db.DataKondisiKorban.InsertAndGetLastID(item.Kondisi);
                        if (item.Kondisi.Id <= 0)
                            throw new SystemException("Data Kondisi Korban Tidak Tersimpan");

                    }
                    else
                    {
                        if (!db.DataKondisiKorban.Update(x => new { x.Fisik, x.Psikis, x.PsikisText, x.Sex, x.SexText }, item.Kondisi, x => x.Id == item.Kondisi.Id))
                            throw new SystemException("Data Kondisi Korban Tidak Tersimpan");

                    }

                    if (item.Dampak.Id != null)
                    {
                        item.Dampak.PengaduanId = item.Id.Value;
                        item.Dampak.Id = db.DataDampak.InsertAndGetLastID(item.Dampak);
                        if (item.Dampak.Id <= 0)
                            throw new SystemException("Data  Dampak Yang Dialami Tidak Tersimpan");

                    }
                    else
                    {

                        if (!db.DataDampak.Update(x => new { x.Fisik, x.Psikis, x.Ekonomi, x.Kesehatan, x.Lain, x.Seksual }, item.Dampak, x => x.Id == item.Dampak.Id))
                            throw new SystemException("Data Dampak Yang Dialami Korban Tidak Tersimpan");

                    }


                    trans.Commit();
                    return true;
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    throw new SystemException(ex.Message);
                }

            }

        }

        public void Clear()
        {
            list.Clear();
        }

        public bool Contains(Pengaduan item)
        {
            return list.Contains(item);
        }

        public void CopyTo(Pengaduan[] array, int arrayIndex)
        {
            list.CopyTo(array, arrayIndex);
        }

        public IEnumerator<Pengaduan> GetEnumerator()
        {
            return new PengaduanEnumerator(this.ToList());
        }

        public int IndexOf(Pengaduan item)
        {
            throw new NotImplementedException();
        }

        public void Insert(int index, Pengaduan item)
        {
            throw new NotImplementedException();
        }

        public bool Remove(Pengaduan item)
        {
            try
            {
                list.Remove(item);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public void RemoveAt(int index)
        {
            throw new NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return list.GetEnumerator();
        }
    }



    public class PengaduanEnumerator : IEnumerator<Pengaduan>
    {
        private List<Pengaduan> _collection;
        private int curIndex;
        private Pengaduan curBox;


        public PengaduanEnumerator(List<Pengaduan> collection)
        {
            _collection = collection;
            curIndex = -1;
            curBox = default(Pengaduan);

        }

        public bool MoveNext()
        {
            //Avoids going beyond the end of the collection.
            if (++curIndex >= _collection.Count)
            {
                return false;
            }
            else
            {
                // Set current box to next item in collection.
                curBox = _collection[curIndex];
            }
            return true;
        }

        public void Reset() { curIndex = -1; }

        void IDisposable.Dispose() { }

        public Pengaduan Current
        {
            get { return curBox; }
        }


        object IEnumerator.Current
        {
            get { return Current; }
        }

    }




}
