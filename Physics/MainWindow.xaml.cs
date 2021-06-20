using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;
using System;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Media.Media3D;
using HelixToolkit.Wpf;
using System.Windows.Media;

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
        TextVisual3D resText = new TextVisual3D();
        TextVisual3D voltText = new TextVisual3D();
        TextVisual3D amperText = new TextVisual3D();
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
     
            _tableManager = new TableManager(dataGrid);
            InitializePlotModels();
            
            ModelImporter importer = new ModelImporter();
            Model3D table = importer.Load(@"Resources/tt2/table.obj");
            Model3D res = importer.Load(@"Resources/r/resistor.obj");
            Model3D voltmeter = importer.Load(@"Resources/volt/volt.obj");
            Model3D ampermeter = importer.Load(@"Resources/amper/amper.obj");
            Model3D generator = importer.Load(@"Resources/ge/gen.obj");
            var resTg = new Transform3DGroup();
            resTg.Children.Add(new TranslateTransform3D(new Vector3D(0, 10, 4)));
            resTg.Children.Add(new RotateTransform3D(new AxisAngleRotation3D(new Vector3D(1, 0, 0), -90)));

            var voltTg = new Transform3DGroup();
            voltTg.Children.Add(new TranslateTransform3D(new Vector3D(0, -10, 4)));
            voltTg.Children.Add(new RotateTransform3D(new AxisAngleRotation3D(new Vector3D(1, 0, 0), -90)));

            var amperTg = new Transform3DGroup();
            amperTg.Children.Add(new TranslateTransform3D(new Vector3D(25, -10, 4)));
            amperTg.Children.Add(new RotateTransform3D(new AxisAngleRotation3D(new Vector3D(1, 0, 0), -90)));

            var genTg = new Transform3DGroup();
            genTg.Children.Add(new TranslateTransform3D(new Vector3D(10, -10, 4)));
            genTg.Children.Add(new RotateTransform3D(new AxisAngleRotation3D(new Vector3D(1, 0, 0), -90)));
            genTg.Children.Add(new RotateTransform3D(new AxisAngleRotation3D(new Vector3D(0, 1, 0), 180)));

            table.Transform = new ScaleTransform3D(1.1, 1.1, 1.1);


            res.Transform = resTg;
            ampermeter.Transform = amperTg;

            voltmeter.Transform = voltTg;
            generator.Transform = genTg;

            
            resText.Text = "100";
            var resTextTg = new Transform3DGroup();
            var col = Color.FromRgb(85, 213, 136);
            var br = new SolidColorBrush(col);
            resText.Background = br;
            resText.BorderBrush = br;
         
            resTextTg.Children.Add(new RotateTransform3D(new AxisAngleRotation3D(new Vector3D(0, 1, 0), 180)));
            resTextTg.Children.Add(new TranslateTransform3D(new Vector3D(0, 19.28, -60)));
            resTextTg.Children.Add(new ScaleTransform3D(new Vector3D(0.3, 0.3, 0.3)));
            resText.FontFamily = new FontFamily(new Uri("pack://application:,,,/"), "Resources/fonts/#digital-7, dig");
            resText.Transform = resTextTg;


            var voltTextTg = new Transform3DGroup();
            voltTextTg.Children.Add(new RotateTransform3D(new AxisAngleRotation3D(new Vector3D(1, 0, 0), 180)));
            voltTextTg.Children.Add(new TranslateTransform3D(new Vector3D(0, 19.28, 7)));
            voltTextTg.Children.Add(new ScaleTransform3D(new Vector3D(0.3, 0.3, 0.3)));
            voltText.Transform = voltTextTg;
            voltText.Text = "2,43";
            voltText.FontFamily = new FontFamily(new Uri("pack://application:,,,/"), "Resources/fonts/#digital-7, dig");
            voltText.Background = br;
            voltText.BorderBrush = br;
            var amperTextTg = new Transform3DGroup();
            amperTextTg.Children.Add(new RotateTransform3D(new AxisAngleRotation3D(new Vector3D(1, 0, 0), 180)));
            amperTextTg.Children.Add(new TranslateTransform3D(new Vector3D(83, 19.28, 7)));
            amperTextTg.Children.Add(new ScaleTransform3D(new Vector3D(0.3, 0.3, 0.3)));
            amperText.Transform = amperTextTg;
            amperText.Text = "24,26";
            amperText.FontFamily = new FontFamily(new Uri("pack://application:,,,/"), "Resources/fonts/#digital-7, dig");
            amperText.Background = br;
            amperText.BorderBrush = br;
            modelVisual.Children.Add(resText);
            modelVisual.Children.Add(voltText);
            modelVisual.Children.Add(amperText);

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
            modelVisual.Children.Add(new ModelVisual3D
            {
                Content = ampermeter
            });
            modelVisual.Children.Add(new ModelVisual3D
            {
                Content = generator
            });

            var fromPlusVolt = new TubeVisual3D();
            fromPlusVolt.Fill = new SolidColorBrush(Colors.IndianRed);
            fromPlusVolt.Diameter += 0.2;
            fromPlusVolt.Path = new Point3DCollection();
            fromPlusVolt.Path.Add(new Point3D(-3.9, 6.01, 6.95));
            fromPlusVolt.Path.Add(new Point3D(-3.9, 6.55, 6.95));
            fromPlusVolt.Path.Add(new Point3D(-5.9, 6.55, 6.95));
            fromPlusVolt.Path.Add(new Point3D(-6.3, 5.55, 6.95));
            fromPlusVolt.Path.Add(new Point3D(-6.6, 4.55, 6.95));
            fromPlusVolt.Path.Add(new Point3D(-7.6, 4.55, 6.95));
            fromPlusVolt.Path.Add(new Point3D(-12.6, 4.55, 6.95));
            fromPlusVolt.Path.Add(new Point3D(-13.6, 4.55, 5.95));
            fromPlusVolt.Path.Add(new Point3D(-13.6, 4.55, -13));


            var fromMinusRes = new TubeVisual3D();
            fromMinusRes.Fill = new SolidColorBrush(Colors.IndianRed);
            fromMinusRes.Diameter += 0.2;
            fromMinusRes.Path = new Point3DCollection();
            fromMinusRes.Path.Add(new Point3D(-4.9, 6.01, -13));
            fromMinusRes.Path.Add(new Point3D(-4.9, 6.55, -13));
            fromMinusRes.Path.Add(new Point3D(-6.9, 6.55, -13));
            fromMinusRes.Path.Add(new Point3D(-7.3, 5.55, -13));
            fromMinusRes.Path.Add(new Point3D(-7.3, 5.0, -13));
            fromMinusRes.Path.Add(new Point3D(-7.3, 4.55, -13));
            fromMinusRes.Path.Add(new Point3D(-8.3, 4.55, -13));
            fromMinusRes.Path.Add(new Point3D(-22.0, 4.55, -13));
            fromMinusRes.Path.Add(new Point3D(-22.3, 4.55, -13));
            fromMinusRes.Path.Add(new Point3D(-22.7, 4.55, -12.5));
            fromMinusRes.Path.Add(new Point3D(-22.7, 4.55, -11));
            fromMinusRes.Path.Add(new Point3D(-22.7, 6, -10));
            fromMinusRes.Path.Add(new Point3D(-22.7, 6, -9));
            fromMinusRes.Path.Add(new Point3D(-22.7, 5.5, -8.1));
            fromMinusRes.Path.Add(new Point3D(-22.7, 4.5, -8.0));

            var fromMinusGen = new TubeVisual3D();
            fromMinusGen.Fill = new SolidColorBrush(Colors.IndianRed);
            fromMinusGen.Diameter += 0.2;
            fromMinusGen.Path = new Point3DCollection();
            fromMinusGen.Path.Add(new Point3D(-22.7, 4.5, -1.1));
            fromMinusGen.Path.Add(new Point3D(-22.7, 5.5, -1.1));
            fromMinusGen.Path.Add(new Point3D(-22.7, 6.5, 0));
            fromMinusGen.Path.Add(new Point3D(-22.7, 6.5, 0.4));
            fromMinusGen.Path.Add(new Point3D(-22.7, 6.0, 1));
            fromMinusGen.Path.Add(new Point3D(-22.7, 5.0, 1.5));
            fromMinusGen.Path.Add(new Point3D(-22.7, 4.5, 1.7));
            fromMinusGen.Path.Add(new Point3D(-22.7, 4.5, 20));
            fromMinusGen.Path.Add(new Point3D(-21.7, 4.5, 21));
            fromMinusGen.Path.Add(new Point3D(-20.7, 4.5, 21.5));
            fromMinusGen.Path.Add(new Point3D(16, 4.5, 21.5));
            fromMinusGen.Path.Add(new Point3D(17, 4.5, 20.5));
            fromMinusGen.Path.Add(new Point3D(17.5, 4.5, 18.0));
            fromMinusGen.Path.Add(new Point3D(17.5, 4.5, 8.0));
            fromMinusGen.Path.Add(new Point3D(18.0, 4.6, 7.0));
            fromMinusGen.Path.Add(new Point3D(18.5, 4.9, 7.0));
            fromMinusGen.Path.Add(new Point3D(19.0, 5.4, 7.0));
            fromMinusGen.Path.Add(new Point3D(19.2, 6.5, 7.0));
            fromMinusGen.Path.Add(new Point3D(20.5, 6.5, 7.0));
            fromMinusGen.Path.Add(new Point3D(21, 6.0, 7.0));
            fromMinusGen.Path.Add(new Point3D(21.3, 4.6, 7.0));


            var fromPlusAmper = new TubeVisual3D();
            fromPlusAmper.Fill = new SolidColorBrush(Colors.IndianRed);
            fromPlusAmper.Diameter += 0.2;
            fromPlusAmper.Path = new Point3DCollection();

            fromPlusAmper.Path.Add(new Point3D(28.1, 4.6, 7.0));
            fromPlusAmper.Path.Add(new Point3D(28.1, 6.3, 7.0));
            fromPlusAmper.Path.Add(new Point3D(29.1, 6.6, 7.0));
            fromPlusAmper.Path.Add(new Point3D(30.1, 6.7, 7.0));
            fromPlusAmper.Path.Add(new Point3D(30.4, 6.3, 7.0));
            fromPlusAmper.Path.Add(new Point3D(30.6, 6.0, 7.0));
            fromPlusAmper.Path.Add(new Point3D(30.8, 5, 7.0));
            fromPlusAmper.Path.Add(new Point3D(31.9, 4.5, 7.0));
            fromPlusAmper.Path.Add(new Point3D(32.9, 4.5, 7.0));
            fromPlusAmper.Path.Add(new Point3D(33.5, 4.5, 6.5));
            fromPlusAmper.Path.Add(new Point3D(33.7, 4.5, 6.2));
            fromPlusAmper.Path.Add(new Point3D(33.5, 4.5, -11));
            fromPlusAmper.Path.Add(new Point3D(33.2, 4.5, -11.5));
            fromPlusAmper.Path.Add(new Point3D(32.0, 4.5, -13));
            fromPlusAmper.Path.Add(new Point3D(7.0, 4.5, -13));
            fromPlusAmper.Path.Add(new Point3D(6.8, 4.9, -13));
            fromPlusAmper.Path.Add(new Point3D(6.4, 5.5, -13));
            fromPlusAmper.Path.Add(new Point3D(6.2, 5.8, -13));
            fromPlusAmper.Path.Add(new Point3D(5.9, 6.3, -13));
            fromPlusAmper.Path.Add(new Point3D(5.9, 6.5, -13));
            fromPlusAmper.Path.Add(new Point3D(5.3, 6.5, -13));
            fromPlusAmper.Path.Add(new Point3D(5.0, 6.5, -13));
            fromPlusAmper.Path.Add(new Point3D(4.6, 6.3, -13));
            fromPlusAmper.Path.Add(new Point3D(4.0, 5.8, -13));
            fromPlusAmper.Path.Add(new Point3D(3.9, 4.6, -13));


            var fromMinVol = new TubeVisual3D();
            fromMinVol.Fill = new SolidColorBrush(Colors.IndianRed);
            fromMinVol.Diameter += 0.2;
            fromMinVol.Path = new Point3DCollection();
            fromMinVol.Path.Add(new Point3D(2.9, 4.51, 6.95));
            fromMinVol.Path.Add(new Point3D(2.9, 6.01, 6.95));
            fromMinVol.Path.Add(new Point3D(3.5, 6.51, 6.95));
            fromMinVol.Path.Add(new Point3D(3.8, 6.51, 6.95));
            fromMinVol.Path.Add(new Point3D(4.3, 6.61, 6.95));
            fromMinVol.Path.Add(new Point3D(4.5, 6.71, 6.95));
            fromMinVol.Path.Add(new Point3D(5, 6.5, 6.95));
            fromMinVol.Path.Add(new Point3D(5.5, 6.0, 6.95));
            fromMinVol.Path.Add(new Point3D(6.0, 5.4, 6.95));
            fromMinVol.Path.Add(new Point3D(6.5, 5.0, 6.95));
            fromMinVol.Path.Add(new Point3D(7.0, 4.6, 6.95));
            fromMinVol.Path.Add(new Point3D(8.0, 4.6, 6.95));
            fromMinVol.Path.Add(new Point3D(8.3, 4.6, 6.7));
            fromMinVol.Path.Add(new Point3D(8.6, 4.6, 6.4));
            fromMinVol.Path.Add(new Point3D(9, 4.6, 6.0));
            fromMinVol.Path.Add(new Point3D(9.4, 4.6, 5.6));
            fromMinVol.Path.Add(new Point3D(9.8, 4.6, 5.2));
            fromMinVol.Path.Add(new Point3D(10.2, 4.6, 4.8));
            fromMinVol.Path.Add(new Point3D(10.2, 4.6, -13));

            var fromPlusVolt2 = new TubeVisual3D();
            fromPlusVolt2.Fill = new SolidColorBrush(Colors.IndianRed);
            fromPlusVolt2.Diameter += 0.2;
            fromPlusVolt2.Path = new Point3DCollection();
          
            fromPlusVolt2.Path.Add(new Point3D(-13.6, 4.55, -5));
            fromPlusVolt2.Path.Add(new Point3D(-1.3, 4.55, -5));
            fromPlusVolt2.Path.Add(new Point3D(-0.9, 4.55, -5.3));
            fromPlusVolt2.Path.Add(new Point3D(-0.7, 4.55, -5.6));
            //fromPlusVolt2.Path.Add(new Point3D(-0.4, 4.55, -5.9));
            fromPlusVolt2.Path.Add(new Point3D(-0.4, 4.55, -6.3));
            fromPlusVolt2.Path.Add(new Point3D(-0.4, 4.55, -12.3));


            modelVisual.Children.Add(fromPlusVolt);
            modelVisual.Children.Add(fromMinusRes);
            modelVisual.Children.Add(fromMinusGen);
            modelVisual.Children.Add(fromPlusAmper);
            modelVisual.Children.Add(fromMinVol);
            modelVisual.Children.Add(fromPlusVolt2);
        }

        private void InitializePlotModels()
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
            plot1.Model = plotModel1;

            var plotModel2 = new PlotModel();
            var linearAxis12 = new LinearAxis();
            linearAxis12.MajorGridlineStyle = LineStyle.Solid;
            linearAxis12.MinorGridlineStyle = LineStyle.Dot;
            plotModel2.Axes.Add(linearAxis12);
            var linearAxis22 = new LinearAxis();
            linearAxis22.MajorGridlineStyle = LineStyle.Solid;
            linearAxis22.MinorGridlineStyle = LineStyle.Dot;
            linearAxis22.Position = AxisPosition.Bottom;
            linearAxis12.AbsoluteMinimum = 0;
            linearAxis22.AbsoluteMinimum = 0;
            linearAxis12.AbsoluteMaximum = 150;//ось y
            linearAxis22.AbsoluteMaximum = 30;//ось х
            plotModel2.Axes.Add(linearAxis22);
            plot2.Model = plotModel2;

            var plotModel3 = new PlotModel();
            var linearAxis13 = new LinearAxis();
            linearAxis13.MajorGridlineStyle = LineStyle.Solid;
            linearAxis13.MinorGridlineStyle = LineStyle.Dot;
            plotModel3.Axes.Add(linearAxis13);
            var linearAxis23 = new LinearAxis();
            linearAxis23.MajorGridlineStyle = LineStyle.Solid;
            linearAxis23.MinorGridlineStyle = LineStyle.Dot;
            linearAxis23.Position = AxisPosition.Bottom;
            linearAxis13.AbsoluteMinimum = 0;
            linearAxis23.AbsoluteMinimum = 0;
            linearAxis13.AbsoluteMaximum = 400;//ось y
            linearAxis23.AbsoluteMaximum = 30;//ось х
            plotModel3.Axes.Add(linearAxis23);
            plot3.Model = plotModel3;

            var plotModel4 = new PlotModel();
            var linearAxis14 = new LinearAxis();
            linearAxis14.MajorGridlineStyle = LineStyle.Solid;
            linearAxis14.MinorGridlineStyle = LineStyle.Dot;
            plotModel4.Axes.Add(linearAxis14);
            var linearAxis24 = new LinearAxis();
            linearAxis24.MajorGridlineStyle = LineStyle.Solid;
            linearAxis24.MinorGridlineStyle = LineStyle.Dot;
            linearAxis24.Position = AxisPosition.Bottom;
            linearAxis14.AbsoluteMinimum = 0;
            linearAxis24.AbsoluteMinimum = 0;
            linearAxis14.AbsoluteMaximum = 600;//ось y
            linearAxis24.AbsoluteMaximum = 30;//ось х
            plotModel4.Axes.Add(linearAxis24);
            plot4.Model = plotModel4;

            var plotModel5 = new PlotModel();
            var linearAxis15 = new LinearAxis();
            linearAxis15.MajorGridlineStyle = LineStyle.Solid;
            linearAxis15.MinorGridlineStyle = LineStyle.Dot;
            plotModel5.Axes.Add(linearAxis15);
            var linearAxis25 = new LinearAxis();
            linearAxis25.MajorGridlineStyle = LineStyle.Solid;
            linearAxis25.MinorGridlineStyle = LineStyle.Dot;
            linearAxis25.Position = AxisPosition.Bottom;
            linearAxis15.AbsoluteMinimum = 0;
            linearAxis25.AbsoluteMinimum = 0;
            linearAxis15.AbsoluteMaximum = 100;//ось y
            linearAxis25.AbsoluteMaximum = 30;//ось х
            plotModel5.Axes.Add(linearAxis25);
            plot5.Model = plotModel5;
        }

        private void OnClearTableClicked(object sender, System.Windows.RoutedEventArgs e)
        {
            if (_tableManager.Uextrapolated) return;
            _tableManager.Clear();
            DrawUButton.IsEnabled = false;
        }

        private void OnFillTableClicked(object sender, System.Windows.RoutedEventArgs e)
        {
            if (!_tableManager.Uextrapolated) return;
            _tableManager.YrowsFilled = true;
            DrawPButton.IsEnabled = true;
            DrawP2Button.IsEnabled = true;
            DrawP1Button.IsEnabled = true;
            DrawNuButton.IsEnabled = true;
            FillButton.IsEnabled = false;
            for (int i = 0; i < _tableManager.Height; i++)
            {
                var row = _tableManager.GetGreenRow(i);
                var p1 = Math.Round(row.I * row.U, 2);
                r = Math.Round(EDS / Ikz, 2);
                var p2 = Math.Round(row.I * row.I * r, 2);
                var p = Math.Round(p1 + p2, 2);
                var nu = Math.Round(p1 * 100 / p, 2);
                _tableManager.AddYellow(p, p1, p2, nu);

            }
        }

        private void OnSaveClicked(object sender, System.Windows.RoutedEventArgs e)
        {
            if (_tableManager.Uextrapolated) return;
            _tableManager.AddGreenRow(R, U, I);
            if (_tableManager.measurementsdone) DrawUButton.IsEnabled = true;
        }

        private void OnSliderValueChanged(object sender, System.Windows.RoutedPropertyChangedEventArgs<double> e)
        {

            var eds = 18.0;
            var r = 642.0;
            var R = rSlider.Value;
            var I = eds / (R + r) * 1000;
            var U = I * R * 0.001;
            this.R = Math.Round(R, 2);
            this.U = Math.Round(U, 2);
            this.I = Math.Round(I, 2);
            resText.Text = this.R.ToString();
            voltText.Text = this.U.ToString();
            amperText.Text = this.I.ToString();
        }

        private void OnExtrapolateUClicked(object sender, System.Windows.RoutedEventArgs e)
        {

            //   plot1.Model.Series.Add(functionSeries);
        }

        private void OnDrawUClicked(object sender, System.Windows.RoutedEventArgs e)
        {
            if (!_tableManager.measurementsdone) return;
            _tableManager.Uextrapolated = true;
            SaveButton.IsEnabled = false;
            ClearButton.IsEnabled = false;
            DrawUButton.IsEnabled = false;
            FillButton.IsEnabled = true;
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
            IkzLabel.Content = Math.Round(b, 2);
            EDSLabel.Content = Math.Round(-b / k, 2);
            Ikz = Math.Round(b, 2);
            EDS = Math.Round(-b / k, 2);
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
            var (k, b) = Extrapolation.LinExtrapollation(points);
            var functionSeries = new FunctionSeries((x) =>
            {
                return k * x + b;
            }, 0, 30, 0.01);
            plotModel1.Series.Add(functionSeries);

            plot5.Model = plotModel1;

        }
    }
}