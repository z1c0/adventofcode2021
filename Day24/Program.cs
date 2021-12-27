using System.Diagnostics;

Console.WriteLine("Day 24 - START");
var sw = Stopwatch.StartNew();
Part1();
Console.WriteLine($"END (after {sw.Elapsed.TotalSeconds} seconds)");

static void Part1()
{
	//Alu.Transpile(instructions);
	Transpiled.Test();
	
	var instructions = ReadInput().ToList();
	var alu = new Alu();
	var (largest, smallest) = Transpiled.Solve();
	Console.WriteLine($"Largest: {string.Join(string.Empty, largest)}");
	alu.Reset(largest);
	alu.Run(instructions);
	Console.WriteLine(alu);
	Console.WriteLine($"Smallest: {string.Join(string.Empty, smallest)}");
	alu.Reset(smallest);
	alu.Run(instructions);
	Console.WriteLine(alu);
}

static IEnumerable<(OpCode opCode, Operand op1, Operand op2)> ReadInput()
{
	static Operand ParseOperand(string s)
	{
		if (int.TryParse(s, out var i))
		{
			return new Operand(i);
		}
		return new Operand(s.Single());
	}
	foreach (var line in File.ReadAllLines("input.txt"))
	{
		var tokens = line.Split(' ');
		var opCode = tokens[0] switch
		{
			"inp" => OpCode.Inp,
			"add" => OpCode.Add,
			"mul" => OpCode.Mul,
			"div" => OpCode.Div,
			"mod" => OpCode.Mod,
			"eql" => OpCode.Eql,
			_ => throw new InvalidOperationException(line),
		};
		var op1 = ParseOperand(tokens[1]);
		var op2 = tokens.Length > 2 ? ParseOperand(tokens[2]) : new Operand();
		yield return (opCode, op1, op2);
	}
}
