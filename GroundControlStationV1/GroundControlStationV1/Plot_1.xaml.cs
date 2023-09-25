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
    /// Interaction logic for Plot_1.xaml
    /// </summary>
    public partial class Plot_1 : UserControl
    {

        public SeriesCollection SeriesCollection1 { get; set; }
        public string[]? Labels { get; set; }

        public Plot_1()
        {
            InitializeComponent();

            SeriesCollection1 = new SeriesCollection
            {
                new LineSeries
                {
                    Title = "Series 1",
                    Values = new ChartValues<double> { 4, 6, 5, 2 ,4 },
                    PointGeometry = null
                },
                new LineSeries
                {
                    Title = "Series 2",
                    Values = new ChartValues<double> { 6, 7, 3, 4 ,6 },
                    PointGeometry = null
                },

            };

            //Labels = new[] { "Jan", "Feb", "Mar", "Apr", "May" };

            DataContext = this;
        }
    }
}
