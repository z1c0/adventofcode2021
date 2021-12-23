using System.Diagnostics;

Console.WriteLine("Day 19 - START");
var sw = Stopwatch.StartNew();
Part1();
Console.WriteLine($"END (after {sw.Elapsed.TotalSeconds} seconds)");

static void Part1()
{
	var scans = ReadInput();
	var scannerPositions = new HashSet<(int X, int Y, int Z)> { ( 0, 0, 0) };
	while (scans.Count > 1)
	{
		for (var i = 1; i < scans.Count; i++)
		{
			var s1 = scans[0];
			var s2 = scans[i];
			TryMatchMerge(s1, s2, scans, scannerPositions);
		}
	}
	Console.WriteLine($"Number of beacons: {scans.Single().Count}");

	var max = int.MinValue;
	foreach (var p1 in scannerPositions)
	{
		foreach (var p2 in scannerPositions)
		{
			if (p1 != p2)
			{
				var d = Math.Abs(p1.X - p2.X) + Math.Abs(p1.Y - p2.Y) + Math.Abs(p1.Z - p2.Z);
				max = Math.Max(max, d);
			}
		}
	}
	Console.WriteLine($"Maximum distance between two scanners: {max}");
}

static bool TryMatchMerge(HashSet<(int X, int Y, int Z)> scan1, HashSet<(int X, int Y, int Z)> scan2, List<HashSet<(int X, int Y, int Z)>> scans, HashSet<(int X, int Y, int Z)> scannerPositions)
{
	var dictX = new Dictionary<int, int>();
	var dictNX1 = new Dictionary<int, int>();
	var dictNX2 = new Dictionary<int, int>();
	var dictXY = new Dictionary<int, int>();
	var dictNXY = new Dictionary<int, int>();
	var dictXZ = new Dictionary<int, int>();
	var dictNXZ = new Dictionary<int, int>();

	var dictY = new Dictionary<int, int>();
	var dictNY1 = new Dictionary<int, int>();
	var dictNY2 = new Dictionary<int, int>();
	var dictYZ = new Dictionary<int, int>();
	var dictNYZ = new Dictionary<int, int>();
	var dictYX = new Dictionary<int, int>();
	var dictNYX = new Dictionary<int, int>();

	var dictZ = new Dictionary<int, int>();
	var dictNZ1 = new Dictionary<int, int>();
	var dictNZ2 = new Dictionary<int, int>();
	var dictZX = new Dictionary<int, int>();
	var dictNZX = new Dictionary<int, int>();
	var dictZY = new Dictionary<int, int>();
	var dictNZY = new Dictionary<int, int>();

	static void Add(Dictionary<int, int> dict, int d)
	{
		dict.TryAdd(d, 0);
		dict[d]++;
	}

	foreach (var e1 in scan1)
	{
		foreach (var e2 in scan2)
		{
			// x
			Add(dictX,    e1.X + e2.X);
			Add(dictNX2,  e1.X - e2.X);
			Add(dictNX1, -e1.X + e2.X);
			Add(dictXY,   e1.X + e2.Y);
			Add(dictNXY,  e1.X - e2.Y);
			Add(dictXZ,   e1.X + e2.Z);
			Add(dictNXZ,  e1.X - e2.Z);

			// y
			Add(dictY,    e1.Y + e2.Y);
			Add(dictNY2,  e1.Y - e2.Y);
			Add(dictNY1, -e1.Y + e2.Y);
			Add(dictYX,   e1.Y + e2.X);
			Add(dictNYX,  e1.Y - e2.X);
			Add(dictYZ,   e1.Y + e2.Z);
			Add(dictNYZ,  e1.Y - e2.Z);

			// z
			Add(dictZ,    e1.Z + e2.Z);
			Add(dictNZ2,  e1.Z - e2.Z);
			Add(dictNZ1, -e1.Z + e2.Z);
			Add(dictZX,   e1.Z + e2.X);
			Add(dictNZX,  e1.Z - e2.X);
			Add(dictZY,   e1.Z + e2.Y);
			Add(dictNZY,  e1.Z - e2.Y);
		}
	}

	static bool Collect(Dictionary<int, int> dict, List<(int v, int f1, int f2, bool switch1, bool switch2)> list, int f1, int f2, bool switch1, bool switch2)
	{
		if (dict.Any(e => e.Value >= 12))
		{
			var max = dict.Max(e => e.Value);
			foreach (var v in dict.Where(e => e.Value == max).Select(e => e.Key))
			{
				list.Add((v, f1, f2, switch1, switch2));
			}
			return true;
		}
		return false;
	}

	// x
	var xList = new List<(int x, int fx1, int fx2, bool switchXY, bool switchXZ)>();
	Collect(dictX, xList, 1, 1, false, false);
	Collect(dictNX1, xList, -1, 1, false, false);
	Collect(dictNX2, xList, 1, -1, false, false);
	Collect(dictXY, xList, 1, 1, true, false);
	Collect(dictNXY, xList, 1, -1, true, false);
	Collect(dictXZ, xList, 1, 1, false, true);
	Collect(dictNXZ, xList, 1, -1, false, true);

	// y
	var yList = new List<(int y, int fy1, int fy2, bool switchYZ, bool switchYX)>();
	Collect(dictY, yList, 1, 1, false, false);
	Collect(dictNY1, yList, -1, 1, false, false);
	Collect(dictNY2, yList, 1, -1, false, false);
	Collect(dictYZ, yList, 1, 1, true, false);
	Collect(dictNYZ, yList, 1, -1, true, false);
	Collect(dictYX, yList, 1, 1, false, true);
	Collect(dictNYX, yList, 1, -1, false, true); 

	// z
	var zList = new List<(int z, int fz1, int fz2, bool switchZX, bool switchZY)>();
	Collect(dictZ, zList, 1, 1, false, false);
	Collect(dictNZ1, zList, -1, 1, false, false);
	Collect(dictNZ2, zList, 1, -1, false, false);
	Collect(dictZX, zList, 1, 1, true, false);
	Collect(dictNZX, zList, 1, -1, true, false);
	Collect(dictZY, zList, 1, 1, false, true);
	Collect(dictNZY, zList, 1, -1, false, true);

	static (int, int, int) MakePoint((int X, int Y, int Z) e2, (int x, int fx1, int fx2, bool switchXY, bool switchXZ) tx, (int y, int fy1, int fy2, bool switchYZ, bool switchYX) ty, (int z, int fz1, int fz2, bool switchZX, bool switchZY) tz)
	{
		var ex = e2.X;
		var ey = e2.Y;
		var ez = e2.Z;
		if (tx.switchXY)
		{
			ex = e2.Y;
		}
		if (tx.switchXZ)
		{
			ex = e2.Z;
		}
		if (ty.switchYZ)
		{
			ey = e2.Z;
		}
		if (ty.switchYX)
		{
			ey = e2.X;
		}
		if (tz.switchZX)
		{
			ez = e2.X;
		}
		if (tz.switchZY)
		{
			ez = e2.Y;
		}
		return (tx.fx1 * (tx.x - tx.fx2 * ex), ty.fy1 * (ty.y - ty.fy2 * ey), tz.fz1 * (tz.z - tz.fz2 * ez));
	}

	foreach (var tz in zList)
	{
		foreach (var ty in yList)
		{
			foreach (var tx in xList)
			{
				var p = (tx.x, ty.y, tz.z);
				
				var count = 0;
				foreach (var e2 in scan2)
				{
					p = MakePoint(e2, tx, ty, tz);
					if (scan1.Contains(p))
					{
						count++;
					}
				}

				if (count >= 12)
				{
					foreach (var e2 in scan2)
					{
						scannerPositions.Add((tx.fx1 * tx.x, ty.fy1 * ty.y, tz.fz1 * tz.z));
						p = MakePoint(e2, tx, ty, tz);
						if (!scan1.Contains(p))
						{
							scan1.Add(p);
						}
					}
					scans.Remove(scan2);
					return true;
				}
			}
		}
	}
	return false;
}

static List<HashSet<(int X, int Y, int Z)>> ReadInput()
{
	var scanners = new List<HashSet<(int X, int Y, int Z)>>();
	foreach (var line in File.ReadAllLines("input.txt"))
	{
		if (!string.IsNullOrEmpty(line))
		{
			if (line.StartsWith("---"))
			{
				scanners.Add(new ());
			}
			else
			{
				var tokens = line.Split(',');
				scanners.Last().Add((
					int.Parse(tokens[0]),
					int.Parse(tokens[1]),
					int.Parse(tokens[2])
				));
			}
		}
	}
	return scanners;
}
