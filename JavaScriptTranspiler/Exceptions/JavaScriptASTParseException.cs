namespace JavaScriptTranspiler.Exceptions;

public class JavaScriptASTParseException : Exception
{
    public JavaScriptASTParseException(string innerExceptionMessage)
        : this(innerExceptionMessage, null)
    {
    }

    public JavaScriptASTParseException(string innerExceptionMessage, Exception? innerException)
        : base(innerExceptionMessage, innerException)
    {
    }
}