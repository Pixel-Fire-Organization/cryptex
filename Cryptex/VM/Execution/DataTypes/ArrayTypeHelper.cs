namespace Cryptex.VM.Execution.DataTypes;

internal static class ArrayTypeHelper
{
    private const string ARRAY_PREFIX                = "&arr{";
    private const string NUMBER_INTEGER_TYPE_PREFIX  = "i";
    private const string NUMBER_FLOATING_TYPE_PREFIX = "f";

    public static string? CreateEmptyArray(DataTypes arrayType)
    {
        return arrayType switch
        {
            DataTypes.Null   => null,
            DataTypes.Number => CreateEmptyNumberArray(),
            _                => throw new ArgumentOutOfRangeException(nameof(arrayType), arrayType, null)
        };
    }

    public static string? CreateArray(DataTypes arrayType, int length) => string.Empty;

    public static int GetLength(ExecutorMemory memory, int slot)
    {
        string? slotValue = memory.GetSlot(slot);
        if (string.IsNullOrEmpty(slotValue) || !slotValue.StartsWith(ARRAY_PREFIX))
            return -1;

        string array = slotValue.Remove(0, ARRAY_PREFIX.Length);
        array = array.Remove(array.Length - 1);

        string[] contents = array.Split(",");
        return contents.Length;
    }

    public static string? GetElementAtIndex(ExecutorMemory memory, int slot, int index)
    {
        string? slotValue = memory.GetSlot(slot);
        if (string.IsNullOrEmpty(slotValue) || !slotValue.StartsWith(ARRAY_PREFIX))
            return null;

        string array = slotValue.Remove(0, ARRAY_PREFIX.Length);
        array = array.Remove(array.Length - 1);

        string[] contents = array.Split(",");
        if (index < 0 || index >= contents.Length)
            return null;

        return contents[index];
    }

    public static void SetElementAtIndex(ExecutorMemory memory, int slot, int index, string value)
    {
        string? slotValue = memory.GetSlot(slot);
        if (string.IsNullOrEmpty(slotValue) || !slotValue.StartsWith(ARRAY_PREFIX))
            return;

        string array = slotValue.Remove(0, ARRAY_PREFIX.Length);
        array = array.Remove(array.Length - 1);

        string[] contents = array.Split(",");
        if (index < 0 || index >= contents.Length)
            return;

        contents[index] = value;

        array = string.Format("{0}{1}}}", ARRAY_PREFIX, string.Join(",", contents));
        memory.SetSlot(slot, array);
    }

    private static string CreateEmptyNumberArray() => ARRAY_PREFIX + "}";
}
