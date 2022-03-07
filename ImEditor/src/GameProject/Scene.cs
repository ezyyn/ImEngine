using ImEditor.Components;
using ImEditor.Utilities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ImEditor.GameProject
{
    [DataContract]
    public class Scene : ViewModelBase
    {
        private string m_Name;
        private bool m_Active;

        [DataMember]
        public string Name
        {
            get => m_Name;
            set
            {
                if(m_Name != value)
                {
                    m_Name = value;
                    OnPropertyChange(nameof(Name));
                }
            }
        }
        [DataMember]
        public Project Project { get; private set; }
        [DataMember]
        public bool Active
        {
            get => m_Active;
            set
            {
                if(m_Active != value)
                {
                    m_Active = value;
                    OnPropertyChange(nameof(Active));
                }
            }

        }

        [DataMember(Name = nameof(Entities))]
        private readonly ObservableCollection<Entity> m_Entities = new ObservableCollection<Entity>();

        public ReadOnlyObservableCollection<Entity> Entities { get; private set; }

        public ICommand AddEntityCommand { get; private set; }
        public ICommand RemoveEntityCommand { get; private set; }

        private void AddEntityInternal(Entity entity)
        {
            Debug.Assert(!m_Entities.Contains(entity));
            m_Entities.Add(entity);
        }

        private void RemoveEntityInternal(Entity entity)
        {
            Debug.Assert(m_Entities.Contains(entity));
            m_Entities.Remove(entity);
        }

        [OnDeserialized]
        private void OnDeserialized(StreamingContext context)
        {
            if (m_Entities != null)
            {
                Entities = new ReadOnlyObservableCollection<Entity>(m_Entities);
                OnPropertyChange(nameof(Entities));
            }

            AddEntityCommand = new RelayCommand<Entity>(x =>
            {
                AddEntityInternal(x);
                var index = m_Entities.Count - 1;
                Project.UndoRedo.Add(new UndoRedoAction(
                    () => RemoveEntityInternal(x),
                    () => m_Entities.Insert(index, x),
                    $"Add {x.Name} to {Name}"
                    ));
            });

            RemoveEntityCommand = new RelayCommand<Entity>(x =>
            {
                var index = m_Entities.IndexOf(x);
                RemoveEntityInternal(x);

                Project.UndoRedo.Add(new UndoRedoAction(
                     () => m_Entities.Insert(index, x),
                     () => RemoveEntityInternal(x),
                      $"Remove {x.Name} from {Name}"
                     ));
            });
        }

        public Scene(Project project, string name)
        {
            Debug.Assert(project != null);
            Project = project;
            Name = name;
            OnDeserialized(new StreamingContext());
        }
    }
}
