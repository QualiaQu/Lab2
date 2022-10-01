using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;


namespace Кенийские_башни
{
    /// <summary>
    /// Логика взаимодействия для Manual_Window.xaml
    /// </summary>
    public partial class Manual_Window : Window
    {
        int RingsCount;
        int Moves_Count = 0;
        public void CreateField()
        {
            col1.Children.Clear();
            col2.Children.Clear();
            col3.Children.Clear();

            int RingWidth = Help_Class.RingMinWidth;
            for (int i = 0; i < RingsCount; i++)
            {
                Rectangle r = new Rectangle();
                r.Width = RingWidth - i * (Help_Class.Difference);
                r.Height = Help_Class.RingHeight;
                r.Fill = Help_Class.ColorBrash(Help_Class.Colors.colors[i]);
                Canvas.SetLeft(r, 120 - r.Width / 2);
                Canvas.SetBottom(r, r.Height * i);
                r.MouseDown += r_MouseDown;
                col1.Children.Add(r);
            }
        }
        
        public void VictoryMessage(int MovesCount)
        {
            if (col2.Children.Count == RingsCount || col3.Children.Count == RingsCount)
            {
                if (MovesCount > Math.Pow(2,RingsCount)-1)
                {
                    MessageBox.Show("Вы прошли игру, сделав при этом " + MovesCount + " ходов, к сожалению, это больше идеального прохождения на " + (MovesCount - (2 ^ RingsCount) - 1) + " ход :(");
                }
                else
                {
                    MessageBox.Show("Вы прошли игру, сделав при этом " + MovesCount + " ходов, это идеальное прохождение :)");
                }
                CreateField();
            }
            
        }
        public void RectangleCopy(Rectangle source, Rectangle copy, int SourceCol)
        {
            copy.Fill = source.Fill;
            copy.Width = source.Width;
            copy.Height = source.Height;

            switch (SourceCol)
            {
                case 0:
                    Canvas.SetLeft(copy, Canvas.GetLeft(source) + Canvas.GetLeft(col1));
                    Canvas.SetBottom(copy, Canvas.GetBottom(source) + Canvas.GetBottom(col1));
                    break;
                case 1:
                    Canvas.SetLeft(copy, Canvas.GetLeft(source) + Canvas.GetLeft(col2));
                    Canvas.SetBottom(copy, Canvas.GetBottom(source) + Canvas.GetBottom(col2));
                    break;
                case 2:
                    Canvas.SetLeft(copy, Canvas.GetLeft(source) + Canvas.GetLeft(col3));
                    Canvas.SetBottom(copy, Canvas.GetBottom(source) + Canvas.GetBottom(col3));
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
                    LeftAnimation.To = Canvas.GetLeft(col1) + ((col1.Width / 2) - (r.Width / 2));
                    BottomAnimation.To = Canvas.GetBottom(col1) + (col1.Children.Count * Help_Class.RingHeight);
                    break;
                case 1:
                    LeftAnimation.To = Canvas.GetLeft(col2) + ((col2.Width / 2) - r.Width / 2);
                    BottomAnimation.To = Canvas.GetBottom(col1) + (col2.Children.Count * Help_Class.RingHeight);
                    break;
                case 2:
                    LeftAnimation.To = Canvas.GetLeft(col3) + (col3.Width / 2 - r.Width / 2);
                    BottomAnimation.To = Canvas.GetBottom(col1) + (col3.Children.Count * Help_Class.RingHeight);
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
                Moves_Count++;
                VictoryMessage(Moves_Count);
            }
            else
            {
                Destination.Background = Help_Class.ColorBrash("Transparent");
                return;
            }
            Destination.Background = Help_Class.ColorBrash("Transparent");
            
        }
        public Manual_Window(Help_Class HA)
        {
            InitializeComponent();
            RingsCount = HA.RingsCount;
            
            
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            CreateField();
            
        }
        
    }
}
