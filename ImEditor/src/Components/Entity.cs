using ImEditor.GameProject;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ImEditor.Components
{
    [DataContract]
    [KnownType(typeof(Transform))]
    public class Entity : ViewModelBase
    {
        private string m_Name;
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
