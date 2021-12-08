using System.Diagnostics;

Console.WriteLine("Day 8 - START");
var sw = Stopwatch.StartNew();
Part1();
Part2();
Console.WriteLine($"END (after {sw.Elapsed.TotalSeconds} seconds)");

static void Part1()
{
	var data = ReadInput().ToList();
	var count = 0;
	foreach (var (_, output) in data)
	{
		count += output.Count(IsUniqueDigit);
	}
	Console.WriteLine($"{count} unique digits appear.");
}

static void Part2()
{
	var data = ReadInput().ToList();
	var sum = 0;
	foreach (var (input, output) in data)
	{
		var dict = DecodeInput(input);
		sum += DecodeOutput(output, dict);
	}
	Console.WriteLine($"The sum of the output values is: {sum}.");
}

static int DecodeOutput(List<string> output, Dictionary<string, int> dict)
{
	return
		dict[output[0]] * 1000 +
		dict[output[1]] * 100 +
		dict[output[2]] * 10 +
		dict[output[3]];
}

static Dictionary<string, int> DecodeInput(List<string> inputs)
{
	var dict = new Dictionary<string, int>();
	void AddRemove(string s, int v)
	{
		dict.Add(s, v);
		inputs.Remove(s);
	}

	var one = inputs.Single(i => i.Length == 2);
	AddRemove(one, 1);
	var four = inputs.Single(i => i.Length == 4);
	AddRemove(four, 4);
	var seven = inputs.Single(i => i.Length == 3);
	AddRemove(seven, 7);
	var eight = inputs.Single(i => i.Length == 7);
	AddRemove(eight, 8);

	var three = inputs.Single(i => i.Length == 5 && ContainsSegment(i, one));
	AddRemove(three, 3);
	var nine = inputs.Single(i => i.Length == 6 && SegmentsDiff(i, three) == 1);
	AddRemove(nine, 9);
	var zero = inputs.Single(i => i.Length == 6 && ContainsSegment(i, one));
	AddRemove(zero, 0);
	var six = inputs.Single(i => i.Length == 6);
	AddRemove(six, 6);
	var five = inputs.Single(i => i.Length == 5 && ContainsSegment(six, i));
	AddRemove(five, 5);
	var two = inputs.Single();
	AddRemove(two, 2);

	return dict;
}

static bool ContainsSegment(string candidate, string pattern)
{
	foreach (var c in pattern)
	{
		if (!candidate.Contains(c))
		{
			return false;
		}
	}
	return true;
}

static int SegmentsDiff(string segment1, string segment2)
{
	return segment1.Except(segment2).Count();
}

static bool IsUniqueDigit(string digit)
{
	return
		digit.Length == 2 ||  // one
		digit.Length == 3 ||  // seven
		digit.Length == 4 ||  // four
		digit.Length == 7;    // eight
}

static IEnumerable<(List<string> Input, List<string> Output)> ReadInput()
{
	static IEnumerable<string> Sanitize(string data)
	{
		foreach (var token in data.Split(' ', StringSplitOptions.RemoveEmptyEntries))
		{
			yield return string.Concat(token.OrderBy(c => c));
		}
	}
	foreach (var line in File.ReadAllLines("input.txt"))
	{
		var tokens = line.Split('|');
		yield return (Sanitize(tokens[0]).ToList(), Sanitize(tokens[1]).ToList());
	}
}
