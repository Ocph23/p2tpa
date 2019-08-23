using Ocph.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Main.Models
{
    [TableName("KetegoriInstansi")]
    public class KategoriInstansi:BaseNotify
    {
        private int? id;
        private string code;
        private string name;

        [PrimaryKey("Id")]
        [DbColumn("Id")]
        public int? Id
        {
            get { return id; }
            set { SetProperty(ref id, value); }
        }

        [DbColumn("Name")]
        public string Name
        {
            get { return name; }
            set { SetProperty(ref name ,value); }
        }

        [DbColumn("Code")]
        public string Code
        {
            get { return code; }
            set { SetProperty(ref code ,value); }
        }

    }
}
