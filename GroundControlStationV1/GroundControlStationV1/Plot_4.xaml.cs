using LiveCharts.Wpf;
using LiveCharts;
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
    /// Interaction logic for Plot_4.xaml
    /// </summary>
    public partial class Plot_4 : UserControl
    {

        public SeriesCollection SeriesCollection4 { get; set; }
        public string[] Labels { get; set; }

        public Plot_4()
        {
            InitializeComponent();

            SeriesCollection4 = new SeriesCollection
            {
                new LineSeries
                {
                    Title = "Set",
                    Values = new ChartValues<double> { 4, 6, 5, 2 ,4, 4, 6, 5, 2 ,4,4, 6, 5, 2 ,4, 4, 6, 5, 2 ,4 },
                    PointGeometry = null
                },
                new LineSeries
                {
                    Title = "Current",
                    Values = new ChartValues<double> { 6, 7, 3, 4 ,6, 11, 15, 13, 7, 6, 6, 7, 3, 4, 6, 11, 15, 13, 7, 6},
                    PointGeometry = null
                },

            };

            Labels = new[] { "Label 1", "Label 2", "Label 3", "Label 4", "Label 5" };

            DataContext = this;
        }
    }
}
