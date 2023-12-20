using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using CryptexScriptInspector.Dialogs;

namespace CryptexScriptInspector;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    public MainWindow() { InitializeComponent(); }

    private void File_Exit_OnClick(object sender, RoutedEventArgs e) { Application.Current.Shutdown(0); }

    private void BtnDeleteInstruction_OnClick(object sender, RoutedEventArgs e)
    {
        if (listboxInstructions.SelectedIndex != -1)
        {
            listboxInstructions.Items.RemoveAt(listboxInstructions.SelectedIndex);
            listboxInstructions.SelectedIndex = -1;
        }
    }

    private void BtnMoveUpInstruction_OnClick(object sender, RoutedEventArgs e)
    {
        //cannot move up the first element, so element 1 is the first this operation can be performed on.
        if (listboxInstructions.SelectedIndex > 0)
        {
            var index = listboxInstructions.SelectedIndex;
            var curr  = listboxInstructions.Items[listboxInstructions.SelectedIndex];
            var above = listboxInstructions.Items[listboxInstructions.SelectedIndex - 1];

            listboxInstructions.Items.Remove(curr);
            listboxInstructions.Items.Remove(above);

            listboxInstructions.Items.Insert(index - 1, curr);
            listboxInstructions.Items.Insert(index, above);

            listboxInstructions.SelectedIndex = index - 1;
        }
    }

    private void BtnMoveDownInstruction_OnClick(object sender, RoutedEventArgs e)
    {
        //cannot move down the last element, so the penultimate element is the first this operation can be performed on.
        if (listboxInstructions.SelectedIndex != -1 && listboxInstructions.SelectedIndex < listboxInstructions.Items.Count - 1)
        {
            var index = listboxInstructions.SelectedIndex;
            var curr  = listboxInstructions.Items[listboxInstructions.SelectedIndex];
            var below = listboxInstructions.Items[listboxInstructions.SelectedIndex + 1];

            listboxInstructions.Items.Remove(curr);
            listboxInstructions.Items.Remove(below);

            listboxInstructions.Items.Insert(index, curr);
            listboxInstructions.Items.Insert(index, below);

            listboxInstructions.SelectedIndex = index + 1;
        }
    }

    private void BtnAddInstruction_OnClick(object sender, RoutedEventArgs e)
    {
        AddInstructionDialog dlg = new AddInstructionDialog();
        dlg.ShowDialog();
        if (dlg.DialogResult is true)
        {
            
        }
    }
}
