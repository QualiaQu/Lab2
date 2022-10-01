/*
 * C# Program to Demonstrate Tower Of Hanoi
 */

using System.Runtime.InteropServices;
using Convert = System.Convert;

namespace Tests;

static class Program
{
    private static int _count;
    class TowerOfHanoi
    {
        int _mNumdiscs;
        public TowerOfHanoi()
        {
            Numdiscs = 0;
        }
        public TowerOfHanoi(int newval)
        {
            Numdiscs = newval;
        }
        public int Numdiscs
        {
            get => _mNumdiscs;
            set
            {
                if (value > 0)
                    _mNumdiscs = value;
            }
        }
        public static void Movetower(int n, int from = 1, int to = 3, int other = 2)
        {
            if (n > 0)
            {
                Movetower(n - 1, from, other, to);
                Console.WriteLine($"Move disk {n} from tower {from} to tower {to}");
                Movetower(n - 1, other, to, from);
            }
        }
    }
    
    static class TowersOfHanoiApp
    {
        public static void Main()
        {
            //TowerOfHanoi T = new TowerOfHanoi();
            var ringCount = Convert.ToInt32(Console.ReadLine());
            // TowerOfHanoi.Movetower(ringCount, 1, 3, 2);
            TowerOfHanoi.Movetower(ringCount);
        }
    }
}
