using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Physics
{
    public class GreenRow
    {
        public double R { get; set; }
        public double U { get; set; }
        public double I { get; set; }
    }
    public class YellowRow
    {
        public double P { get; set; }
        public double P1 { get; set; }
        public double P2 { get; set; }
        public double Nu { get; set; }
    }
    public class TableManager
    {
        private readonly Grid _dataGrid;
        private int _currentIndexGreen = 0;
        private int _currentIndexYellow = 0;
        private int _width = 8;
        private int _height = 5;
        public bool measurementsdone = false;
        public bool Uextrapolated = false;
        public bool YrowsFilled = false;

        public int Width { get => _width; set => _width = value; }
        public int Height { get => _height; set => _height = value; }

        public TableManager(Grid dataGrid)
        {
            _dataGrid = dataGrid;
        }

        public GreenRow GetGreenRow(int row)
        {
            var greenRow = new GreenRow();
            greenRow.R = (double)(_dataGrid.FindName($"lbl{row}_{1}") as Label).Content;
            greenRow.U = (double)(_dataGrid.FindName($"lbl{row}_{2}") as Label).Content;
            greenRow.I = (double)(_dataGrid.FindName($"lbl{row}_{3}") as Label).Content;
            return greenRow;
        }

        public void AddGreenRow(double R, double U, double I)
        {
            if(_currentIndexGreen >=5)
            {
                return;
            }

            (_dataGrid.FindName($"lbl{_currentIndexGreen}_{0}") as Label).Content = _currentIndexGreen + 1;
            (_dataGrid.FindName($"lbl{_currentIndexGreen}_{1}") as Label).Content = R;
            (_dataGrid.FindName($"lbl{_currentIndexGreen}_{2}") as Label).Content = U;
            (_dataGrid.FindName($"lbl{_currentIndexGreen}_{3}") as Label).Content = I;
            _currentIndexGreen++;
            if (_currentIndexGreen == 5)
            {
                measurementsdone = true;
            }
        }

        public void AddYellow(double P, double P1, double P2, double Nu)
        {
            if (_currentIndexYellow >= 5)
            {
                return;
            }

            (_dataGrid.FindName($"lbl{_currentIndexYellow}_{4}") as Label).Content = P;
            (_dataGrid.FindName($"lbl{_currentIndexYellow}_{5}") as Label).Content = P1;
            (_dataGrid.FindName($"lbl{_currentIndexYellow}_{6}") as Label).Content = P2;
            (_dataGrid.FindName($"lbl{_currentIndexYellow}_{7}") as Label).Content = Nu;
            _currentIndexYellow++;
        }

        public YellowRow GetYellowRow(int row)
        {
            var yellowRow = new YellowRow();
            yellowRow.P = (double)(_dataGrid.FindName($"lbl{row}_{4}") as Label).Content;
            yellowRow.P1 = (double)(_dataGrid.FindName($"lbl{row}_{5}") as Label).Content;
            yellowRow.P2 = (double)(_dataGrid.FindName($"lbl{row}_{6}") as Label).Content;
            yellowRow.Nu = (double)(_dataGrid.FindName($"lbl{row}_{7}") as Label).Content;
            return yellowRow;
        }

        public void Clear()
        {
            _currentIndexGreen = 0;
            _currentIndexYellow = 0;
            measurementsdone = false;
            for (int i = 0; i < _width; i++)
            {
                for (int j = 0; j < _height; j++)
                {
                    var label = _dataGrid.FindName($"lbl{j}_{i}") as Label;

                    label.Content = string.Empty;
                }
            }
        }

        
    }
}
