using ImEditor.Utilities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ImEditor.GameProject 
{
    [DataContract(Name = "Game")]
    public class Project : ViewModelBase
    {
        public static string Extension { get; } = ".imengine";

        public static string DataDirectory { get; } = ".ImEngine";

        [DataMember(Name = "Scenes")]
        private ObservableCollection<Scene> m_Scenes = new ObservableCollection<Scene>();

        [DataMember]
        public string Name { get; private set; } = "NewProject";

        [DataMember]
        public string Path { get; private set; }

        public string FullPath => $"{Path}{Name}{Extension}";

        public ReadOnlyObservableCollection<Scene> Scenes { get; private set; }

        private Scene m_ActiveScene;

        public Scene ActiveScene
        {
            get => m_ActiveScene;

            set
            {
                if(m_ActiveScene != value)
                {
                    m_ActiveScene = value;
                    OnPropertyChange(nameof(ActiveScene));
                }
            }
        }

        public static Project Current => Application.Current.MainWindow.DataContext as Project;

        public static Project Load(string file)
        {
            Debug.Assert(File.Exists(file));

            return ImSerializer.FromFileXml<Project>(file);
        }

        public static void Save(Project project)
        {
            ImSerializer.ToFileXml<Project>(project, project.FullPath);
        }

        [OnDeserialized]
        private void OnDeserialized(StreamingContext context)
        {
            if(m_Scenes != null)
            {
                Scenes = new ReadOnlyObservableCollection<Scene>(m_Scenes);
                OnPropertyChange(nameof(Scenes));
            }

            ActiveScene = Scenes.FirstOrDefault(x => x.Active);
            //m_Scenes.Add(new Scene(this, "Default Scene"));
        }

        public void Unload()
        {

        }

        public Project(string name, string path)
        {
            Name = name;
            Path = path;

            OnDeserialized(new StreamingContext());
        }
    }
}
