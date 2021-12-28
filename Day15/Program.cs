using System.Diagnostics;
using aoc;

Console.WriteLine("Day 15 - START");
var sw = Stopwatch.StartNew();
Part1(false);
Part1(true);
Console.WriteLine($"END (after {sw.Elapsed.TotalSeconds} seconds)");

static void Part1(bool enlargeGrid)
{
	var grid = Input.ReadIntGrid();
	if (enlargeGrid)
	{
		grid = EnlargeGrid(grid);
	}
	FindPath(grid);
}

static Grid<int> EnlargeGrid(Grid<int> grid)
{
	var largeGrid = new Grid<int>(grid.Width * 5, grid.Height * 5);
	for (var y = 0; y < largeGrid.Height; y += grid.Height)
	{
		for (var x = 0; x < largeGrid.Width; x += grid.Width)
		{
			grid.ForEach((p) =>
			{
				var pp = (x + p.X, y + p.Y);
				var risk = (grid[p] + x + y) % 9;
				if (risk == 0) 
				{
					risk = 9;
				}
				largeGrid[pp] = risk;
			});
		}
	}
	return largeGrid;
}

static void FindPath(Grid<int> grid)
{
	var visitedNodes = new Dictionary<(int X, int Y), int>()
	{
		{ (0, 0), 0 }
	};
	var queue = new PriorityQueue<(int X, int Y), int>();
	queue.Enqueue((0, 0), 0);
	while (queue.Count > 0)
	{
		var p = queue.Dequeue();
		var adjacentStates = grid.GetAdjacent4(p).ToList();
		if (adjacentStates.Any())
		{
			foreach (var a in adjacentStates)
			{
				var risk = grid[a];
				if (!visitedNodes.ContainsKey(a) || visitedNodes[a] > risk + visitedNodes[p])
				{
					visitedNodes[a] = risk + visitedNodes[p];
					if (a.X == grid.Width - 1 && a.Y == grid.Height - 1)
					{
						Console.WriteLine($"Path found, total risk: {visitedNodes[a]}");
					}
					queue.Enqueue(a, risk);
				}
			}
		}
	}
}
