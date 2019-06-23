using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
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
    public partial class ComboChips : UserControl  , INotifyPropertyChanged
    {
        public ComboChips()
        {
            InitializeComponent();
            DatacomboBoxSource.CollectionChanged += DatacomboBoxSource_CollectionChanged;
            DatalistBoxSource.CollectionChanged += DatalistBoxSource_CollectionChanged;
            ComboBoxSource = (CollectionView)CollectionViewSource.GetDefaultView(DatacomboBoxSource);
            ListBoxSource = (CollectionView)CollectionViewSource.GetDefaultView(DatalistBoxSource);
            DeleteItem = new CommandHandler { CanExecuteAction = x => true, ExecuteAction = DeleteAction };
            this.DataContext = this;
        }

        private void DatalistBoxSource_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
             ListBoxSource.Refresh();
                Result = DatalistBoxSource.ToList();
        }

        private void DatacomboBoxSource_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
                ListBoxSource.Refresh();

        }

        private void CurrentChip_DeleteClick(object sender, RoutedEventArgs e)
        {
            var data = ((Chip)sender).Content;
            DeleteItem.Execute(data);
        }


        private void DeleteAction(object obj)
        {
            SelectedItem = null;
            if (obj != null)
            {
                DatalistBoxSource.Remove(obj as string);
                DatacomboBoxSource.Add(obj as string);
            }
        }

        public CollectionView ComboBoxSource { get; set; }
        public CollectionView ListBoxSource { get; set; }
        public CommandHandler DeleteItem { get; }

        private string selected;
        private static ICollection<string> _values;

        public string SelectedItem
        {
            set
            {
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
            DatalistBoxSource.Add(value);
            DatacomboBoxSource.Remove(value);
         
        }


        public event PropertyChangedEventHandler PropertyChanged;

        public bool SetProperty<T>(ref T backingStore, T value,
        [CallerMemberName]string propertyName = "", Action onChanged = null)
        {
            if (EqualityComparer<T>.Default.Equals(backingStore, value))
                return false;

            backingStore = value;
            onChanged?.Invoke();
            OnPropertyChanged(propertyName);
            return true;
        }

        public string GetPropertyName<T>(Expression<Func<T>> propertyExpression)
        {
            return (propertyExpression.Body as MemberExpression).Member.Name;
        }
        #region INotifyPropertyChanged



        public void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            var changed = PropertyChanged;
            if (changed == null)
                return;
            changed.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion


        public List<string> ItemSource
        {
            get { return (List<string>)GetValue(ItemSourceProperty); }
            set { SetValue(ItemSourceProperty, value); }
        }

        public IEnumerable<string> Result
        {
            get { return (List<string>)GetValue(ResultProperty); }
            set { SetValue(ResultProperty, value); }
        }





        /// static
        /// 

        public static ObservableCollection<string> DatalistBoxSource { get; set; } = new ObservableCollection<string>();
        public static ObservableCollection<string> DatacomboBoxSource { get; set; } = new ObservableCollection<string>();
        public static ICollection<string> Values {
             get { return _values; }
            set {
                _values = value;
            } }

        public static readonly DependencyProperty ItemSourceProperty =
                DependencyProperty.Register("ItemSource", typeof(IEnumerable<string>), typeof(ComboChips), new
                PropertyMetadata(null, new PropertyChangedCallback(onChangeSource)));

        private static void onChangeSource(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var results = e.NewValue as ICollection<string>;
            DatacomboBoxSource.Clear();
            foreach (var item in results)
            {
                DatacomboBoxSource.Add(item);
            }
        }


        public static readonly DependencyProperty ResultProperty =
        DependencyProperty.Register("Result", typeof(IEnumerable<string>), typeof(ComboChips), new UIPropertyMetadata(null));

    }

}
