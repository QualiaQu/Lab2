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
            HelpClass a = new HelpClass
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
