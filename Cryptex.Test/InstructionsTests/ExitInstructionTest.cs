using Cryptex.VM.Execution;

namespace Cryptex.Test.InstructionsTests;

public sealed class ExitInstructionTest
{
    [Fact]
    public void TestVmExecute_ExitInstruction()
    {
        ScriptChunk mainChunk = new ScriptChunk("main", new[] { new ScriptChunkOpCode(OpCodes.Exit, "#0") });
        Script      script    = new Script("script", new[] { mainChunk });

        Executor executor = new Executor(script);
        executor.BeginExecution();
    }
}
