using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;

using Cryptex.VM.Execution;

using CryptexScriptInspector.Controls;

using MaterialDesignThemes.Wpf;

using static CryptexScriptInspector.OpCodeDescriptions;

namespace CryptexScriptInspector.Dialogs;

public partial class AddInstructionDialog : Window
{
    private bool m_filterFirstRun = true;

    public AddInstructionDialog()
    {
        InitializeComponent();
        DisplayInstructionsByFilter(OpCodeType.All);
        listBoxInstructions.SelectedIndex = 0;
    }

    private void ListBoxInstructions_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (listBoxInstructions.SelectedIndex != -1)
        {
            if (listBoxInstructions.Items[listBoxInstructions.SelectedIndex] is ListBoxItem it)
            {
                string  opcode = it.Name.Remove(0, 1);
                OpCodes code;
                try { code = Enum.Parse<OpCodes>(opcode, true); }
                catch(Exception ex)
                {
                    MessageBox.Show($"Caught an exception while trying to get opcode info. Exception: {ex.GetType()} | {ex.Message}");
                    return;
                }

                tblInstructionInfo.Inlines.Clear();
                tblInstructionInfo.Inlines.AddRange(GetInfoForOpCode(code));
                ValidateArguments();
            }
        }
    }

    private Inline[] GetInfoForOpCode(OpCodes code)
    {
        var TC = InlineTextCreator.TOKEN_COLOR;
        var TH = InlineTextCreator.TOKEN_HYPERLINK;
        var TE = InlineTextCreator.TOKEN_END;

        return code switch
        {
            OpCodes.Term  => InlineTextCreator.ParseText(TERM_DESC),
            OpCodes.Nop   => InlineTextCreator.ParseText(NOP_DESC),
            OpCodes.Exit  => InlineTextCreator.ParseText(EXIT_DESC),
            OpCodes.Crash => InlineTextCreator.ParseText(CRASH_DESC),
            OpCodes.Inc   => InlineTextCreator.ParseText(INC_DESC),
            OpCodes.Add   => InlineTextCreator.ParseText(ADD_DESC),
            OpCodes.Sub   => InlineTextCreator.ParseText(SUB_DESC),
            OpCodes.Dec   => InlineTextCreator.ParseText(DEC_DESC),
            OpCodes.Mul   => InlineTextCreator.ParseText(MUL_DESC),
            OpCodes.Div   => InlineTextCreator.ParseText(DIV_DESC),
            OpCodes.IncF  => InlineTextCreator.ParseText(INCF_DESC),
            OpCodes.AddF  => InlineTextCreator.ParseText(ADDF_DESC),
            OpCodes.SubF  => InlineTextCreator.ParseText(SUBF_DESC),
            OpCodes.DecF  => InlineTextCreator.ParseText(DECF_DESC),
            OpCodes.MulF  => InlineTextCreator.ParseText(MULF_DESC),
            OpCodes.DivF  => InlineTextCreator.ParseText(DIVF_DESC),
            OpCodes.Mod   => InlineTextCreator.ParseText(MOD_DESC),
            OpCodes.Arg   => InlineTextCreator.ParseText(ARG_DESC),
            OpCodes.Exec  => InlineTextCreator.ParseText(EXEC_DESC),
            OpCodes.Call  => InlineTextCreator.ParseText(CALL_DESC),
            OpCodes.Ret   => InlineTextCreator.ParseText(RET_DESC),
            OpCodes.Res   => InlineTextCreator.ParseText(RES_DESC),
            OpCodes.Cmp =>
            [
                new Run
                {
                    Text       = "WIP",
                    Foreground = new SolidColorBrush(Colors.Green)
                }
            ],
            OpCodes.Jmp =>
            [
                new Run
                {
                    Text       = "WIP",
                    Foreground = new SolidColorBrush(Colors.Green)
                }
            ],
            OpCodes.Jeq =>
            [
                new Run
                {
                    Text       = "WIP",
                    Foreground = new SolidColorBrush(Colors.Green)
                }
            ],
            OpCodes.Jnq =>
            [
                new Run
                {
                    Text       = "WIP",
                    Foreground = new SolidColorBrush(Colors.Green)
                }
            ],
            OpCodes.Jls =>
            [
                new Run
                {
                    Text       = "WIP",
                    Foreground = new SolidColorBrush(Colors.Green)
                }
            ],
            OpCodes.Jgr =>
            [
                new Run
                {
                    Text       = "WIP",
                    Foreground = new SolidColorBrush(Colors.Green)
                }
            ],
            OpCodes.Jge =>
            [
                new Run
                {
                    Text       = "WIP",
                    Foreground = new SolidColorBrush(Colors.Green)
                }
            ],
            OpCodes.Jle =>
            [
                new Run
                {
                    Text       = "WIP",
                    Foreground = new SolidColorBrush(Colors.Green)
                }
            ],
            OpCodes.Shl =>
            [
                new Run
                {
                    Text       = "WIP",
                    Foreground = new SolidColorBrush(Colors.Green)
                }
            ],
            OpCodes.Shr =>
            [
                new Run
                {
                    Text       = "WIP",
                    Foreground = new SolidColorBrush(Colors.Green)
                }
            ],
            OpCodes.And =>
            [
                new Run
                {
                    Text       = "WIP",
                    Foreground = new SolidColorBrush(Colors.Green)
                }
            ],
            OpCodes.Or =>
            [
                new Run
                {
                    Text       = "WIP",
                    Foreground = new SolidColorBrush(Colors.Green)
                }
            ],
            OpCodes.Xor =>
            [
                new Run
                {
                    Text       = "WIP",
                    Foreground = new SolidColorBrush(Colors.Green)
                }
            ],
            OpCodes.Not =>
            [
                new Run
                {
                    Text       = "WIP",
                    Foreground = new SolidColorBrush(Colors.Green)
                }
            ],
            OpCodes.Load => InlineTextCreator.ParseText(LOAD_DESC),
            OpCodes.Free =>
            [
                new Run
                {
                    Text       = "WIP",
                    Foreground = new SolidColorBrush(Colors.Green)
                }
            ],
            OpCodes.Reg =>
            [
                new Run
                {
                    Text       = "WIP",
                    Foreground = new SolidColorBrush(Colors.Green)
                }
            ],
            OpCodes.UnReg =>
            [
                new Run
                {
                    Text       = "WIP",
                    Foreground = new SolidColorBrush(Colors.Green)
                }
            ],
            OpCodes.ArrAccess =>
            [
                new Run
                {
                    Text       = "WIP",
                    Foreground = new SolidColorBrush(Colors.Green)
                }
            ],
            OpCodes.ArrCreate =>
            [
                new Run
                {
                    Text       = "WIP",
                    Foreground = new SolidColorBrush(Colors.Green)
                }
            ],
            OpCodes.ArrFree =>
            [
                new Run
                {
                    Text       = "WIP",
                    Foreground = new SolidColorBrush(Colors.Green)
                }
            ],
            OpCodes.ArrLen =>
            [
                new Run
                {
                    Text       = "WIP",
                    Foreground = new SolidColorBrush(Colors.Green)
                }
            ],
            OpCodes.ArrSet =>
            [
                new Run
                {
                    Text       = "WIP",
                    Foreground = new SolidColorBrush(Colors.Green)
                }
            ],
            OpCodes.StrCreate =>
            [
                new Run
                {
                    Text       = "WIP",
                    Foreground = new SolidColorBrush(Colors.Green)
                }
            ],
            OpCodes.StrSub =>
            [
                new Run
                {
                    Text       = "WIP",
                    Foreground = new SolidColorBrush(Colors.Green)
                }
            ],
            OpCodes.StrAppend =>
            [
                new Run
                {
                    Text       = "WIP",
                    Foreground = new SolidColorBrush(Colors.Green)
                }
            ],
            OpCodes.StrFree =>
            [
                new Run
                {
                    Text       = "WIP",
                    Foreground = new SolidColorBrush(Colors.Green)
                }
            ],
            OpCodes.StrNum =>
            [
                new Run
                {
                    Text       = "WIP",
                    Foreground = new SolidColorBrush(Colors.Green)
                }
            ],
            OpCodes.StrChar =>
            [
                new Run
                {
                    Text       = "WIP",
                    Foreground = new SolidColorBrush(Colors.Green)
                }
            ],
            OpCodes.Print =>
            [
                new Run
                {
                    Text       = "WIP",
                    Foreground = new SolidColorBrush(Colors.Green)
                }
            ],
            OpCodes.Read =>
            [
                new Run
                {
                    Text       = "WIP",
                    Foreground = new SolidColorBrush(Colors.Green)
                }
            ],
            OpCodes.ReadLine =>
            [
                new Run
                {
                    Text       = "WIP",
                    Foreground = new SolidColorBrush(Colors.Green)
                }
            ],
            OpCodes.Random =>
            [
                new Run
                {
                    Text       = "WIP",
                    Foreground = new SolidColorBrush(Colors.Green)
                }
            ],
            OpCodes.RandomF =>
            [
                new Run
                {
                    Text       = "WIP",
                    Foreground = new SolidColorBrush(Colors.Green)
                }
            ],
            _ => throw new ArgumentOutOfRangeException(nameof(code), code, null)
        };
    }

    private void cbFilterInstructions_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        //When the app loads it will trigger this callback, but it depends on all controls to be initialized.
        //However then they aren't, so we exit.
        if (m_filterFirstRun)
        {
            m_filterFirstRun = false;
            return;
        }

        if (cbFilterInstructions.SelectedIndex == -1)
            cbFilterInstructions.SelectedIndex = 0;

        DisplayInstructionsByFilter(GetCurrentFilter());
    }

    private OpCodeType GetCurrentFilter()
    {
        var selectedItem = cbFilterInstructions.SelectedItem as ComboBoxItem;
        if (selectedItem is null)
        {
            //ERROR: Cannot be null!
            throw new NullReferenceException();
        }

        return selectedItem.Name switch
        {
            "cbiAllInstructions"      => OpCodeType.All,
            "cbiVMInstructions"       => OpCodeType.VM,
            "cbiMathInstructions"     => OpCodeType.Math,
            "cbiBitwiseInstructions"  => OpCodeType.Bitwise,
            "cbiCallInstructions"     => OpCodeType.Call,
            "cbiBranchInstructions"   => OpCodeType.Branching,
            "cbiMemoryInstructions"   => OpCodeType.Memory,
            "cbiArrayInstructions"    => OpCodeType.Arrays,
            "cbiStringInstructions"   => OpCodeType.String,
            "cbiFunctionInstructions" => OpCodeType.Functions,
            _                         => throw new ArgumentOutOfRangeException("", "Not all filters for adding instructions are added in code!")
        };
    }

    private void DisplayInstructionsByFilter(OpCodeType typeToFilter)
    {
        listBoxInstructions.Items.Clear();

        OpCodes[] opcodes = (OpCodes[])Enum.GetValues(typeof(OpCodes));
        foreach (var opcode in opcodes)
        {
            PackIcon  icon = new PackIcon { Margin = new Thickness(0, 0, 5, 0) };
            TextBlock text = new TextBlock { Text  = opcode.ToString() };

            PackIconKind kind = PackIconKind.Error;
            foreach (var set in OpCodeTypes.OpCodeIcons.Keys)
            {
                if (set.Contains(opcode))
                    kind = OpCodeTypes.OpCodeIcons[set];
            }

            //filters the instructions.
            if (typeToFilter != OpCodeType.All && OpCodeTypes.IconToType[kind] != typeToFilter)
                continue;

            icon.Kind = kind;

            StackPanel content = new StackPanel { Orientation = Orientation.Horizontal };
            content.Children.Add(icon);
            content.Children.Add(text);

            listBoxInstructions.Items.Add(new ListBoxItem
            {
                Content = content,
                Name    = $"_{opcode}"
            });
        }
    }

    private void ValidateArguments()
    {
        if (listBoxInstructions.SelectedIndex == -1)
            return;

        var selectedItem = listBoxInstructions.Items[listBoxInstructions.SelectedIndex] as ListBoxItem;
        var opCode       = Enum.Parse<OpCodes>(selectedItem?.Name.Remove(0, 1) ?? "Term", true);

        if (!OpCodeArguments.OpCodeArgs.TryGetValue(opCode, out var opCodeArgTypes))
            opCodeArgTypes = OpCodeArguments.OpCodeArgs[OpCodes.Term];

        InstructionArgumentControl[] argControls = new InstructionArgumentControl[spInstructionArguments.Children.Count];
        for (int i = 0; i < spInstructionArguments.Children.Count; i++)
        {
            if (spInstructionArguments.Children[i] is not InstructionArgumentControl ctrl)
            {
                MessageBox.Show("Invalid child in the arguments panel!");
                tbValidInstruction.Text = "Invalid! (invalid argument in panel)";
                tbFinalResult.Text      = "";
                return;
            }

            argControls[i] = ctrl;
        }

        if (opCodeArgTypes.Count > 0 && !AreRestArgumentsTypeNone(opCodeArgTypes.Count, argControls))
        {
            tbValidInstruction.Text = "Invalid! (different argument count)";
            tbFinalResult.Text      = "";
            return;
        }

        for (int i = 0; i < argControls.Length; i++)
        {
            if (i >= opCodeArgTypes.Count)
                break;

            if (argControls[i].GetArgumentType() != opCodeArgTypes[i])
            {
                tbValidInstruction.Text = $"Invalid! (different argument types | argument: {i + 1})";
                tbFinalResult.Text      = "";
                return;
            }

            if (argControls[i].GetContent() == "")
            {
                tbValidInstruction.Text = $"Invalid! (invalid argument content | argument: {i + 1})";
                tbFinalResult.Text      = "";
                return;
            }
        }

        tbValidInstruction.Text = "Valid!";
        tbFinalResult.Text      = $"{opCode.ToString().ToLower()} ";
        for (int i = 0; i < opCodeArgTypes.Count; i++)
        {
            if (i == opCodeArgTypes.Count - 1)
                tbFinalResult.Text += argControls[i].GetContent();
            else
                tbFinalResult.Text += $"{argControls[i].GetContent()}, ";
        }
    }

    private bool AreRestArgumentsTypeNone(int startIndex, InstructionArgumentControl[] controls)
    {
        if (startIndex < 0 || startIndex >= controls.Length)
            return false;

        for (int i = startIndex; i < controls.Length; i++)
            if (controls[i].GetArgumentType() != InstructionArgumentControl.InstructionArgumentType.None)
                return false;

        return true;
    }

    private void BtnValidate_OnClick(object sender, RoutedEventArgs e) => ValidateArguments();

    private void BtnSubmit_OnClick(object sender, RoutedEventArgs e)
    {
        //TODO: make the window close and pass the ready instruction to the caller.
    }

    private void BtnClear_OnClick(object sender, RoutedEventArgs e)
    {
        for (int i = 0; i < spInstructionArguments.Children.Count; i++)
        {
            if (spInstructionArguments.Children[i] is not InstructionArgumentControl ctrl)
            {
                MessageBox.Show("Invalid child in the arguments panel!");
                continue;
            }

            ctrl.SetArgumentType(InstructionArgumentControl.InstructionArgumentType.None);
        }
    }

    private void InstructionArgumentControl_OnControlEdit(object? sender, EventArgs e) => ValidateArguments();
}
