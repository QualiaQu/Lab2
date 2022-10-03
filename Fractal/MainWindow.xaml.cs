using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Fractal
{
    public partial class MainWindow
    {
        private int _depth;
        private int _i;
        private const double LengthScale = 0.8;
        private const double DeltaTheta = Math.PI / 8;
        public MainWindow()
        {
            InitializeComponent();
        }

        private void btnStart_Click(object sender, RoutedEventArgs e)
        {
            Canvas1.Children.Clear();
            TbLabel.Text = null;
            
            _depth = 1;
            CompositionTarget.Rendering += StartAnimation!;
        }

        private void StartAnimation(object sender, EventArgs e)
        {
            _i += 1;
            if (_i % 60 != 0) return;
            
            DrawBinaryTree(Canvas1, _depth, new Point(Canvas1.Width / 2, 0.85 * Canvas1.Height), 0.2 * Canvas1.Width,
                -Math.PI / 2);
            TbLabel.Text = "Количество шагов = " + _depth;
            _depth += 1;
            
            if (_depth < 15) return;
            
            TbLabel.Text = $"Количество шагов = {_depth}. Дерево выращено!";
            CompositionTarget.Rendering -= StartAnimation!;
        }

        private void DrawBinaryTree(Canvas canvas, int depth, Point pt, double length, double theta)
        {
            double x2 = pt.X + length * Math.Cos(theta);
            double y2 = pt.Y + length * Math.Sin(theta);
            Line line = new Line
            {
                Stroke = Brushes.Azure,
                X1 = pt.X,
                Y1 = pt.Y,
                X2 = x2,
                Y2 = y2
            };
            canvas.Children.Add(line);
            
            if (depth == 1) return;
            
            DrawBinaryTree(canvas, depth - 1, new Point(x2, y2), 
                length * LengthScale, theta + DeltaTheta);
            DrawBinaryTree(canvas, depth - 1, new Point(x2, y2), 
                length * LengthScale, theta - DeltaTheta);
        }
    }
}