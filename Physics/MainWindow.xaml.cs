using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;
using System;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Media.Media3D;
using HelixToolkit.Wpf;

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
        double EDS;
        double Ikz;
        double r;
        public MainWindow()
        {
            InitializeComponent();

            sunLight.Transform = new Transform3DGroup
            {
                Children = new Transform3DCollection
                {
                    new TranslateTransform3D(0, 0, 10),
                    new RotateTransform3D(new AxisAngleRotation3D(new Vector3D(1, 0, 0), -90))
                }
            };
            //sunLight.Ambient += 100000;
            _tableManager = new TableManager(dataGrid);
            
            ModelImporter importer = new ModelImporter();
            Model3D table = importer.Load(@"Resources/table/table.obj");
            Model3D res = importer.Load(@"Resources/res/res.obj");
            Model3D voltmeter = importer.Load(@"Resources/volt/volt.obj");
            var tg = new Transform3DGroup();
            tg.Children.Add(new TranslateTransform3D(new Vector3D(0, 0, 5)));
            tg.Children.Add(new RotateTransform3D(new AxisAngleRotation3D(new Vector3D(1, 0, 0), -90)));
            
            var tg2 = new Transform3DGroup();
            tg2.Children.Add(new TranslateTransform3D(new Vector3D(20, 0, 5)));
            tg2.Children.Add(new RotateTransform3D(new AxisAngleRotation3D(new Vector3D(1, 0, 0), -90)));
            table.Transform = new ScaleTransform3D(1.4, 1.4, 1.4);
            res.Transform = tg;

            voltmeter.Transform =tg2;
            modelVisual.Children.Add(new ModelVisual3D
            {
                Content = table
            });
            modelVisual.Children.Add(new ModelVisual3D
            {
                Content = res
            });
            modelVisual.Children.Add(new ModelVisual3D
            {
                Content = voltmeter
            });
        }

        private void OnClearTableClicked(object sender, System.Windows.RoutedEventArgs e)
        {
            if (_tableManager.Uextrapolated) return;
            _tableManager.Clear();
        }

        private void OnFillTableClicked(object sender, System.Windows.RoutedEventArgs e)
        {
            if(!_tableManager.Uextrapolated)return;
            _tableManager.YrowsFilled=true;
             for (int i = 0; i < _tableManager.Height; i++)
            {
                var row = _tableManager.GetGreenRow(i);
                var p1 = Math.Round(row.I * row.U, 3);
                r = Math.Round(EDS / Ikz, 3);
                var p2 = Math.Round(row.I * row.I * r, 3);
                var p = Math.Round(p1 + p2, 3);
                var nu = Math.Round(p1 / p, 2) * 100;
                _tableManager.AddYellow(p,p1,p2,nu);
               
            }
        }

        private void OnSaveClicked(object sender, System.Windows.RoutedEventArgs e)
        {
            if (_tableManager.Uextrapolated) return;
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

            //   plot1.Model.Series.Add(functionSeries);
        }

        private void OnDrawUClicked(object sender, System.Windows.RoutedEventArgs e)
        {
            if (!_tableManager.measurementsdone) return;
            _tableManager.Uextrapolated = true;
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




            var points = new List<(double, double)>();
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
                return k * x + b;
            }, 0, 30, 0.01);
            plotModel1.Series.Add(functionSeries);
            IkzLabel.Content = Math.Round(b, 3);
            EDSLabel.Content = Math.Round(-b / k, 3);
            Ikz = Math.Round(b, 3);
            EDS = Math.Round(-b / k, 3);
            plot1.Model = plotModel1;

        }

        private void OnDrawP1Clicked(object sender, System.Windows.RoutedEventArgs e)
        {
            if (!_tableManager.YrowsFilled) return;
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
            linearAxis1.AbsoluteMaximum = 150;//ось y
            linearAxis2.AbsoluteMaximum = 30;//ось х
            plotModel1.Axes.Add(linearAxis2);


            var scatter = new ScatterSeries();
            for (int i = 0; i < _tableManager.Height; i++)
            {
                var grow = _tableManager.GetGreenRow(i);
                var yrow = _tableManager.GetYellowRow(i);
                var P1 = yrow.P1;
                var I = grow.I;
                scatter.Points.Add(new ScatterPoint(I, P1));
            }
            plotModel1.Series.Add(scatter);




            var points = new List<(double, double)>();
            for (int i = 0; i < _tableManager.Height; i++)
            {
                var row = _tableManager.GetGreenRow(i);
                var yrow = _tableManager.GetYellowRow(i);
                var P1 = yrow.P1;
                var I = row.I;
                points.Add((I, P1));
            }
            var (a, b) = Extrapolation.QuadExtrapollation(points);
            var functionSeries = new FunctionSeries((x) =>
            {
                return a * x * x + b * x;
            }, 0, 30, 0.01);
            plotModel1.Series.Add(functionSeries);

            plot2.Model = plotModel1;

        }
        private void OnDrawP2Clicked(object sender, System.Windows.RoutedEventArgs e)
        {
            if (!_tableManager.YrowsFilled) return;
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
            linearAxis1.AbsoluteMaximum = 400;//ось y
            linearAxis2.AbsoluteMaximum = 30;//ось х
            plotModel1.Axes.Add(linearAxis2);


            var scatter = new ScatterSeries();
            for (int i = 0; i < _tableManager.Height; i++)
            {
                var grow = _tableManager.GetGreenRow(i);
                var yrow = _tableManager.GetYellowRow(i);
                var P2 = yrow.P2;
                var I = grow.I;
                scatter.Points.Add(new ScatterPoint(I, P2));
            }
            plotModel1.Series.Add(scatter);




            var points = new List<(double, double)>();
            for (int i = 0; i < _tableManager.Height; i++)
            {
                var row = _tableManager.GetGreenRow(i);
                var yrow = _tableManager.GetYellowRow(i);
                var P2 = yrow.P2;
                var I = row.I;
                points.Add((I, P2));
            }
            var (a, b) = Extrapolation.QuadExtrapollation(points);
            var functionSeries = new FunctionSeries((x) =>
            {
                return a * x * x + b * x;
            }, 0, 30, 0.01);
            plotModel1.Series.Add(functionSeries);

            plot3.Model = plotModel1;

        }
        private void OnDrawPClicked(object sender, System.Windows.RoutedEventArgs e)
        {
            if (!_tableManager.YrowsFilled) return;
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
            linearAxis1.AbsoluteMaximum = 600;//ось y
            linearAxis2.AbsoluteMaximum = 30;//ось х
            plotModel1.Axes.Add(linearAxis2);


            var scatter = new ScatterSeries();
            for (int i = 0; i < _tableManager.Height; i++)
            {
                var grow = _tableManager.GetGreenRow(i);
                var yrow = _tableManager.GetYellowRow(i);
                var P = yrow.P;
                var I = grow.I;
                scatter.Points.Add(new ScatterPoint(I, P));
            }
            plotModel1.Series.Add(scatter);




            var points = new List<(double, double)>();
            for (int i = 0; i < _tableManager.Height; i++)
            {
                var row = _tableManager.GetGreenRow(i);
                var yrow = _tableManager.GetYellowRow(i);
                var P = yrow.P;
                var I = row.I;
                points.Add((I, P));
            }
            var k = Extrapolation.Lin0Extrapollation(points);
            var functionSeries = new FunctionSeries((x) =>
            {
                return k * x;
            }, 0, 30, 0.01);
            plotModel1.Series.Add(functionSeries);

            plot4.Model = plotModel1;

        }
        private void OnDrawNuClicked(object sender, System.Windows.RoutedEventArgs e)
        {
            if (!_tableManager.YrowsFilled) return;
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
            linearAxis1.AbsoluteMaximum = 100;//ось y
            linearAxis2.AbsoluteMaximum = 30;//ось х
            plotModel1.Axes.Add(linearAxis2);


            var scatter = new ScatterSeries();
            for (int i = 0; i < _tableManager.Height; i++)
            {
                var grow = _tableManager.GetGreenRow(i);
                var yrow = _tableManager.GetYellowRow(i);
                var Nu = yrow.Nu;
                var I = grow.I;
                scatter.Points.Add(new ScatterPoint(I, Nu));
            }
            plotModel1.Series.Add(scatter);




            var points = new List<(double, double)>();
            for (int i = 0; i < _tableManager.Height; i++)
            {
                var row = _tableManager.GetGreenRow(i);
                var yrow = _tableManager.GetYellowRow(i);
                var Nu = yrow.Nu;
                var I = row.I;
                points.Add((I, Nu));
            }
            var (k,b )= Extrapolation.LinExtrapollation(points);
            var functionSeries = new FunctionSeries((x) =>
            {
                return k * x+b;
            }, 0, 30, 0.01);
            plotModel1.Series.Add(functionSeries);

            plot5.Model = plotModel1;

        }
    }
}