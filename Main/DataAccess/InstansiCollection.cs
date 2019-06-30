using Main.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Main.DataAccess
{
    public  class InstansiCollection : ICollection<Instansi>
    {
        ArrayList list = new ArrayList();
        public InstansiCollection()
        {
            using (var db = new DbContext())
            {
                try
                {
                  var  data = db.Instansi.Select();
                    if (data.Count() > 0)
                        foreach (var item in data)
                        {
                            list.Add(item);
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

        public void Add(Instansi item)
        {

            using (var db = new DbContext())
            {
                var trans = db.BeginTransaction();
                try
                {

                    item.Id = db.Instansi.InsertAndGetLastID(item);
                    if (item.Id <= 0)
                        throw new SystemException("Data Pengaduan Tidak Tersimpan");

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
        public void Update(Instansi data)
        {
            using (var db = new DbContext())
            {
                var trans = db.BeginTransaction();
                try
                {
                    Instansi item = new Instansi { Alamat = data.Alamat, Kategori = data.Kategori, Tingkat = data.Tingkat, DistrikName = data.DistrikName, Name = data.Name, Id = data.Id };
                     var saved = db.Instansi.Update(x=> new { x.Kategori,x.Tingkat,x.DistrikName,x.Name,x.Alamat},item,x=>x.Id==item.Id);
                    if (!saved)
                        throw new SystemException("Data Instansi Tidak Tersimpan");

                    trans.Commit();
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

        public bool Contains(Instansi item)
        {
            return list.Contains(item);
        }

        public void CopyTo(Instansi[] array, int arrayIndex)
        {
            list.CopyTo(array, arrayIndex);
        }

        public IEnumerator<Instansi> GetEnumerator()
        {
            return new InstansiEnumerator(this.ToList());
        }

      

        public bool Remove(Instansi item)
        {
            try
            {
                using (var db = new DbContext())
                {
                    if (db.Instansi.Delete(x => x.Id == item.Id))
                    {
                        list.Remove(item);
                        return true;
                    }
                }
                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }


        IEnumerator IEnumerable.GetEnumerator()
        {
            return list.GetEnumerator();
        }
    }



    public class InstansiEnumerator : IEnumerator<Instansi>
    {
        private List<Instansi> _collection;
        private int curIndex;
        private Instansi curBox;


        public InstansiEnumerator(List<Instansi> collection)
        {
            _collection = collection;
            curIndex = -1;
            curBox = default(Instansi);

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

        public Instansi Current
        {
            get { return curBox; }
        }


        object IEnumerator.Current
        {
            get { return Current; }
        }

    }


}
