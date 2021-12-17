using System.Diagnostics;

Console.WriteLine("Day 17 - START");
var sw = Stopwatch.StartNew();
Part1();
Console.WriteLine($"END (after {sw.Elapsed.TotalSeconds} seconds)");

static void Part1()
{
	var target =ReadInput();
	var maxMaxY = int.MinValue;
	var count = 0;
	for (var y = -1000; y < 1000; y ++)
	{
		for (var x = 0; x < 1000; x ++)
		{
			var maxY = Simulate((x, y), target);
			maxMaxY = Math.Max(maxMaxY, maxY);
			if (maxY != int.MinValue)
			{
				count++;
			}
		}
	}
	Console.WriteLine($"Maximum y: {maxMaxY}, successful probes: {count}.");
}

static int Simulate((int X, int Y) v, (int X1, int X2, int Y1, int Y2) target)
{
	var x = 0;
	var y = 0;
	var maxY = int.MinValue;
	while (true)
	{
		x += v.X;
		y += v.Y;
		maxY = Math.Max(maxY, y);
		if (x >= target.X1 && y <= target.Y2 && x <= target.X2 && y >= target.Y1)
		{
			return maxY;
		}
		if (x > target.X2 || y < target.Y1)
		{
			// miss
			return int.MinValue;
		}

		if (v.X > 0)
		{
			v.X--;
		}
		else if (v.X < 0)
		{
			v.X++;
		}
		v.Y--;
	}
}

static (int X1, int X2, int Y1, int Y2) ReadInput()
{
	var tokens = File.ReadAllText("input.txt").Split(": =").Last().Split(", ");
	var tokens1 = tokens.First().Split('=').Last().Split(".."); 
	var x1 = int.Parse(tokens1.First());
	var x2 = int.Parse(tokens1.Last());
	tokens1 = tokens.Last().Split('=').Last().Split(".."); 
	var y1 = int.Parse(tokens1.First());
	var y2 = int.Parse(tokens1.Last());
	return (x1, x2, y1, y2);
}
