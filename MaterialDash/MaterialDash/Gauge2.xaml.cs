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
using System.Windows.Media.Media3D;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MaterialDash
{
    /// <summary>
    /// Interaction logic for Gauge1.xaml
    /// </summary>
    public partial class Gauge2 : UserControl
    {
        public Gauge2()
        {
            InitializeComponent();
        }
        public void UpdateGaugeRotation(double newValue)
        {
            double angle = newValue;
            RotateTransform rotate = new RotateTransform(angle);
            arrow.RenderTransform = rotate;
            pitch_indicator.RenderTransform = rotate;
        }
        public void UpdateOSD(double newValue, double newValue2, double newValue3)
        {
            double roll = newValue;
            double pitch = newValue2;
            double battery = newValue3;
            OSD_roll.Content = "Roll : "+ roll+"°";
            OSD_pitch.Content = "Pitch : " + pitch + "°";
            OSD_batt_indicator.Width = batteryPercentageIndicatorCalculator(battery);
            OSD_batt_level.Content = battery+"%";
        }
        public void UpdateCompass(double newValue)
        {
            double heading = newValue;
            RotateTransform rotate = new RotateTransform(heading);
            OSD_Compass.RenderTransform = rotate;
        }
        public double batteryPercentageIndicatorCalculator(double battery)
        {
            double width = battery*0.4;
            return width;
        }
        public void UpdateGaugeTranslation(double newValue, double newValue2)
        {
            double translationX = 0; // Adjust this value as needed
            double translationY = 0+newValue2*6; // You can adjust the Y translation if needed

            RotateTransform rotate = new RotateTransform(newValue);
            TranslateTransform translate = new TranslateTransform(translationX, translationY);

            // Create a TransformGroup to combine rotation and translation
            TransformGroup transformGroup = new TransformGroup();
            transformGroup.Children.Add(rotate);
            transformGroup.Children.Add(translate);

            // Apply the combined transform to the arrow element
            arrow.RenderTransform = transformGroup;
            pitch_indicator.RenderTransform = transformGroup;
        }
    }
}
