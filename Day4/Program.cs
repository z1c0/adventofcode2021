using System.Diagnostics;

Console.WriteLine("Day 4 - START");
var sw = Stopwatch.StartNew();
Part1(true);
Part1(false);
Console.WriteLine($"END (after {sw.Elapsed.TotalSeconds} seconds)");

static void Part1(bool firstWin)
{
	var (Numbers, Boards) = ReadInput();
	foreach (var n in Numbers)
	{
		foreach (var b in Boards)
		{
			if (!b.HasWon)
			{
				b.Mark(n);
				if (b.CheckForWin())
				{
					var unmarked = b.GetUnmarkedScore();
					Console.WriteLine($"Final Score: {unmarked} * {n} = {unmarked * n}");
					if (firstWin)
					{
						return;
					}
				}
			}
		}
	}
}

static (List<int> Numbers, List<Board> Boards) ReadInput()
{
	var lines = File.ReadAllLines("input.txt");
	var numbers = lines.First().Split(',').Select(t => int.Parse(t)).ToList();
	var boards = new List<Board>();
	Board? board = null;
	foreach (var line in lines.Skip(1))
	{
		if (string.IsNullOrEmpty(line))
		{
			board = new Board();
			boards.Add(board);
		}
		else
		{
			var row = line.Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(t => int.Parse(t));
			board?.AddRow(row);
		}
	}
	return (numbers, boards);
}

internal class Board
{
	private readonly List<List<(int Number, bool Marked)>> _rows = new();

	internal bool HasWon { get; private set; }

	internal void AddRow(IEnumerable<int> numbers)
	{
		var row = new List<(int, bool)>();
		foreach (var n in numbers)
		{
			row.Add((n, false));
		}
		_rows.Add(row);
	}

	internal bool CheckForWin()
	{
		foreach (var row in _rows)
		{
			if (row.All(e => e.Marked))
			{
				HasWon = true;
				return true;
			}
		}
		for (var col  = 0; col < _rows.First().Count; col++)
		{
			var count = 0;
			for (var row = 0; row < _rows.Count; row++)
			{
				if (_rows[row][col].Marked)
				{
					count++;
				}
			}
			if (count == _rows.Count)
			{
				HasWon = true;
				return true;
			}
		}
		return false;
	}

	internal int GetUnmarkedScore()
	{
		var score = 0;
		foreach (var row in _rows)
		{
			score += row.Where(r => !r.Marked).Sum(r => r.Number);
		}
		return score;
	}

	internal void Mark(int n)
	{
		foreach (var row in _rows)
		{
			for (var i = 0; i < row.Count; i++)
			{
				if (row[i].Number == n)
				{
					row[i] = (n, true);
				}
			}
		}
	}
}


