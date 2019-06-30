using LiveCharts;
using LiveCharts.Defaults;
using LiveCharts.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Main.Charts
{
    /// <summary>
    /// Interaction logic for KorbanBerdasarkanUsia.xaml
    /// </summary>
    public partial class KorbanBerdasarkanUsia : ChartMaster
    {
        public KorbanBerdasarkanUsia()
        {
            InitializeComponent();
            this.RefreshChartCommand = new CommandHandler { CanExecuteAction = x => true, ExecuteAction = RefreshAction };
            this.RefreshChartCommand.Execute(null);
            Title = "Korban Berdasarkan Usia";
            this.DataContext = this;
        }

        private void RefreshAction(object obj)
        {
            List<string> labels = new List<string>();
            labels.Add("0-5");
            labels.Add("6-12");
            labels.Add("13-17");
            labels.Add("18-24");
            labels.Add("25-44");
            labels.Add("45-59");
            labels.Add("60+");
            List<int> datas = new List<int>();

            var result = from p in DataAccess.DataBasic.DataPengaduan
                         from korban in p.Korban
                         where korban.TanggalLahir != null
                         let age = p.TanggalLapor.Value.Year - korban.TanggalLahir.Year
                         group p by 
                            age <6 ? "0-5" : 
                            age < 13 ? "6-12": 
                            age < 18 ? "13-17" :
                            age <25 ? "18-24":
                            age < 45 ? "25-44":
                            age <60 ?"45-59": "60+" into ages
                         select new { Age = ages.Key, Persons = ages };

            foreach(var item in labels)
            {

                int value = 0;
                var data = result.Where(x => x.Age == item).FirstOrDefault();
                if (data != null)
                    value = data.Persons.Count();
                datas.Add(value);
                SeriesCollection.Add(new ColumnSeries
                {
                    Title = item,
                    Values = new ChartValues<int> { value}
                });
            }

           

            Labels = labels.ToArray();
            //new[] { "Jan", "Feb", "Mar", "Apr", "May" };
            YFormatter = value => ((int)value).ToString("N");
            XFormatter = value => ((int)value) <= 0 ? "" : ((int)value).ToString("N");
        }
    }
}
