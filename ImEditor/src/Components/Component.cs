using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ImEditor.Components
{
    [DataContract]
    public class Component : ViewModelBase
    {
        [DataMember]
        public Entity Owner { get; private set; }

        public Component(Entity owner)
        {
            Debug.Assert(owner != null);
            Owner = owner;
        }
    }
}
