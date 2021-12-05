using System.Diagnostics;
using aoc;

Console.WriteLine("Day 5 - START");
var sw = Stopwatch.StartNew();
Part1(true);
Part1(false);
Console.WriteLine($"END (after {sw.Elapsed.TotalSeconds} seconds)");

static void Part1(bool horizontalOnly)
{
	var lines = ReadInput().ToList();
	var maxX = Math.Max(lines.Max(l => l.From.X), lines.Max(l => l.To.X));
	var maxY = Math.Max(lines.Max(l => l.From.Y), lines.Max(l => l.To.Y));
	var grid = new Grid<int>(maxX + 1, maxY + 1);
	foreach (var line in lines)
	{
		DrawLine(line, grid, horizontalOnly);
	}
	Print(grid);
}

static void Print(Grid<int> grid)
{
	//grid.Print();
	var count = grid.Count(x => x >= 2);
	Console.WriteLine($"In {count} areas, at least 2 lines overlap.");
}

static void DrawLine(((int X, int Y) From, (int X, int Y) To) line, Grid<int> grid, bool horizontalOnly)
{
	if (horizontalOnly && line.From.X != line.To.X && line.From.Y != line.To.Y)
	{
		return;
	}

	var dx = 0;
	if (line.From.X > line.To.X)
	{
		dx = -1;
	}
	else if (line.From.X < line.To.X)
	{
		dx = 1;
	}
	var dy = 0;
	if (line.From.Y > line.To.Y)
	{
		dy = -1;
	}
	else if (line.From.Y < line.To.Y)
	{
		dy = 1;
	}

	grid[line.From.X, line.From.Y]++;
	while (line.From.X != line.To.X || line.From.Y != line.To.Y)
	{
		line.From.X += dx;
		line.From.Y += dy;
		grid[line.From.X, line.From.Y]++;
	}
}

static IEnumerable<((int X, int Y) From, (int X, int Y) To)> ReadInput()
{
	foreach (var line in File.ReadAllLines("input.txt"))
	{
		var tokens = line.Split(' ');
		var tokens1 = tokens[0].Split(',');
		var tokens2 = tokens[2].Split(',');
		yield return (
			(int.Parse(tokens1[0]), int.Parse(tokens1[1])),
			(int.Parse(tokens2[0]), int.Parse(tokens2[1]))
		);
	}
}

