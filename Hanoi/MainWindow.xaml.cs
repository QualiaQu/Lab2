using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace Hanoi;
public partial class MainWindow
{
    private int _maxCount;
    const double Dw = 20;
    const double H = 40;
    readonly List<int>[] _list = new List<int>[3];
    readonly List<Canvas>[] _canList = new List<Canvas>[3];
    int _currentList;
    readonly double[] _positions = new double[3];
    delegate void MoveDelList(int f);

    readonly MoveDelList[] _mvDel;
    const double MoveSpeed = 0.1;
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
    }

    void InitForm()
    {
        _maxCount = int.Parse(RingCount.Text);
        Canvas1.Children.Clear();
        foreach (var v in _list)
        {
            v.Clear();
        }
        foreach (var v in _canList)
        {
            v.Clear();
        }
        for (int i = 0; i < _maxCount; i++) { _list[0].Add(i); }
        _currentList = 0;
        double d = Canvas1.Width / 3;
        for (int i = 0; i < 3; i++)
        {
            _positions[i] = i * d;
        }
        SolidColorBrush scb = new SolidColorBrush
        {
            Color = Color.FromArgb(255, 20, 255, 0)
        };
        for(int i = 0; i < _maxCount; i++)
        {
            Canvas cn = new Canvas
            {
                Width = d - (_maxCount - i) * Dw,
                Height = H + 20
            };
            Panel.SetZIndex(cn, _maxCount - i);
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
            Canvas.SetBottom(el1, H - H / 2);
            el.StrokeThickness = 2;
            el.Stroke = Brushes.Black;
            el1.StrokeThickness = 2;
            el1.Stroke = Brushes.Black;
            cn.Children.Add(el1);
            cn.Children.Add(el);
            Canvas.SetLeft(cn, (d - cn.Width) / 2);
            Canvas.SetTop(cn, i * H);
            Canvas1.Children.Add(cn);
            _canList[0].Add(cn);
        }
    }

    private void MoveTower(int n, int from, int to , int other, Canvas cnv)
    {
        if (n > 0)
        {
            MoveTower(n - 1, from, other, to, cnv);
            //Console.WriteLine($"Move disk {n} from tower {from} to tower {to}");
            AnimateMove(0, cnv, from, to);
            MoveTower(n - 1, other, to, from,cnv);
        }
    }
    private void AnimateMove(int f, Canvas cn, double x, double y)
    {
        Panel.SetZIndex(cn, Panel.GetZIndex(cn) * 10);
        DoubleAnimation dax = new DoubleAnimation
        {
            Duration = new Duration(TimeSpan.FromSeconds(MoveSpeed)),
            From = cn.RenderTransformOrigin.X,
            To = x
        };
        
        DoubleAnimation day = new DoubleAnimation
        {
            Duration = new Duration(TimeSpan.FromSeconds(MoveSpeed)),
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
    private void button1_Click(object sender, RoutedEventArgs e)
    {
        InitForm();
        ThreadPool.QueueUserWorkItem(StartProc);
        Button1.IsEnabled = false;
        //MoveTower(_maxCount,1,3,4, Canvas1);
    }

    private void StartProc(object? stateInfo)
    {
        MoveItem(0);
    }
    
    private void MoveItem(int f)
    {
        if (_list[2].Count == _maxCount || _list[1].Count == _maxCount)
        {
            SetButEnable();
            return;
        }
        Dispatcher.Invoke(
            System.Windows.Threading.DispatcherPriority.Normal,
            new OnAnimateMove(_mvDel[f]), f);
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
                new OnDisplayCurrent(SetButEnable));
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
        Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Normal,
            new OnAnimate(AnimateMove), f, cn, _positions[dist], (_maxCount - _canList[dist].Count) * H);
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
        double y = (_maxCount - _canList[to].Count) * H - Canvas.GetTop(cn);
        AnimateMove(f, cn, _positions[to], y);
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
            Duration = new Duration(TimeSpan.FromSeconds(MoveSpeed)),
            From = cn.RenderTransformOrigin.X,
            To = x
        };
        
        DoubleAnimation day = new DoubleAnimation
        {
            Duration = new Duration(TimeSpan.FromSeconds(MoveSpeed)),
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