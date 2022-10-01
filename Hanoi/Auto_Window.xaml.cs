using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Кенийские_башни;

namespace Hanoi
{
    public partial class AutoWindow 
    {
        readonly int _ringsCount;
        /// <summary>
        /// Список передвижений
        /// </summary>
        readonly List<Tuple<int, int>> _movementsList=new();
        public AutoWindow(Help_Class help)
        {
            InitializeComponent();
            _ringsCount = help.RingsCount;
            CreateField();
        }
        /// <summary>
        /// Создает поле
        /// </summary>
        private void CreateField()
        {
            col1.Children.Clear();
            col2.Children.Clear();
            col3.Children.Clear();

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
                Canvas.SetBottom(r, r.Height *i);
                col1.Children.Add(r);
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

        private void Anima(Rectangle r, int to, DoubleAnimation leftAnimation, DoubleAnimation bottomAnimation)
        { 
            leftAnimation.From = Canvas.GetLeft(r);
            bottomAnimation.From = Canvas.GetBottom(r);

            switch (to)
            {

                case 0:
                    leftAnimation.To = Canvas.GetLeft(col1) + ((col1.Width / 2) - (r.Width / 2));
                    bottomAnimation.To = Canvas.GetBottom(col1) + (col1.Children.Count * Help_Class.RingHeight);
                    break;
                case 1:
                    leftAnimation.To = Canvas.GetLeft(col2) + ((col2.Width / 2) - r.Width / 2);
                    bottomAnimation.To = Canvas.GetBottom(col1) + (col2.Children.Count * Help_Class.RingHeight);
                    break;
                case 2:
                    leftAnimation.To = Canvas.GetLeft(col3) + (col3.Width / 2 - r.Width / 2);
                    bottomAnimation.To = Canvas.GetBottom(col1) + (col3.Children.Count * Help_Class.RingHeight);
                    break;
            }
            leftAnimation.Duration = TimeSpan.FromSeconds(Int32.Parse((string)((ComboBoxItem)Speed.SelectedItem).Content)*0.35);
            bottomAnimation.Duration = TimeSpan.FromSeconds(Int32.Parse((string)((ComboBoxItem)Speed.SelectedItem).Content)*0.35);
            //int32.Parse((string)((ComboBoxItem)Speed.SelectedItem).Content)*350
        }


        private async Task Move(int from,int to)
        {
            Canvas fromCol;
            switch (from)
            {
                case 0:
                    fromCol = col1;
                    break;
                case 1:
                    fromCol = col2;
                    break;
                case 2:
                    fromCol = col3;
                    break;
                default:
                    fromCol = col1;
                    break;
            }
            Canvas toCol;
            switch (to)
            {
                case 0:
                    toCol = col1;
                    break;
                case 1:
                    toCol = col2;
                    break;
                case 2:
                    toCol = col3;
                    break;
                default:
                    toCol = col1;
                    break;
            }


            DoubleAnimation leftAnimation = new DoubleAnimation();
            DoubleAnimation bottomAnimation = new DoubleAnimation();

            Rectangle copy = new Rectangle();
            //Забираем колечко из фром
            Rectangle r = (Rectangle)fromCol.Children[^1];
            
            RectangleCopy(r, copy, from);
            Anima(copy, to, leftAnimation, bottomAnimation);
            //Убираем колечко из фром
            fromCol.Children.Remove(r);
            MainCanvas.Children.Add(copy);
            copy.BeginAnimation(Canvas.LeftProperty,leftAnimation);
            copy.BeginAnimation(Canvas.BottomProperty, bottomAnimation);
            //Позиционируем колечко в ту
            Canvas.SetBottom(r, toCol.Children.Count * Help_Class.RingHeight);
            await Task.Delay(Int32.Parse((string)((ComboBoxItem)Speed.SelectedItem).Content)*350);
            //Добавляем колечко в ту
            toCol.Children.Add(r);
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
                    _movementsList.Add(new Tuple<int, int>(from, to));
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
            Solution(_ringsCount);
            foreach(var t in _movementsList)
            {
                
                await Move(t.Item1, t.Item2);
            }
           
            Start_Button.IsEnabled = true;
        }


    }
}
