using Main.ViewModels;
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
                var aaa = db.DataPengaduan.Select();

                var data = from pengaduan in db.DataPengaduan.Select()
                           join pelapor in db.DataPelapor.Select().DefaultIfEmpty() on pengaduan.Id equals pelapor.PengaduanId
                           join dampak in db.DataDampak.Select().DefaultIfEmpty() on pengaduan.Id equals dampak.PengaduanId
                           join kejadian in db.DataKejadian.Select().DefaultIfEmpty() on pengaduan.Id equals kejadian.PengaduanId
                           join kondisi in db.DataKondisiKorban.Select().DefaultIfEmpty() on pengaduan.Id equals kondisi.PengaduanId
                           select new Pengaduan()
                           {
                               KodeDistrik = pengaduan.KodeDistrik,
                               HubunganKorbanDenganTerlapor = pengaduan.HubunganKorbanDenganTerlapor,
                               Catatan = pengaduan.Catatan,
                               Hari = pengaduan.Hari,
                               Id = pengaduan.Id,
                               Nomor = pengaduan.Nomor,
                               Pelapor = pelapor,
                               Penerima = pengaduan.Penerima,
                               Rujukan = pengaduan.Rujukan,
                               Tanggal = pengaduan.Tanggal,
                               Waktu = pengaduan.Waktu,
                               UraianKejadian = pengaduan.UraianKejadian,
                               Tempat = pengaduan.Tempat,
                               Penanganan = pengaduan.Penanganan,
                               Dampak = dampak,
                               Kejadian = kejadian,
                               Kondisi = kondisi
                           };

                ;
                foreach (var item in data)
                {
                    var id = item.Id.Value;
                    item.Korban = db.DataKorban.Where(x => x.PengaduanId == id).ToList();
                    item.Terlapor= db.DataTerlapor.Where(x => x.PengaduanId == id).ToList();
                    list.Add(item);
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

                    item.Id = db.DataPengaduan.InsertAndGetLastID(item);
                    if (item.Id <= 0)
                        throw new SystemException("Data Pengaduan Tidak Tersimpan");


                     if(item.Korban!=null)
                        foreach(var data in item.Korban)
                        {
                            if (data.Id<=0)
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
                        }


                    if (item.Terlapor != null)
                        foreach (var data in item.Terlapor)
                        {
                            if (data.Id <= 0)
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
                        }

                    item.Pelapor.PengaduanId = item.Id.Value;
                    item.Pelapor.Id = db.DataPelapor.InsertAndGetLastID(item.Pelapor);
                    if (item.Pelapor.Id <= 0)
                        throw new SystemException("Data Pelapor Tidak Tersimpan");
                  
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


                    foreach (var data in item.Perkembangan)
                    {
                        data.PengaduanId = item.Id.Value;
                        data.Id = db.DataTahapanPerkembangan.InsertAndGetLastID(data);
                        if (data.Id <= 0)
                            throw new SystemException("Data Tahapan  Perkembangan Kejadian Tidak Tersimpan");
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
