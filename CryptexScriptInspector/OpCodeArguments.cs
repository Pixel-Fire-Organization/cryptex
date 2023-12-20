using System.Collections.Frozen;

using Cryptex.VM.Execution;

using CryptexScriptInspector.Controls;

namespace CryptexScriptInspector;

internal sealed class OpCodeArguments
{
    public static FrozenDictionary<OpCodes, IReadOnlyList<InstructionArgumentControl.InstructionArgumentType>> OpCodeArgs { get; } = new Dictionary<OpCodes, IReadOnlyList<InstructionArgumentControl.InstructionArgumentType>>
    {
        { OpCodes.Term, new List<InstructionArgumentControl.InstructionArgumentType>() },
        { OpCodes.Nop, new List<InstructionArgumentControl.InstructionArgumentType> { InstructionArgumentControl.InstructionArgumentType.Decimal } },
        { OpCodes.Exit, new List<InstructionArgumentControl.InstructionArgumentType> { InstructionArgumentControl.InstructionArgumentType.Decimal } },
        { OpCodes.Crash, new List<InstructionArgumentControl.InstructionArgumentType> { InstructionArgumentControl.InstructionArgumentType.Decimal } },
        
        { OpCodes.Add, new List<InstructionArgumentControl.InstructionArgumentType> { InstructionArgumentControl.InstructionArgumentType.Memory, InstructionArgumentControl.InstructionArgumentType.Memory } },
        { OpCodes.Mul, new List<InstructionArgumentControl.InstructionArgumentType> { InstructionArgumentControl.InstructionArgumentType.Memory, InstructionArgumentControl.InstructionArgumentType.Memory } },
        { OpCodes.Div, new List<InstructionArgumentControl.InstructionArgumentType> { InstructionArgumentControl.InstructionArgumentType.Memory, InstructionArgumentControl.InstructionArgumentType.Memory } },
        { OpCodes.Sub, new List<InstructionArgumentControl.InstructionArgumentType> { InstructionArgumentControl.InstructionArgumentType.Memory, InstructionArgumentControl.InstructionArgumentType.Memory } },
        { OpCodes.Inc, new List<InstructionArgumentControl.InstructionArgumentType> { InstructionArgumentControl.InstructionArgumentType.Memory } },
        { OpCodes.Dec, new List<InstructionArgumentControl.InstructionArgumentType> { InstructionArgumentControl.InstructionArgumentType.Memory } },
        { OpCodes.AddF, new List<InstructionArgumentControl.InstructionArgumentType> { InstructionArgumentControl.InstructionArgumentType.Memory, InstructionArgumentControl.InstructionArgumentType.Memory } },
        { OpCodes.MulF, new List<InstructionArgumentControl.InstructionArgumentType> { InstructionArgumentControl.InstructionArgumentType.Memory, InstructionArgumentControl.InstructionArgumentType.Memory } },
        { OpCodes.DivF, new List<InstructionArgumentControl.InstructionArgumentType> { InstructionArgumentControl.InstructionArgumentType.Memory, InstructionArgumentControl.InstructionArgumentType.Memory } },
        { OpCodes.SubF, new List<InstructionArgumentControl.InstructionArgumentType> { InstructionArgumentControl.InstructionArgumentType.Memory, InstructionArgumentControl.InstructionArgumentType.Memory } },
        { OpCodes.IncF, new List<InstructionArgumentControl.InstructionArgumentType> { InstructionArgumentControl.InstructionArgumentType.Memory } },
        { OpCodes.DecF, new List<InstructionArgumentControl.InstructionArgumentType> { InstructionArgumentControl.InstructionArgumentType.Memory } },
        { OpCodes.Mod, new List<InstructionArgumentControl.InstructionArgumentType> { InstructionArgumentControl.InstructionArgumentType.Memory, InstructionArgumentControl.InstructionArgumentType.Memory } },
    }.ToFrozenDictionary();
}
