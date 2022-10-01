using System;
using System.Windows;
using System.Windows.Controls;
using Кенийские_башни;

namespace Hanoi
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Help_Class a = new Help_Class();
            a.RingsCount = Int32.Parse((string)((ComboBoxItem)rings_count.SelectedItem).Content);

            if (mode.SelectedIndex == 0)
            {
                Manual_Window manual_window = new Manual_Window(a);
                manual_window.ShowDialog();
            }
            else
            {
                Auto_Window auto_window = new Auto_Window(a);
                auto_window.ShowDialog();
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            /*System.Diagnostics.Process.GetCurrentProcess().Kill();*/
        }
    }
}
