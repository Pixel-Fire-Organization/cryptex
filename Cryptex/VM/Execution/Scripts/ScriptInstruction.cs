using MessagePack;

namespace Cryptex.VM.Execution.Scripts;

[MessagePackObject(true)]
public struct ScriptInstruction
{
    public OpCodes Code { get; set; }
    public ScriptInstructionArgument[] Args { get; set; }

    /// <summary>
    ///     Pre-parsed inline constants extracted from the human-readable argument string passed
    ///     to <see cref="ScriptInstruction(Cryptex.VM.Execution.OpCodes, string)" />.
    ///     For instructions built from raw <see cref="ScriptInstructionArgument" /> arrays the
    ///     constants come from <see cref="Script.ConstantsBlock" /> instead.
    ///     Not persisted by MessagePack.
    /// </summary>
    [IgnoreMember]
    public VMValue[]? LocalConstants { get; private set; }

    public ScriptInstruction()
    {
        Code = OpCodes.Crash;
        Args = [ScriptInstructionArgument.DEFAULT];
        LocalConstants = null;
    }

    public ScriptInstruction(OpCodes code)
    {
        Code = code;
        Args = [ScriptInstructionArgument.DEFAULT];
        LocalConstants = null;
    }

    public ScriptInstruction(OpCodes code, ScriptInstructionArgument[] args)
    {
        Code = code;
        Args = args;
        LocalConstants = null;
    }

    /// <summary>
    ///     Convenience constructor that parses a human-readable argument string such as
    ///     <c>"$1, #5, %7f"</c> into typed <see cref="ScriptInstructionArgument" /> entries and
    ///     a pre-parsed <see cref="LocalConstants" /> array.
    ///     Constants are parsed eagerly; invalid hex strings result in
    ///     <see cref="VMValueKind.Error" /> entries surfaced at execution time.
    /// </summary>
    public ScriptInstruction(OpCodes code, string args)
    {
        Code = code;
        (Args, LocalConstants) = ScriptArgumentParser.Parse(args);
    }
}