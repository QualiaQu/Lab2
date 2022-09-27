/*
 * C# Program to Demonstrate Tower Of Hanoi
 */

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
        public static void Movetower(int n, int from, int to, int other)
        {
            if (n > 0)
            {
                Movetower(n - 1, from, other, to);
                Console.WriteLine("Move disk {0} from tower {1} to tower {2}", 
                    n, from, to);
                _count++;
                Movetower(n - 1, other, to, from);
            }
        }
    }
    
    static class TowersOfHanoiApp
    {
        public static void Main()
        {
            TowerOfHanoi T = new TowerOfHanoi();
            Console.Write("Enter the number of discs: ");
            T.Numdiscs = Convert.ToInt32(8);
            TowerOfHanoi.Movetower(T.Numdiscs, 1, 3, 2);
            Console.WriteLine(_count);
        }
    }
}
