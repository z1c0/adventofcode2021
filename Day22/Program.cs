using System.Diagnostics;

Console.WriteLine("Day 22 - START");
var sw = Stopwatch.StartNew();
Part1();
Part2();
Console.WriteLine($"END (after {sw.Elapsed.TotalSeconds} seconds)");

static void Part1()
{	
	const int dim = 102;
	var cube = new bool[dim, dim, dim];
	foreach (var i in ReadInput().ToList())
	{
		if (Math.Abs(i.X1) < 50)
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
	var input = ReadInput().Reverse().ToList();
	for (var i = 0; i < input.Count; i++)
	{
		var a = input[i];
		for (var j = i + 1; j < input.Count; j++)
		{
			var b = input[j];
			if (Overlap(a, b))
			{
				Console.WriteLine(a + " covers " + b);
			}
		}
	}
}

static bool Overlap((bool On, int X1, int X2, int Y1, int Y2, int Z1, int Z2) a, (bool On, int X1, int X2, int Y1, int Y2, int Z1, int Z2) b)
{
	/*
	return
		a.X1 <= b.X1 && a.X2 >= b.X2 &&
		a.Y1 <= b.Y1 && a.Y2 >= b.Z2 &&
		a.Z1 <= b.Z1 && a.Z2 >= b.Z2;*/
	return
		a.X1 < b.X1 && a.X2 > b.X1 ||a.X1 < b.X2 && a.X2 > b.X2 ||
		a.Y1 < b.Y1 && a.Y2 > b.Y1 ||a.Y1 < b.Y2 && a.Y2 > b.Y2 ||
		a.Z1 < b.Z1 && a.Z2 > b.Z1 ||a.Z1 < b.Z2 && a.Z2 > b.Z2;
}

static IEnumerable<(bool On, int X1, int X2, int Y1, int Y2, int Z1, int Z2)> ReadInput()
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
		yield return (on, x1, x2, y1, y2, z1, z2);
	}
}
