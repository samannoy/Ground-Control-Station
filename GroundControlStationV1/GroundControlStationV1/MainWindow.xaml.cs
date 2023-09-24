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
using System.Windows.Navigation;
using System.Windows.Shapes;


namespace GroundControlStationV1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            // The following section decides to maximize or keep the screen normal based on screen resolution
            // Get the screen's working area
            double screenWidth = SystemParameters.PrimaryScreenWidth;
            double screenHeight = SystemParameters.PrimaryScreenHeight;

            // Check if the screen resolution is 1366x768 or smaller
            if (screenWidth <= 1366 && screenHeight <= 768)
            {
                // Set WindowState to Maximized
                WindowState = WindowState.Maximized;
            }
            else
            {
                // Set WindowState to Normal
                WindowState = WindowState.Normal;
                WindowStartupLocation=WindowStartupLocation.CenterScreen;
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Do you want to close the application?", "Exit Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                // Close the application
                Application.Current.Shutdown();
            }
            // If the user selects "No" or closes the dialog, the application will not close.
        }

    }
}
