using Cryptex.VM.Execution.Instructions;
using Cryptex.VM.Execution.Instructions.BitwiseInstructions;
using Cryptex.VM.Execution.Instructions.IntegratedFunctionInstructions;
using Cryptex.VM.Execution.Instructions.LogicInstructions;
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
            case OpCodes.Term:    return new TermInstruction();
            case OpCodes.Nop:     return new NopInstruction();
            case OpCodes.Exit:    return new ExitInstruction();
            case OpCodes.Crash:   return new CrashInstruction();
            case OpCodes.GetError: return new GetErrorInstruction();

            case OpCodes.Inc:    return new IncInstruction();
            case OpCodes.Dec:    return new DecInstruction();
            case OpCodes.IncF:   return new IncFInstruction();
            case OpCodes.DecF:   return new DecFInstruction();
            case OpCodes.Add:    return new AddInstruction();
            case OpCodes.Sub:    return new SubInstruction();
            case OpCodes.Mul:    return new MulInstruction();
            case OpCodes.Div:    return new DivInstruction();
            case OpCodes.AddF:   return new AddFInstruction();
            case OpCodes.SubF:   return new SubFInstruction();
            case OpCodes.MulF:   return new MulFInstruction();
            case OpCodes.DivF:   return new DivFInstruction();
            case OpCodes.Mod:    return new ModInstruction();
            case OpCodes.AddImm: return new AddImmInstruction();
            case OpCodes.SubImm: return new SubImmInstruction();
            case OpCodes.MulImm: return new MulImmInstruction();
            case OpCodes.DivImm: return new DivImmInstruction();
            case OpCodes.ModImm: return new ModImmInstruction();

            case OpCodes.Load:   return new LoadInstruction();
            case OpCodes.Free:   return new FreeInstruction();
            case OpCodes.Reg:    break;
            case OpCodes.UnReg:  break;

            case OpCodes.Arg:    break;
            case OpCodes.Exec:   break;
            case OpCodes.Call:   break;
            case OpCodes.Ret:    break;
            case OpCodes.Res:    break;

            case OpCodes.Cmp:    return new CmpInstruction();
            case OpCodes.Jmp:    return new JmpInstruction();
            case OpCodes.Jeq:    return new JeqInstruction();
            case OpCodes.Jnq:    return new JnqInstruction();
            case OpCodes.Jls:    return new JlsInstruction();
            case OpCodes.Jgr:    return new JgrInstruction();
            case OpCodes.Jge:    return new JgeInstruction();
            case OpCodes.Jle:    return new JleInstruction();

            case OpCodes.Shl:    return new ShlInstruction();
            case OpCodes.Shr:    return new ShrInstruction();
            case OpCodes.And:    return new AndInstruction();
            case OpCodes.Or:     return new OrInstruction();
            case OpCodes.Xor:    return new XorInstruction();
            case OpCodes.Not:    return new NotInstruction();

            case OpCodes.ArrAccess: break;
            case OpCodes.ArrCreate: break;
            case OpCodes.ArrFree:   break;
            case OpCodes.ArrLen:    break;
            case OpCodes.ArrSet:    break;

            case OpCodes.StrCreate: break;
            case OpCodes.StrSub:    break;
            case OpCodes.StrAppend: break;
            case OpCodes.StrFree:   break;
            case OpCodes.StrNum:    break;
            case OpCodes.StrChar:   break;

            case OpCodes.Print:    return new PrintInstruction();
            case OpCodes.Read:     return new ReadInstruction();
            case OpCodes.ReadLine: return new ReadLineInstruction();
            case OpCodes.Random:   return new RandomInstruction();
            case OpCodes.RandomF:  return new RandomFInstruction();

            default:
                throw new ArgumentOutOfRangeException(nameof(code), code, "Invalid opcode specified or it wasn't added!");
        }

        return null;
    }
}