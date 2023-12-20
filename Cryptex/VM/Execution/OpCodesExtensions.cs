using Cryptex.VM.Execution.Instructions;
using Cryptex.VM.Execution.Instructions.BitwiseInstructions;
using Cryptex.VM.Execution.Instructions.MathInstructions;
using Cryptex.VM.Execution.Instructions.MemoryInstructions;
using Cryptex.VM.Execution.Instructions.VMControlInstructions;

namespace Cryptex.VM.Execution;

internal static class OpCodesExtensions
{
    public static IInstruction? GetByCode(this OpCodes code)
    {
        switch (code)
        {
            case OpCodes.Term:
                return new TermInstruction();
            case OpCodes.Nop:
                return new NopInstruction();
            case OpCodes.Exit:
                return new ExitInstruction();
            case OpCodes.Crash:
                return new CrashInstruction();
            
            case OpCodes.Inc:
                return new IncrementDecrementInstruction(IncrementDecrementInstruction.InstructionFunction.Increment, IncrementDecrementInstruction.ExpectedType.Integer);
            case OpCodes.Dec:
                return new IncrementDecrementInstruction(IncrementDecrementInstruction.InstructionFunction.Decrement, IncrementDecrementInstruction.ExpectedType.Integer);
            
            case OpCodes.IncF:
                return new IncrementDecrementInstruction(IncrementDecrementInstruction.InstructionFunction.Increment, IncrementDecrementInstruction.ExpectedType.Floating);
            case OpCodes.DecF:
                return new IncrementDecrementInstruction(IncrementDecrementInstruction.InstructionFunction.Decrement, IncrementDecrementInstruction.ExpectedType.Floating);
            
            case OpCodes.Add:
                return new AddSubMulDivInstruction(AddSubMulDivInstruction.InstructionFunction.Add, AddSubMulDivInstruction.ExpectedType.Integer);
            case OpCodes.Sub:
                return new AddSubMulDivInstruction(AddSubMulDivInstruction.InstructionFunction.Subtract, AddSubMulDivInstruction.ExpectedType.Integer);
            case OpCodes.Mul:
                return new AddSubMulDivInstruction(AddSubMulDivInstruction.InstructionFunction.Multiply, AddSubMulDivInstruction.ExpectedType.Integer);
            case OpCodes.Div:
                return new AddSubMulDivInstruction(AddSubMulDivInstruction.InstructionFunction.Divide, AddSubMulDivInstruction.ExpectedType.Integer);
            
            case OpCodes.AddF:
                return new AddSubMulDivInstruction(AddSubMulDivInstruction.InstructionFunction.Add, AddSubMulDivInstruction.ExpectedType.Floating);
            case OpCodes.SubF:
                return new AddSubMulDivInstruction(AddSubMulDivInstruction.InstructionFunction.Subtract, AddSubMulDivInstruction.ExpectedType.Floating);
            case OpCodes.MulF:
                return new AddSubMulDivInstruction(AddSubMulDivInstruction.InstructionFunction.Multiply, AddSubMulDivInstruction.ExpectedType.Floating);
            case OpCodes.DivF:
                return new AddSubMulDivInstruction(AddSubMulDivInstruction.InstructionFunction.Divide, AddSubMulDivInstruction.ExpectedType.Floating);

            case OpCodes.Mod:
                break;
            
            case OpCodes.Load:
                return new LoadInstruction();
            case OpCodes.Free:
                return new FreeInstruction();
            case OpCodes.Reg:
                break;
            case OpCodes.UnReg:
                break;
            
            case OpCodes.Arg:
                break;
            case OpCodes.Exec:
                break;
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
                return new ShiftLeftRightInstruction(ShiftLeftRightInstruction.Orientation.Left);
            case OpCodes.Shr:
                return new ShiftLeftRightInstruction(ShiftLeftRightInstruction.Orientation.Right);
            case OpCodes.And:
                return new AndOrXorInstruction(AndOrXorInstruction.Mode.And);
            case OpCodes.Or:
                return new AndOrXorInstruction(AndOrXorInstruction.Mode.Or);
            case OpCodes.Xor:
                return new AndOrXorInstruction(AndOrXorInstruction.Mode.Xor);
            case OpCodes.Not:
                return new NotInstruction();
            
            case OpCodes.ArrAccess:
                break;
            case OpCodes.ArrCreate:
                break;
            case OpCodes.ArrFree:
                break;
            case OpCodes.ArrLen:
                break;
            case OpCodes.ArrSet:
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
            
            case OpCodes.Print:
                break;
            case OpCodes.Read:
                break;
            case OpCodes.ReadLine:
                break;
            case OpCodes.Random:
                break;
            case OpCodes.RandomF:
                break;
            
            default:
                throw new ArgumentOutOfRangeException(nameof(code), code, "Invalid opcode specified or it wasn't added!");
        }

        return null;
    }
}
