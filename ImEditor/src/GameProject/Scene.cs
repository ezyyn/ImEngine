using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

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

        public Scene(Project project, string name)
        {
            Debug.Assert(project != null);
            Project = project;
            Name = name;
        }
    }
}
