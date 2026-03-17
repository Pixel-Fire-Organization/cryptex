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
    internal static bool IsKnownOpCode(OpCodes code) => (uint)code <= (uint)OpCodes.Last;

    /// <summary>Returns the <see cref="OpCodeInfo" /> for <paramref name="code" />.</summary>
    internal static OpCodeInfo Get(OpCodes code) => code switch
    {
        OpCodes.Term => new OpCodeInfo(1, 0, new TermInstruction()),
        OpCodes.Nop => new OpCodeInfo(1, 1, new NopInstruction(), AllowedArgTypes.Constant),
        OpCodes.Exit => new OpCodeInfo(1, 1, new ExitInstruction(), AllowedArgTypes.Constant),
        OpCodes.Crash => new OpCodeInfo(1, 1, new CrashInstruction(), AllowedArgTypes.Constant),
        OpCodes.GetError => new OpCodeInfo(1, 1, new GetErrorInstruction(), AllowedArgTypes.MemoryAddress),
        OpCodes.Inc => new OpCodeInfo(1, 1, new IncInstruction(), AllowedArgTypes.MemoryAddress),
        OpCodes.Dec => new OpCodeInfo(1, 1, new DecInstruction(), AllowedArgTypes.MemoryAddress),
        OpCodes.IncF => new OpCodeInfo(1, 1, new IncFInstruction(), AllowedArgTypes.MemoryAddress),
        OpCodes.DecF => new OpCodeInfo(1, 1, new DecFInstruction(), AllowedArgTypes.MemoryAddress),
        OpCodes.Add => new OpCodeInfo(1, 2, new AddInstruction(), AllowedArgTypes.MemoryAddress, AllowedArgTypes.MemoryAddress),
        OpCodes.Sub => new OpCodeInfo(1, 2, new SubInstruction(), AllowedArgTypes.MemoryAddress, AllowedArgTypes.MemoryAddress),
        OpCodes.Mul => new OpCodeInfo(1, 2, new MulInstruction(), AllowedArgTypes.MemoryAddress, AllowedArgTypes.MemoryAddress),
        OpCodes.Div => new OpCodeInfo(1, 2, new DivInstruction(), AllowedArgTypes.MemoryAddress, AllowedArgTypes.MemoryAddress),
        OpCodes.AddF => new OpCodeInfo(1, 2, new AddFInstruction(), AllowedArgTypes.MemoryAddress, AllowedArgTypes.MemoryAddress),
        OpCodes.SubF => new OpCodeInfo(1, 2, new SubFInstruction(), AllowedArgTypes.MemoryAddress, AllowedArgTypes.MemoryAddress),
        OpCodes.MulF => new OpCodeInfo(1, 2, new MulFInstruction(), AllowedArgTypes.MemoryAddress, AllowedArgTypes.MemoryAddress),
        OpCodes.DivF => new OpCodeInfo(1, 2, new DivFInstruction(), AllowedArgTypes.MemoryAddress, AllowedArgTypes.MemoryAddress),
        OpCodes.Mod => new OpCodeInfo(1, 2, new ModInstruction(), AllowedArgTypes.MemoryAddress, AllowedArgTypes.MemoryAddress),
        OpCodes.AddImm => new OpCodeInfo(1, 2, new AddImmInstruction(), AllowedArgTypes.MemoryAddress, AllowedArgTypes.Constant),
        OpCodes.SubImm => new OpCodeInfo(1, 2, new SubImmInstruction(), AllowedArgTypes.MemoryAddress, AllowedArgTypes.Constant),
        OpCodes.MulImm => new OpCodeInfo(1, 2, new MulImmInstruction(), AllowedArgTypes.MemoryAddress, AllowedArgTypes.Constant),
        OpCodes.DivImm => new OpCodeInfo(1, 2, new DivImmInstruction(), AllowedArgTypes.MemoryAddress, AllowedArgTypes.Constant),
        OpCodes.ModImm => new OpCodeInfo(1, 2, new ModImmInstruction(), AllowedArgTypes.MemoryAddress, AllowedArgTypes.Constant),
        OpCodes.Load => new OpCodeInfo(1, 2, new LoadInstruction(), AllowedArgTypes.MemoryAddress,
            AllowedArgTypes.MemoryAddress | AllowedArgTypes.Constant),
        OpCodes.Free => new OpCodeInfo(1, 1, new FreeInstruction(), AllowedArgTypes.MemoryAddress),
        OpCodes.Cmp => new OpCodeInfo(1, 2, new CmpInstruction(), AllowedArgTypes.MemoryAddress, AllowedArgTypes.MemoryAddress),
        OpCodes.Jmp => new OpCodeInfo(1, 1, new JmpInstruction(), AllowedArgTypes.Label),
        OpCodes.Jeq => new OpCodeInfo(1, 1, new JeqInstruction(), AllowedArgTypes.Label),
        OpCodes.Jnq => new OpCodeInfo(1, 1, new JnqInstruction(), AllowedArgTypes.Label),
        OpCodes.Jls => new OpCodeInfo(1, 1, new JlsInstruction(), AllowedArgTypes.Label),
        OpCodes.Jgr => new OpCodeInfo(1, 1, new JgrInstruction(), AllowedArgTypes.Label),
        OpCodes.Jge => new OpCodeInfo(1, 1, new JgeInstruction(), AllowedArgTypes.Label),
        OpCodes.Jle => new OpCodeInfo(1, 1, new JleInstruction(), AllowedArgTypes.Label),
        OpCodes.Shl => new OpCodeInfo(1, 2, new ShlInstruction(), AllowedArgTypes.MemoryAddress, AllowedArgTypes.MemoryAddress),
        OpCodes.Shr => new OpCodeInfo(1, 2, new ShrInstruction(), AllowedArgTypes.MemoryAddress, AllowedArgTypes.MemoryAddress),
        OpCodes.And => new OpCodeInfo(1, 2, new AndInstruction(), AllowedArgTypes.MemoryAddress, AllowedArgTypes.MemoryAddress),
        OpCodes.Or => new OpCodeInfo(1, 2, new OrInstruction(), AllowedArgTypes.MemoryAddress, AllowedArgTypes.MemoryAddress),
        OpCodes.Xor => new OpCodeInfo(1, 2, new XorInstruction(), AllowedArgTypes.MemoryAddress, AllowedArgTypes.MemoryAddress),
        OpCodes.Not => new OpCodeInfo(1, 1, new NotInstruction(), AllowedArgTypes.MemoryAddress),
        OpCodes.Print => new OpCodeInfo(1, 1, new PrintInstruction(), AllowedArgTypes.MemoryAddress),
        OpCodes.Read => new OpCodeInfo(1, 1, new ReadInstruction(), AllowedArgTypes.MemoryAddress),
        OpCodes.ReadLine => new OpCodeInfo(1, 1, new ReadLineInstruction(), AllowedArgTypes.MemoryAddress),
        OpCodes.Random => new OpCodeInfo(1, 1, new RandomInstruction(), AllowedArgTypes.MemoryAddress),
        OpCodes.RandomF => new OpCodeInfo(1, 1, new RandomFInstruction(), AllowedArgTypes.MemoryAddress),
        _ => default,
    };
}