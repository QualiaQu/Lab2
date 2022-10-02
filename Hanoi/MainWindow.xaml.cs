﻿using System;
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
            Animation animation = new Animation(a);
            animation.ShowDialog();
        }
    }
}
