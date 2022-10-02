using System.Collections.Generic;
using System.Windows.Media;

namespace Hanoi
{
    public class HelpClass
    {
        public static SolidColorBrush ColorBrash(string color)
        {
            return (SolidColorBrush)new BrushConverter().ConvertFrom(color)!;
        }

        public static class Colors
        {
            public static readonly List<string> ColorsList = new() 
            { "#ff0000", "#ff8c00", "#fff700", "#00ff15", "#005eff", "#5905a3", "#ce42eb", "#eda6ea" , "#a6e8ed" , "#bf1b5d" };
        }
        public int RingsCount;
        public static readonly int RingMinWidth = 240;
        public static readonly int RingHeight = 40;
        public static readonly int Difference = 20;
    }
}
