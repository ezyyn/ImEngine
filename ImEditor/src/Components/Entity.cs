using ImEditor.GameProject;
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

namespace ImEditor.Components
{
    [DataContract]
    [KnownType(typeof(Transform))]
    public class Entity : ViewModelBase
    {
        private bool m_Enabled = true;
        private string m_Name;

        [DataMember]
        public bool Enabled
        {
            get => m_Enabled;

            set
            {
                if(m_Enabled != value)
                {
                    m_Enabled = value;
                    OnPropertyChange(nameof(Enabled));
                }
            }
        }
        [DataMember]
        public string Name
        {
            get => m_Name;
            set
            {
                if (m_Name != value)
                {
                    m_Name = value;
                    OnPropertyChange(nameof(Name));
                }
            }
        }
        [DataMember]
        public Scene ParentScene { get; private set; }
        public ICommand RenameCommand { get; private set; }
        public ICommand EnableCommand { get; private set; }

        [DataMember(Name = nameof(Components))]
        private readonly ObservableCollection<Component> m_Components = new ObservableCollection<Component>();
        public ReadOnlyObservableCollection<Component> Components { get; private set; }

        [OnDeserialized]
        void OnDeserialized(StreamingContext context)
        {
            if(m_Components != null)
            {
                Components = new ReadOnlyObservableCollection<Component>(m_Components);
                OnPropertyChange(nameof(Components));
            }

            RenameCommand = new RelayCommand<string>(x =>
            {
                var oldName = m_Name;
                Name = x;

                Project.UndoRedo.Add(new UndoRedoAction(nameof(Name)));
            }
            );
        }

        public Entity(Scene scene)
        {
            Debug.Assert(scene != null);
            ParentScene = scene;
            m_Components.Add(new Transform(this));
            OnDeserialized(new StreamingContext());
        }
    }
}
