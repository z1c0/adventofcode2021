using System.Diagnostics;
using aoc;

Console.WriteLine("Day 20 - START");
var sw = Stopwatch.StartNew();
Part1(2);
Part1(50);
Console.WriteLine($"END (after {sw.Elapsed.TotalSeconds} seconds)");

static void Part1(int iterations)
{	
	var (index, image) = ReadInput();
	for (var i = 0; i < iterations; i++)
	{
		image = Enhance(index, image, i % 2 == 1);
	}
	if (iterations < 10)
	{
		Print(image);
	}
	var count = image.Count(p => p.Value);
	Console.WriteLine($"{count} pixels are lit.");
}

static Dictionary<(int X, int Y), bool> Enhance(string index, Dictionary<(int X, int Y), bool> image, bool shrink)
{
	const int d = 3;
	var tmp = new Dictionary<(int X, int Y), bool>();
	var minX = image.Min(p => p.Key.X) - d;
	var minY = image.Min(p => p.Key.Y) - d;
	var maxX = image.Max(p => p.Key.X) + d;
	var maxY = image.Max(p => p.Key.Y) + d;
	for (var y = minY; y <= maxY; y++)
	{
		for (var x = minX; x <= maxX; x++)
		{
			var i = GetPixelValue(x, y, image, index);
			tmp[(x, y)] = index[i] == '#';
		}
	}

	if (shrink)
	{
		image.Clear();
		minX = tmp.Min(p => p.Key.X) + d + 1;
		minY = tmp.Min(p => p.Key.Y) + d + 1;
		maxX = tmp.Max(p => p.Key.X) - d - 1;
		maxY = tmp.Max(p => p.Key.Y) - d - 1;
		for (var y = minY; y <= maxY; y++)
		{
			for (var x = minX; x <= maxX; x++)
			{
				image[(x, y)] = tmp[(x, y)];
			}
		}
	}
	else
	{
		image = tmp;
	}
	return image;
}

static int GetPixelValue(int x, int y, Dictionary<(int X, int Y), bool> image, string index)
{
	var bits = new bool[9];
	bits[0] = GetPixel(x - 1, y - 1, image);
	bits[1] = GetPixel(x    , y - 1, image);
	bits[2] = GetPixel(x + 1, y - 1, image);
	bits[3] = GetPixel(x - 1, y    , image);
	bits[4] = GetPixel(x    , y    , image);
	bits[5] = GetPixel(x + 1, y    , image);
	bits[6] = GetPixel(x - 1, y + 1, image);
	bits[7] = GetPixel(x    , y + 1, image);
	bits[8] = GetPixel(x + 1, y + 1, image);
	var i = ConvertBits(bits, index);
	return i;
}

static int ConvertBits(bool[] bits, string index)
{
	var i = 0;
	foreach (var b in bits)
	{
		i <<= 1;
		if (b)
		{
			i++;
		}
	}
	return i;
}

static bool GetPixel(int x, int y, Dictionary<(int X, int Y), bool> image)
{
	var p = (x, y);
	if (!image.ContainsKey(p))
	{
		image.Add(p, false);
	}
	return image[p];
}

static void Print(Dictionary<(int X, int Y), bool> image)
{
	var minX = image.Min(p => p.Key.X);
	var minY = image.Min(p => p.Key.Y);
	var maxX = image.Max(p => p.Key.X);
	var maxY = image.Max(p => p.Key.Y);
	for (var y = minY; y <= maxY; y++)
	{
		for (var x = minX; x <= maxX; x++)
		{
			Console.Write(image[(x, y)] ? '#' : '.');
		}
		Console.WriteLine();
	}
}

static (string Index, Dictionary<(int X, int Y), bool> Image) ReadInput()
{
	var lines = File.ReadAllLines("input.txt");
	var grid = Input.ParseCharGrid(lines.Skip(2));
	var image = new Dictionary<(int X, int Y), bool>();
	grid.ForEach(c => image[c] = grid[c] == '#');
	Debug.Assert(lines[0].Length == 512);
	return (lines[0], image);
}
