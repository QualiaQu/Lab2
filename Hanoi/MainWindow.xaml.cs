using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Media.Animation;
using System.Threading;

namespace WPF1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        const int MaxCount = 3;
        const double Dw = 20;
        const double H = 40;
        readonly List<int>[] _list = new List<int>[3];
        readonly List<Canvas>[] _canList = new List<Canvas>[3];
        int _currentList;
        readonly double[] _positions = new double[3];
        delegate void MoveDelList(int f);

        readonly MoveDelList[] _mvDel;
        const double MoVeSpeed = 0.4;
        public MainWindow()
        {
            
            InitializeComponent();
            _mvDel = new MoveDelList[2];
            _mvDel[0] = MoveMin;
            _mvDel[1] = MoveMax;

            _list[0] = new List<int>(5);
            _list[1] = new List<int>(5);
            _list[2] = new List<int>(5);

            _canList[0] = new List<Canvas>();
            _canList[1] = new List<Canvas>();
            _canList[2] = new List<Canvas>();
            InitForm();
        }

        void InitForm()
        {
            Canvas1.Children.Clear();
            foreach (var v in _list)
            {
                v.Clear();
            }
            foreach (var v in _canList)
            {
                v.Clear();
            }
            for (int i = 0; i < MaxCount; i++) { _list[0].Add(i); }
            _currentList = 0;
            double d = Canvas1.Width / 3;
            for (int i = 0; i < 3; i++)
            {
                _positions[i] = i * d;// +d / 2;
            }
            SolidColorBrush scb = new SolidColorBrush
            {
                Color = Color.FromArgb(255, 255, 255, 0)
            };
            for(int i = 0;i < MaxCount;i++)
            {
                Canvas cn = new Canvas
                {
                    Width = d - (MaxCount - i) * Dw,
                    Height = H + 20
                };
                Rectangle rc = new Rectangle
                {
                    Fill = scb,
                    Width = cn.Width - 8,
                    Height = H / 2
                };
                Panel.SetZIndex(cn, MaxCount - i);
                Ellipse el = new Ellipse
                {
                    Fill = scb,
                    Width = cn.Width,
                    Height = cn.Height / 2
                };
                Ellipse el1 = new Ellipse
                {
                    Fill = scb,
                    Width = cn.Width,
                    Height = cn.Height / 2
                };
                Canvas.SetTop(el1, H - H / 2);
                Canvas.SetTop(rc, H - rc.Height - rc.Height / 2 + 10);
                Canvas.SetLeft(rc, 4);
                el.StrokeThickness = 2;
                el.Stroke = Brushes.Black;
                el1.StrokeThickness = 2;
                el1.Stroke = Brushes.Black;
                cn.Children.Add(el1);
                cn.Children.Add(rc);
                cn.Children.Add(el);
                Canvas.SetLeft(cn, (d - cn.Width) / 2);
                Canvas.SetTop(cn, i * H);
                Canvas1.Children.Add(cn);
                _canList[0].Add(cn);
            }
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            InitForm();
            ThreadPool.QueueUserWorkItem(StartProc);
            Button1.IsEnabled = false;
        }

        private void StartProc(object? stateInfo)
        {
            MoveItem(0);
        }

        private void MoveItem(int f)
        {
            if (_list[2].Count == MaxCount || _list[1].Count == MaxCount)
            {
                SetButEnable();
                return;
            }
            Dispatcher.Invoke(
            System.Windows.Threading.DispatcherPriority.Normal,
            new OnAnimateMove(_mvDel[f]), f);
            //DisplayCurrent();
        }

        private void DisplayCurrent()
        {
            
        }

        delegate void OnDisplayCurrent();
        delegate void OnAnimateMove(int f);
        private void SetButEnable()
        {
            try
            {
                Button1.IsEnabled = true;
            }
            catch
            {
                Button1.Dispatcher.Invoke(
                System.Windows.Threading.DispatcherPriority.Normal,
                new OnDisplayCurrent(this.SetButEnable));
            }
        }

        private void MoveMin(int f)
        {
            int i = _list[_currentList][0];
            _list[_currentList].RemoveAt(0);
            Canvas cn = _canList[_currentList][0];
            _canList[_currentList].RemoveAt(0);

            int dist = _currentList + 1 >= _list.Length ? 0 : _currentList + 1;
            _list[dist].Insert(0, i);
            _canList[dist].Insert(0, cn);

            _currentList++;
            if (_currentList == _list.Length)
            {
                _currentList = 0;
            }
            Dispatcher.Invoke(
            System.Windows.Threading.DispatcherPriority.Normal,
            new OnAnimate(Animate), f, cn, _positions[dist], (MaxCount - _canList[dist].Count) * H);
        }

        private void MoveMax(int f)
        {
            int from = -1;
            int minVal = int.MaxValue;
            for (int i = 0; i < 3; i++)
            {
                if (_list[i].Count == 0) {
                    continue; }
                if (i == _currentList) { continue; }
                if (minVal > _list[i][0]) { minVal = _list[i][0]; from = i; }
            }
            var to = GetIndex(from, _currentList);
            int v = _list[from][0];
            _list[from].RemoveAt(0);
            _list[to].Insert(0, v);

            Canvas cn = _canList[from][0];
            _canList[from].RemoveAt(0);
            _canList[to].Insert(0, cn);
            double y = (MaxCount - _canList[to].Count) * H - Canvas.GetTop(cn);
            Animate(f, cn, _positions[to], y);
        }

        private int GetIndex(int i, int j) { return 3 - (i + j); }

        delegate void OnAnimate(int f, Canvas cn, double x, double y);

        static readonly DependencyProperty Dprop = DependencyProperty.RegisterAttached(
            "temp", typeof(int), typeof(int));
        static readonly DependencyProperty CanvasProp = DependencyProperty.RegisterAttached(
            "ccc", typeof(Canvas), typeof(Canvas));

        private void Animate(int f, Canvas cn, double x, double y)
        {
            Panel.SetZIndex(cn, Panel.GetZIndex(cn) * 10);
            DoubleAnimation dax = new DoubleAnimation
            {
                Duration = new Duration(TimeSpan.FromSeconds(MoVeSpeed)),
                From = cn.RenderTransformOrigin.X,
                To = x
            };

            DoubleAnimation day = new DoubleAnimation
            {
                Duration = new Duration(TimeSpan.FromSeconds(MoVeSpeed)),
                From = cn.RenderTransformOrigin.Y,
                To = y
            };

            TranslateTransform tf = new TranslateTransform();
            cn.RenderTransform = tf;
            tf.SetValue(Canvas.LeftProperty, x);
            tf.SetValue(Canvas.TopProperty, y);
            tf.SetValue(Dprop, f);
            tf.SetValue(CanvasProp, cn);

            tf.Changed += tf_Changed;
            tf.BeginAnimation(TranslateTransform.XProperty, dax);
            tf.BeginAnimation(TranslateTransform.YProperty, day);
        }

        void tf_Changed(object? sender, EventArgs e)
        {
            TranslateTransform tf = (TranslateTransform)sender!;
            double x = (double)tf.GetValue(Canvas.LeftProperty);
            double y = (double)tf.GetValue(Canvas.TopProperty);
            if (tf.X.Equals(x) && tf.Y.Equals(y))
            {
                Canvas cn = (Canvas)tf.GetValue(CanvasProp);
                Panel.SetZIndex(cn, Panel.GetZIndex(cn) / 10);
                cn.RenderTransformOrigin = new Point(x, y);
                MoveItem(1 - (int)tf.GetValue(Dprop));
            }
        }
    }
}
