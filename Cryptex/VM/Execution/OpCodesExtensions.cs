using Cryptex.VM.Execution.OpCodeLogic;
using Cryptex.VM.Execution.OpCodeLogic.MathInstructions;
using Cryptex.VM.Execution.OpCodeLogic.MemoryInstructions;
using Cryptex.VM.Execution.OpCodeLogic.VMControlInstructions;

namespace Cryptex.VM.Execution;

internal static class OpCodesExtensions
{
    public static IInstruction? GetByCode(this OpCodes code)
    {
        switch (code)
        {
            case OpCodes.Inc:
                return new IncrementDecrementInstruction(IncrementDecrementInstruction.InstructionFunction.Increment, IncrementDecrementInstruction.ExpectedType.Integer);
            case OpCodes.Add:
                return new AddSubInstruction(AddSubInstruction.InstructionFunction.Add, AddSubInstruction.ExpectedType.Integer);
            case OpCodes.Sub:
                return new AddSubInstruction(AddSubInstruction.InstructionFunction.Subtract, AddSubInstruction.ExpectedType.Integer);
            case OpCodes.Exec:
                break;
            case OpCodes.Load:
                return new LoadInstruction();
            case OpCodes.Free:
                return new FreeInstruction();
            case OpCodes.Arg:
                break;
            case OpCodes.Nop:
                return new NopInstruction();
            case OpCodes.Exit:
                return new ExitInstruction();
            case OpCodes.Crash:
                return new CrashInstruction();
            case OpCodes.Dec:
                return new IncrementDecrementInstruction(IncrementDecrementInstruction.InstructionFunction.Decrement, IncrementDecrementInstruction.ExpectedType.Integer);
            case OpCodes.Call:
                break;
            case OpCodes.Ret:
                break;
            case OpCodes.Res:
                break;
            case OpCodes.Cmp:
                break;
            case OpCodes.Jmp:
                break;
            case OpCodes.Jeq:
                break;
            case OpCodes.Jnq:
                break;
            case OpCodes.Jls:
                break;
            case OpCodes.Jgr:
                break;
            case OpCodes.Jge:
                break;
            case OpCodes.Jle:
                break;
            case OpCodes.Shl:
                break;
            case OpCodes.Shr:
                break;
            case OpCodes.And:
                break;
            case OpCodes.Or:
                break;
            case OpCodes.Not:
                break;
            case OpCodes.Xor:
                break;
            case OpCodes.ArrAccess:
                break;
            case OpCodes.ArrCreate:
                break;
            case OpCodes.ArrFree:
                break;
            case OpCodes.StrCreate:
                break;
            case OpCodes.StrSub:
                break;
            case OpCodes.StrAppend:
                break;
            case OpCodes.StrFree:
                break;
            case OpCodes.StrNum:
                break;
            case OpCodes.StrChar:
                break;
            case OpCodes.IncD:
                return new IncrementDecrementInstruction(IncrementDecrementInstruction.InstructionFunction.Increment, IncrementDecrementInstruction.ExpectedType.Floating);
            case OpCodes.AddD:
                return new AddSubInstruction(AddSubInstruction.InstructionFunction.Add, AddSubInstruction.ExpectedType.Floating);
            case OpCodes.SubD:
                return new AddSubInstruction(AddSubInstruction.InstructionFunction.Subtract, AddSubInstruction.ExpectedType.Floating);
            case OpCodes.DecD:
                return new IncrementDecrementInstruction(IncrementDecrementInstruction.InstructionFunction.Decrement, IncrementDecrementInstruction.ExpectedType.Floating);
            default:
                throw new ArgumentOutOfRangeException(nameof(code), code, "Invalid opcode specified or it wasn't added!");
        }

        return null;
    }
}
