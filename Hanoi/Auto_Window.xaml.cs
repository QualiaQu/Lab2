using System;
using System.Collections.Generic;
using System.Diagnostics;
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
    /// Логика взаимодействия для Auto_Window.xaml
    /// </summary>
    public partial class Auto_Window : Window
    {
        int RingsCount;
        /// <summary>
        /// Список передвижений
        /// </summary>
        List<Tuple<int, int>> MovementsList=new List<Tuple<int, int>>();
        public Auto_Window(Help_Class Help)
        {
            InitializeComponent();
            RingsCount = Help.RingsCount;
            CreateField();
        }
        /// <summary>
        /// Создает поле
        /// </summary>
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
                Canvas.SetBottom(r, r.Height *i);
                col1.Children.Add(r);
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
            LeftAnimation.Duration = TimeSpan.FromSeconds(Int32.Parse((string)((ComboBoxItem)Speed.SelectedItem).Content)*0.35);
            BottomAnimation.Duration = TimeSpan.FromSeconds(Int32.Parse((string)((ComboBoxItem)Speed.SelectedItem).Content)*0.35);
            //int32.Parse((string)((ComboBoxItem)Speed.SelectedItem).Content)*350
        }


        public async Task Move(int from,int to)
        {
            Canvas FromCol = new Canvas();
            switch (from)
            {
                case 0:
                    FromCol = col1;
                    break;
                case 1:
                    FromCol = col2;
                    break;
                case 2:
                    FromCol = col3;
                    break;
                default:
                    FromCol = col1;
                    break;
            }
            Canvas ToCol = new Canvas();
            switch (to)
            {
                case 0:
                    ToCol = col1;
                    break;
                case 1:
                    ToCol = col2;
                    break;
                case 2:
                    ToCol = col3;
                    break;
                default:
                    ToCol = col1;
                    break;
            }


            DoubleAnimation LeftAnimation = new DoubleAnimation();
            DoubleAnimation BottomAnimation = new DoubleAnimation();

            Rectangle copy = new Rectangle();
            //Забираем колечко из фром
            Rectangle r = (Rectangle)FromCol.Children[FromCol.Children.Count - 1];
            
            RectangleCopy(r, copy, from);
            Anima(copy, to, LeftAnimation, BottomAnimation);
            //Убираем колечко из фром
            FromCol.Children.Remove(r);
            MainCanvas.Children.Add(copy);
            copy.BeginAnimation(Canvas.LeftProperty,LeftAnimation);
            copy.BeginAnimation(Canvas.BottomProperty, BottomAnimation);
            //Позиционируем колечко в ту
            Canvas.SetBottom(r, ToCol.Children.Count * Help_Class.RingHeight);
            await Task.Delay(Int32.Parse((string)((ComboBoxItem)Speed.SelectedItem).Content)*350);
            //Добавляем колечко в ту
            ToCol.Children.Add(r);
            MainCanvas.Children.Remove(copy);
        }
        /// <summary>
        /// Решение Кенобийских башень
        /// </summary>
        /// <param name="n"></param>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <param name="aux"></param>
        private void Solution(int n, int from=0, int to=1, int aux=2)
        {
            try
            {
                if (n > 0)
                {
                    Solution(n - 1, from, aux, to);
                    MovementsList.Add(new Tuple<int, int>(from, to));
                    Solution(n - 1, aux, to, from);
                }

            }
            catch (Exception err)
            {
                Debug.WriteLine(err.Message);
            }

        }
        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            CreateField();
            Start_Button.IsEnabled = false;
            Solution(RingsCount);
            foreach(var t in MovementsList)
            {
                
                await Move(t.Item1, t.Item2);
            }
           
            Start_Button.IsEnabled = true;
        }


    }
}
