using Cryptex.VM.Execution.Instructions;

namespace Cryptex.VM.Execution;

internal static class OpCodesExtensions
{
    public static IInstruction? GetByCode(this OpCodes code)
    {
        switch (code)
        {
            case OpCodes.Term:
                break;
            case OpCodes.Nop:
                break;
            case OpCodes.Exit:
                break;
            case OpCodes.Crash:
                break;
            
            case OpCodes.Inc:
                break;
            case OpCodes.Dec:
                break;
            
            case OpCodes.IncF:
                break;
            case OpCodes.DecF:
                break;
            
            case OpCodes.Add:
                break;
            case OpCodes.Sub:
                break;
            case OpCodes.Mul:
                break;
            case OpCodes.Div:
                break;
            
            case OpCodes.AddF:
                break;
            case OpCodes.SubF:
                break;
            case OpCodes.MulF:
                break;
            case OpCodes.DivF:
                break;

            case OpCodes.Mod:
                break;
            
            case OpCodes.Load:
                break;
            case OpCodes.Free:
                break;
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
                break;
            case OpCodes.Shr:
                break;
            case OpCodes.And:
                break;
            case OpCodes.Or:
                break;
            case OpCodes.Xor:
                break;
            case OpCodes.Not:
                break;
            
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
