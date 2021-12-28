using System.Diagnostics;
using aoc;

Console.WriteLine("Day 25 - START");
var sw = Stopwatch.StartNew();
Part1();
Console.WriteLine($"END (after {sw.Elapsed.TotalSeconds} seconds)");

static void Part1()
{
	var grid = Input.ReadCharGrid();
	var count = 0;
	while (true)
	{
		count++;
		if (!Simulate(grid))
		{
			break;
		}
	}
	grid.Print();
	Console.WriteLine($"The sea cucumbers stop moving after {count} steps.");
}

static bool Simulate(Grid<char> grid)
{
	var moves = 0;
	var newGrid = grid.Clone();
	// east: >
	grid.ForEach(c =>
	{
		var x1 = (c.X + 1) % grid.Width;
		if (grid[c] == '>' && grid[x1, c.Y] == '.')
		{
			newGrid[c] = '.';	
			newGrid[x1, c.Y] = '>';
			moves++;	
		}
	});
	(grid, newGrid) = (newGrid, grid);
	grid.ForEach(c => newGrid[c] = grid[c]);
	// south: v
	grid.ForEach(c =>
	{
		var y1 = (c.Y + 1) % grid.Height;
		if (grid[c] == 'v' && grid[c.X, y1] == '.')
		{
			newGrid[c] = '.';	
			newGrid[c.X, y1] = 'v';
			moves++;	
		}
	});
	return moves > 0;
}