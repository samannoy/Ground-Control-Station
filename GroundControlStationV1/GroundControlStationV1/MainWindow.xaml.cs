using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.IO;
using System.IO.Ports;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using LiveCharts.Wpf;


namespace GroundControlStationV1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        private SerialPort? serialPort; // Make serialPort nullable


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

            // COMPort initialization
            InitializeCOMPorts();
        }

        // Code to parse received data and update all the labels with raw or processed data
        // This section invokes the call to fill all the labels, gauges and user control forms such as HUD
        private void SerialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            //if (!isLoggingEnabled)
            //   return; // Skip data processing if logging is disabled
            if (serialPort == null)
                return; // Skip data processing if serialPort is null
            

            string receivedData = serialPort.ReadLine();
            string[] values = receivedData.Split(',');
            

            // Second line in plot 1
            if (values.Length >= 5 && double.TryParse(values[3], out double newValue) && double.TryParse(values[4], out double newValue2) && double.TryParse(values[0], out double newValue3))

            {
                //UpdateLiveChart(newValue, newValue2);

                // Log the data to the CSV file
                //string logData = string.Join(",", values);
                //LogDataToCsv(logFilePath, logData);

                Dispatcher.Invoke(() =>
                {
                    //Display values here
                    lbl_Roll.Content = "Roll Angle : " + values[0];
                    lbl_Pitch.Content = "Pitch Angle : " + values[1];
                    lbl_Heading.Content = "Heading : " + values[2];
                    //lbl_hor_spd.Content = "Horizontal speed : " + values[3];
                    //lbl_ver_spd.Content = "Vertical speed : " + values[4];

                   // gaugeControl.UpdateGaugeValue(newValue);
                    //gaugeControl2.UpdateGaugeRotation(newValue);
                    //gaugeControl2.UpdateOSD(newValue, newValue2, newValue3);
                    //gaugeControl2.UpdateGaugeTranslation(newValue, newValue2);
                    //gaugeControl2.UpdateCompass(newValue);
                    //UpdateLiveChart_set(values[0]);
                    //UpdateLiveChart_actual(values[1]);
                });
            }
        }



        // Initialize the COMPorts functionality
        private void InitializeCOMPorts()
        {
            string[] comPorts = SerialPort.GetPortNames();
            foreach (string portName in comPorts)
            {
                comboBoxCOMPorts.Items.Add(portName);
            }

        }

        // The Serial conect button functionality
        private void btnConnect_Click(object sender, RoutedEventArgs e)
        {
            if (comboBoxCOMPorts.SelectedItem != null)
            {
                string? selectedPort = comboBoxCOMPorts.SelectedItem as string; // Nullable string
                if (selectedPort != null)
                {
                    if (serialPort == null || !serialPort.IsOpen)
                    {
                        serialPort = new SerialPort(selectedPort, 9600);
                        serialPort.DataReceived += SerialPort_DataReceived;

                        try
                        {
                            serialPort.Open();

                            // Show a success message
                            MessageBox.Show($"Connected to {selectedPort} successfully.", "Connection Successful", MessageBoxButton.OK, MessageBoxImage.Information);

                            // Change button text to "Disconnect"
                            btnConnect.Content = "Disconnect";

                            // Disable the dropdown COM port selector
                            comboBoxCOMPorts.IsEnabled = false;
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show($"Error connecting to {selectedPort}: {ex.Message}", "Connection Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                    else
                    {
                        // Close the serial port
                        serialPort.Close();

                        // Show a success message
                        MessageBox.Show($"Disconnected successfully.", "Serial port disconnected", MessageBoxButton.OK, MessageBoxImage.Information);

                        // Change button text back to "Connect"
                        btnConnect.Content = "Connect";

                        // Enable the dropdown COM port selector
                        comboBoxCOMPorts.IsEnabled = true;
                    }
                }
            }
        }

        // On Load functionality of the Window
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            // Use the ChartNames here from MainWindow.xaml
            // Plot for 
            RenderChart0.Children.Clear();
            RenderChart0.Children.Add(new Plot_0());

            // Plot for 
            RenderChart1.Children.Clear();
            RenderChart1.Children.Add(new Plot_1());

            // Plot for 
            RenderChart2.Children.Clear();
            RenderChart2.Children.Add(new Plot_2());

            // Plot for 
            RenderChart3.Children.Clear();
            RenderChart3.Children.Add(new Plot_3());

            // Plot for 
            RenderChart4.Children.Clear();
            RenderChart4.Children.Add(new Plot_4());

            // Plot for 
            RenderChart5.Children.Clear();
            RenderChart5.Children.Add(new Plot_5());
        }

        // Exit button to close the app
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

        private void btnRefresh_Click(object sender, RoutedEventArgs e)
        {
            comboBoxCOMPorts.Items.Clear(); // Clear existing items
            InitializeCOMPorts();
        }
    }
}
