using Cryptex;
using Cryptex.VM.Execution;
using Cryptex.VM.Execution.OperationCodes;
using Cryptex.VM.Execution.Scripts;

PrintingDelegates.WriteMessage = Console.WriteLine;

// Constants: [0]=11 (initial sum)  [1]=1 (initial i / inc)  [2]=1000 (limit)  [3]=2 (multiplier)
VmValue[] constants =
[
    VmValue.FromInteger(11),
    VmValue.FromInteger(1),
    VmValue.FromInteger(1000),
    VmValue.FromInteger(2),
];

// Memory: $1 = sum  $2 = i  $3 = limit  $4 = temp (i * 2)
ScriptChunk main = new("main",
[
    new(OpCodes.Load, [Mem(1), Const(0)]), // 0: sum   = 11
    new(OpCodes.Load, [Mem(2), Const(1)]), // 1: i     = 1
    new(OpCodes.Load, [Mem(3), Const(2)]), // 2: limit = 1000

    new(OpCodes.Print, [Mem(2)]), // 3: print i          ← loop start
    new(OpCodes.Load, [Mem(4), Mem(2)]), // 4: temp  = i
    new(OpCodes.MulImm, [Mem(4), Const(3)]), // 5: temp  = i * 2
    new(OpCodes.Add, [Mem(1), Mem(4)]), // 6: sum  += i * 2
    new(OpCodes.Inc, [Mem(2)]), // 7: i++
    new(OpCodes.Cmp, [Mem(2), Mem(3)]), // 8: cmp i, limit
    new(OpCodes.Jle, [Label(3)]), // 9: i <= 1000 → jump to 3
    new(OpCodes.Print, [Mem(1)]), // 3: print sum
]);

new Executor(new Script("loop-test", [main], constants)).ExecuteScript();
Console.ReadLine();

static ScriptInstructionArgument Mem(int slot) => new(slot, InstructionArgumentType.MemoryAddress);

static ScriptInstructionArgument Const(int index) => new(index, InstructionArgumentType.Constant);

static ScriptInstructionArgument Label(int index) => new(index, InstructionArgumentType.Label);