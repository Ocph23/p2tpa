using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Data;
using AutoMapper;
using Main.Models;
using Main.ViewModels;

namespace Main.Views
{
    public partial class DataView : Window
    {
        public DataView()
        {
            InitializeComponent();
           
            this.DataContext = new DataViewModel();
        }
    }


    public class DataViewModel
    {
        public DataViewModel()
        {
            Datas = CollectionViewSource.GetDefaultView(DataAccess.DataBasic.MasterPengaduan);
            EditCommand = new CommandHandler { CanExecuteAction = x => true, ExecuteAction = EditAction };
            DeleteCommand = new CommandHandler { CanExecuteAction = x => true, ExecuteAction = DeleteAction };
            AddCommand = new CommandHandler { CanExecuteAction = x => true, ExecuteAction = AddAction };
        }

        private void AddAction(object obj)
        {
            var form = new TambahPengaduan(false);
            form.DataContext = new PengaduanViewModel();
            form.ShowDialog();

            Datas.Refresh();

        }

        private  void DeleteAction(object obj)
        {
            MessageBoxResult dialog = MessageBox.Show("Yakin Hapus Data Ini ? ", "Perhatian", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if(dialog== MessageBoxResult.Yes)
            {
                try
                {
                    DataAccess.DataBasic.MasterPengaduan.Remove(obj as Pengaduan);
                    MessageBox.Show("Berhasil !", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
                    Datas.Refresh();
                  
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void EditAction(object obj)
        {
            if(obj!=null)
            {
                var form = new TambahPengaduan(false);
                var data=Mapper.Map<PengaduanViewModel>(obj as Pengaduan);
                form.DataContext = data;
                form.ShowDialog();
            }
          
        }

        public ICollectionView Datas { get; set; }
        public CommandHandler EditCommand { get; }
        public CommandHandler DeleteCommand { get; }
        public CommandHandler AddCommand { get; }
    }
}
