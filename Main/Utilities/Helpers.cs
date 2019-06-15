using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Main.Utilities
{
    class Helpers
    {
    }

    public static class WindowHelpers
    {
        public static Window GetWindow()
        {
            return Application.Current.MainWindow;
        }
        public static double CurrentScreen(this Window window)
        {
            return System.Windows.SystemParameters.PrimaryScreenWidth;
        }
    }


    
}
