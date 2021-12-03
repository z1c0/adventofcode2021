using System.Diagnostics;

Console.WriteLine("Day 3 - START");
var sw = Stopwatch.StartNew();
Part1();
Part2();
Console.WriteLine($"END (after {sw.Elapsed.TotalSeconds} seconds)");

static void Part1()
{
	var epsilonDigits = new List<int>();
	var gammaDigits = new List<int>();
	var diagnostics = ReadInput().ToList();
	var oxygen = diagnostics.ToList();
	var co2Scrubber = diagnostics.ToList();

	for (var x = 0; x < diagnostics.First().Count; x++)
	{
		var countOne = 0;
		for (var y = 0; y < diagnostics.Count; y++)
		{	
			if (diagnostics[y][x] == 1)
			{
				countOne++;
			}
		}
		if (countOne > diagnostics.Count / 2)
		{
			gammaDigits.Add(1);
			epsilonDigits.Add(0);
		}
		else
		{
			gammaDigits.Add(0);
			epsilonDigits.Add(1);
		}
	}

	var gamma = Convert(gammaDigits);
	var epsilon = Convert(epsilonDigits);
	Console.WriteLine($"epsilon ({epsilon}) * gamma ({gamma}) = {epsilon * gamma}");
}

static void Part2()
{
	var numbers = ReadInput();
	var oxygen = FindRating(numbers.ToList(), false);
	var co2 = FindRating(numbers.ToList(), true);
	Console.WriteLine($"Oxygen ({oxygen}) * CO2 ({co2}) = {oxygen * co2}");
}

static int FindRating(List<List<int>> numbers, bool flip)
{
	var criterion = 1;
	for (var x = 0; x < numbers.First().Count; x++)
	{
		var ones = 0;
		var zeros = 0;
		for (var y = 0; y < numbers.Count; y++)
		{	
			if (numbers[y][x] == 1)
			{
				ones++;
			}
			else
			{
				zeros++;
			}
		}
		if (ones >= zeros)
		{
			criterion = flip ? 1 : 0;
		}
		else
		{
			criterion = flip ? 0 : 1;
		}

		numbers.RemoveAll(n => n[x] != criterion);
		if (numbers.Count == 1)
		{
			break;
		}
	}
	return Convert(numbers.Single());
}

static int Convert(List<int> digits)
{
	var n = 0;
	foreach (var d in digits)
	{
		n <<= 1;
		n |= d;
	}
	return n;
}


static IEnumerable<List<int>> ReadInput()
{
	foreach (var line in File.ReadAllLines("input.txt"))
	{
		var digits = new List<int>();
		foreach (var d in line)
		{
			digits.Add(d == '1' ? 1 : 0);
		}
		yield return digits;
	}
}
