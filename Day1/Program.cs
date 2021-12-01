using System.Diagnostics;

Console.WriteLine("Day 1 - START");
var sw = Stopwatch.StartNew();
Part1(1);
Part1(3);
Console.WriteLine($"END (after {sw.Elapsed.TotalSeconds} seconds)");

static void Part1(int windowSize)
{
	var depths = ReadInput().ToList();
	var count = 0;
	for (var i = 0; i < depths.Count - windowSize; i++)
	{
		var window1 = 0;
		var window2 = 0;
		for (var j = 0; j < windowSize; j++)
		{
			window1 += depths[i + j];
			window2 += depths[i + j + 1];
		}
		if (window2 > window1)
		{
			count++;
		}
	}
	Console.WriteLine($"{count} measurements are larger than the previous one.");
}

static IEnumerable<int> ReadInput()
{
	foreach (var line in File.ReadAllLines("input.txt"))
	{
		yield return int.Parse(line);
	}
}
