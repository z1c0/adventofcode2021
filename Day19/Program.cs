using System.Diagnostics;

Console.WriteLine("Day 19 - START");
var sw = Stopwatch.StartNew();
Part1();
Console.WriteLine($"END (after {sw.Elapsed.TotalSeconds} seconds)");

static void Part1()
{
	var scanners = ReadInput();
	while (scanners.Count > 1)
	{
		for (var i = 0; i < scanners.Count; i++)
		{
			var found = false;
			for (var j = i + 1; j < scanners.Count; j++)
			{
				var s1 = scanners[i];
				var s2 = scanners[j];
				Console.WriteLine($"matching ... ({i} and {j} of {scanners.Count})");
				var m = Match(s1, s2);
				if (m != null)
				{
					var c1 = m.Value.c1;
					var c2 = m.Value.c2;
					var p = m.Value.p;
					Console.WriteLine("before: " + c1.Count);
					foreach (var (x, y, z) in c2)
					{ 
						var pp = (p.X + x, p.Y + y, p.Z + z);
						c1.Add(pp);
					}
					Console.WriteLine("before: " + c1.Count);
					scanners.Remove(s1);
					scanners.Remove(s2);
					scanners.Add(c1);
					found = true;
					break;
				}
				if (found)
				{
					break;
				}
			}
		}
	}
	Console.WriteLine(scanners.Single().Count);
}

static (HashSet<(int X, int Y, int Z)> c1, HashSet<(int X, int Y, int Z)> c2, (int X, int Y, int Z) p)? Match(HashSet<(int X, int Y, int Z)> scanner1, HashSet<(int X, int Y, int Z)> scanner2)
{
	var combinations1 = CreateCombinations(scanner1);
	var combinations2 = CreateCombinations(scanner2);
	foreach (var c1 in combinations1)
	{
		var mapX = c1.Select(e => e.X).ToHashSet();
		var mapY = c1.Select(e => e.Y).ToHashSet();
		var mapZ = c1.Select(e => e.Z).ToHashSet();

		var matchesX = new HashSet<int>();
		var matchesY = new HashSet<int>();
		var matchesZ = new HashSet<int>();
		foreach (var c2 in combinations2)
		{
			const int range = 2000;
			for (var a = -range; a < range; a++)
			{
				var countX = 0;
				var countY = 0;
				var countZ = 0;
				foreach (var e2 in c2)
				{
					if (mapX.Contains(e2.X + a))
					{
						countX++;
					}
					if (mapY.Contains(e2.Y + a))
					{
						countY++;
					}
					if (mapZ.Contains(e2.Z + a))
					{
						countZ++;
					}
				}
				if (countX >= 12)
				{
					matchesX.Add(a);
				}
				if (countY >= 12)
				{
					matchesY.Add(a);
				}
				if (countZ >= 12)
				{
					matchesZ.Add(a);
				}
			}
		}
		if (matchesX.Any() && matchesY.Any() && matchesZ.Any())
		{
			var x = matchesX.Single();
			var y = matchesY.Single();
			var z = matchesZ.Single();
			foreach (var c2 in combinations2)
			{
				if (c2.Count(e => c1.Contains((e.X + x, e.Y + y, e.Z + z))) >= 12)
				{
					return (c1, c2, (x, y, z));
				}
			}
		}
	}
	return null;
}

static IEnumerable<HashSet<(int X, int Y, int Z)>> CreateCombinations(HashSet<(int X, int Y, int Z)> scanner)
{
	static void Permutate(HashSet<(int X, int Y, int Z)>[] combinations, int i, int x, int y, int z)
	{
		combinations[i + 0].Add(( x,  y, -z));
		combinations[i + 1].Add(( x,  y,  z));
		combinations[i + 2].Add(( x, -y, -z));
		combinations[i + 3].Add(( x, -y,  z));
		combinations[i + 4].Add((-x,  y, -z));
		combinations[i + 5].Add((-x,  y,  z));
		combinations[i + 6].Add((-x, -y, -z));
		combinations[i + 7].Add((-x, -y,  z));
	}
	var combinations = new HashSet<( int X, int Y, int Z)>[24];
	for (var i = 0; i < combinations.Length; i++)
	{
		combinations[i] = new();
	}
	foreach (var (x, y, z) in scanner)
	{
		Permutate(combinations,  0, x, y, z);
		Permutate(combinations,  8, y, x, z);
		Permutate(combinations, 16, z, x, y);
	}
	return combinations;
}

static List<HashSet<(int X, int Y, int Z)>> ReadInput()
{
	var scanners = new List<HashSet<(int X, int Y, int Z)>>();
	foreach (var line in File.ReadAllLines("input.txt"))
	{
		if (!string.IsNullOrEmpty(line))
		{
			if (line.StartsWith("---"))
			{
				scanners.Add(new ());
			}
			else
			{
				var tokens = line.Split(',');
				scanners.Last().Add((
					int.Parse(tokens[0]),
					int.Parse(tokens[1]),
					int.Parse(tokens[2])
				));
			}
		}
	}
	return scanners;
}
