using Microsoft.Reporting.WinForms;
using System;
using System.Windows;
using System.Windows.Controls;

namespace Main.Reports.Views
{
    /// <summary>
    /// Interaction logic for ReportContent.xaml
    /// </summary>
    public partial class ReportContent : UserControl
    {
        public ReportContent(ReportDataSource reportDataSource, string layout, ReportParameter[] parameters)
        {
            InitializeComponent();
            reportViewer.LocalReport.ReportEmbeddedResource = layout;
            reportViewer.SetDisplayMode(DisplayMode.PrintLayout);
            reportViewer.ZoomMode = ZoomMode.PageWidth;
            reportViewer.LocalReport.DataSources.Add(reportDataSource);
            reportDataSource.Name = "DataSet1"; // Name of the DataSet we set in .rdlc
            if (parameters != null)
                reportViewer.LocalReport.SetParameters(parameters);
            reportViewer.RefreshReport();
        }

        public ReportContent(ReportDataSource dataHeader, ReportDataSource items, string layout, ReportParameter[] parameters)
        {
            InitializeComponent();
            reportViewer.LocalReport.ReportEmbeddedResource = layout;
            reportViewer.SetDisplayMode(DisplayMode.PrintLayout);
            reportViewer.ZoomMode = ZoomMode.PageWidth;
            reportViewer.LocalReport.DataSources.Add(dataHeader);
            reportViewer.LocalReport.DataSources.Add(items);
            if (parameters != null)
                reportViewer.LocalReport.SetParameters(parameters);
            reportViewer.RefreshReport();
        }

        public Action WindowClose { get; internal set; }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            WindowClose();
        }
    }
}
