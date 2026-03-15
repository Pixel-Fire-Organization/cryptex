using Cryptex.VM.Execution.Scripts;

namespace Cryptex.VM.Execution.Instructions.BitwiseInstructions;

internal sealed class XorInstruction : IInstruction
{
    public OpCodes OpCode => OpCodes.Xor;

    public void Execute(ScriptInstruction c, Executor vm)
    {
        var (a, b) = BinaryBitwiseHelper.ReadOperands(in c, vm);
        BinaryBitwiseHelper.GetMemory(vm).SetSlot(c.Args[0].Value, VMValue.FromInteger(a ^ b));
    }
}