namespace Cryptex.VM.Execution;

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
    RandomF
}