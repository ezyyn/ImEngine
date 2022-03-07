using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ImEditor.Components
{
    [DataContract]
    public class Transform : Component
    {
        private Vector3 m_Translation;
        private Vector3 m_Rotation;
        private Vector3 m_Scale;

        [DataMember]
        public Vector3 Translation
        {
            get => m_Translation;
            set
            {
                if (m_Translation != value)
                {
                    m_Translation = value;
                    OnPropertyChange(nameof(Translation));
                }
            }
        }
        [DataMember]
        public Vector3 Rotation
        {
            get => m_Rotation;
            set
            {
                if (m_Rotation != value)
                {
                    m_Rotation = value;
                    OnPropertyChange(nameof(Rotation));
                }
            }
        }
        [DataMember]
        public Vector3 Scale
        {
            get => m_Scale;
            set
            {
                if (m_Scale != value)
                {
                    m_Scale = value;
                    OnPropertyChange(nameof(Scale));
                }
            }
        }

        public Transform(Entity owner) : base(owner)
        {
        }
    }
}
