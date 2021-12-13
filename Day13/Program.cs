using System.Diagnostics;
using aoc;

Console.WriteLine("Day 13 - START");
var sw = Stopwatch.StartNew();
Part1();
Console.WriteLine($"END (after {sw.Elapsed.TotalSeconds} seconds)");

static void Part1()
{
	var (grid, folds) = ReadInput();
	foreach (var f in folds)
	{
		grid = Fold(grid, f);
		Console.WriteLine($"Number of visible dots: {grid.Count('#')}");
	}
	grid.Print();
}

static Grid<char> Fold(Grid<char> grid, (bool Vertical, int Value) fold)
{
	if (fold.Vertical)
	{
		grid.ForEach(p => {
			if (p.Y > fold.Value && grid[p] == '#')
			{
				var y = fold.Value - (p.Y - fold.Value);
				grid[p.X, y] = grid[p];
			}
		});
		return grid.Resize(grid.Width, grid.Height / 2);
	}
	else
	{
		grid.ForEach(p => {
			if (p.X > fold.Value && grid[p] == '#')
			{
				var x = fold.Value - (p.X - fold.Value);
				grid[x, p.Y] = grid[p];
			}
		});
		return grid.Resize(grid.Width / 2, grid.Height);
	}
}

static (Grid<char> Grid, List<(bool Vertical, int Value)> Folds) ReadInput()
{
	var points = new List<(int, int)>();
	var folds = new List<(bool Vertical, int Value)>();
	foreach (var line in File.ReadAllLines("input.txt"))
	{
		if (!string.IsNullOrEmpty(line))
		{
			if (char.IsDigit(line.First()))
			{
				var tokens = line.Split(',');
				points.Add((int.Parse(tokens[0]), int.Parse(tokens[1])));
			}
			else
			{
				var tokens = line.Split('=');
				folds.Add((tokens[0].Last() == 'y', int.Parse(tokens[1])));
			}
		}
	}
	var width = points.Max(p => p.Item1) + 1;
	var height = points.Max(p => p.Item2) + 1;
	var grid = new Grid<char>(width, height);
	grid.Fill('.');
	foreach (var p in points)
	{
		grid[p] = '#';
	}
	return (grid, folds);
}
