using Cryptex.VM.Execution.Scripts;

namespace Cryptex.VM.Execution.Instructions.BitwiseInstructions;

/// <summary>
///     <c>or $A, ($B | #V | %H)</c> — bitwise OR of the integer at slot <c>$A</c> with a second
///     integer operand. Result stored in <c>$A</c>.
/// </summary>
internal sealed class OrInstruction : IInstruction
{
    public OpCodes OpCode => OpCodes.Or;

    public void Execute(ScriptInstruction c, Executor vm)
    {
        var (a, b) = BinaryBitwiseHelper.ReadOperands(in c, vm);
        BinaryBitwiseHelper.GetMemory(vm).SetSlot(c.Args[0].Value, VMValue.FromInteger(a | b));
    }
}