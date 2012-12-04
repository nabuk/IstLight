// Copyright 2012 Jakub Niemyjski
//
// This file is part of IstLight.
//
// IstLight is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// IstLight is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with IstLight.  If not, see <http://www.gnu.org/licenses/>.

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

using winChart = System.Windows.Forms.DataVisualization.Charting;

namespace IstLight.Controls
{
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
        private winChart.Chart ChartCtrl { get { return (winChart.Chart)this.winHost.Child; } }

        private void SetChartPoints(KeyValuePair<DateTime, double>[] points)
        {
            var chart = ChartCtrl;
            chart.BeginInit();
            chart.BackColor = System.Drawing.Color.FromArgb(0);
            chart.Series.Clear();
            chart.ChartAreas.Clear();

            var area = chart.ChartAreas.Add("DefaultArea");
            var series = chart.Series.Add("DefaultSeries");
            series.ChartArea = area.Name;
            series.ChartType = winChart.SeriesChartType.Area;
            series.Color = System.Drawing.Color.FromArgb(180, 255, 64, 0);
            series.BackSecondaryColor = System.Drawing.Color.FromArgb(180, 210, 96, 0);
            series.BackGradientStyle = winChart.GradientStyle.TopBottom;

            area.AxisX.IsMarginVisible = false;
            area.AxisX.InterlacedColor = System.Drawing.Color.Red;
            area.AxisX.LineWidth = 0;
            area.AxisY.LineWidth = 0;
            
            area.BorderDashStyle = winChart.ChartDashStyle.NotSet;

            var font = new System.Drawing.Font(area.AxisY.LabelStyle.Font.FontFamily, 8);
            area.AxisX.LabelStyle.Font = font;
            area.AxisY.LabelStyle.Font = font;

            area.AxisY.IsLogarithmic = !points.Any(p => p.Value <= 0);
            area.AxisY.LogarithmBase = 2;
            area.AxisY.IsStartedFromZero = false;
            area.AxisY.IntervalType = winChart.DateTimeIntervalType.Number;
            area.BackColor = System.Drawing.Color.FromArgb(0);
            foreach (var p in points)
                series.Points.AddXY(p.Key.ToShortDateString(),p.Value);

            chart.EndInit();
        }
        private static void PointsChangedCallback(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue == null) return;

            var ctrl = (Chart)sender;
            var points = UpTo(((IEnumerable<KeyValuePair<DateTime, double>>)e.NewValue).ToArray(), 2000);

            ctrl.SetChartPoints(points);
        }
        public Chart()
        {
            InitializeComponent();

            winHost.Child = new winChart.Chart();
        }
        
        public static readonly DependencyProperty PointsProperty =
            DependencyProperty.Register("Points", typeof(IEnumerable<KeyValuePair<DateTime,double>>), typeof(Chart), new PropertyMetadata(PointsChangedCallback));
        
        public IEnumerable<KeyValuePair<DateTime,double>> Points
        {
            get
            {
                return (IEnumerable<KeyValuePair<DateTime, double>>)GetValue(PointsProperty);
            }
            set
            {
                SetValue(PointsProperty, value);
            }
        }
    }
}