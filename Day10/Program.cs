using System.Diagnostics;

Console.WriteLine("Day 10 - START");
var sw = Stopwatch.StartNew();
var incompleteLines = Part1();
Part2(incompleteLines.ToList());
Console.WriteLine($"END (after {sw.Elapsed.TotalSeconds} seconds)");

static IEnumerable<string> Part1()
{
	var totalErrorScore = 0;
	var lines = ReadInput().ToList();
	foreach (var line in lines)
	{
		var pos = 0;
		var errorScore = 0;
		var completionScore = 0L;
		ParseChunk(line, ref pos, ref errorScore, ref completionScore);
		if (errorScore == 0) 
		{
			yield return line;
		}
		totalErrorScore += errorScore;
	}
	Console.WriteLine($"Error score: {totalErrorScore}");
}

static void Part2(List<string> lines)
{
	var completionScores = new List<long>();
	foreach (var line in lines)
	{
		var pos = 0;
		var errorScore = 0;
		var completionScore = 0L;
		while (pos < line.Length)
		{
			ParseChunk(line, ref pos, ref errorScore, ref completionScore);
		}
		completionScores.Add(completionScore);
	}
	completionScores.Sort();
	Console.WriteLine($"Middle completion score: {completionScores[completionScores.Count / 2]}");
}

static char GetCloseChar(char openChar)
{
	return openChar switch
	{
		'(' => ')',
		'[' => ']',
		'{' => '}',
		'<' => '>',
		_ => '\0',
	};
}

static bool IsStartOfChunk(char openChar) => GetCloseChar(openChar) != '\0';

static bool ParseChunk(string line, ref int pos, ref int errorScore, ref long completionScore)
{
	var openChar = line[pos++];
	var expected = GetCloseChar(openChar);
	while (pos < line.Length)
	{
		var ch = line[pos];
		if (IsStartOfChunk(ch))
		{
			if (!ParseChunk(line, ref pos, ref errorScore, ref completionScore))
			{
				return false;
			}
		}
		else
		{
			if (ch != expected)
			{
				errorScore += ch switch
				{
					')' => 3,
					']' => 57,
					'}' => 1197,
					'>' => 25137,
					_ => throw new InvalidOperationException(),
				};
				//Console.WriteLine($"Expected: {expected}, found {ch}.");
				return false;
			}
			else
			{
				pos++;
				return true;
			}
		}
	}

  completionScore *= 5;
	completionScore += expected switch
	{
		')' => 1,
		']' => 2,
		'}' => 3,
		'>' => 4,
		_ => throw new InvalidOperationException(),
	};
	return true;
}

static IEnumerable<string> ReadInput()
{
	return File.ReadAllLines("input.txt");
}
