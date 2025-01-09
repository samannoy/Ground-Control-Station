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
using System.Windows.Threading;
using System.Reflection;

namespace GroundControlStationV1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        private SerialPort serialPort; // Make serialPort nullable
        private List<double> dataBuffer_set;
        private List<double> dataBuffer_act;
        private int maxDataPoints = 20; // Maximum number of data points to display
        private DispatcherTimer timer;
        private DateTime startTime;
        private bool isTimerRunning = false;
        private DateTime pauseTime;
        private bool isArmed = false; // Initialize as disarmed

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

            // Buffer to store values to update plots
            dataBuffer_set = new List<double>(maxDataPoints);
            dataBuffer_act = new List<double>(maxDataPoints);

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
                UpdatePlot_0(newValue, newValue2);

                // Log the data to the CSV file
                //string logData = string.Join(",", values);
                //LogDataToCsv(logFilePath, logData);

                Dispatcher.Invoke(() =>
                {
                    //Display values here
                    lbl_Roll.Content = "Roll Angle : " + values[0];
                    lbl_Pitch.Content = "Pitch Angle : " + values[1];
                    lbl_Heading.Content = "Heading : " + values[2];
                    //lbl_GroundSpd.Content = "Ground speed (m/s): " + values[3];
                    //lbl_VerSpd.Content = "Vertical speed (m/s): " + values[4];

                   // gaugeControl.UpdateGaugeValue(newValue);
                    //gaugeControl2.UpdateGaugeRotation(newValue);
                    //HUD_1.UpdateOSD(newValue, newValue2, newValue3);
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
            comboBoxCOMPorts.Text = "-- Select Port -- ";
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
                string selectedPort = comboBoxCOMPorts.SelectedItem as string; // Nullable string
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


        // Function to update plots
        private void UpdatePlot_0(double newValue, double newValue2)
        {
            dataBuffer_set.Add(newValue);
            dataBuffer_act.Add(newValue2);
            // Set value
            if (dataBuffer_set.Count > maxDataPoints)
            {
                dataBuffer_set.RemoveAt(0);
            }
            // Current value
            if (dataBuffer_act.Count > maxDataPoints)
            {
                dataBuffer_act.RemoveAt(0);
            }
            Dispatcher.Invoke(() =>
            {
                // Update the appropriate chart's series data with the dataBuffer
                var liveChart = RenderChart0.Children[0] as Plot_0;
                liveChart.SeriesCollection0[0].Values.Clear();
                var liveChart2 = RenderChart0.Children[0] as Plot_0;
                liveChart2.SeriesCollection0[1].Values.Clear();

                // Set value
                foreach (double value in dataBuffer_set)
                {
                    liveChart.SeriesCollection0[0].Values.Add(value);
                }
                // Current value
                foreach (double value in dataBuffer_act)
                {
                    liveChart2.SeriesCollection0[1].Values.Add(value);
                }
            });
        }

        // Controls Button CLick Event
        private void btnControlsClick(object sender, RoutedEventArgs e)
        {
            
            
                Raw_Data_Panel.Visibility = Visibility.Collapsed;
                Buttons_Panel.Visibility = Visibility.Visible;
            
        }

        // Home Button CLick Event
        private void btnHomeClick(object sender, RoutedEventArgs e)
        {
            
                Raw_Data_Panel.Visibility = Visibility.Visible;
                Buttons_Panel.Visibility = Visibility.Collapsed;
            
        }

        // Mission Timer Button Click Event
        private void btnStartStopTimer_Click(object sender, RoutedEventArgs e)
        {
            if (isTimerRunning)
            {
                // Stop the timer
                timer.Stop();
                isTimerRunning = false;
                
                // Change button text
                btnStartStopTimer.Content = "Start Mission Timer";
            }
            else
            {
                if (timer == null)
                {
                    // If the timer is null, create a new one
                    timer = new DispatcherTimer();
                    timer.Interval = TimeSpan.FromSeconds(1); // Update every second
                    timer.Tick += Timer_Tick;
                }

                // Start the timer
                startTime = isTimerRunning ? startTime.Add(DateTime.Now - pauseTime) : DateTime.Now;
                timer.Start();
                isTimerRunning = true;

                // Change button text
                btnStartStopTimer.Content = "Stop Mission Timer";
            }
        }


        
        // Function to update mission timer
        private void Timer_Tick(object sender, EventArgs e)
        {
            // Calculate the elapsed time
            TimeSpan elapsedTime = DateTime.Now - startTime;

            // Update the label with the elapsed time in hh:mm:ss format
            lbl_MissionDur.Content = "Mission Duration(s): " + elapsedTime.ToString(@"hh\:mm\:ss");
        }


        // Arm Button Click Event
        private void btnArmDisarmClick(object sender, RoutedEventArgs e)
        {
            if (isArmed)
            {
                // Perform actions for disarming here
                // For example, stop motors, change settings, etc.
                HUD_1.OSD_Arming.Content = "DISARMED";

                // Change button content to "Arm"
                btnArmDisarm.Content = "Arm";

                // Update the state to disarmed
                isArmed = false;
            }
            else
            {
                // Perform actions for arming here
                // For example, start motors, change settings, etc.
                HUD_1.OSD_Arming.Content = "ARMED";

                // Change button content to "Disarm"
                btnArmDisarm.Content = "Disarm";

                // Update the state to armed
                isArmed = true;
            }
        }

        private void HUD_1_Loaded()
        {

        }
    }
}
