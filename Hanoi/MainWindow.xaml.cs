using System;
using System.Windows;
using Кенийские_башни;

namespace Hanoi
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Help_Class a = new Help_Class
            {
                RingsCount = Int32.Parse(RingCount.Text)
            };
            AutoWindow autoWindow = new AutoWindow(a);
            autoWindow.ShowDialog();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            /*System.Diagnostics.Process.GetCurrentProcess().Kill();*/
        }
    }
}
