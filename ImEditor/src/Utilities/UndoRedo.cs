using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImEditor.Utilities
{
    public interface IUndoRedo
    {
        string Name { get; }

        void Undo();
        void Redo();
    }

    public class UndoRedoAction : IUndoRedo
    {
        private Action m_UndoAction;
        private Action m_RedoAction;

        public string Name { get; }

        public void Redo() => m_RedoAction();

        public void Undo() => m_UndoAction();

        public UndoRedoAction(string name)
        {
            Name = name;
        }

        public UndoRedoAction(Action undo, Action redo, string name) : this(name)
        {
            Debug.Assert(undo != null && redo != null);
            m_UndoAction = undo;
            m_RedoAction = redo;
        }
    }
    public class UndoRedo
    {
        private bool m_EnableAdd = true;
        private readonly ObservableCollection<IUndoRedo> m_UndoList = new ObservableCollection<IUndoRedo>();
        private readonly ObservableCollection<IUndoRedo> m_RedoList = new ObservableCollection<IUndoRedo>();

        public ReadOnlyObservableCollection<IUndoRedo> UndoList { get; } 
        public ReadOnlyObservableCollection<IUndoRedo> RedoList { get; } 

        public void Reset()
        {
            m_RedoList.Clear();
            m_UndoList.Clear();
        }

        public void Add(IUndoRedo cmd)
        {
            if(m_EnableAdd)
            {
                m_UndoList.Add(cmd);
                m_RedoList.Clear();
            }
        }

        public void Undo()
        {
            if(m_UndoList.Any())
            {
                var last = m_UndoList.Last();
                m_UndoList.RemoveAt(m_UndoList.Count - 1);
                m_EnableAdd = false;
                last.Undo();
                m_EnableAdd = true;
                m_RedoList.Insert(0, last);
            }
        }

        public void Redo()
        {
            if (m_RedoList.Any())
            {
                var first = m_UndoList.First();
                m_RedoList.RemoveAt(0);
                m_EnableAdd = false;
                first.Redo();
                m_EnableAdd= true;
                m_UndoList.Add(first);
            }
        }


        public UndoRedo()
        {
            RedoList = new ReadOnlyObservableCollection<IUndoRedo>(m_RedoList);
            UndoList = new ReadOnlyObservableCollection<IUndoRedo>(m_UndoList);
        }
    }
}
