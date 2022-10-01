using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Кенийские_башни;

namespace Hanoi
{
    /// <summary>
    /// Логика взаимодействия для Manual_Window.xaml
    /// </summary>
    public partial class ManualWindow
    {
        readonly int _ringsCount;
        int _movesCount;

        private void CreateField()
        {
            Col1.Children.Clear();
            Col2.Children.Clear();
            Col3.Children.Clear();

            int ringWidth = Help_Class.RingMinWidth;
            for (int i = 0; i < _ringsCount; i++)
            {
                Rectangle r = new Rectangle
                {
                    Width = ringWidth - i * (Help_Class.Difference),
                    Height = Help_Class.RingHeight,
                    Fill = Help_Class.ColorBrash(Help_Class.Colors.colors[i])
                };
                Canvas.SetLeft(r, 120 - r.Width / 2);
                Canvas.SetBottom(r, r.Height * i);
                r.MouseDown += r_MouseDown;
                Col1.Children.Add(r);
            }
        }

        private void VictoryMessage(int movesCount)
        {
            if (Col2.Children.Count == _ringsCount || Col3.Children.Count == _ringsCount)
            {
                if (movesCount > Math.Pow(2,_ringsCount)-1)
                {
                    MessageBox.Show("Вы прошли игру, сделав при этом " + movesCount + " ходов, к сожалению, это больше идеального прохождения на " + (movesCount - (2 ^ _ringsCount) - 1) + " ход :(");
                }
                else
                {
                    MessageBox.Show("Вы прошли игру, сделав при этом " + movesCount + " ходов, это идеальное прохождение :)");
                }
                CreateField();
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
        public void Anima(Rectangle r, int to, DoubleAnimation LeftAnimation, DoubleAnimation BottomAnimation)
        {
            LeftAnimation.From = Canvas.GetLeft(r);
            BottomAnimation.From = Canvas.GetBottom(r);

            switch (to)
            {

                case 0:
                    LeftAnimation.To = Canvas.GetLeft(Col1) + ((Col1.Width / 2) - (r.Width / 2));
                    BottomAnimation.To = Canvas.GetBottom(Col1) + (Col1.Children.Count * Help_Class.RingHeight);
                    break;
                case 1:
                    LeftAnimation.To = Canvas.GetLeft(Col2) + ((Col2.Width / 2) - r.Width / 2);
                    BottomAnimation.To = Canvas.GetBottom(Col1) + (Col2.Children.Count * Help_Class.RingHeight);
                    break;
                case 2:
                    LeftAnimation.To = Canvas.GetLeft(Col3) + (Col3.Width / 2 - r.Width / 2);
                    BottomAnimation.To = Canvas.GetBottom(Col1) + (Col3.Children.Count * Help_Class.RingHeight);
                    break;
            }
            LeftAnimation.Duration = TimeSpan.FromSeconds(0.2);
            BottomAnimation.Duration = TimeSpan.FromSeconds(0.2);
        }
        private void Drag_Enter(object sender, DragEventArgs e)
        {
            Canvas Destination = (Canvas)sender;
            Rectangle DroppedItem = (Rectangle)e.Data.GetData(DataFormats.Serializable);
            Canvas Source = (Canvas)DroppedItem.Parent;
            if ((Destination.Children.Count == 0 || ((Rectangle)Destination.Children[Destination.Children.Count - 1]).Width > DroppedItem.Width) && Destination != Source)
                Destination.Background = Help_Class.ColorBrash(Help_Class.Colors.GoodColor);
            else if (Destination != Source)
                Destination.Background = Help_Class.ColorBrash(Help_Class.Colors.BadColor);
        }
        private void Drag_Leave(object sender, DragEventArgs e)
        {
            Canvas Destination = (Canvas)sender;
            Destination.Background = Help_Class.ColorBrash("Transparent");
        }
        private void r_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Canvas roditel = (Canvas)((Rectangle)sender).Parent;
            if (roditel.Children[roditel.Children.Count-1]==sender)
            DragDrop.DoDragDrop((Rectangle)sender, new DataObject(DataFormats.Serializable,(Rectangle)sender), DragDropEffects.Move);
            
        }
        public async void col_Drop(object sender, DragEventArgs e)
        {
            Canvas Destination = (Canvas)sender;
            Rectangle DroppedItem = (Rectangle)e.Data.GetData(DataFormats.Serializable);
            Canvas Source = (Canvas)DroppedItem.Parent;
            DoubleAnimation LeftAnimation = new DoubleAnimation();
            DoubleAnimation BottomAnimation = new DoubleAnimation();
            Rectangle copy = new Rectangle();

            switch (Source.Name)
            {
                case "col1":
                    RectangleCopy(DroppedItem, copy, 0);
                    break;
                case "col2":
                    RectangleCopy(DroppedItem, copy, 1);
                    break;
                case "col3":
                    RectangleCopy(DroppedItem, copy, 2);
                    break;
            }
            switch (Destination.Name)
            {
                case "col1":
                    Anima(copy, 0, LeftAnimation, BottomAnimation);
                    break;
                case "col2":
                    Anima(copy, 1, LeftAnimation, BottomAnimation);
                    break;
                case "col3":
                    Anima(copy, 2, LeftAnimation, BottomAnimation);
                    break;
            }

            /*Anima(copy, Int32.Parse(Destination.Name), LeftAnimation, BottomAnimation);*/
            if ((Destination.Children.Count == 0 || ((Rectangle)Destination.Children[Destination.Children.Count - 1]).Width > DroppedItem.Width) && Destination != Source)
            { 
                Canvas.SetBottom(DroppedItem, Help_Class.RingHeight * (Destination.Children.Count));
                Source.Children.Remove(DroppedItem);
                MainCanvas.Children.Add(copy);
                copy.BeginAnimation(Canvas.LeftProperty, LeftAnimation);
                copy.BeginAnimation(Canvas.BottomProperty, BottomAnimation);
                await Task.Delay(200);
                Destination.Children.Add(DroppedItem);
                MainCanvas.Children.Remove(copy);
                _movesCount++;
                VictoryMessage(_movesCount);
            }
            else
            {
                Destination.Background = Help_Class.ColorBrash("Transparent");
                return;
            }
            Destination.Background = Help_Class.ColorBrash("Transparent");
            
        }
        public ManualWindow(Help_Class HA)
        {
            InitializeComponent();
            _ringsCount = HA.RingsCount;
            
            
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            CreateField();
            
        }
        
    }
}
