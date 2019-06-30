using Ocph.DAL;
using System;
using System.ComponentModel;

namespace Main.ViewModels
{
    [TableName("Korban")]
    public class Korban :Identitas
    {
        [DbColumn("Fisik")]
        public KondisiFisik Fisik { get => fisik; set => SetProperty(ref fisik, value); }

        [DbColumn("Psikis")]
        public KondisiPsikis Psikis { get => psikis; set => SetProperty(ref psikis, value); }

        [DbColumn("Sex")]
        public KondisiSex Sex { get => sex; set => SetProperty(ref sex, value); }


        [DbColumn("PsikisText")]
        public string PsikisText { get => psikisText; set => SetProperty(ref psikisText, value); }

        [DbColumn("SexText")]
        public string SexText { get => sextText; set => SetProperty(ref sextText, value); }

        private KondisiFisik fisik;
        private KondisiPsikis psikis;
        private KondisiSex sex;
        private string sextText;
        private string psikisText;
    }
}
