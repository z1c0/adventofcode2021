using System.Diagnostics;
using aoc;

Console.WriteLine("Day 9 - START");
var sw = Stopwatch.StartNew();
Part1();
Part2();
Console.WriteLine($"END (after {sw.Elapsed.TotalSeconds} seconds)");

static void Part1()
{
	var grid = ReadInput();
	var lowPoints = GetLowPoints(grid);
	var riskLevel = lowPoints.Sum(p => grid[p] + 1);
	Console.WriteLine($"Total risk level: {riskLevel}");
}

static void Part2()
{
	var grid = ReadInput();
	var lowPoints = GetLowPoints(grid);
	var basinSizes = new List<int>();
	foreach (var p in lowPoints)
	{
		basinSizes.Add(GetBasinSize(p, grid));
	}
	basinSizes.Sort();
	var s1 = basinSizes[^1];
	var s2 = basinSizes[^2];
	var s3 = basinSizes[^3];
	Console.WriteLine($"3 largest basin sizes multiplied: {s1 * s2 * s3}");
}

static int GetBasinSize((int X, int y) from, Grid<int> grid)
{
	var size = 1;
	var visited = new HashSet<(int x, int y)>() { from };
	var queue = new Queue<(int x, int y)>();
	queue.Enqueue(from);
	while (queue.Any())
	{
		var (x, y) = queue.Dequeue();
		foreach (var n in grid.GetAdjacent4(x, y))
		{
			if (grid[n] < 9 && !visited.Contains(n))
			{
				visited.Add(n);
				queue.Enqueue(n);
				size++;
			}
		}
	}
	return size;
}

static IEnumerable<(int X, int Y)> GetLowPoints(Grid<int> grid)
{
	for (var y = 0; y < grid.Height; y++)
	{
		for (var x = 0; x < grid.Width; x++)
		{
			var h = grid[x, y];
			var adjacent = grid.GetAdjacent4(x, y);
			if (adjacent.All(a => grid[a] > h))
			{
				yield return (x, y);
			}
		}
	}
}

static Grid<int> ReadInput()
{
	return IntGrid.FromFile("input.txt");
}
