using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace Hanoi;

public partial class Animation 
{
    readonly int _ringsCount;
    readonly List<Tuple<int, int>> _movementsList = new();
    public Animation(HelpClass help)
    {
        InitializeComponent();
        _ringsCount = help.RingsCount;
        Start();
    }
    private void CreateField()
    {
        Col1.Children.Clear();
        Col2.Children.Clear();
        Col3.Children.Clear();

        int ringWidth = HelpClass.RingMinWidth;
        for (int i = 0; i < _ringsCount; i++)
        {
            Rectangle r = new Rectangle
            {
                Width = ringWidth - i * (HelpClass.Difference),
                Height = HelpClass.RingHeight,
                Fill = HelpClass.ColorBrash(HelpClass.Colors.ColorsList[i])
            };
            Canvas.SetLeft(r, 120 - r.Width / 2);
            Canvas.SetBottom(r, r.Height *i);
            Col1.Children.Add(r);
        }
    }


    private void RectangleCopy(Rectangle source, Rectangle copy, int sourceCol)
    {
        copy.Fill = source.Fill;
        copy.Width = source.Width;
        copy.Height = source.Height;
        
        switch (sourceCol)
        {
            case 0:
                Canvas.SetLeft(copy, Canvas.GetLeft(source) + Canvas.GetLeft(Col1));
                Canvas.SetBottom(copy, Canvas.GetBottom(source) + Canvas.GetBottom(Col1));
                break;
            case 1:
                Canvas.SetLeft(copy, Canvas.GetLeft(source) + Canvas.GetLeft(Col2));
                Canvas.SetBottom(copy, Canvas.GetBottom(source) + Canvas.GetBottom(Col2));
                break;
            case 2:
                Canvas.SetLeft(copy, Canvas.GetLeft(source) + Canvas.GetLeft(Col3));
                Canvas.SetBottom(copy, Canvas.GetBottom(source) + Canvas.GetBottom(Col3));
                break;
        }
    }
    private void Anima(Rectangle r, int to, DoubleAnimation leftAnimation, DoubleAnimation bottomAnimation)
    { 
        leftAnimation.From = Canvas.GetLeft(r);
        bottomAnimation.From = Canvas.GetBottom(r);

        switch (to)
        {
            case 0:
                leftAnimation.To = Canvas.GetLeft(Col1) + ((Col1.Width / 2) - (r.Width / 2));
                bottomAnimation.To = Canvas.GetBottom(Col1) + (Col1.Children.Count * HelpClass.RingHeight);
                break;
            case 1:
                leftAnimation.To = Canvas.GetLeft(Col2) + ((Col2.Width / 2) - r.Width / 2);
                bottomAnimation.To = Canvas.GetBottom(Col1) + (Col2.Children.Count * HelpClass.RingHeight);
                break;
            case 2:
                leftAnimation.To = Canvas.GetLeft(Col3) + (Col3.Width / 2 - r.Width / 2);
                bottomAnimation.To = Canvas.GetBottom(Col1) + (Col3.Children.Count * HelpClass.RingHeight);
                break;
        }
        leftAnimation.Duration = TimeSpan.FromSeconds((int)Slider.Value * 0.35);
        bottomAnimation.Duration = TimeSpan.FromSeconds((int)Slider.Value * 0.35);
    }
    private async Task Move(int from,int to)
    {
        Canvas fromCol = from switch
        {
            0 => Col1,
            1 => Col2,
            2 => Col3,
            _ => Col1
        };
        Canvas toCol = to switch
        {
            0 => Col1,
            1 => Col2,
            2 => Col3,
            _ => Col1
        };
        DoubleAnimation leftAnimation = new DoubleAnimation();
        DoubleAnimation bottomAnimation = new DoubleAnimation();

        Rectangle copy = new Rectangle();
        Rectangle r = (Rectangle)fromCol.Children[^1];
            
        RectangleCopy(r, copy, from);
        Anima(copy, to, leftAnimation, bottomAnimation);
        fromCol.Children.Remove(r);
        MainCanvas.Children.Add(copy);
        copy.BeginAnimation(Canvas.LeftProperty,leftAnimation);
        copy.BeginAnimation(Canvas.BottomProperty, bottomAnimation);
        Canvas.SetBottom(r, toCol.Children.Count * HelpClass.RingHeight);
        await Task.Delay((int) (Slider.Value * 350));
        toCol.Children.Add(r);
        MainCanvas.Children.Remove(copy);
    }
    private void HanoiTower(int n, int from=0, int to=1, int aux=2)
    {
        if (n <= 0) return;
        
        HanoiTower(n - 1, from, aux, to);
        
        _movementsList.Add(new Tuple<int, int>(from, to));
        
        HanoiTower(n - 1, aux, to, from);
    }
    private async void Start()
    {
        CreateField();
        HanoiTower(_ringsCount);
        foreach(var t in _movementsList)
        {
            await Move(t.Item1, t.Item2);
        }
    }
    private void Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
    {
        ((Slider)sender).SelectionEnd = e.NewValue;
    }
}