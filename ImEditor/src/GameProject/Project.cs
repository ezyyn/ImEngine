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
using System.Windows.Input;

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

        public string FullPath => $@"{Path}{Name}\{Name}{Extension}";

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

        public  static UndoRedo UndoRedo { get; } = new UndoRedo();

        private void AddSceneInternal(string sceneName)
        {
            Debug.Assert(!string.IsNullOrEmpty(sceneName.Trim()));

            m_Scenes.Add(new Scene(this, sceneName));
        }

        private void RemoveSceneInternal(Scene scene)
        {
            Debug.Assert(m_Scenes.Contains(scene));
            m_Scenes.Remove(scene);
        }

        public ICommand AddSceneCommand { get; set; }
        public ICommand RemoveSceneCommand { get; set; }
        public ICommand UndoCommand { get; set; }
        public ICommand RedoCommand { get; set; }
        public ICommand SaveCommand { get; set; }

        public static Project Load(string file)
        {
            Debug.Assert(File.Exists(file));

            return ImSerializer.FromFileXml<Project>(file);
        }

        public static void Save(Project project)
        {
            ImSerializer.ToFileXml<Project>(project, project.FullPath);
            Logger.Log(MessageType.Info, $"Project saved to {project.FullPath}");
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

            AddSceneCommand = new RelayCommand<object>(x =>
            {
                AddSceneInternal($"New Scene {m_Scenes.Count}");
                var newScene = m_Scenes.Last();
                var index = m_Scenes.Count - 1;
                UndoRedo.Add(new UndoRedoAction(
                    () => RemoveSceneInternal(newScene),
                    () => m_Scenes.Insert(index, newScene),
                    $"Add {newScene.Name}"
                    ));
            });

            RemoveSceneCommand = new RelayCommand<Scene>(x =>
            {
                var index = m_Scenes.IndexOf(x);
                RemoveSceneInternal(x);

                UndoRedo.Add(new UndoRedoAction(
                     () => m_Scenes.Insert(index, x),
                     () => RemoveSceneInternal(x),
                     $"Remove {x.Name}"
                     ));
            }, x => !x.Active);

            UndoCommand = new RelayCommand<object>(x => UndoRedo.Undo());
            RedoCommand = new RelayCommand<object>(x => UndoRedo.Redo());

            SaveCommand = new RelayCommand<object>(x => Save(this));
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
