using System.Diagnostics;

Console.WriteLine("Day 16 - START");
var sw = Stopwatch.StartNew();
Part1();
Console.WriteLine($"END (after {sw.Elapsed.TotalSeconds} seconds)");

static void Part1()
{
	var bits = ReadInput();
	var pos = 0;
	var p = DecodePacket(bits, ref pos);
	Console.WriteLine($"Header version sum: {GetHeaderVersionSum(p)}");
	Console.WriteLine($"Evaluation result: {Evaluate(p)}");
}

static long Evaluate(Packet packet)
{
	return packet.TypeId switch
	{
		// sum
		0 => packet.SubPackets.Sum(p => Evaluate(p)),
		// product
		1 => packet.SubPackets.Aggregate(1L, (v, p)  => Evaluate(p) * v),
		// minimum
		2 => packet.SubPackets.Min(p => Evaluate(p)),
		// maximum
		3 => packet.SubPackets.Max(p => Evaluate(p)),
		// literal
		4 => packet.Value,
		// greater than
		5 => Evaluate(packet.SubPackets[0]) > Evaluate(packet.SubPackets[1]) ? 1L : 0L,
		// less than
		6 => Evaluate(packet.SubPackets[0]) < Evaluate(packet.SubPackets[1]) ? 1L : 0L,
		// equal
		7 => Evaluate(packet.SubPackets[0]) == Evaluate(packet.SubPackets[1]) ? 1L : 0L,
		_ => throw new InvalidOperationException($"TypeId: {packet.TypeId}"),
	};
}

static int GetHeaderVersionSum(Packet packet)
{
	var sum = packet.Version;
	packet.SubPackets.ForEach(p => sum += GetHeaderVersionSum(p));
	return sum;
}

static Packet DecodePacket(List<int> bits, ref int pos)
{
	var version = Decode(bits, ref pos, 3);
	var typeId = Decode(bits, ref pos, 3);
	var packet = new Packet(version, typeId);
	if (typeId == 4)  // literal
	{
		packet.Value = DecodeLiteral(bits, ref pos);
	}
	else // operator
	{
		packet.SubPackets.AddRange(DecodeOperator(bits, ref pos));
	}
	return packet;
}

static List<Packet> DecodeOperator(List<int> bits, ref int pos)
{
	var subPackets = new List<Packet>();
	var lengthTypeId = Decode(bits, ref pos, 1);
	if (lengthTypeId == 0)
	{
		var length = Decode(bits, ref pos, 15);
		var tmp = pos + length;
		while (pos < tmp)
		{
			subPackets.Add(DecodePacket(bits, ref pos));
		}
	}
	else
	{
		var count = Decode(bits, ref pos, 11);
		while (count-- > 0)
		{
			subPackets.Add(DecodePacket(bits, ref pos));
		}
	}
	return subPackets;
}

static long DecodeLiteral(List<int> bits, ref int pos)
{
	var literal = 0L;
	while (true)
	{
		var prefix = Decode(bits, ref pos, 1);
		var n = Decode(bits, ref pos, 4);
		literal |= (uint)n;
		literal <<= 4;
		if (prefix == 0)
		{
			break;
		}
	}
	literal >>= 4;
	return literal;
}

static int Decode(List<int> bits, ref int pos, int length)
{
	var n = 0;
	while (length > 1)
	{
		n += bits[pos++];
		n <<= 1;
		length--;
	}
	n += bits[pos++];
	return n;
}

static List<int> ReadInput()
{
	var bits = new List<int>();
	var table = new Dictionary<char, int[]>
	{
		{ '0', new[] { 0, 0, 0, 0 } },
		{ '1', new[] { 0, 0, 0, 1 } },
		{ '2', new[] { 0, 0, 1, 0 } },
		{ '3', new[] { 0, 0, 1, 1 } },
		{ '4', new[] { 0, 1, 0, 0 } },
		{ '5', new[] { 0, 1, 0, 1 } },
		{ '6', new[] { 0, 1, 1, 0 } },
		{ '7', new[] { 0, 1, 1, 1 } },
		{ '8', new[] { 1, 0, 0, 0 } },
		{ '9', new[] { 1, 0, 0, 1 } },
		{ 'A', new[] { 1, 0, 1, 0 } },
		{ 'B', new[] { 1, 0, 1, 1 } },
		{ 'C', new[] { 1, 1, 0, 0 } },
		{ 'D', new[] { 1, 1, 0, 1 } },
		{ 'E', new[] { 1, 1, 1, 0 } },
		{ 'F', new[] { 1, 1, 1, 1 } },
	};
	foreach (var c in File.ReadAllText("input.txt"))
	{
		bits.AddRange(table[c]);
	}
	return bits;
}

internal record Packet(int Version, int TypeId)
{
	internal long Value { get; set; }
	internal List<Packet> SubPackets { get; } = new();
}
