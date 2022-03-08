using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace ImEditor.Dictionaries
{
    public partial class ControlTemplates : ResourceDictionary
    {
        private void OnTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            var textbox = sender as TextBox;
            var exp = textbox.GetBindingExpression(TextBox.TextProperty);
            if (exp == null) 
                return;

            if (e.Key == Key.Enter)
            {
                if (textbox.Tag is ICommand command && command.CanExecute(textbox.Text))
                {
                    command.Execute(textbox.Text);
                }
                else
                {
                    exp.UpdateSource();
                }

                Keyboard.ClearFocus();
                e.Handled = true;
            }
            else if (e.Key == Key.Escape)
            {
                exp.UpdateTarget();
                Keyboard.ClearFocus();
            }

        }
        /*   public ControlTemplates()
        {

        }*/
    }
}
