using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ImEditor.GameProject
{
    /// <summary>
    /// Interaction logic for ProjectBrowserDialog.xaml
    /// </summary>
    public partial class ProjectBrowserDialog : Window
    {
        public ProjectBrowserDialog()
        {
            InitializeComponent();

            openProjectbutton.Click += OnToggleButton_Click;
            newProjectbutton.Click += OnToggleButton_Click;

            openProjectbutton.IsChecked = true;
        }

        private void OnToggleButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender == openProjectbutton)
            {
                if(newProjectbutton.IsChecked == true)
                {
                    newProjectbutton.IsChecked = false;
                    browserContent.Margin = new Thickness(0);
                }
                openProjectbutton.IsChecked = true;
            }
            else
            {
                if (openProjectbutton.IsChecked == true)
                {
                    openProjectbutton.IsChecked = false;
                    browserContent.Margin = new Thickness(-800, 0, 0, 0);
                }
                newProjectbutton.IsChecked = true;
            }
        }
    }
}
