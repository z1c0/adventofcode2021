using System.Diagnostics;

Console.WriteLine("Day 21 - START");
var sw = Stopwatch.StartNew();
Part1();
Part2();
Console.WriteLine($"END (after {sw.Elapsed.TotalSeconds} seconds)");

static void Part1()
{	
	var (pos1, pos2) = ReadInput();
	var score1 = 0;
	var score2 = 0;
	var i = 0;
	var rolls = 0;
	while (true)
	{
		score1 += Move(ref pos1, ref i, ref rolls);
		if (Check(score1, score2, rolls))
		{
			break;
		}
		score2 += Move(ref pos2, ref i, ref rolls);
		if (Check(score1, score2, rolls))
		{
			break;
		}
	}
}

static void Part2()
{
	var (pos1, pos2) = ReadInput();
	var cache = new Dictionary<string, (long, long)>();
	var (wins1, wins2) = RollDirac(true, pos1, pos2, 0, 0, 1, cache);
	Console.WriteLine($"Player 1 wins {wins1} times.");
	Console.WriteLine($"Player 2 wins {wins2} times.");
}

static (long wins1, long wins2) RollDirac(bool player1, int pos1, int pos2, int score1, int score2, int roll, Dictionary<string, (long, long)> cache)
{
	var (w1a, w2a) = SplitUniverse(player1, pos1, pos2, score1, score2, roll, 1, cache);
	var (w1b, w2b) = SplitUniverse(player1, pos1, pos2, score1, score2, roll, 2, cache);
	var (w1c, w2c) = SplitUniverse(player1, pos1, pos2, score1, score2, roll, 3, cache);
	return (w1a + w1b + w1c, w2a + w2b + w2c);
}

static (long wins1, long wins2) SplitUniverse(bool player1, int pos1, int pos2, int score1, int score2, int roll, int dieValue, Dictionary<string, (long, long)> cache)
{
	var fingerPrint = $"{player1}:{pos1}:{pos2}:{score1}:{score2}:{roll}:{dieValue}";
	if (cache.ContainsKey(fingerPrint))
	{
		return cache[fingerPrint];
	}
	var wins1 = 0L;
	var wins2 = 0L;
	if (roll == 3)
	{
		if (player1)
		{
			pos1 = (pos1 + dieValue - 1) % 10 + 1;
			score1 += pos1;
			if (score1 >= 21)
			{
				return (1, 0);
			}
		}
		else
		{
			pos2 = (pos2 + dieValue - 1) % 10 + 1;
			score2 += pos2;
			if (score2 >= 21)
			{
				return (0, 1);
			}
		}
		// switch players
		var (w1, w2) = RollDirac(!player1, pos1, pos2, score1, score2, 1, cache);
		wins1 += w1;
		wins2 += w2;
	}
	else
	{
		var (w1a, w2a) = SplitUniverse(player1, pos1, pos2, score1, score2, roll + 1, dieValue + 1, cache);
		var (w1b, w2b) = SplitUniverse(player1, pos1, pos2, score1, score2, roll + 1, dieValue + 2, cache);
		var (w1c, w2c) = SplitUniverse(player1, pos1, pos2, score1, score2, roll + 1, dieValue + 3, cache);
		wins1 += w1a + w1b + w1c;
		wins2 += w2a + w2b + w2c;
	}	

	cache[fingerPrint] = (wins1, wins2);
	return (wins1, wins2);
}

static bool Check(int score1, int score2, int rolls)
{
	var max = Math.Max(score1, score2);
	var min = Math.Min(score1, score2);
	if (max >= 1000)
	{
		Console.WriteLine($"Total: {min} * {rolls} = {min * rolls}");
		return true;
	}
	return false;
}

static int Move(ref int pos, ref int i, ref int rolls)
{
	static int Roll(ref int i, ref int rolls)
	{
		rolls++;
		i++;
		if (i > 100)
		{
			i = 1;
		}
		return i;
	}
	var i1 = Roll(ref i, ref rolls);
	var i2 = Roll(ref i, ref rolls);
	var i3 = Roll(ref i, ref rolls);
	pos = (pos + i1 + i2 + i3 - 1) % 10 + 1;
	return pos;
}

static (int Pos1, int Pos2) ReadInput()
{
	var lines = File.ReadAllLines("input.txt");
	return
	(
		int.Parse(lines[0].Split(": ")[1]),
		int.Parse(lines[1].Split(": ")[1])
	);
}
