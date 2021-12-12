using System.Diagnostics;

Console.WriteLine("Day 12 - START");
var sw = Stopwatch.StartNew();
Part1(true);
Part1(false);
Console.WriteLine($"END (after {sw.Elapsed.TotalSeconds} seconds)");

static void Part1(bool visitOnlyOnce)
{
	var paths = ReadInput().ToList();
	var numberOfPathsFound = 0;
	FindPath("start", paths, new(), ref numberOfPathsFound, visitOnlyOnce);
	Console.WriteLine($"Number of paths found: {numberOfPathsFound}.");
}

static void FindPath(string from, List<(string From, string To)> paths, List<string> path, ref int numberOfPathsFound, bool visitOnlyOnce)
{
	path.Add(from);
	var candidates = paths.Where(p => p.From == from).Select(p => p.To).ToList();
	foreach (var c in candidates)
	{
		if (IsValidPath(c, path, visitOnlyOnce))
		{
			if (c == "end")
			{
				//Console.WriteLine(string.Join(',', path));
				numberOfPathsFound++;
			}
			else
			{
				FindPath(c, paths, path.ToList(), ref numberOfPathsFound, visitOnlyOnce);
			}
		}
	}
}

static bool IsValidPath(string p, List<string> path, bool visitOnlyOnce)
{
	if (p == "start")
	{
		return false;
	}
	if (char.IsLower(p.First()) && path.Contains(p))
	{
		if (visitOnlyOnce)
		{
			return false;
		}
		else
		{
			var tmp = new Dictionary<string, int>();
			foreach (var c in path.Where(p => char.IsLower(p.First())))
			{
				if (!tmp.ContainsKey(c))
				{
					tmp.Add(c, 0);
				}
				tmp[c]++;
			}
			if (tmp.Values.Any(v => v >= 2))
			{
				return false;
			}
		}
	}
	return true;
}

static IEnumerable<(string From, string To)> ReadInput()
{
	var paths = new Dictionary<string, string>();
	foreach (var line in File.ReadAllLines("input.txt"))
	{
		var tokens = line.Split('-');
		yield return (tokens[0], tokens[1]);
		yield return (tokens[1], tokens[0]);
	}
}
