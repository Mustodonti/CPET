using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.DataVisualization.Charting;
using System.IO;
using System.Windows;
using System.Globalization;
using System.Drawing;

namespace CPET
{
    public class GraphicsInit
    {
        string _nameOfChartArea { get;}
        string _nameOfSeries    { get; }
        string _nameOfSeries2   { get; }
        string _nameOfSeries3 { get; }
        string _nameOfSeries4 { get; }
        int    _countOfChartAreas { get;}
        Chart _chart { get; }
        public List<double> H = new List<double>() { };
        public GraphicsInit(Chart chart,string NameOfChartArea,string NameOfSeries,int Number)
        {
            _chart = chart;
            _nameOfChartArea = NameOfChartArea;
            _nameOfSeries = NameOfSeries;
            _countOfChartAreas = Number;
            _chart.ChartAreas.Add(new ChartArea(_nameOfChartArea));
            _chart.Series.Add(new Series(_nameOfSeries));
            _chart.Series[_nameOfSeries].ChartArea = _nameOfChartArea;
            _chart.Series[_nameOfSeries].ChartType = SeriesChartType.Line;//2d diagramma
            _chart.Series[_nameOfSeries].BorderWidth = 3;


        }
        public GraphicsInit(Chart chart, string NameOfChartArea, string NameOfSeries, string NameOfSeries2, int Number)
        {
            _chart = chart;
            _nameOfChartArea = NameOfChartArea;
            _nameOfSeries = NameOfSeries;
            _nameOfSeries2 = NameOfSeries2;
            _countOfChartAreas = Number;
            _chart.ChartAreas.Add(new ChartArea(_nameOfChartArea));
            _chart.Series.Add(new Series(_nameOfSeries));
            _chart.Series[_nameOfSeries].ChartArea = _nameOfChartArea;
            _chart.Series[_nameOfSeries].ChartType = SeriesChartType.Line;//2d diagramma
            _chart.Series[_nameOfSeries].BorderWidth = 3;
            _chart.Series.Add(new Series(_nameOfSeries2));
            _chart.Series[_nameOfSeries2].ChartArea = _nameOfChartArea;
            _chart.Series[_nameOfSeries2].ChartType = SeriesChartType.Line;//2d diagramma
            _chart.Series[_nameOfSeries2].BorderWidth = 4;
            _chart.Series[_nameOfSeries2].YAxisType = AxisType.Primary;
            _chart.Series[_nameOfSeries2].Color = System.Drawing.Color.Green;
            
            //_chart.Series[_nameOfSeries2].BorderDashStyle = ChartDashStyle.Dot;
        }
        public GraphicsInit(Chart chart, string NameOfChartArea, string NameOfSeries, string NameOfSeries2, string NameOfSeries3, int Number)
        {
            _chart = chart;
            _nameOfChartArea = NameOfChartArea;
            _nameOfSeries = NameOfSeries;
            _nameOfSeries2 = NameOfSeries2;
            _nameOfSeries3 = NameOfSeries3;
            _countOfChartAreas = Number;
            _chart.ChartAreas.Add(new ChartArea(_nameOfChartArea));
            _chart.Series.Add(new Series(_nameOfSeries));
            _chart.Series[_nameOfSeries].ChartArea = _nameOfChartArea;
            _chart.Series[_nameOfSeries].ChartType = SeriesChartType.Line;//2d diagramma
            _chart.Series[_nameOfSeries].BorderWidth = 3;
            _chart.Series.Add(new Series(_nameOfSeries2));
            _chart.Series[_nameOfSeries2].ChartArea = _nameOfChartArea;
            _chart.Series[_nameOfSeries2].ChartType = SeriesChartType.Line;//2d diagramma
            _chart.Series[_nameOfSeries2].BorderWidth = 3;
            _chart.Series[_nameOfSeries2].YAxisType = AxisType.Primary;
            _chart.Series.Add(new Series(_nameOfSeries3));
            _chart.Series[_nameOfSeries3].ChartArea = _nameOfChartArea;
            _chart.Series[_nameOfSeries3].ChartType = SeriesChartType.Line;//2d diagramma
            _chart.Series[_nameOfSeries3].BorderWidth = 2;
            _chart.Series[_nameOfSeries3].YAxisType = AxisType.Primary;
            _chart.Series[_nameOfSeries3].Color = System.Drawing.Color.Green;
            _chart.Series[_nameOfSeries3].BorderDashStyle = ChartDashStyle.Dot;

        }
        public GraphicsInit(Chart chart, string NameOfChartArea, string NameOfSeries, string NameOfSeries2, string NameOfSeries3, string NameOfSeries4, int Number)
        {
            _chart = chart;
            _nameOfChartArea = NameOfChartArea;
            _nameOfSeries = NameOfSeries;
            _nameOfSeries2 = NameOfSeries2;
            _nameOfSeries3 = NameOfSeries3;
            _nameOfSeries4 = NameOfSeries4;
            _countOfChartAreas = Number;
            _chart.ChartAreas.Add(new ChartArea(_nameOfChartArea));
            _chart.Series.Add(new Series(_nameOfSeries));
            _chart.Series[_nameOfSeries].ChartArea = _nameOfChartArea;
            _chart.Series[_nameOfSeries].ChartType = SeriesChartType.Line;//2d diagramma
            _chart.Series[_nameOfSeries].BorderWidth = 3;
            _chart.Series.Add(new Series(_nameOfSeries2));
            _chart.Series[_nameOfSeries2].ChartArea = _nameOfChartArea;
            _chart.Series[_nameOfSeries2].ChartType = SeriesChartType.Line;//2d diagramma
            _chart.Series[_nameOfSeries2].BorderWidth = 3;
            _chart.Series[_nameOfSeries2].YAxisType = AxisType.Primary;
            _chart.Series.Add(new Series(_nameOfSeries3));
            _chart.Series[_nameOfSeries3].ChartArea = _nameOfChartArea;
            _chart.Series[_nameOfSeries3].ChartType = SeriesChartType.Line;//2d diagramma
            _chart.Series[_nameOfSeries3].BorderWidth = 2;
            _chart.Series[_nameOfSeries3].YAxisType = AxisType.Primary;
            _chart.Series[_nameOfSeries3].Color = System.Drawing.Color.Green;
            
            _chart.Series.Add(new Series(_nameOfSeries4));
            _chart.Series[_nameOfSeries4].ChartArea = _nameOfChartArea;
            _chart.Series[_nameOfSeries4].ChartType = SeriesChartType.Line;//2d diagramma
            _chart.Series[_nameOfSeries4].BorderWidth = 2;
            _chart.Series[_nameOfSeries4].YAxisType = AxisType.Primary;
            _chart.Series[_nameOfSeries4].Color = System.Drawing.Color.Red;
            

        }
        public void ChangeDot2()
        {
            _chart.Series[_nameOfSeries2].Color = System.Drawing.Color.Purple;
            _chart.Series[_nameOfSeries2].BorderWidth = 4;
            _chart.Series[_nameOfSeries2].BorderDashStyle = ChartDashStyle.Dot;
        }
        public void AddXY (double X,double Y)
        {
            _chart.Series[_nameOfSeries].Points.AddXY(X, Y);
        }
        public void AddXY_Color(double X, double Y, System.Drawing.Color Color)
        {
            _chart.Series[_nameOfSeries].Color = Color;
            _chart.Series[_nameOfSeries].Points.AddXY(X, Y);

        }
        public void AddXY2(double X, double Y)
        {
            _chart.Series[_nameOfSeries2].Points.AddXY(X, Y);
        }
        public void AddXY3(double X, double Y)
        {
            _chart.Series[_nameOfSeries3].Points.AddXY(X, Y);
        }
        public void AddXY(double X, double Y,double status_binary)
        {
            if (status_binary != 0)
            {
                _chart.Series[_nameOfSeries].Points.AddXY(X, Y);
            }
            else 
            {
                _chart.Series[_nameOfSeries].Points.AddXY(0, 0);
            }
        }
        public List<double> AddXY_SIN(double stepX,double F, int N)
        {
            List<double> Y = new List<double>() { };
            List<double> X = new List<double>() { };
            double a;
            double f;
            double b;
            double c;
            for (double x =0; x <= N; x += 1)
            {
                a = 0.5*Math.Sin(2 * Math.PI * (F/2)* x * stepX);
                b = Math.Sin(2 * Math.PI * F * x * stepX );
                f = 0.5 * Math.Sin(2 * Math.PI * (F *10) * x * stepX);
                c = f;
                _chart.Series[_nameOfSeries].Points.AddXY(x* stepX, c) ;//2 * Math.PI *
                Y.Add(c);
                X.Add(x * stepX);
            }
            H = X;
            return Y;
            
        }
        public void AddDataXY(List<double> axisXData, List<double> axisYData)
        {
            _chart.Series[_nameOfSeries].Points.DataBindXY(axisXData, axisYData);
        }
        public void AddDataXY(string NameOfSeries, List<double> axisXData, List<double> axisYData)
        {
            _chart.Series[NameOfSeries].Points.DataBindXY(axisXData, axisYData);
        }
        public void AddDataXY2(List<double> axisXData, List<double> axisYData)
        {
            _chart.Series[_nameOfSeries2].Points.DataBindXY(axisXData, axisYData);
        }
        public void AddDataXY3(List<double> axisXData, List<double> axisYData)
        {
            _chart.Series[_nameOfSeries3].Points.DataBindXY(axisXData, axisYData);
        }
        public void AddDataXY4(List<double> axisXData, List<double> axisYData)
        {
            _chart.Series[_nameOfSeries4].Points.DataBindXY(axisXData, axisYData);
        }
        public void SetOfAxis ( double MinX,double MaxX,double MinY,double MaxY,double IntervalX,double IntervalY,string NameX="",string NameY="")
        {
            ChartArea area1 = _chart.ChartAreas[_countOfChartAreas];
            //Для прокрутки внутри графика
            //area1.AxisX.ScaleView.Zoomable = true;
            //area1.CursorX.AutoScroll = true;
            //area1.AxisX.ScrollBar.Enabled = true;
            //area1.CursorX.IsUserEnabled = true;
            //area1.CursorX.IsUserSelectionEnabled = true;
            //area1.AxisX.ScrollBar.IsPositionedInside = true;

            area1.AxisX.Maximum = MaxX;
            area1.AxisX.Minimum = MinX;
            area1.AxisY.Maximum = MaxY;
            area1.AxisY.Minimum = MinY;
            area1.AxisX.Title = NameX;
            area1.AxisY.Title = NameY;
            area1.AxisX.TitleFont = new Font(FontFamily.GenericSansSerif, 13.0F, System.Drawing.FontStyle.Bold);
            area1.AxisY.TitleFont = new Font(FontFamily.GenericSansSerif, 13.0F, System.Drawing.FontStyle.Bold);
            area1.AxisX2.TitleFont = new Font(FontFamily.GenericSansSerif, 13.0F, System.Drawing.FontStyle.Bold);
            area1.AxisY2.TitleFont = new Font(FontFamily.GenericSansSerif, 13.0F, System.Drawing.FontStyle.Bold);
            area1.AxisX.LabelAutoFitMaxFontSize = 15;
            area1.AxisY.LabelAutoFitMinFontSize = 15;
            area1.AxisX2.LabelAutoFitMaxFontSize = 15;
            area1.AxisY2.LabelAutoFitMinFontSize = 15;
            area1.AxisX.Interval = IntervalX;
            area1.AxisY.Interval = IntervalY;
            
        }
        public void SetOfIntervalX(double MinX, double MaxX)
        {
            ChartArea area1 = _chart.ChartAreas[_countOfChartAreas];
            area1.AxisX.Maximum = MaxX;
            area1.AxisX.Minimum = MinX;

        }
        public void AddAxisY2(double MinY2, double MaxY2, double IntervalY2, string NameY2 = "")
        {
            _chart.ChartAreas[_nameOfChartArea].AxisY2.Enabled = System.Windows.Forms.DataVisualization.Charting.AxisEnabled.True;       
            ChartArea area1 = _chart.ChartAreas[_countOfChartAreas];
            area1.AxisY2.Maximum = MaxY2;
            area1.AxisY2.Minimum = MinY2;
            area1.AxisY2.Title = NameY2;
            area1.AxisY2.Interval = IntervalY2;
        }
        public void AddAxisX2(double MinX2, double MaxX2,double IntervalX2,string NameX2 = "")
        {
            _chart.ChartAreas[_nameOfChartArea].AxisX2.Enabled = System.Windows.Forms.DataVisualization.Charting.AxisEnabled.True;
            ChartArea area1 = _chart.ChartAreas[_countOfChartAreas];
            area1.AxisX2.Maximum = MaxX2;
            area1.AxisX2.Minimum = MinX2;
            area1.AxisX2.Title = NameX2;
            area1.AxisX2.Interval = IntervalX2;
        }
        public void CommonLine ( string NameOfSeries, double x1, double x2, double y1, double y2)
        {
            _chart.Series.Add(new Series(NameOfSeries));
            _chart.Series[NameOfSeries].ChartArea = _nameOfChartArea;
            _chart.Series[NameOfSeries].ChartType = SeriesChartType.Line;
            _chart.Series[NameOfSeries].Color = System.Drawing.Color.Black;
            _chart.Series[NameOfSeries].BorderWidth = 3;
            List<double> axisYData = new List<double>() { y1, y2 };
            List<double> axisXData = new List<double>() { x1, x2 };
            _chart.Series[NameOfSeries].Points.DataBindXY(axisXData, axisYData);
        }
        public void DashLine(string NameOfSeries,double x1,double x2,double y1,double y2)
        {

            _chart.Series.Add(new Series(NameOfSeries));
            _chart.Series[NameOfSeries].ChartArea = _nameOfChartArea;
            _chart.Series[NameOfSeries].ChartType = SeriesChartType.Line;
            _chart.Series[NameOfSeries].Color = System.Drawing.Color.Black;
            _chart.Series[NameOfSeries].BorderWidth = 3;
            _chart.Series[NameOfSeries].BorderDashStyle = ChartDashStyle.Dash;
            List<double> axisYData = new List<double>() { y1, y2 };
            List<double> axisXData = new List<double>() { x1, x2 };
            _chart.Series[NameOfSeries].Points.DataBindXY(axisXData, axisYData);
        }
        public void DashLine_S(string NameOfSeries)
        {

            _chart.Series.Add(new Series(NameOfSeries));
            _chart.Series[NameOfSeries].ChartArea = _nameOfChartArea;
            _chart.Series[NameOfSeries].ChartType = SeriesChartType.Line;
            _chart.Series[NameOfSeries].Color = System.Drawing.Color.Black;
            _chart.Series[NameOfSeries].BorderWidth = 3;
            _chart.Series[NameOfSeries].BorderDashStyle = ChartDashStyle.Dash;
        }
        public void AddPointDashLine_S(string NameOfSeries,double x1, double x2, double y1, double y2)
        { 
            List<double> axisYData = new List<double>() { y1, y2 };
            List<double> axisXData = new List<double>() { x1, x2 };
            _chart.Series[NameOfSeries].Points.DataBindXY(axisXData, axisYData);
        }
        public void ControlLine(string NameOfSeries, double x1, double x2, double y1, double y2)
        {
            _chart.Series.Add(new Series(NameOfSeries));
            _chart.Series[NameOfSeries].ChartArea = _nameOfChartArea;
            _chart.Series[NameOfSeries].ChartType = SeriesChartType.Line;
            _chart.Series[NameOfSeries].Color = System.Drawing.Color.Red;
            _chart.Series[NameOfSeries].BorderWidth = 3;
            List<double> axisYData = new List<double>() { y1, y2 };
            List<double> axisXData = new List<double>() { x1, x2 };
            _chart.Series[NameOfSeries].Points.DataBindXY(axisXData, axisYData);
        }
        public void AddSeries(string NameOfSeries, System.Drawing.Color Color)
        {
            _chart.Series.Add(new Series(NameOfSeries));
            _chart.Series[NameOfSeries].ChartArea = _nameOfChartArea;
            _chart.Series[NameOfSeries].ChartType = SeriesChartType.Line;
            //_chart.Series[NameOfSeries].Color = System.Drawing.Color.Purple;
            _chart.Series[NameOfSeries].Color = Color;
            _chart.Series[NameOfSeries].BorderWidth = 3;
        }
        public void Clear()
        {
            _chart.Series[_nameOfSeries].Points.Clear();
       
        }
        public void Clear2()
        {
            _chart.Series[_nameOfSeries].Points.Clear();
            _chart.Series[_nameOfSeries2].Points.Clear();
        }
        public void Clear3()
        {
            _chart.Series[_nameOfSeries].Points.Clear();
            _chart.Series[_nameOfSeries2].Points.Clear();
            _chart.Series[_nameOfSeries3].Points.Clear();
        }
    }
}
