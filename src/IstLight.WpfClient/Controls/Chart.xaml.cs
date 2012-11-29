using System;
using System.Collections.Generic;
using System.Linq;
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

namespace IstLight.Controls
{
    /// <summary>
    /// Interaction logic for Chart.xaml
    /// </summary>
    public partial class Chart : UserControl
    {
        private static T[] UpTo<T>(T[] collection, int maxElements = 0)
        {
            if (maxElements == 1 || maxElements < 0)
                maxElements = 2;

            
            if (maxElements < collection.Length && maxElements != 0)
            {
                double multiplier = (double)(collection.Length - 1) / (double)(maxElements - 1);
                var rows = collection.ToList();
                collection = Enumerable.Range(0, maxElements).Select(i => collection[(int)((double)i * multiplier)]).ToArray();
            }

            return collection;
        }

        private static IEnumerable<double> LogarithmicIfPossible(IEnumerable<double> input)
        {
            double min = input.Min();

            if (min < 0.001)
            {
                return input;
            }
            //can be logarithmic
            else
            {
                //fix for values < 1 (log2 turns them into negative and that is undesired)
                double fix = LogValue(min);

                return input.Select(v => LogValue(v) - fix);
            }

        }
        private static double LogValue(double linearValue, double logBase = 2)
        {
            double signMultiplier = linearValue < 0 ? -1 : 1;
            return Math.Log(Math.Abs(linearValue), logBase) * signMultiplier;
        }

        private LinearGradientBrush areaChartBrush { get { return FindResource("areaBrush") as LinearGradientBrush; } }

        public Chart()
        {
            InitializeComponent();

            areaSeries.TransitionDuration = TimeSpan.FromSeconds(0);
            lineSeries.TransitionDuration = TimeSpan.FromSeconds(0);
        }

        private static void PointsChangedCallback(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue == null) return;

            var ctrl = (Chart)sender;
            var points = UpTo(((IEnumerable<double>)e.NewValue).ToArray(), 300);

            var d2points = LogarithmicIfPossible(points).Select((v, i) => new KeyValuePair<double, double>(i, v)).ToArray();

            ctrl.areaChart.DataContext = d2points;
            ctrl.lineChart.DataContext = d2points;

        }
        public static readonly DependencyProperty PointsProperty =
            DependencyProperty.Register("Points", typeof(IEnumerable<double>), typeof(Chart), new PropertyMetadata(PointsChangedCallback));
        
        public IEnumerable<double> Points
        {
            get
            {
                return (IEnumerable<double>)GetValue(PointsProperty);
            }
            set
            {
                SetValue(PointsProperty, value);
            }
        }

        public Color FirstBackgroundColor
        {
            get
            {
                return areaChartBrush.GradientStops[0].Color;
            }
            set
            {
                areaChartBrush.GradientStops[0].Color = value;
            }
        }
        public Color SecondBackgroundColor
        {
            get
            {
                return areaChartBrush.GradientStops[1].Color;
            }
            set
            {
                areaChartBrush.GradientStops[1].Color = value;
            }
        }
    }
}
