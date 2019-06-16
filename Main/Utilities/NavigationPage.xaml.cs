using Main.DataAccess;
using Main.ViewModels;
using Main.Views.TambahKasusPages;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Main.Utilities
{
    /// <summary>
    /// Interaction logic for NavigationPage.xaml
    /// </summary>
    public partial class NavigationPage : UserControl
    {
        public NavigationPage()
        {
            InitializeComponent();
        }
    }


    public class NavigationPageViewModel : BaseNotify
    {
        private bool avaliableBack;
        private bool avaliableNext;
        private bool avaliableFinish;

        public bool AvaliableNext
        {
            get
            {
                return avaliableNext;
            }
            set { SetProperty(ref avaliableNext, value); }
        }


        public bool AvaliableBack
        {
            get
            {
                return avaliableBack;
            }
            set { SetProperty(ref avaliableBack, value); }
        }

        public bool AvaliableFinish
        {
            get {
                return avaliableFinish; }
            set { SetProperty(ref avaliableFinish, value); }
        }


        private Frame mainFrame;

        public Frame MainFrame
        {
            get { return mainFrame; }
            set { SetProperty(ref mainFrame, value); }
        }

        public ICommand FinishCommand { get; set; }
        public ICommand BackCommand { get; set; }
        public ICommand NextCommand { get; set; }
        public ICommand CancelCommand { get; set; }

        public NavigationPageViewModel(Frame frame, Pengaduan vm)
        {
            FinishCommand = new CommandHandler { CanExecuteAction = x => FinishCommandValidate(), ExecuteAction = x => FinishAction() };
            BackCommand = new CommandHandler { CanExecuteAction = x => BackCommandValidate(), ExecuteAction = x => BackAction() };
            NextCommand = new CommandHandler { CanExecuteAction = x => NextCommandValidate(), ExecuteAction = x => NextAction() };
            CancelCommand = new CommandHandler { CanExecuteAction = x => CancelCommandValidate(), ExecuteAction = x => CancelAction() };
            this.frame = frame;
            this.vm = vm;
            this.Pages = new List<Page>()
            {
                new PengaduanPage(this.vm),
                new KorbanPage(vm),
                new KondisiPage(vm),
                new DampakPage(vm),
                new KejadianPage(vm),
                new PenanganDanUraianPage(vm),
                new PerkembanganDanCatatan(vm),
            };
            currentPage = 0;
            frame.Navigate(Pages[0]);
        }

      

        private void NextAction()
        {
            GoNextPage();
        }

        private void CancelAction()
        {

        }

        private bool CancelCommandValidate()
        {
            return true;
        }

        private bool NextCommandValidate()
        {
            if(currentPage>=0)
            {
                var context = Pages[currentPage].DataContext;
                IDataErrorInfo errorInfo = context as IDataErrorInfo;
                if (string.IsNullOrEmpty(errorInfo.Error))
                    return AvaliableNext;
            }


            return false;
        }

        private bool BackCommandValidate()
        {
            if (!frame.CanGoBack)
            {
                AvaliableBack = false;
                return false;
            }
            return true;
        }

        private void BackAction()
        {
            frame.GoBack();
            if(currentPage>0)
              currentPage--;
        }

        private void FinishAction()
        {

            try
            {
                PengaduanServices service = new PengaduanServices();
                service.Add(vm);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message,"Error", MessageBoxButton.OK, MessageBoxImage.Error);

            }

            MessageBox.Show("Finish");
        }

        private bool FinishCommandValidate()
        {
            if (currentPage >= 0)
            {
                var context = Pages[currentPage].DataContext;
                IDataErrorInfo errorInfo = context as IDataErrorInfo;

                if (!frame.CanGoForward && currentPage == Pages.Count - 1 && string.IsNullOrEmpty(errorInfo.Error))
                {
                    AvaliableFinish = true;
                    AvaliableNext = false;
                }
                else
                {
                    AvaliableFinish = false;
                    AvaliableNext = true;

                }
            }
            return AvaliableFinish;
        }

        readonly Frame frame;
        private  Pengaduan vm;
        private List<Page> Pages;
        private int currentPage;

        private void GoNextPage()
        {
            if (frame.CanGoForward)
            {
                frame.GoForward();
                currentPage++;
            }
            else
            {
                currentPage++;
                frame.Navigate(Pages[currentPage]);
            }
            AvaliableBack = true;
        }
    }




}
