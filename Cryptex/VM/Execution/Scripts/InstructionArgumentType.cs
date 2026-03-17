namespace Cryptex.VM.Execution.Scripts;

public enum InstructionArgumentType
{
    /// <summary>No argument / placeholder. Ignored by instructions.</summary>
    Empty = 0,

    /// <summary>A decimal literal constant (<c>N</c>). Value is an index into the Constants Block.</summary>
    Constant,

    /// <summary>A memory-slot reference (<c>$N</c>). Value is the slot index.</summary>
    MemoryAddress,

    /// <summary>A jump-target label. Value is the label index in the Jump Block.</summary>
    Label
}