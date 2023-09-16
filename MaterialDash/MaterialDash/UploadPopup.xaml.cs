using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace MaterialDash
{
    /// <summary>
    /// Interaction logic for UploadPopup.xaml
    /// </summary>
    public partial class UploadPopup : Window
    {
        public UploadPopup()
        {
            InitializeComponent();
        }
        private void UploadButton_Click(object sender, RoutedEventArgs e)
        {
            // Get the user input from the TextBox
            string userInput = InputTextBox.Text;

            try
            {
                // Create a process to run the command
                System.Diagnostics.Process process = new System.Diagnostics.Process();
                System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo
                {
                    FileName = "cmd.exe",
                    RedirectStandardInput = true,
                    UseShellExecute = false,
                    CreateNoWindow = false
                };

                process.StartInfo = startInfo;
                process.Start();
                Thread.Sleep(2000);

                // Write the user input to the command prompt
                process.StandardInput.WriteLine(userInput);

                // Add a delay of 2 seconds (2000 milliseconds)
                Thread.Sleep(5000);

                // Close the command prompt
                process.StandardInput.WriteLine("exit");
                process.WaitForExit();
                process.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message);
            }

            // Close the pop-up window
            this.Close();
        }
        private void Cancel_Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

    }
}
