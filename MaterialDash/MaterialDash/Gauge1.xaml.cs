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

namespace MaterialDash
{
    /// <summary>
    /// Interaction logic for Gauge1.xaml
    /// </summary>
    public partial class Gauge1 : UserControl
    {
        public Gauge1()
        {
            InitializeComponent();
        }
        public void UpdateGaugeValue(double newValue)
        {
            double angle = -90+newValue;
            RotateTransform rotate = new RotateTransform(angle);
            arrow.RenderTransform = rotate;
        }
    }
}
