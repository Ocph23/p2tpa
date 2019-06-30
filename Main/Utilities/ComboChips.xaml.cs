using MaterialDesignThemes.Wpf;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

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
       DependencyProperty.Register("ItemSource", typeof(ObservableCollection<string>), typeof(ComboChips), new
          PropertyMetadata(null, new PropertyChangedCallback(OnSetTextChanged)));

        private static void OnSetTextChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ComboChips UserControl1Control = d as ComboChips;
            UserControl1Control.OnSetTextChanged(e);
        }

        private void OnSetTextChanged(DependencyPropertyChangedEventArgs e)
        {
            var vm = DataContext as ComboChipsViewModel;
           var data = e.NewValue as IEnumerable<string>;
            foreach(var item in data)
            {
                vm.ComboBoxData.Add(item);
            }
        }

        public ObservableCollection<string> ItemSource
        {
            get { return (ObservableCollection<string>)GetValue(SetTextProperty); }
            set { SetValue(SetTextProperty, value); }
        }


    }

    public class ComboChipsViewModel :BaseNotify
    {
        public ObservableCollection<string> ListBoxData { get; set; }
        public ObservableCollection<string> ComboBoxData { get; set; }
        public ComboChipsViewModel()
        {
            ListBoxData = new ObservableCollection<string>();
            ListBoxData.CollectionChanged += ListBoxData_CollectionChanged;
            ComboBoxData= new ObservableCollection<string>();
            ComboBoxData.CollectionChanged += ComboBoxData_CollectionChanged;
            ComboBoxSource = (CollectionView)CollectionViewSource.GetDefaultView(ComboBoxData);
            ListBoxSource =(CollectionView)CollectionViewSource.GetDefaultView(ListBoxData);
            DeleteItem = new CommandHandler { CanExecuteAction = x => true, ExecuteAction = DeleteAction };
        }

        private async void ComboBoxData_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            await Task.Delay(100);
            ComboBoxSource.Refresh();
        }

        private async void ListBoxData_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            await Task.Delay(100);
            ListBoxSource.Refresh();
        }

       

        private void DeleteAction(object obj)
        {
            SelectedItem = null;
            if (obj!=null)
            {
                ListBoxData.Remove(obj as string);
                ComboBoxData.Add(obj as string);

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
            ListBoxData.Add(value);
            ComboBoxData.Remove(value);
        }
    }
}
