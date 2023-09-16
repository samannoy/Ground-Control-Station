using LiveCharts.Wpf;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Ports;
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

namespace MaterialDash
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private SerialPort serialPort;
        private List<double> dataBuffer_set;
        // Second line in plot 1
        private List<double> dataBuffer_act;
        private int maxDataPoints = 20; // Maximum number of data points to display
        private bool isLoggingEnabled = false; // Flag to control data logging
        private string logFilePath = "J:\\G drive backup\\Samannoy\\IEM\\Startup\\WPF VS\\Logs\\log.txt"; // Path to the CSV log file
        

        public MainWindow()
        {
            InitializeComponent();
            InitializeCOMPorts();
            dataBuffer_set = new List<double>(maxDataPoints);
            // Second line in plot 1
            dataBuffer_act = new List<double>(maxDataPoints);

            
        }

        private void InitializeCOMPorts()
        {
            string[] comPorts = SerialPort.GetPortNames();
            foreach (string portName in comPorts)
            {
                comboBoxCOMPorts.Items.Add(portName);
            }

        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            // Load a web page into the WebBrowser control

            //webBrowserControl.Navigate(new Uri("https://www.google.com"));

            // Use the ChartNames here from MainWindow.xaml
            RenderChart1.Children.Clear();
            RenderChart1.Children.Add(new LiveChart());

            RenderChart2.Children.Clear();
            RenderChart2.Children.Add(new LiveChart2());

            
        }
        private void SerialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            if (!isLoggingEnabled)
                return; // Skip data processing if logging is disabled

            string receivedData = serialPort.ReadLine();
            string[] values = receivedData.Split(',');
            
            // Second line in plot 1
            if (values.Length >= 5 && double.TryParse(values[3], out double newValue) && double.TryParse(values[4], out double newValue2) && double.TryParse(values[0], out double newValue3))

            {
                UpdateLiveChart(newValue,newValue2);

                // Log the data to the CSV file
                string logData = string.Join(",", values);
                LogDataToCsv(logFilePath, logData);

                Dispatcher.Invoke(() =>
                {
                    //Display values here
                    lbl_X_val.Content = "X : " + values[0];
                    lbl_Y_val.Content = "Y : " + values[1];
                    lbl_Z_val.Content = "Z : " + values[2];
                    lbl_hor_spd.Content = "Horizontal speed : " + values[3];
                    lbl_ver_spd.Content = "Vertical speed : " + values[4];

                    gaugeControl.UpdateGaugeValue(newValue);
                    //gaugeControl2.UpdateGaugeRotation(newValue);
                    gaugeControl2.UpdateOSD(newValue,newValue2, newValue3);
                    gaugeControl2.UpdateGaugeTranslation(newValue,newValue2);
                    gaugeControl2.UpdateCompass(newValue);
                    //UpdateLiveChart_set(values[0]);
                    //UpdateLiveChart_actual(values[1]);
                });
            }
        }
        
        private void UpdateLiveChart(double newValue, double newValue2)
        {
            dataBuffer_set.Add(newValue);
            // Second line in plot 1
            dataBuffer_act.Add(newValue2);
            if (dataBuffer_set.Count > maxDataPoints)
            {
                dataBuffer_set.RemoveAt(0);
            }
            // Second line in plot 1
            if (dataBuffer_act.Count > maxDataPoints)
            {
                dataBuffer_act.RemoveAt(0);
            }
            Dispatcher.Invoke(() =>
            {
                // Update the appropriate chart's series data with the dataBuffer
                var liveChart = RenderChart1.Children[0] as LiveChart;
                liveChart.SeriesCollection[0].Values.Clear();
                // Second line in plot 1
                var liveChart2 = RenderChart1.Children[0] as LiveChart;
                liveChart2.SeriesCollection[1].Values.Clear();

                foreach (double value in dataBuffer_set)
                {
                    liveChart.SeriesCollection[0].Values.Add(value);
                }
                // Second line in plot 1
                foreach (double value in dataBuffer_act)
                {
                    liveChart2.SeriesCollection[1].Values.Add(value);
                }
            });
        }



        private void UpdateLiveChart_set(string value)
        {
            // Parse the value to double
            if (double.TryParse(value, out double newValue))
            {
                // Update the appropriate chart's series data
                (RenderChart1.Children[0] as LiveChart).SeriesCollection[0].Values.Add(newValue);
            }
        }


        private void UpdateLiveChart_actual(string value)
        {
            // Parse the value to double
            if (double.TryParse(value, out double newValue))
            {
                // Update the appropriate chart's series data
                (RenderChart2.Children[0] as LiveChart2).SeriesCollection2[1].Values.Add(newValue);
            }
        }



        private void btnConnect_Click(object sender, RoutedEventArgs e)
        {

            if (comboBoxCOMPorts.SelectedItem != null)
            {
                if (serialPort == null || !serialPort.IsOpen)
                {
                    string selectedPort = comboBoxCOMPorts.SelectedItem.ToString();
                    serialPort = new SerialPort(selectedPort, 9600);
                    serialPort.DataReceived += SerialPort_DataReceived;
                    isLoggingEnabled = true; // Enable data logging


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
                    isLoggingEnabled = false; // Disable data logging
                    serialPort.Close();
                    //lblOutput.Text = "Disconnected";

                    // Show a success message
                    MessageBox.Show($"Disconnected successfully.", "Serial port disconnected", MessageBoxButton.OK, MessageBoxImage.Information);

                    // Change button text back to "Connect"
                    btnConnect.Content = "Connect";

                    // Enable the dropdown COM port selector
                    comboBoxCOMPorts.IsEnabled = true;
                }
            }
        }

        private void Toggle_Display(object sender, RoutedEventArgs e)
        {
            if (Hidden_GUI.Visibility == Visibility.Collapsed)
            {
                Hidden_GUI.Visibility = Visibility.Visible;
            }
            else
            {
                Hidden_GUI.Visibility = Visibility.Collapsed;
            }
        }

        private void btn_Arm_Click(object sender, RoutedEventArgs e)
        {

            try
            {
                // Check if the serial port is open
                if (serialPort != null && serialPort.IsOpen)
                {
                    // Send the command "1"
                    serialPort.Write("1");

                    // You can also append a newline character if needed:
                    // serialPort.Write("1\n");

                    // Show a message to indicate that the command has been sent
                    MessageBox.Show("Command '1' sent.", "Command Sent", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    MessageBox.Show("Serial port is not open.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error sending command: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btn_IdleRun_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Check if the serial port is open
                if (serialPort != null && serialPort.IsOpen)
                {
                    // Send the command "1"
                    serialPort.Write("0");

                    // You can also append a newline character if needed:
                    // serialPort.Write("1\n");

                    // Show a message to indicate that the command has been sent
                    MessageBox.Show("Command '2' sent.", "Command Sent", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    MessageBox.Show("Serial port is not open.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error sending command: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void LogDataToCsv(string filePath, string data)
        {
            try
            {
                string timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                string logData = $"{timestamp},{data}"; // Use "\r\n" for newline
                File.AppendAllText(filePath, logData);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error logging data to CSV: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            UploadPopup uploadPopup = new UploadPopup();
            uploadPopup.ShowDialog();
        }
    }
}
