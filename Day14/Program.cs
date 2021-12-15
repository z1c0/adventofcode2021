using System.Diagnostics;
using System.Text;

Console.WriteLine("Day 14 - START");
var sw = Stopwatch.StartNew();
Part1(10);
Part2(40);
Console.WriteLine($"END (after {sw.Elapsed.TotalSeconds} seconds)");

static void Part1(int steps)
{
	var (template, rules) = ReadInput();
	var sb = new StringBuilder();
	for (var i = 0; i < steps; i++)
	{
		sb.Append(template.First());
		for (var j = 0; j < template.Length - 1; j ++)
		{
			var c = template[j];
			var cc = template[j + 1];
			var to = rules[(c, cc)];
			sb.Append(to);
			sb.Append(cc);
		}
		template = sb.ToString();
		sb.Length = 0;
	}
	var dict = new Dictionary<char, int>();
	foreach (var c in template)
	{
		if (!dict.TryAdd(c, 1))
		{
			dict[c]++;
		}
	}
	var min = dict.Min(p => p.Value);
	var minElement = dict.Single(p => p.Value == min);
	var max = dict.Max(p => p.Value);
	var maxElement = dict.Single(p => p.Value == max);
	Console.WriteLine($"Most common ({maxElement}) - least common ({minElement}) = {max - min}.");
}

static void Part2(int steps)
{
	var (template, rules) = ReadInput();
	var pairs = new Dictionary<(char c, char cc), long>();
	for (var j = 0; j < template.Length - 1; j ++)
	{
		var c = template[j];
		var cc = template[j + 1];
		pairs[(c, cc)] = 1;
	}

	for (var i = 0; i < steps; i++)
	{
		var newPairs = new Dictionary<(char c, char cc), long>();
		foreach (var k in pairs.Keys)
		{
			var n = pairs[k];
			var to = rules[k];

			var p1 = (k.c, to);
			if (!newPairs.TryAdd(p1, n))
			{
				newPairs[p1] += n;
			}
			var p2 = (to, k.cc);
			if (!newPairs.TryAdd(p2, n))
			{
				newPairs[p2] += n;
			}
		}
		pairs = newPairs;
	}

	var dict = new Dictionary<char, long>
	{
		{ pairs.First().Key.c, pairs.First().Value }
	};
	foreach (var p in pairs)
	{
		var cc = p.Key.cc;
		if (!dict.TryAdd(cc, p.Value))
		{
			dict[cc] += p.Value;
		}
	}
	var min = dict.Min(p => p.Value);
	var minElement = dict.Single(p => p.Value == min);
	var max = dict.Max(p => p.Value);
	var maxElement = dict.Single(p => p.Value == max);
	Console.WriteLine($"Most common ({maxElement}) - least common ({minElement}) = {max - min}.");
}

static (string Template, Dictionary<(char, char), char> Rules) ReadInput()
{
	var template = string.Empty;
	var rules = new Dictionary<(char, char), char>();
	foreach (var line in File.ReadAllLines("input.txt"))
	{
		if (!string.IsNullOrEmpty(line))
		{
			var tokens = line.Split(" -> ");
			if (tokens.Length == 1)
			{
				template = line;
			}
			else
			{
				rules.Add((tokens.First().First(), tokens.First().Last()), tokens.Last().Single());
			}
		}
	}
	return (template, rules);
}
