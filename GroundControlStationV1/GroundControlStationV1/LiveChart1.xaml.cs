using LiveCharts;
using LiveCharts.Wpf;
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
    /// Interaction logic for LiveChart1.xaml
    /// </summary>
    public partial class LiveChart1 : UserControl
    {
        public SeriesCollection SeriesCollection { get; set; }
        public string[]? Labels { get; set; }

        public LiveChart1()
        {
            InitializeComponent();

            SeriesCollection = new SeriesCollection
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
               // new LineSeries
               // {
                //    Title = "Series 3",
                //    Values = new ChartValues<double> { 4,2,7,2,7 },
               //     PointGeometry = DefaultGeometries.Square,
               //     PointGeometrySize = 15
               // }
            };

            //Labels = new[] { "Jan", "Feb", "Mar", "Apr", "May" };

            DataContext = this;
        }
    }
}
