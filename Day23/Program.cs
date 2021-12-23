using System.Diagnostics;
using aoc;

Console.WriteLine("Day 23 - START");
var sw = Stopwatch.StartNew();
Part1("input.txt");
Part1("input2.txt");
Console.WriteLine($"END (after {sw.Elapsed.TotalSeconds} seconds)");

static void Part1(string fileName)
{	
	var grid = Input.ParseCharGrid(fileName);
	grid.Print();
	var cache = new Dictionary<string, int>();
	var minEnergy = int.MaxValue;
	Simulate(new State(grid), cache, ref minEnergy);
	Console.WriteLine($"Minimum energy required: {minEnergy}");
	Console.WriteLine();
}

static void Simulate(State state, Dictionary<string, int> cache, ref int minEnergy)
{
	var queue = new PriorityQueue<State, int>();
	queue.Enqueue(state, state.Energy);
	while (queue.Count > 0)
	{
		var current = queue.Dequeue();
		if (Check(current, cache, ref minEnergy))
		{
			foreach (var pod in current.Amphipods)
			{
				var destinations = BFS(pod.Pos, current.Grid);
				foreach (var d in destinations)
				{
					var newState = TryMove(pod, d, current);
					if (newState != null)
					{
						queue.Enqueue(newState, newState.Energy);
					}
				}
			}
		}
	}
}

static bool Check(State state, Dictionary<string, int> cache, ref int minEnergy)
{
	var fingerPrint = state.Encode();
	if (cache.ContainsKey(fingerPrint) && cache[fingerPrint] <= state.Energy)
	{
		return false;
	}
	cache[fingerPrint] = state.Energy;

	if (state.Energy > minEnergy)
	{
		return false;
	}

	if (state.IsDone())
	{
		minEnergy = Math.Min(minEnergy, state.Energy);
	}

	return true;
}

static State? TryMove((char Kind, (int X, int Y) Pos) pod, ((int X, int Y) P, int D) d, State state)
{
	static bool IsHallway((int _, int Y) p) => p.Y == 1;
	static bool IsRoom((int, int) p) => !IsHallway(p);
	static bool IsDestinationRoom(char kind, (int X, int _) p)
	{
		return kind switch
		{
			'A' => p.X == 3,
			'B' => p.X == 5,
			'C' => p.X == 7,
			'D' => p.X == 9,
			_ => throw new InvalidOperationException(),
		};
	}
	static bool IsOtherAmphipodInRoom(char kind, (int X, int _) p, State state)
	{
		return state.Amphipods.Any(a => a.Pos.X == p.X && a.Kind != kind);
	}

	// If the amphipod has arrived in its room.
	if (IsDestinationRoom(pod.Kind, pod.Pos))
	{
		var below = state.Grid[pod.Pos.X, pod.Pos.Y + 1];
		if (pod.Pos.Y >= 2 && (below == '#' || below == pod.Kind))
		{
			return null;
		}
	}
	// Once an amphipod stops moving in the hallway, it will stay in that spot until it can move into a room.
	if (IsHallway(pod.Pos) && IsHallway(d.P))
	{
		return null;
	}
	// Never stop on the space immediately outside any room.
	if (d.P.Y == 1 && (d.P.X == 3 || d.P.X == 5 || d.P.X == 7 || d.P.X == 9))
	{
		return null;
	}
	// Never move from the hallway into a room unless that room is their destination room
	// and that room contains no amphipods which do not also have that room as their own destination.
	if (IsRoom(d.P))
	{
		if (!IsDestinationRoom(pod.Kind, d.P))
		{
			return null;
		}
		if (IsOtherAmphipodInRoom(pod.Kind, d.P, state))
		{
			return null;
		}
	}

	// move
	var newGrid = state.Grid.Clone();
	newGrid[pod.Pos] = '.';
	newGrid[d.P] = pod.Kind;
	// energy cost
	var e = d.D;
	e *= pod.Kind switch
	{
		'A' => 1,
		'B' => 10,
		'C' => 100,
		'D' => 1000,
		_ => throw new NotImplementedException(),
	};
	return new State(newGrid)
	{
		Energy = state.Energy + e
	};
}

static List<((int X, int Y) P, int D)> BFS((int X, int Y) pos, Grid<char> grid)
{
	var visited = new HashSet<(int x, int y)>() { pos };
	var queue = new Queue<((int x, int y) p, int d)>();
	queue.Enqueue((pos, 0));
	var result = new List<((int x, int y) p, int d)>();
	while (queue.Any())
	{
		var (current, d) = queue.Dequeue();
		foreach (var n in grid.GetAdjacent4(current))
		{
			if (grid[n] == '.' && !visited.Contains(n))
			{
				visited.Add(n);
				result.Add((n, d + 1));
				queue.Enqueue((n, d + 1));
			}
		}
	}
	return result.OrderByDescending(e => e.p.y).ToList();
}

public class State
{
	public List<(char Kind, (int X, int Y) Pos)> Amphipods { get; } = new();
	public Grid<char> Grid { get; }
	public int Energy { get; set; }

	internal State(Grid<char> grid)
	{
		Grid = grid;
		foreach(var p in grid.FindAll('A').Concat(grid.FindAll('B')).Concat(grid.FindAll('C')).Concat(grid.FindAll('D')))
		{
			Amphipods.Add((grid[p], p));
		}
	}

	internal string Encode()
	{
		return string.Join('/', Amphipods);
	}

	internal bool IsDone()
	{
		foreach (var a in Amphipods)
		{
			if (a.Pos.Y == 1)
			{
				return false;
			}
			if (a.Kind == 'A' && a.Pos.X != 3)
			{
				return false;
			}
			if (a.Kind == 'B' && a.Pos.X != 5)
			{
				return false;
			}
			if (a.Kind == 'C' && a.Pos.X != 7)
			{
				return false;
			}
			if (a.Kind == 'D' && a.Pos.X != 9)
			{
				return false;
			}
		}
		return true;
	}
}