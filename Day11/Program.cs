using System.Diagnostics;
using aoc;

Console.WriteLine("Day 11 - START");
var sw = Stopwatch.StartNew();
Part1(100);
Part1(1000);
Console.WriteLine($"END (after {sw.Elapsed.TotalSeconds} seconds)");

static void Part1(int steps)
{
	var grid = ReadInput();
	var flashCount = 0;
	for (var i = 0; i < steps; i++)
	{
		// Increase all energy levels by 1
		grid.ForEach(p => grid[p]++);
		// Flash
		while (true)
		{
			var flashing = grid.FindAll((x, y) => grid[x, y] > 9).ToList();
			if (!flashing.Any())
			{
				break;
			}
			foreach (var f in flashing)
			{
				var adjacent = grid.GetAdjacent8(f);
				foreach (var a in adjacent)
				{
					if (grid[a] >= 0)
					{
						grid[a]++;
					}
				}
				// Mark as "has flashed"
				grid[f] = -1;
				flashCount++;
			}
		}
		// Reset all marked (= has flashed) to 0.
		var marked = grid.FindAll((x, y) => grid[x, y] == -1).ToList();
		if (marked.Count == grid.Size)
		{
			Console.WriteLine($"In step {i + 1} all flashes are synchronized.");
			return;
		}
		foreach (var m in marked)
		{
			grid[m] = 0;
		}
	}
	Console.WriteLine($"Number of flashes after {steps} steps: {flashCount}");
}

static Grid<int> ReadInput()
{
	return IntGrid.FromFile("input.txt");
}
