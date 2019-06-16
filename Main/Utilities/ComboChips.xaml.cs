using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace Main.Utilities
{
    /// <summary>
    /// Interaction logic for ComboChips.xaml
    /// </summary>
    public partial class ComboChips : UserControl
    {
        public ComboChips()
        {
            InitializeComponent();
            DataContext = new ComboChipsViewModel();
        }

        private void CurrentChip_DeleteClick(object sender, RoutedEventArgs e)
        {
            var data = ((Chip)sender).Content;
            var vm = DataContext as ComboChipsViewModel;
            vm.DeleteItem.Execute(data);
        }

        public static readonly DependencyProperty SetTextProperty =
       DependencyProperty.Register("ItemSource", typeof(CollectionView), typeof(ComboChips), new
          PropertyMetadata("", new PropertyChangedCallback(OnSetTextChanged)));

        private static void OnSetTextChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ComboChips UserControl1Control = d as ComboChips;
            UserControl1Control.OnSetTextChanged(e);
        }

        private void OnSetTextChanged(DependencyPropertyChangedEventArgs e)
        {
            var vm = DataContext as ComboChipsViewModel;
            vm.ComboBoxSource = e.NewValue as CollectionView;
        }

        public CollectionView ItemSource
        {
            get { return (CollectionView)GetValue(SetTextProperty); }
            set { SetValue(SetTextProperty, value); }
        }

       
    }

    public class ComboChipsViewModel :BaseNotify
    {
        ObservableCollection<string> listBoxSource;
        ObservableCollection<string> comboBoxSource;
        public ComboChipsViewModel()
        {
            listBoxSource = new ObservableCollection<string>();
            listBoxSource.CollectionChanged += ListBoxSource_CollectionChanged;
            comboBoxSource = new ObservableCollection<string>(EnumSource.DataStatusPernikahan());
            ComboBoxSource = (CollectionView)CollectionViewSource.GetDefaultView(comboBoxSource);
            ListBoxSource =(CollectionView)CollectionViewSource.GetDefaultView(listBoxSource);
            DeleteItem = new CommandHandler { CanExecuteAction = x => true, ExecuteAction = DeleteAction };
        }

        private void ListBoxSource_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void DeleteAction(object obj)
        {
            SelectedItem = null;
            if (obj!=null)
            {
                listBoxSource.Remove(obj as string);
                comboBoxSource.Add(obj as string);
                ListBoxSource.Refresh();
                ComboBoxSource.Refresh();

            }
        }

        public CollectionView ComboBoxSource { get; }
        public CollectionView ListBoxSource { get; }
        public CommandHandler DeleteItem { get; }

        private string selected;

        public string SelectedItem
        {
            set {
                SetProperty(ref selected, value);
                if (!string.IsNullOrEmpty(value))
                {
                    AddToListBox(value);
                }
            }
        }


        private async void AddToListBox(string value)
        {
            await Task.Delay(100);
            SelectedItem = null;
            listBoxSource.Add(value);
            comboBoxSource.Remove(value);
            ListBoxSource.Refresh();
            ComboBoxSource.Refresh();

        }
    }
}
