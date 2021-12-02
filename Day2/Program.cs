using System.Diagnostics;

Console.WriteLine("Day 2 - START");
var sw = Stopwatch.StartNew();
Part1();
Part2();
Console.WriteLine($"END (after {sw.Elapsed.TotalSeconds} seconds)");

static void Part1()
{
	var commands = ReadInput().ToList();
	var x = 0;
	var y = 0;
	foreach (var c in commands)
	{
		switch (c.Direction)
		{
			case Direction.Forward:
				x += c.Value;
				break;

			case Direction.Down:
				y += c.Value;
				break;

			case Direction.Up:
				y -= c.Value;
				break;

			default:
				throw new InvalidOperationException();
		}
	}
	Console.WriteLine($"Position: {x}/{y} => {x * y}");
}

static void Part2()
{
	var commands = ReadInput().ToList();
	var aim = 0;
	var x = 0;
	var y = 0;
	foreach (var c in commands)
	{
		switch (c.Direction)
		{
			case Direction.Forward:
				x += c.Value;
				y += aim * c.Value;
				break;

			case Direction.Down:
				aim += c.Value;
				break;

			case Direction.Up:
				aim -= c.Value;
				break;

			default:
				throw new InvalidOperationException();
		}
	}
	Console.WriteLine($"Position: {x}/{y} => {x * y}");
}

static IEnumerable<(Direction Direction, int Value)> ReadInput()
{
	foreach (var line in File.ReadAllLines("input.txt"))
	{
		var tokens = line.Split(' ');
		var direction = tokens[0] switch
		{
			"forward" => Direction.Forward,
			"down" => Direction.Down,
			"up" => Direction.Up,
			_ => throw new InvalidOperationException(),
		};
		yield return (direction, int.Parse(tokens[1]));
	}
}

enum Direction
{
	Forward,
	Down,
	Up,
}