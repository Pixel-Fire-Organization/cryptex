using System.Globalization;
using System.Text.RegularExpressions;
using System.Windows.Controls;

namespace CryptexScriptInspector.Controls;

public partial class InstructionArgumentContentValidationRule : ValidationRule
{
    public override ValidationResult Validate(object? value, CultureInfo cultureInfo)
    {
        string? s = (value ?? "").ToString();
        if (string.IsNullOrEmpty(s) || string.IsNullOrWhiteSpace(s))
            return new ValidationResult(false, "Field must be filled!");

        //TODO: fix fix: not working
        /*Regex r = RegexAllowedCharacters();
        if (!r.Match(s).Success)
            return new ValidationResult(false, "Only numbers, decimal dot and letters(A-F) allowed!");

        if (RegexHexLetters().Match(s).Success && s.Contains('.'))
            return new ValidationResult(false, "Cannot have hex and floating numbers!");*/

        return ValidationResult.ValidResult;
    }

    [GeneratedRegex("[a-f]")]
    private static partial Regex RegexHexLetters();

    [GeneratedRegex("[a-f]|[A-F]|[.]|[1-9]")]
    private static partial Regex RegexAllowedCharacters();
}
