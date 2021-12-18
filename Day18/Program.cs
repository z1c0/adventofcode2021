using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Text;

Console.WriteLine("Day 18 - START");
var sw = Stopwatch.StartNew();
Part1();
Part2();
Console.WriteLine($"END (after {sw.Elapsed.TotalSeconds} seconds)");

static void Part1()
{
	var numbers = ReadInput().ToList();
	var sum = numbers.First();
	foreach (var n in numbers.Skip(1))
	{
		sum += n;
	}
	SnailfishNumber.Reduce(sum);
	Console.WriteLine(sum);
	Console.WriteLine($"Magnitude: {sum.GetMagnitude()}");
}

static void Part2()
{
	var max = 0;
	var numbers = ReadInput().ToList();
	foreach (var n1 in numbers)
	{
		foreach (var n2 in numbers)
		{
			if (n1 != n2)
			{
				max = Math.Max(max, (n1 + n2).GetMagnitude());
			}
		}
	}
	Console.WriteLine($"Maximum magnitude: {max}");
}

static IEnumerable<SnailfishNumber> ReadInput()
{
	foreach (var line in File.ReadAllLines("input.txt"))
	{
		var pos = 0;
		yield return SnailfishNumber.Parse(line, ref pos);
	}
}

internal class SnailfishNumber
{
	internal SnailfishNumber? _parent;

	private SnailfishNumber? _left;
	private SnailfishNumber? _right;

	internal SnailfishNumber Left
	{ 
		get => _left!;
		private set
		{
			if (value != null)
			{
				value._parent = this;
			}
			_left = value;
		}
	}
	internal SnailfishNumber Right
	{ 
		get => _right!;
		private set
		{
			if (value != null)
			{
				value._parent = this;
			}
			_right = value;
		}
	}

	internal int Value { get; private set; }
	internal bool IsPair => Left != null && Right != null;

	public static SnailfishNumber operator +(SnailfishNumber n1, SnailfishNumber n2)
	{
		return Reduce(new SnailfishNumber
		{
			Left = n1.Clone(),
			Right = n2.Clone(),
		});
	}

	private SnailfishNumber Clone() =>
		IsPair ?
			new SnailfishNumber
			{
				Left = Left.Clone(),
				Right = Right.Clone(),
			} :
			new SnailfishNumber { Value = Value };

	internal static SnailfishNumber Reduce(SnailfishNumber n)
	{
		while (true)
		{
			if (!Explode(n, 0) && !Split(n))
			{
				break;
			}
		}
		return n;
	}

	private static bool Explode(SnailfishNumber n, int level)
	{
		if (n != null)
		{
			if (n.IsPair)
			{
				if (level == 4)
				{
					// explode
					var l = n.GetPreviousLeft();
					if (l != null)
					{
						l.Value += n.Left.Value;
					}
					var r = n.GetNextRight();
					if (r != null)
					{
						r.Value += n.Right.Value;
					}
					n.Left = null!;
					n.Right = null!;
					n.Value = 0;
					return true;
				}
			}

			if (Explode(n.Left, level + 1))
			{
				return true;
			}
			if (Explode(n.Right, level + 1))
			{
				return true;
			}
		}
		return false;
	}

	private static bool Split(SnailfishNumber n)
	{
		if (n != null)
		{
			if (!n.IsPair)
			{
				if (n.Value > 9)
				{
					// split
					n.Left = new SnailfishNumber() { Value = n.Value / 2 };
					n.Right = new SnailfishNumber() { Value = (n.Value + 1) / 2 };
					n.Value = 0;
					return true;
				}
			}
			else
			{
				if (Split(n.Left))
				{
					return true;
				}
				if (Split(n.Right))
				{
					return true;
				}
			}
		}
		return false;
	}

	private SnailfishNumber? GetPreviousLeft()
	{
		if (_parent == null)
		{
			return null;
		}
		if (_parent.Left != this)
		{
			return _parent.Left.GetFirstRightValue();
		}
		return _parent.GetPreviousLeft();
	}

	private SnailfishNumber? GetNextRight()
	{
		if (_parent == null)
		{
			return null;
		}
		if (_parent.Right != this)
		{
			return _parent.Right.GetFirstLeftValue();
		}
		return _parent.GetNextRight();
	}

	private SnailfishNumber GetFirstLeftValue() => 
		IsPair ? Left.GetFirstLeftValue() : this;

	private SnailfishNumber GetFirstRightValue() => 
		IsPair ? Right.GetFirstRightValue() : this;

	internal static SnailfishNumber Parse(string s, ref int pos)
	{
		static void Expect(string s, ref int pos, char c)
		{
			if (s[pos++] != c)
			{
				throw new InvalidOperationException($"'{c}' expected");
			}
		}
		static SnailfishNumber ParseNumber(string s, ref int pos)
		{
			if (s[pos] == '[')
			{
				return Parse(s, ref pos);
			}
			else
			{
				return new SnailfishNumber
				{
					Value = int.Parse(s[pos++].ToString()),
				};
			}
		}

		var number = new SnailfishNumber();
		Expect(s, ref pos, '[');
		number.Left = ParseNumber(s, ref pos);
		Expect(s, ref pos, ',');
		number.Right = ParseNumber(s, ref pos);
		Expect(s, ref pos, ']');
		return number;
	}

	public override string ToString()
	{
		var sb = new StringBuilder();
		PrintTo(this, sb);
		return sb.ToString();
	}

	private static void PrintTo(SnailfishNumber number, StringBuilder sb)
	{
		if (number.IsPair)
		{
			sb.Append('[');
			PrintTo(number.Left, sb);
			sb.Append(',');
			PrintTo(number.Right, sb);
			sb.Append(']');
		}
		else
		{
			sb.Append(number.Value);
		}
	}

	internal int GetMagnitude() =>
		IsPair ? 3 * Left.GetMagnitude() + 2 * Right.GetMagnitude() : Value;
}