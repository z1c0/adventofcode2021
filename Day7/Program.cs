using System.Diagnostics;

Console.WriteLine("Day 7 - START");
var sw = Stopwatch.StartNew();
Part1(true);
Part1(false);
Console.WriteLine($"END (after {sw.Elapsed.TotalSeconds} seconds)");

static void Part1(bool constantFuel)
{
	var positions = ReadInput().ToList();
	var max = positions.Max();
	var minFuel = int.MaxValue;
	for (var i = 0; i <= max; i++)
	{
		var fuel = 0;
		foreach (var p in positions)
		{
			if (constantFuel)
			{
				fuel += Math.Abs(p - i);
			}
			else
			{
				var diff = Math.Abs(p - i);
				for (var d = 0; d <= diff; d++)
				{
					fuel += d;
				}
			}
		}
		minFuel = Math.Min(fuel, minFuel);
	}
	Console.WriteLine($"Minimum fuel spent: {minFuel}");
}

static IEnumerable<int> ReadInput()
{
	return File.ReadAllText("input.txt").Split(',').Select(t => int.Parse(t));
}
