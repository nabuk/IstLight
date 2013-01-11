﻿// Copyright 2012 Jakub Niemyjski
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
            chart.BackColor = System.Drawing.Color.FromArgb(255, 50,50,50);
            chart.Series.Clear();
            chart.ChartAreas.Clear();

            var area = chart.ChartAreas.Add("DefaultArea");
            var series = chart.Series.Add("DefaultSeries");
            series.ChartArea = area.Name;
            series.ChartType = winChart.SeriesChartType.Area;
            series.Color = System.Drawing.Color.FromArgb(180, 17, 177, 232);
            series.BackSecondaryColor = System.Drawing.Color.FromArgb(180, 0, 80, 120);
            series.BackGradientStyle = winChart.GradientStyle.TopBottom;

            area.BackColor = System.Drawing.Color.FromArgb(255, 11, 84, 136);
            area.BackSecondaryColor = System.Drawing.Color.FromArgb(255, 0,0,0);
            area.BackGradientStyle = winChart.GradientStyle.TopBottom;
            area.AxisX.IsMarginVisible = false;
            area.AxisX.InterlacedColor = System.Drawing.Color.Red;
            area.AxisX.LineWidth = 0;
            area.AxisY.LineWidth = 0;
            
            area.BorderDashStyle = winChart.ChartDashStyle.NotSet;

            var font = new System.Drawing.Font(area.AxisY.LabelStyle.Font.FontFamily, 8);
            area.AxisX.LabelStyle.Font = font;
            area.AxisY.LabelStyle.Font = font;
            area.AxisX.LabelStyle.ForeColor = System.Drawing.Color.FromArgb(255, 220, 220, 220);
            area.AxisY.LabelStyle.ForeColor = System.Drawing.Color.FromArgb(255, 220, 220, 220);

            //series.ShadowOffset = -2;
            //series.ShadowColor = System.Drawing.Color.FromArgb(120, 0, 0, 0);

            area.AxisX.MajorGrid.LineColor = System.Drawing.Color.FromArgb(60, 255, 255, 255);
            area.AxisY.MajorGrid.LineColor = System.Drawing.Color.FromArgb(60, 255, 255, 255);
            area.AxisX.MinorGrid.LineColor = System.Drawing.Color.FromArgb(30, 255, 255, 255);
            area.AxisY.MinorGrid.LineColor = System.Drawing.Color.FromArgb(30, 255, 255, 255);
            

            area.AxisY.LabelStyle.Format = "F2";
            //area.AxisX.IntervalType = winChart.DateTimeIntervalType.Days;
            //area.AxisX.MajorGrid.IntervalType = winChart.DateTimeIntervalType.Days;
            //area.AxisX.MinorGrid.IntervalType = winChart.DateTimeIntervalType.Days;
            area.AxisX.MinorGrid.Enabled = true;

            area.AxisX.MajorTickMark.Enabled = false;
            area.AxisY.MajorTickMark.Enabled = false;


            //area.AxisY.IsLogarithmic = points.All(p => p.Value > 0);
            //area.AxisY.LogarithmBase = 2;
            area.AxisY.IsStartedFromZero = false;
            area.AxisY.IntervalType = winChart.DateTimeIntervalType.Number;

            double minY = points.Select(p => p.Value).Min();
            double maxY = points.Select(p => p.Value).Max();
            if (minY != maxY)
            {
                area.AxisY.Interval = Math.Abs((maxY - minY) / 5);
                area.AxisY.Minimum = minY;
                area.AxisY.Maximum = maxY;

                //area.AxisY.MinorGrid.Interval = area.AxisY.Interval / 2;
            }

            //DateTime minX = points[0].Key;
            //DateTime maxX = points[points.Length - 1].Key;
            //area.AxisX.Minimum = minX.ToOADate();
            //area.AxisX.Maximum = maxX.ToOADate();
            //area.AxisX.IntervalType = winChart.DateTimeIntervalType.Days;
            //area.AxisX.Interval = Math.Abs((maxX - minX).TotalDays / 5);
            //area.AxisX.MinorGrid.Interval = area.AxisX.Interval / 10;
            //area.AxisX.LabelStyle.Angle = 45;

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