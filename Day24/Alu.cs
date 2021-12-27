internal enum OpCode
{
	Inp,
	Mul,
	Eql,
	Add,
	Mod,
	Div
}

internal struct Operand
{
	public Operand(int i)
	{
		IntValue = i;
		CharValue = default;
		IsInt = true;
	}

	public Operand(char c)
	{
		IntValue = default;
		CharValue = c;
		IsInt = false;
	}

	public override string ToString()
	{
		return IsInt ? IntValue.ToString() : CharValue.ToString();
	}

	internal int IntValue { get; }
	public char CharValue { get; }
	public bool IsInt { get; }
}

internal class Alu
{
	public int X { get; private set; }
	public int Y { get; private set; }
	public int Z { get; private set; }
	public int W { get; private set; }
	private int _inputCount;
	private List<int> _digits = new();

	internal static void Transpile(List<(OpCode opCode, Operand op1, Operand op2)> instructions)
	{
		var count = 0;
		for (var i = 0; i < instructions.Count; i++)
		{
			var inst = instructions[i];
			switch (inst.opCode)
			{
				case OpCode.Inp:
					if (i != 0)
					{
						Console.WriteLine("  return z;");
						Console.WriteLine("}");
					}
					Console.WriteLine($"static int ReadDigit_{count++}(int {inst.op1}, int z)");
					Console.WriteLine("{");
					Console.WriteLine("  int x = 0;");
					Console.WriteLine("  int y = 0;");
					break;

				case OpCode.Mul:
					Console.WriteLine($"  {inst.op1} = {inst.op1} * {inst.op2};");
					break;

				case OpCode.Add:
					Console.WriteLine($"  {inst.op1} = {inst.op1} + {inst.op2};");
					break;

				case OpCode.Div:
					Console.WriteLine($"  {inst.op1} = {inst.op1} / {inst.op2};");
					break;

				case OpCode.Mod:
					Console.WriteLine($"  {inst.op1} = {inst.op1} % {inst.op2};");
					break;

				case OpCode.Eql:
					Console.WriteLine($"  {inst.op1} = {inst.op1} == {inst.op2} ? 1 : 0;");
					break;

				default:
					throw new InvalidOperationException(inst.opCode.ToString());
			}
		}
		Console.WriteLine("  return z;");
		Console.WriteLine("}");
	}

	internal void Run(List<(OpCode opCode, Operand op1, Operand op2)> instructions)
	{
		foreach (var i in instructions)
		{
			switch (i.opCode)
			{
				case OpCode.Inp:
					AssignTo(i.op1.CharValue, Input());
					break;

				case OpCode.Mul:
					AssignTo(i.op1.CharValue, ReadFrom(i.op1) * ReadFrom(i.op2));
					break;

				case OpCode.Add:
					AssignTo(i.op1.CharValue, ReadFrom(i.op1) + ReadFrom(i.op2));
					break;

				case OpCode.Div:
					AssignTo(i.op1.CharValue, ReadFrom(i.op1) / ReadFrom(i.op2));
					break;

				case OpCode.Mod:
					AssignTo(i.op1.CharValue, ReadFrom(i.op1) % ReadFrom(i.op2));
					break;

				case OpCode.Eql:
					AssignTo(i.op1.CharValue, ReadFrom(i.op1) == ReadFrom(i.op2) ? 1 : 0);
					break;

				default:
					throw new InvalidOperationException(i.opCode.ToString());
			}
		}
	}

	private int ReadFrom(Operand operand)
	{
		if (operand.IsInt)
		{
			return operand.IntValue;
		}
		return operand.CharValue switch
		{
			'x' => X,
			'y' => Y,
			'z' => Z,
			'w' => W,
			_ => throw new InvalidOperationException()
		};
	}

	public override string ToString()
	{
		var n = string.Join("", _digits);
		return $"{n} - X: {X}, Y: {Y}, Z: {Z}, W: {W}";
	}

	private int Input()
	{
		return _digits[_inputCount++];
	}

	private void AssignTo(char variable, int value)
	{
		switch (variable)
		{
			case 'x':
				X = value;
				break;
			case 'y':
				Y = value;
				break;
			case 'z':
				Z = value;
				break;
			case 'w':
				W = value;
				break;
			default:
				throw new InvalidOperationException();
		}
	}

	internal void Reset(IEnumerable<int> digits)
	{
		X = 0;
		Y = 0;
		Z = 0;
		W = 0;
		_inputCount = 0;
		_digits = digits.ToList();
	}
}
