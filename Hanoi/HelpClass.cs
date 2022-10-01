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
            public static readonly List<string> colors = new() 
            { "#FFD3F8E2", "#FFDCDDEE", "#FFE4C1F9", "#FFEDABDD", "#FFEDC9C7", "#FFEDE7B1", "#FFF2BEB9", "#FFCBE3D5" , "#FFF694C1" , "#FFA9DEF9" };
            public static readonly string GoodColor = "#3381C14B";
            public static readonly string BadColor = "#33A4243B";
            public static readonly string FreeColor = "#59E5E5E5";
        }
        public int RingsCount;
        public static readonly int RingMinWidth = 240;
        public static readonly int RingHeight = 40;
        public static readonly int Difference = 20;


    }
}
