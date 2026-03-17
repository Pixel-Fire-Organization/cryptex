using Cryptex.VM.Execution.Instructions;
using Cryptex.VM.Execution.Instructions.BitwiseInstructions;
using Cryptex.VM.Execution.Instructions.IntegratedFunctionInstructions;
using Cryptex.VM.Execution.Instructions.LogicInstructions;
using Cryptex.VM.Execution.Instructions.MathInstructions;
using Cryptex.VM.Execution.Instructions.MemoryInstructions;
using Cryptex.VM.Execution.Instructions.VMControlInstructions;
using Cryptex.VM.Execution.Scripts;

namespace Cryptex.VM.Execution.OperationCodes;

/// <summary>
///     Combines opcode metadata (argument constraints, introduced version) with the
///     version-correct <see cref="IInstruction" /> implementation in a single value.
///     The zero-value default represents an unknown or unimplemented opcode.
/// </summary>
internal readonly struct OpCodeInfo
{
    private OpCodeInfo(int introducedInVersion, int minArgs, IInstruction instruction,
        params AllowedArgTypes[] argTypes)
    {
        IsSupported = true;
        IntroducedInVersion = introducedInVersion;
        MinArgCount = minArgs;
        ArgAllowedTypes = argTypes;
        Instruction = instruction;
    }

    internal bool IsSupported { get; }
    internal int IntroducedInVersion { get; }
    internal int MinArgCount { get; }

    /// <summary>Maximum number of arguments: derived from the length of <see cref="ArgAllowedTypes" />.</summary>
    internal int MaxArgCount => ArgAllowedTypes?.Length ?? 0;

    /// <summary>
    ///     Per-position allowed argument types. Index <c>i</c> describes what
    ///     <see cref="InstructionArgumentType" /> is accepted at argument position <c>i</c>.
    ///     For opcodes with a variable argument count <see cref="MinArgCount" /> may be less than
    ///     the array length; positions beyond the supplied argument count are never validated.
    /// </summary>
    internal AllowedArgTypes[] ArgAllowedTypes { get; }

    /// <summary>
    ///     The version-correct instruction implementation for the opcode.
    ///     <c>null</c> when <see cref="IsSupported" /> is <c>false</c>.
    /// </summary>
    internal IInstruction? Instruction { get; }

    /// <summary>
    ///     Returns <c>true</c> when <paramref name="code" /> is within the known
    ///     <see cref="OpCodes" /> range. A <c>true</c> result does not guarantee the
    ///     opcode is implemented; check <see cref="IsSupported" /> on the returned
    ///     <see cref="OpCodeInfo" /> for that.
    /// </summary>
    internal static bool IsKnownOpCode(OpCodes code) => (uint)code <= (uint)OpCodes.__LAST;

    /// <summary>
    ///     Returns the <see cref="OpCodeInfo" /> for <paramref name="code" />,
    ///     selecting the instruction implementation appropriate for <paramref name="scriptVersion" />.
    ///     When the current VM is newer than the script and an instruction changed behaviour
    ///     between versions, the implementation matching <paramref name="scriptVersion" /> is
    ///     selected to guarantee backwards compatibility.
    /// </summary>
    internal static OpCodeInfo Get(OpCodes code, int scriptVersion) => code switch
    {
        OpCodes.Term => new OpCodeInfo(1, 0, new TermInstruction(scriptVersion)),
        OpCodes.Nop => new OpCodeInfo(1, 1, new NopInstruction(scriptVersion), AllowedArgTypes.Constant),
        OpCodes.Exit => new OpCodeInfo(1, 1, new ExitInstruction(scriptVersion), AllowedArgTypes.Constant),
        OpCodes.Crash => new OpCodeInfo(1, 1, new CrashInstruction(scriptVersion), AllowedArgTypes.Constant),
        OpCodes.GetError => new OpCodeInfo(1, 1, new GetErrorInstruction(scriptVersion), AllowedArgTypes.MemoryAddress),
        OpCodes.Inc => new OpCodeInfo(1, 1, new IncInstruction(scriptVersion), AllowedArgTypes.MemoryAddress),
        OpCodes.Dec => new OpCodeInfo(1, 1, new DecInstruction(scriptVersion), AllowedArgTypes.MemoryAddress),
        OpCodes.IncF => new OpCodeInfo(1, 1, new IncFInstruction(scriptVersion), AllowedArgTypes.MemoryAddress),
        OpCodes.DecF => new OpCodeInfo(1, 1, new DecFInstruction(scriptVersion), AllowedArgTypes.MemoryAddress),
        OpCodes.Add => new OpCodeInfo(1, 2, new AddInstruction(scriptVersion), AllowedArgTypes.MemoryAddress, AllowedArgTypes.MemoryAddress),
        OpCodes.Sub => new OpCodeInfo(1, 2, new SubInstruction(scriptVersion), AllowedArgTypes.MemoryAddress, AllowedArgTypes.MemoryAddress),
        OpCodes.Mul => new OpCodeInfo(1, 2, new MulInstruction(scriptVersion), AllowedArgTypes.MemoryAddress, AllowedArgTypes.MemoryAddress),
        OpCodes.Div => new OpCodeInfo(1, 2, new DivInstruction(scriptVersion), AllowedArgTypes.MemoryAddress, AllowedArgTypes.MemoryAddress),
        OpCodes.AddF => new OpCodeInfo(1, 2, new AddFInstruction(scriptVersion), AllowedArgTypes.MemoryAddress, AllowedArgTypes.MemoryAddress),
        OpCodes.SubF => new OpCodeInfo(1, 2, new SubFInstruction(scriptVersion), AllowedArgTypes.MemoryAddress, AllowedArgTypes.MemoryAddress),
        OpCodes.MulF => new OpCodeInfo(1, 2, new MulFInstruction(scriptVersion), AllowedArgTypes.MemoryAddress, AllowedArgTypes.MemoryAddress),
        OpCodes.DivF => new OpCodeInfo(1, 2, new DivFInstruction(scriptVersion), AllowedArgTypes.MemoryAddress, AllowedArgTypes.MemoryAddress),
        OpCodes.Mod => new OpCodeInfo(1, 2, new ModInstruction(scriptVersion), AllowedArgTypes.MemoryAddress, AllowedArgTypes.MemoryAddress),
        OpCodes.AddImm => new OpCodeInfo(1, 2, new AddImmInstruction(scriptVersion), AllowedArgTypes.MemoryAddress, AllowedArgTypes.Constant),
        OpCodes.SubImm => new OpCodeInfo(1, 2, new SubImmInstruction(scriptVersion), AllowedArgTypes.MemoryAddress, AllowedArgTypes.Constant),
        OpCodes.MulImm => new OpCodeInfo(1, 2, new MulImmInstruction(scriptVersion), AllowedArgTypes.MemoryAddress, AllowedArgTypes.Constant),
        OpCodes.DivImm => new OpCodeInfo(1, 2, new DivImmInstruction(scriptVersion), AllowedArgTypes.MemoryAddress, AllowedArgTypes.Constant),
        OpCodes.ModImm => new OpCodeInfo(1, 2, new ModImmInstruction(scriptVersion), AllowedArgTypes.MemoryAddress, AllowedArgTypes.Constant),
        OpCodes.Load => new OpCodeInfo(1, 2, new LoadInstruction(scriptVersion), AllowedArgTypes.MemoryAddress,
            AllowedArgTypes.MemoryAddress | AllowedArgTypes.Constant),
        OpCodes.Free => new OpCodeInfo(1, 1, new FreeInstruction(scriptVersion), AllowedArgTypes.MemoryAddress),
        OpCodes.Cmp => new OpCodeInfo(1, 2, new CmpInstruction(scriptVersion), AllowedArgTypes.MemoryAddress, AllowedArgTypes.MemoryAddress),
        OpCodes.Jmp => new OpCodeInfo(1, 1, new JmpInstruction(scriptVersion), AllowedArgTypes.Label),
        OpCodes.Jeq => new OpCodeInfo(1, 1, new JeqInstruction(scriptVersion), AllowedArgTypes.Label),
        OpCodes.Jnq => new OpCodeInfo(1, 1, new JnqInstruction(scriptVersion), AllowedArgTypes.Label),
        OpCodes.Jls => new OpCodeInfo(1, 1, new JlsInstruction(scriptVersion), AllowedArgTypes.Label),
        OpCodes.Jgr => new OpCodeInfo(1, 1, new JgrInstruction(scriptVersion), AllowedArgTypes.Label),
        OpCodes.Jge => new OpCodeInfo(1, 1, new JgeInstruction(scriptVersion), AllowedArgTypes.Label),
        OpCodes.Jle => new OpCodeInfo(1, 1, new JleInstruction(scriptVersion), AllowedArgTypes.Label),
        OpCodes.Shl => new OpCodeInfo(1, 2, new ShlInstruction(scriptVersion), AllowedArgTypes.MemoryAddress, AllowedArgTypes.MemoryAddress),
        OpCodes.Shr => new OpCodeInfo(1, 2, new ShrInstruction(scriptVersion), AllowedArgTypes.MemoryAddress, AllowedArgTypes.MemoryAddress),
        OpCodes.And => new OpCodeInfo(1, 2, new AndInstruction(scriptVersion), AllowedArgTypes.MemoryAddress, AllowedArgTypes.MemoryAddress),
        OpCodes.Or => new OpCodeInfo(1, 2, new OrInstruction(scriptVersion), AllowedArgTypes.MemoryAddress, AllowedArgTypes.MemoryAddress),
        OpCodes.Xor => new OpCodeInfo(1, 2, new XorInstruction(scriptVersion), AllowedArgTypes.MemoryAddress, AllowedArgTypes.MemoryAddress),
        OpCodes.Not => new OpCodeInfo(1, 1, new NotInstruction(scriptVersion), AllowedArgTypes.MemoryAddress),
        OpCodes.Print => new OpCodeInfo(1, 1, new PrintInstruction(scriptVersion), AllowedArgTypes.MemoryAddress),
        OpCodes.Read => new OpCodeInfo(1, 1, new ReadInstruction(scriptVersion), AllowedArgTypes.MemoryAddress),
        OpCodes.ReadLine => new OpCodeInfo(1, 1, new ReadLineInstruction(scriptVersion), AllowedArgTypes.MemoryAddress),
        OpCodes.Random => new OpCodeInfo(1, 1, new RandomInstruction(scriptVersion), AllowedArgTypes.MemoryAddress),
        OpCodes.RandomF => new OpCodeInfo(1, 1, new RandomFInstruction(scriptVersion), AllowedArgTypes.MemoryAddress),
        _ => default,
    };
}