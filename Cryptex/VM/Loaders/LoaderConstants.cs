namespace Cryptex.VM.Loaders;

internal sealed class LoaderConstants
{
    public const int INSTRUCTION_ARG_SIZE    = 1024;
    public const int INSTRUCTION_SIZE        = sizeof(byte) + INSTRUCTION_ARG_SIZE;
    public const int MAX_CHUNK_INSTRUCTIONS  = 16384;
    public const int CHUNK_INSTRUCTIONS_SIZE = INSTRUCTION_SIZE * MAX_CHUNK_INSTRUCTIONS;

    public const int CHUNK_NAME_SIZE = 256;
}
