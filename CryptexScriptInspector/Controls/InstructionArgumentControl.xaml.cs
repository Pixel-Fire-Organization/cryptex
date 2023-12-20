using System.Globalization;
using System.Numerics;
using System.Windows;
using System.Windows.Controls;

using MaterialDesignThemes.Wpf;

namespace CryptexScriptInspector.Controls;

public partial class InstructionArgumentControl : UserControl
{
    public enum InstructionArgumentType
    {
        None, Memory, Hex, Decimal, Label
    }

    public string Title { get => (string)GetValue(TitleProperty); set => SetValue(TitleProperty, value); }

    // Using a DependencyProperty as the backing store for Property1.  
    // This enables animation, styling, binding, etc...
    public static readonly DependencyProperty TitleProperty
        = DependencyProperty.Register(nameof(Title),
                                      typeof(string),
                                      typeof(InstructionArgumentControl),
                                      new PropertyMetadata(""));

    public event EventHandler<EventArgs>? ControlEdit;

    public InstructionArgumentControl() { InitializeComponent(); }

    public InstructionArgumentType GetArgumentType()
    {
        if (rbNone.IsChecked is true) { return InstructionArgumentType.None; }
        if (rbMemory.IsChecked is true) { return InstructionArgumentType.Memory; }
        if (rbHex.IsChecked is true) { return InstructionArgumentType.Hex; }
        if (rbDecimal.IsChecked is true) { return InstructionArgumentType.Decimal; }
        if (rbLabel.IsChecked is true) { return InstructionArgumentType.Label; }

        //Should never come here!
        rbNone.IsChecked = true;
        return InstructionArgumentType.None;
    }

    public void SetArgumentType(InstructionArgumentType type)
    {
        switch (type)
        {
            case InstructionArgumentType.None:
                rbNone.IsChecked = true;
                break;
            case InstructionArgumentType.Memory:
                rbMemory.IsChecked = true;
                break;
            case InstructionArgumentType.Hex:
                rbHex.IsChecked = true;
                break;
            case InstructionArgumentType.Decimal:
                rbDecimal.IsChecked = true;
                break;
            case InstructionArgumentType.Label:
                rbLabel.IsChecked = true;
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(type), type, null);
        }
        
        tbContent.Text = "";
    }

    public string GetContent()
    {
        var argType = GetArgumentType();

        return argType switch
        {
            InstructionArgumentType.Memory  => int.TryParse(tbContent.Text, out int addr) ? $"${addr}" : "",
            InstructionArgumentType.Hex     => BigInteger.TryParse(tbContent.Text, NumberStyles.HexNumber, CultureInfo.InvariantCulture, out BigInteger addr) ? $"%{addr}" : "",
            InstructionArgumentType.Decimal => BigInteger.TryParse(tbContent.Text, out BigInteger addr) ? $"#{addr}" : decimal.TryParse(tbContent.Text, out decimal d) ? $"#{d}" : "",
            InstructionArgumentType.Label   => string.IsNullOrEmpty(tbContent.Text) || string.IsNullOrWhiteSpace(tbContent.Text) ? "" : tbContent.Text,
            _                               => ""
        };

    }

    private void RbMemory_OnChecked(object sender, RoutedEventArgs e)
    {
        if (tbContent is null) return;
        tbContent.IsEnabled = true;
        TextFieldAssist.SetPrefixText(tbContent, "$");
        ControlEdit?.Invoke(this, EventArgs.Empty);
    }

    private void RbHex_OnChecked(object sender, RoutedEventArgs e)
    {
        if (tbContent is null) return;
        tbContent.IsEnabled = true;
        TextFieldAssist.SetPrefixText(tbContent, "%");
        ControlEdit?.Invoke(this, EventArgs.Empty);
    }

    private void RbDecimal_OnChecked(object sender, RoutedEventArgs e)
    {
        if (tbContent is null) return;
        tbContent.IsEnabled = true;
        TextFieldAssist.SetPrefixText(tbContent, "#");
        ControlEdit?.Invoke(this, EventArgs.Empty);
    }

    private void RbLabel_OnChecked(object sender, RoutedEventArgs e)
    {
        if (tbContent is null) return;
        tbContent.IsEnabled = true;
        TextFieldAssist.SetPrefixText(tbContent, "↓");
        ControlEdit?.Invoke(this, EventArgs.Empty);
    }
    
    private void RbNone_OnChecked(object sender, RoutedEventArgs e)
    {
        if (tbContent is null) return;
        tbContent.IsEnabled = false;
        ControlEdit?.Invoke(this, EventArgs.Empty);
    }

    private void TbContent_OnTextChanged(object sender, TextChangedEventArgs e)
    {
        ControlEdit?.Invoke(this, EventArgs.Empty);
    }
}
