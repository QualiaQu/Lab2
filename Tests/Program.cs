using System.Diagnostics;

namespace Tests;

static class Program
{
    private static void HanoiTower(int n, int from=0, int to=1, int aux=2)
    {
        if (n > 0)
        {
            HanoiTower(n - 1, from, aux, to);
            HanoiTower(n - 1, aux, to, from);
        }
    }
    public static void Main()
    {
        var startTime = DateTime.Now;
        Stopwatch stopwatch = new Stopwatch();
        string results = "Количество колец;Время (микросекунды)\n";
        double averageTime = 0;
        for (int ringCount = 1; ringCount <= 30; ringCount++)
        {
            for (int i = 0; i < 5; i++)
            {
                stopwatch.Restart();
                HanoiTower(ringCount);
                stopwatch.Stop();
                averageTime += stopwatch.Elapsed.TotalMilliseconds;
            }
            
            results += $"{ringCount};{Math.Round(averageTime /= 5, 6) * 1000};\n";
            Console.WriteLine($"{ringCount}");
        }
        var finishTime = DateTime.Now;
        Console.WriteLine(finishTime - startTime);
        File.WriteAllText(Path.GetFullPath("./results.csv"), string.Empty);
        File.AppendAllText(Path.GetFullPath("./results.csv"), results);
    }
    
}
