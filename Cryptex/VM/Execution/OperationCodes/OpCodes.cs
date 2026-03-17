namespace Cryptex.VM.Execution.OperationCodes;

public enum OpCodes : byte
{
    //VM opcodes.
    Term,
    Nop,
    Exit,
    Crash,
    GetError,

    //Math opcodes.
    Inc,
    Add,
    Sub,
    Dec,
    Mul,
    Div,
    IncF,
    AddF,
    SubF,
    DecF,
    MulF,
    DivF,
    Mod,
    AddImm,
    SubImm,
    MulImm,
    DivImm,
    ModImm,

    //Function opcodes.
    Arg,
    Exec,
    Call,
    Ret,
    Res,

    //Logic opcodes.
    Cmp,
    Jmp,
    Jeq,
    Jnq,
    Jls,
    Jgr,
    Jge,
    Jle,

    //Bitwise opcodes.
    Shl,
    Shr,
    And,
    Or,
    Xor,
    Not,

    //Memory opcodes.
    Load,
    Free,
    Reg,
    UnReg,

    //Array opcodes.
    ArrAccess,
    ArrCreate,
    ArrFree,
    ArrLen,
    ArrSet,

    //String opcodes.
    StrCreate,
    StrSub,
    StrAppend,
    StrFree,
    StrNum,
    StrChar,

    //Integrated functions.
    Print,
    Read,
    ReadLine,
    Random,
    RandomF,

    /// <summary>
    /// This is the last member of this enum. Used to count it.
    /// </summary>
    /// <remarks>
    /// <list type="bullet">
    ///     <item>
    ///         This is not a real instruction, don't use it in this context.
    ///     </item>
    ///     <item>
    ///         This should always be the last member of this enum.
    ///     </item>
    /// </list>
    /// </remarks>
    __LAST = byte.MaxValue - 1
}