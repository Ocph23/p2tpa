using LiveCharts;
using LiveCharts.Wpf;
using System.Collections.Generic;
using System.Linq;

namespace Main.Charts.Dialogs
{
    /// <summary>
    /// Interaction logic for JumlahKasusKorbanMenurutUmur.xaml
    /// </summary>
    public partial class JumlahKasusKorbanMenurutUmur : ChartMaster
    {
        public JumlahKasusKorbanMenurutUmur()
        {
            InitializeComponent();
            this.RefreshChartCommand = new CommandHandler { CanExecuteAction = x => true, ExecuteAction = RefreshAction};
            this.DataContext = this;
            this.RefreshChartCommand.Execute(null);
            Title = "% Korban menurut Kelompok Umur";
          
        }

        private void RefreshAction(object obj)
        {
            var source = DataAccess.DataBasic.DataPengaduan;
            List<string> labels = new List<string>();
            labels.Add("0-5");
            labels.Add("6-12");
            labels.Add("13-17");
            labels.Add("18-24");
            labels.Add("25-44");
            labels.Add("45-59");
            labels.Add("60+");
            List<int> datas = new List<int>();

            var result = from p in source
                         from korban in p.Korban
                         where korban.TanggalLahir != null
                         let age = p.Tanggal.Year - korban.TanggalLahir.Year
                         group p by
                            age < 6 ? "0-5" :
                            age < 13 ? "6-12" :
                            age < 18 ? "13-17" :
                            age < 25 ? "18-24" :
                            age < 45 ? "25-44" :
                            age < 60 ? "45-59" : "60+" into ages
                         select new { Age = ages.Key, Persons = ages };

            foreach (var item in labels)
            {

                int value = 0;
                var data = result.Where(x => x.Age == item).FirstOrDefault();
                if (data != null)
                    value = data.Persons.Count();
                datas.Add(value);
                SeriesCollection.Add(new PieSeries
                {
                    Title = item,
                    Values = new ChartValues<double> { value }
                });
            }


              Labels = labels.ToArray();
            //new[] { "Jan", "Feb", "Mar", "Apr", "May" };
            PointLabel = chartPoint =>
               string.Format("{0} ({1:P})", chartPoint.Y, chartPoint.Participation);

        }
    }
}
