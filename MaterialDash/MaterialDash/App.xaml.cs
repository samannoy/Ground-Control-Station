using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;

namespace MaterialDash
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);


            // Create an instance of your main window
            MainWindow mainWindow = new MainWindow();

            // Set the application's main window
            Current.MainWindow = mainWindow;

            // Load the application icon
            Uri iconUri = new Uri("J:\\G drive backup\\Samannoy\\IEM\\Startup\\WPF VS\\MaterialDash\\MaterialDash\\Images\\Nexas-Logo.bmp", UriKind.RelativeOrAbsolute);
            mainWindow.Icon = BitmapFrame.Create(iconUri);

            // Show the main window
            mainWindow.Show();
        }

    }
}
