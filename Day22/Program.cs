using System.Diagnostics;

Console.WriteLine("Day 22 - START");
var sw = Stopwatch.StartNew();
Part1();
Part2();
Console.WriteLine($"END (after {sw.Elapsed.TotalSeconds} seconds)");

static void Part1()
{	
	var cubes = ReadInput().Where(i => Math.Abs(i.X1) < 50).ToList();
	const int dim = 102;
	var cube = new bool[dim, dim, dim];
	foreach (var i in cubes)
	{
		for (var z = i.Z1; z <= i.Z2; z++)
		{
			for (var y = i.Y1; y <= i.Y2; y++)
			{
				for (var x = i.X1; x <= i.X2; x++)
				{
					cube[z + dim / 2, y + dim / 2, x + dim / 2] = i.On;
				}
			}
		}
	}
	var count = 0;
	for (var z = 0; z < dim; z++)
	{
		for (var y = 0; y < dim; y++)
		{
			for (var x = 0; x < dim; x++)
			{
				if (cube[z, y, x])
				{
					count++;
				}
			}
		}
	}
	Console.WriteLine($"{count} cubes are on.");
}

static void Part2()
{
	var count = 0L;
	var cubes = ReadInput().ToList();
	var sortedZ = cubes.SelectMany(c => new List<int> { c.Z1, c.Z2 + 1}).Distinct().OrderBy(z => z).ToList();
	var last = sortedZ.Min();
	foreach (var pos in sortedZ.Skip(1))
	{
		var height = pos - last;
		var subset = SplitZ(cubes, pos);
		count += CalculateAreaXY(subset);
		last = pos;
	}
	Console.WriteLine($"{count} cubes are on.");

	static long CalculateAreaXY(List<Cube> cubes)
	{
		var area = 0L;
		if (cubes.Any())
		{
			var all = new List<Cube>();
			var sortedY = cubes.SelectMany(c => new List<int> { c.Y1, c.Y2 + 1}).Distinct().OrderBy(y => y).ToList();
			var last = sortedY.Min();
			foreach (var pos in sortedY.Skip(1))
			{
				var width = pos - last;
				var sub =  SplitY(cubes, pos);
				area += CalculateAreaX(sub);
				last = pos;
			}
		}
		return area;
	}

	static long CalculateAreaX(List<Cube> cubes)
	{
		var area = 0L;
		if (cubes.Any())
		{
			var sortedX = cubes.SelectMany(c => new List<int> { c.X1, c.X2 + 1}).Distinct().OrderBy(x => x).ToList();
			var last = sortedX.Min();
			foreach (var pos in sortedX.Skip(1))
			{
				var depth = pos - last;
				var sub =  SplitX(cubes, pos);
				last = pos;
				if (sub.Any())
				{
					var c = sub.Last();
					if (c.On)
					{
						long x = c.X2 - c.X1 + 1;
						long y = c.Y2 - c.Y1 + 1;
						long z = c.Z2 - c.Z1 + 1;
						area += x * y * z;
					}
				}
			}
		}
		return area;
	}	


	static List<Cube> SplitX(List<Cube> cubes, int pos)
	{
		var subset = new List<Cube>();
		var remaining = new List<Cube>();
		foreach (var c in cubes)
		{
			if (c.X1 >= pos)
			{
				remaining.Add(c);
			}
			else
			{
				var c1 = c with { X2 = pos - 1 };
				subset.Add(c1);
				if (c.X2 >= pos)
				{
					var c2 = c with { X1 = pos };
					remaining.Add(c2);
				}
			}
		}
		cubes.Clear();
		cubes.AddRange(remaining);
		return subset;
	}

	static List<Cube> SplitY(List<Cube> cubes, int pos)
	{
		var subset = new List<Cube>();
		var remaining = new List<Cube>();
		foreach (var c in cubes)
		{
			if (c.Y1 >= pos)
			{
				remaining.Add(c);
			}
			else
			{
				var c1 = c with { Y2 = pos - 1 };
				subset.Add(c1);
				if (c.Y2 >= pos)
				{
					var c2 = c with { Y1 = pos };
					remaining.Add(c2);
				}
			}
		}
		cubes.Clear();
		cubes.AddRange(remaining);
		return subset;
	}

	static List<Cube> SplitZ(List<Cube> cubes, int pos)
	{
		var subset = new List<Cube>();
		var remaining = new List<Cube>();
		foreach (var c in cubes)
		{
			if (c.Z1 >= pos)
			{
				remaining.Add(c);
			}
			else
			{
				var c1 = c with { Z2 = pos - 1 };
				subset.Add(c1);
				if (c.Z2 >= pos)
				{
					var c2 = c with { Z1 = pos };
					remaining.Add(c2);
				}
			}
		}
		cubes.Clear();
		cubes.AddRange(remaining);
		return subset;
	}
}

static IEnumerable<Cube> ReadInput()
{
	static (int, int) Parse(string token)
	{
		token = token.Split('=').Last();
		var tokens = token.Split("..");
		return (int.Parse(tokens.First()), int.Parse(tokens.Last()));
	}
	foreach (var line in File.ReadAllLines("input.txt"))
	{
		var tokens = line.Split(' ');
		var on = tokens.First() == "on";
		tokens = tokens.Last().Split(',');
		var (x1, x2) = Parse(tokens[0]);
		var (y1, y2) = Parse(tokens[1]);
		var (z1, z2) = Parse(tokens[2]);
		yield return new Cube(on, x1, x2, y1, y2, z1, z2);
	}
}

internal record Cube(bool On, int X1, int X2, int Y1, int Y2, int Z1, int Z2);