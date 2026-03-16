using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Documents;
using System.Windows.Media;

namespace CryptexScriptInspector;

internal static class InlineTextCreator
{
    public const string TOKEN_COLOR     = "&[C;";
    public const string TOKEN_HYPERLINK = "&[H;";
    public const string TOKEN_END       = "]&";

    public static Inline[] ParseText(string text)
    {
        if (string.IsNullOrEmpty(text))
            return [];

        List<Inline> inlines = [];

        StringBuilder sb = new StringBuilder();
        for (int i = 0; i < text.Length; i++)
        {
            string currentSubstring = text[i..];

            if (currentSubstring.StartsWith(TOKEN_COLOR) || currentSubstring.StartsWith(TOKEN_HYPERLINK))
            {
                inlines.Add(new Run { Text = sb.ToString() });

                sb.Clear();

                int advance = 0;

                if (currentSubstring.StartsWith(TOKEN_COLOR))
                    advance = ProcessColorToken(text, i, inlines);
                if (currentSubstring.StartsWith(TOKEN_HYPERLINK))
                    advance = ProcessHyperlinkToken(text, i, inlines);

                i += advance;

                continue;

            }

            sb.Append(text[i]);
        }

        if (sb.Length > 0)
            inlines.Add(new Run { Text = sb.ToString() });

        return inlines.ToArray();
    }

    private static int ProcessColorToken(string text, int i, List<Inline> inlines)
    {
        string coloredText = text[i..(text.IndexOf(TOKEN_END, i, StringComparison.Ordinal) + TOKEN_END.Length)];
        string colorValue  = coloredText.Remove(0, TOKEN_COLOR.Length);
        colorValue = colorValue.Remove(colorValue.IndexOf(';'));

        var clr =
            colorValue.StartsWith('#')
                ? (Color)ColorConverter.ConvertFromString(colorValue)
                : GetFromKnownColors(colorValue);

        string txt = coloredText[(coloredText.LastIndexOf(';') + 1)..coloredText.IndexOf(TOKEN_END, StringComparison.Ordinal)];

        inlines.Add(new Run
        {
            Text       = txt,
            Foreground = new SolidColorBrush(clr)
        });

        return coloredText.Length;
    }

    private static int ProcessHyperlinkToken(string text, int i, List<Inline> inlines)
    {
        string hyperlinkText = text[i..(text.IndexOf(TOKEN_END, i, StringComparison.Ordinal) + TOKEN_END.Length)];
        string hyperlink     = hyperlinkText.Remove(0, TOKEN_COLOR.Length);
        hyperlink = hyperlink.Remove(hyperlink.IndexOf(';'));

        string txt = hyperlinkText[(hyperlinkText.LastIndexOf(';') + 1)..hyperlinkText.IndexOf(TOKEN_END, StringComparison.Ordinal)];

        var hl = new Hyperlink
        {
            NavigateUri = new Uri(hyperlink),
            Inlines     = { new Run { Text = txt } }
        };

        hl.RequestNavigate += (_, args) => { OpenUrl(args.Uri.ToString()); };

        inlines.Add(hl);

        return hyperlinkText.Length;
    }

    private static Color GetFromKnownColors(string colorValue)
    {
        var clr = typeof(Colors).GetProperties().FirstOrDefault(x => x.Name == colorValue);
        if (clr is null)
            return Colors.Black;

        return (Color?)clr.GetValue(null, null) ?? Colors.Black;
    }

    private static void OpenUrl(string url)
    {
        try { Process.Start(url); }
        catch
        {
            // hack because of this: https://github.com/dotnet/corefx/issues/10361
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                url = url.Replace("&", "^&");
                Process.Start(new ProcessStartInfo(url) { UseShellExecute = true });
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux)) { Process.Start("xdg-open", url); }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX)) { Process.Start("open", url); }
            else { throw; }
        }
    }
}
