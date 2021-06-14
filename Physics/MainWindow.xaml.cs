using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;
using System;
using System.Collections.Generic;
using System.Windows.Controls;

namespace Physics
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        private TableManager _tableManager;

        double R;
        double U;
        double I;

        public MainWindow()
        {
            InitializeComponent();

            _tableManager = new TableManager(dataGrid);

        }

        private void OnClearTableClicked(object sender, System.Windows.RoutedEventArgs e)
        {
            _tableManager.Clear();
        }

        private void OnFillTableClicked(object sender, System.Windows.RoutedEventArgs e)
        {
            // Must be overwritten
        }

        private void OnSaveClicked(object sender, System.Windows.RoutedEventArgs e)
        {
            _tableManager.AddGreenRow(R, U, I);
        }

        private void OnSliderValueChanged(object sender, System.Windows.RoutedPropertyChangedEventArgs<double> e)
        {
 
            var eds = 18.0;
            var r = 642.0;
            var R = rSlider.Value;
            var I = eds / (R + r) * 1000;
            var U = I * R * 0.001;
            this.R = Math.Round(R, 3);
            this.U = Math.Round(U, 3);
            this.I = Math.Round(I, 3);
        }

        private void OnExtrapolateUClicked(object sender, System.Windows.RoutedEventArgs e)
        {
            // Must be overwritten
        }

        private void OnDrawUClicked(object sender, System.Windows.RoutedEventArgs e)
        {
            var plotModel1 = new PlotModel();
            var linearAxis1 = new LinearAxis();
            linearAxis1.MajorGridlineStyle = LineStyle.Solid;
            linearAxis1.MinorGridlineStyle = LineStyle.Dot;
            plotModel1.Axes.Add(linearAxis1);
            var linearAxis2 = new LinearAxis();
            linearAxis2.MajorGridlineStyle = LineStyle.Solid;
            linearAxis2.MinorGridlineStyle = LineStyle.Dot;
            linearAxis2.Position = AxisPosition.Bottom;
            linearAxis1.AbsoluteMinimum = 0;
            linearAxis2.AbsoluteMinimum = 0;
            linearAxis1.AbsoluteMaximum = 30;
            linearAxis2.AbsoluteMaximum = 30;
            plotModel1.Axes.Add(linearAxis2);

            var scatter = new ScatterSeries();
            for (int i = 0; i < _tableManager.Height; i++)
            {
                var row = _tableManager.GetGreenRow(i);
                var U = row.U;
                var I = row.I;
                scatter.Points.Add(new ScatterPoint(U, I));
            }
            plotModel1.Series.Add(scatter);

            var points = new List<(double,double)>();
            for (int i = 0; i < _tableManager.Height; i++)
            {
                var row = _tableManager.GetGreenRow(i);
                var U = row.U;
                var I = row.I;
                points.Add((U, I));
            }
            var (k, b) = Extrapolation.LinExtrapollation(points);
            var functionSeries = new FunctionSeries((x) =>
            {
                return k * x +b;
            }, 0, 30, 0.01);
            plotModel1.Series.Add(functionSeries);

            plot1.Model = plotModel1;
        }
    }
}