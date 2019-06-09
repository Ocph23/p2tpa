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
                var data = from pengaduan in db.DataPengaduan.Select()
                           join pelapor in db.DataPelapor.Select() on pengaduan.IdPelapor equals pelapor.Id
                           join korban in db.DataKorban.Select() on pengaduan.IdKorban equals korban.Id
                           join terlapor in db.DataTerlapor.Select() on pengaduan.IdTerlapor equals terlapor.Id
                           join dampak in db.DataDampak.Select() on pengaduan.Id equals dampak.PengaduanId
                           join kejadian in db.DataKejadian.Select() on pengaduan.Id equals kejadian.PengaduanId
                           join kondisi in db.DataKondisiKorban.Select() on pengaduan.Id equals kondisi.PengaduanId
                           select new Pengaduan()
                           {
                               Catatan = pengaduan.Catatan,
                               Hari = pengaduan.Hari,
                               Id = pengaduan.Id,
                               IdKorban = pengaduan.IdKorban,
                               IdPelapor = pengaduan.IdPelapor,
                               IdTerlapor = pengaduan.IdTerlapor,
                               Nomor = pengaduan.Nomor,
                               Korban = korban,
                               Pelapor = pelapor,
                               Terlapor = terlapor,
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
               var trans=  db.BeginTransaction();
                try
                {

                    item.IdPelapor = db.DataPelapor.InsertAndGetLastID(item.Pelapor);
                    if(item.IdPelapor<=0)
                        throw new SystemException("Data Pelapor Tidak Tersimpan");
                    item.Pelapor.Id = item.IdPelapor;

                    item.IdKorban = db.DataKorban.InsertAndGetLastID(item.Korban);
                    if (item.IdKorban <= 0)
                        throw new SystemException("Data Korban Tidak Tersimpan");
                    item.Korban.Id = item.IdKorban;

                    item.IdTerlapor = db.DataTerlapor.InsertAndGetLastID(item.Terlapor);
                    if (item.IdTerlapor<= 0)
                        throw new SystemException("Data Terlapor Tidak Tersimpan");
                    item.Terlapor.Id = item.IdTerlapor;



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
                    if(item.Kejadian.Id<=0)
                        throw new SystemException("Data  Kejadian Tidak Tersimpan");


                  foreach(var data in item.Perkembangan)
                    {
                        data.PengaduanId = item.Id.Value;
                        data.Id= db.DataTahapanPerkembangan.InsertAndGetLastID(data);
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
            throw new NotImplementedException();
        }

        public bool Contains(Pengaduan item)
        {
            throw new NotImplementedException();
        }

        public void CopyTo(Pengaduan[] array, int arrayIndex)
        {
            throw new NotImplementedException();
        }

        public IEnumerator<Pengaduan> GetEnumerator()
        {
            throw new NotImplementedException();
        }

        public bool Remove(Pengaduan item)
        {
            throw new NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }
    }




}
