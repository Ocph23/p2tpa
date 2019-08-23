using Main.Models;
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
using System.Windows.Shapes;

namespace Main.Views
{
    /// <summary>
    /// Interaction logic for KategoriView.xaml
    /// </summary>
    public partial class KategoriView : Window
    {
        public KategoriView(Models.KategoriInstansi kategori)
        {
            InitializeComponent();
            DataContext = new KategoriViewModel(kategori) { WindowClose=this.Close};

        }
    }


    public class KategoriViewModel:KategoriInstansi
    {
        public KategoriViewModel(Models.KategoriInstansi kategori)
        {
            if(kategori != null)
            {
                Id = kategori.Id;
                Name = kategori.Name;
                Code = kategori.Code;
            }

            SaveCommand = new CommandHandler { CanExecuteAction = SaveValidate, ExecuteAction = SaveAction};

        }

        public CommandHandler SaveCommand { get; }
        public Action WindowClose { get;  set; }
        public KategoriInstansi SelectedModel { get; private set; }

        private void SaveAction(object obj)
        {
            try
            {
                var model = new KategoriInstansi { Id = this.Id, Code = Code, Name = Name };
                using (var db = new DbContext())
                {
                    if (Id != null)
                    {
                        db.KategoriInstansi.Update(x=> new {x.Name,x.Code },model,x=>x.Id==model.Id);
                    }else
                    {
                        model.Id = db.KategoriInstansi.InsertAndGetLastID(model);
                    }
                }
                MessageBox.Show("Berhasil", "Sukses", MessageBoxButton.OK, MessageBoxImage.Information);
                SelectedModel = model;
                WindowClose();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private bool SaveValidate(object obj)
        {
            if (string.IsNullOrWhiteSpace(Name) || string.IsNullOrEmpty(Code))
                return false;
            return true;
        }

       
    }
}
