using System;
using System.Windows;

namespace Hanoi
{
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var count = Int32.Parse(RingCount.Text);
            if (count < 1 | count > 10)
            {
                MessageBox.Show("Введите значение от 1 до 10");      
            }
            else
            {
                HelpClass a = new HelpClass
                {
                    RingsCount = Int32.Parse(RingCount.Text)
                };
                Animation animation = new Animation(a);
                animation.ShowDialog();
            }
        }
    }
}
