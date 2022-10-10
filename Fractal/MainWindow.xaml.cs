using System;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Threading;


namespace Fractal;

public partial class MainWindow
{
    private int _depth;
    private const double LengthScale = 0.8;
    private const double DeltaTheta = Math.PI / 8;
    public MainWindow()
    {
        InitializeComponent();
    }
    private void btnStart_Click(object sender, RoutedEventArgs e)
    {
        Canvas.Children.Clear();
        TbLabel.Text = "";
        _depth = 1;
        
        CompositionTarget.Rendering += StartAnimation!;
    }
    private void StartAnimation(object sender, EventArgs e)
    {
        DrawFractalTree(Canvas, _depth++, new Point(Canvas.Width / 2, 0.85 * Canvas.Height), 0.2 * Canvas.Width,
            -Math.PI / 2);
        if (_depth >= 15)
        {
            CompositionTarget.Rendering -= StartAnimation!;
        }
        
    }

    private void DrawFractalTree(Canvas canvas, int depth, Point pt, double length, double theta)
    {
        if (depth > 0)
        {
            double x2 = pt.X + length * Math.Cos(theta);
            double y2 = pt.Y + length * Math.Sin(theta);
            var line = new Line
            {
                Stroke = Brushes.Azure,
                X1 = pt.X,
                Y1 = pt.Y,
                X2 = x2,
                Y2 = y2
            };
            Dispatcher.BeginInvoke(DispatcherPriority.Background, 
                new Action(() =>
            {
                if (depth <= 2) Thread.Sleep(20);
                canvas.Children.Add(line);
                
            }));

            DrawFractalTree(canvas, depth - 1, new Point(x2, y2), 
                length * LengthScale, theta + DeltaTheta);
        
            DrawFractalTree(canvas, depth - 1, new Point(x2, y2), 
                length * LengthScale, theta - DeltaTheta);
        }
    }
}