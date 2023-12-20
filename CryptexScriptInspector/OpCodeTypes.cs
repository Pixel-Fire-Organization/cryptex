using System.Collections.Frozen;

using Cryptex.VM.Execution;

using MaterialDesignThemes.Wpf;

namespace CryptexScriptInspector;

public enum OpCodeType
{
    All,
    VM,
    Math,
    Branching,
    Call,
    Functions,
    String,
    Arrays,
    Bitwise,
    Memory
}

internal sealed class OpCodeTypes
{
    public static FrozenDictionary<FrozenSet<OpCodes>, PackIconKind> OpCodeIcons { get; } = new Dictionary<FrozenSet<OpCodes>, PackIconKind>()
    {
        {
            //math
            new HashSet<OpCodes>
            {
                OpCodes.Add,
                OpCodes.AddF,
                OpCodes.Dec,
                OpCodes.DecF,
                OpCodes.Div,
                OpCodes.DivF,
                OpCodes.Inc,
                OpCodes.IncF,
                OpCodes.Mod,
                OpCodes.Mul,
                OpCodes.MulF,
                OpCodes.Sub,
                OpCodes.SubF
            }.ToFrozenSet(),
            PackIconKind.MathsCompass
        },
        {
            //memory
            new HashSet<OpCodes>
            {
                OpCodes.Load,
                OpCodes.Free,
                OpCodes.Reg,
                OpCodes.UnReg
            }.ToFrozenSet(),
            PackIconKind.Memory
        },
        {
            //VM
            new HashSet<OpCodes>
            {
                OpCodes.Term,
                OpCodes.Nop,
                OpCodes.Exit,
                OpCodes.Crash
            }.ToFrozenSet(),
            PackIconKind.StateMachine
        },
        {
            //Bitwise
            new HashSet<OpCodes>
            {
                OpCodes.And,
                OpCodes.Not,
                OpCodes.Or,
                OpCodes.Xor,
                OpCodes.Shl,
                OpCodes.Shr
            }.ToFrozenSet(),
            PackIconKind.GateXor
        },
        {
            //Branching
            new HashSet<OpCodes>
            {
                OpCodes.Cmp,
                OpCodes.Jge,
                OpCodes.Jle,
                OpCodes.Jgr,
                OpCodes.Jls,
                OpCodes.Jmp,
                OpCodes.Jeq,
                OpCodes.Jnq
            }.ToFrozenSet(),
            PackIconKind.SourceBranch
        },
        {
            //Functions
            new HashSet<OpCodes>
            {
                OpCodes.Print,
                OpCodes.Random,
                OpCodes.RandomF,
                OpCodes.Read,
                OpCodes.ReadLine
            }.ToFrozenSet(),
            PackIconKind.Function
        },
        {
            //Call
            new HashSet<OpCodes>
            {
                OpCodes.Call,
                OpCodes.Exec,
                OpCodes.Arg,
                OpCodes.Ret,
                OpCodes.Res
            }.ToFrozenSet(),
            PackIconKind.CallMade
        },
        {
            //String
            new HashSet<OpCodes>
            {
                OpCodes.StrAppend,
                OpCodes.StrChar,
                OpCodes.StrCreate,
                OpCodes.StrFree,
                OpCodes.StrNum,
                OpCodes.StrSub
            }.ToFrozenSet(),
            PackIconKind.CursorText
        },
        {
            //Array
            new HashSet<OpCodes>
            {
                OpCodes.ArrAccess,
                OpCodes.ArrCreate,
                OpCodes.ArrFree,
                OpCodes.ArrLen,
                OpCodes.ArrSet
            }.ToFrozenSet(),
            PackIconKind.CodeArray
        }
    }.ToFrozenDictionary();

    public static FrozenDictionary<PackIconKind, OpCodeType> IconToType { get; } = new Dictionary<PackIconKind, OpCodeType>
    {
        { PackIconKind.MathCompass, OpCodeType.Math },
        { PackIconKind.CodeArray, OpCodeType.Arrays },
        { PackIconKind.SourceBranch, OpCodeType.Branching },
        { PackIconKind.StateMachine, OpCodeType.VM },
        { PackIconKind.CursorText, OpCodeType.String },
        { PackIconKind.GateXor, OpCodeType.Bitwise },
        { PackIconKind.CallMade, OpCodeType.Call },
        { PackIconKind.Memory, OpCodeType.Memory },
        { PackIconKind.Function, OpCodeType.Functions },
    }.ToFrozenDictionary();
}
