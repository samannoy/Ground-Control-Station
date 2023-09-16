using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using LiveCharts;
using LiveCharts.Wpf;

namespace MaterialDash
{
    /// <summary>
    /// Interaction logic for LiveChart.xaml
    /// </summary>
    public partial class LiveChart2 : UserControl
    {
        public SeriesCollection SeriesCollection2 { get; set; }
        public string[] Labels { get; set; }
        public LiveChart2()
        {
            InitializeComponent();

            SeriesCollection2 = new SeriesCollection
            {
                new LineSeries
                {
                    Title = "Series 1",
                    Values = new ChartValues<double> { 8, 5, 3, 4 ,2 },

                },
                new LineSeries
                {
                    Title = "Series 2",
                    Values = new ChartValues<double> { 5, 2, 7, 6 ,4 },
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

            Labels = new[] { "Jan", "Feb", "Mar", "Apr", "May" };

            DataContext = this;
        }
    }
}
