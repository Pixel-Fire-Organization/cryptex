using Cryptex.VM.Execution;

namespace Cryptex.VM.Loaders;

public static class ScriptFileLoader
{
    public static Script? LoadScript(byte[] data)
    {
        var script = ScriptFile.LoadFromBytes(data);
        if (script.Chunks.Length == 0) //cannot have a script with no chunks.
            return null;

        List<ScriptChunk> chunks = new List<ScriptChunk>(script.Chunks.Length);
        for (int i = 0; i < chunks.Count; i++)
        {
            ScriptChunkOpCode[] instructions = new ScriptChunkOpCode[script.Chunks[i].Instructions.Length];
            Array.Copy(script.Chunks[i].Instructions, instructions,
                       script.Chunks[i].Instructions.Length);

            chunks[i] = new ScriptChunk(script.Chunks[i].ChunkName, instructions);
        }

        if (chunks[0].Name != script.EntryPointName)
        {
            var entryChunk = chunks.FirstOrDefault(chunk => chunk.Name == script.EntryPointName);
            if (entryChunk is null)
                return null;

            chunks.Remove(entryChunk);
            chunks.Insert(0, entryChunk);
        }

        return new Script(script.ScriptName, chunks.ToArray());
    }
    
    public static Script? LoadScript(string path)
    {
        if (string.IsNullOrEmpty(path) || !File.Exists(path)) //invalid path.
            return null;

        byte[] data = File.ReadAllBytes(path);

        return LoadScript(data);
    }
}
