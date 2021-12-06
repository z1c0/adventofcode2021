using System.Diagnostics;

Console.WriteLine("Day 6 - START");
var sw = Stopwatch.StartNew();
Part1(80);
Part2(256);
Console.WriteLine($"END (after {sw.Elapsed.TotalSeconds} seconds)");

static void Part1(int days)
{
	var fish = ReadInput().ToList();
	for (var d = 0; d < days ; d++)
	{
		for (var i = 0; i < fish.Count; i++)
		{
			var f = fish[i];
			if (f == 0)
			{
				f = 6;
				fish.Add(9);
			}
			else
			{
				f--;
			}
			fish[i] = f;
		}
		//Console.WriteLine($"Day {d + 1}, {string.Join(',', fish)}");
	}
	Console.WriteLine($"After {days} days, there are {fish.Count} lanternfish.");
}

static void Part2(int days)
{
	var fish = ReadInput().ToList();
	long total = fish.Count;
	foreach (var f in fish)
	{
		total += CountFish(days - f, new());
	}
	Console.WriteLine($"After {days} days, there are {total} lanternfish.");
}

static long CountFish(int days, Dictionary<int, long> cache)
{
	if (cache.ContainsKey(days))
	{
		return cache[days];
	}
	long fish = 0;
	var d = days;
	while (d > 0)
	{
		fish++;
		d -= 7;
		fish += CountFish(d - 2, cache);
	}
	cache[days] = fish;
	return fish;
}

static IEnumerable<int> ReadInput()
{
	return File.ReadAllText("input.txt").Split(',').Select(t => int.Parse(t));
}
