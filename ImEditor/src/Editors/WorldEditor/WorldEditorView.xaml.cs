﻿using ImEditor.GameProject;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
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

namespace ImEditor.Editors
{
    /// <summary>
    /// Interaction logic for WorldEditor.xaml
    /// </summary>
    public partial class WorldEditorView : UserControl
    {
        public WorldEditorView()
        {
            InitializeComponent();

            Loaded += OnWorldEditorView_Loaded;
        }

        private void OnWorldEditorView_Loaded(object sender, RoutedEventArgs args)
        {
            Loaded -= OnWorldEditorView_Loaded;
            Focus();

            ((INotifyCollectionChanged)Project.UndoRedo.UndoList).CollectionChanged += (s, e) =>Focus();
            ((INotifyCollectionChanged)Project.UndoRedo.RedoList).CollectionChanged += (s, e) =>Focus();
        }
    }
}
