﻿using System;
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
using Main.ViewModels;

namespace Main.Views
{
    /// <summary>
    /// Interaction logic for DataView.xaml
    /// </summary>
    public partial class DataView : Window
    {
        public DataView()
        {
            InitializeComponent();
            Datas = DataAccess.DataBasic.DataPengaduan;
            this.DataContext = this;
        }

        public List<Pengaduan> Datas { get; }

        private void PopupBox_OnOpened(object sender, RoutedEventArgs e)
        {

        }

        private void PopupBox_OnClosed(object sender, RoutedEventArgs e)
        {

        }
    }
}