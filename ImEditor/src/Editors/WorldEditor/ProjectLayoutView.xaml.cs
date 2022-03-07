using ImEditor.Components;
using ImEditor.GameProject;
using ImEditor.Editors;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using ImEditor.Utilities;

namespace ImEditor.Editors
{
    /// <summary>
    /// Interaction logic for ProjectLayoutView.xaml
    /// </summary>
    public partial class ProjectLayoutView : UserControl
    {
        public ProjectLayoutView()
        {
            InitializeComponent();
        }

        private void OnAddEntityButton_Click(object sender, RoutedEventArgs e)
        {
            var btn = sender as Button;
            var vm = btn.DataContext as Scene;

            vm.AddEntityCommand.Execute(new Entity(vm) { Name="Empty Entity" });
        }

        private void OnEntitiesListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            EntityView.Instance.DataContext = null;
            var listBox = sender as ListBox;
            if(e.AddedItems.Count > 0)
            {
                EntityView.Instance.DataContext = listBox.SelectedItems[0];
            }

            var newSelection = listBox.SelectedItems.Cast<Entity>().ToList();

            var previousSelection = newSelection.Except(e.AddedItems.Cast<Entity>()).Concat(e.RemovedItems.Cast<Entity>()).ToList();

            Project.UndoRedo.Add(new UndoRedoAction(
                    () => 
                    {
                        // undo action
                        listBox.UnselectAll();
                        previousSelection.ForEach(x => (listBox.ItemContainerGenerator.ContainerFromItem(x) as ListBoxItem).IsSelected = true);
                    }, 
                    () => 
                    {
                        // redo action
                        listBox.UnselectAll();
                        newSelection.ForEach(x => (listBox.ItemContainerGenerator.ContainerFromItem(x) as ListBoxItem).IsSelected = true);
                    },
                    "Selection changed"
                ));
        }
    }
}
