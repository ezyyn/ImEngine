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

namespace ImEditor.GameProject
{
    [DataContract]
    public class ProjectData
    {
        [DataMember]
        public string ProjectName { get; set; }
        [DataMember]
        public string ProjectPath { get; set; }
        [DataMember]
        public DateTime Date { get; set; }
        public string FullPath { get => $"{ProjectPath}{Project.DataDirectory}"; }
        public string PathToFile { get => $"{ProjectPath}{ProjectName}{Project.Extension}"; }
        public byte[] Icon { get; set; }
        public byte[] ScreenShot { get; set; }
    }
    [DataContract]
    public class ProjectDataList
    {
        [DataMember]
        public List<ProjectData> Projects { get; set; }
    }
    public class OpenProject 
    {
        private static readonly string m_AppDataPath = $@"{Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)}\ImEngineEditor\";
        private static readonly string m_ProjectDataPath;

        private static ObservableCollection<ProjectData> m_Projects = new ObservableCollection<ProjectData>();
        public static ReadOnlyObservableCollection<ProjectData> Projects
        {
            get;
        }

        static OpenProject()
        {
            try
            {
                if(!Directory.Exists(m_AppDataPath))
                    Directory.CreateDirectory(m_AppDataPath);

                m_ProjectDataPath = $@"{m_AppDataPath}ProjectData.xml";

                Projects = new ReadOnlyObservableCollection<ProjectData>(m_Projects);

                ReadProjectData();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }
        private static void ReadProjectData()
        {
            if(File.Exists(m_ProjectDataPath))
            {
                var projects = ImSerializer.FromFileXml<ProjectDataList>(m_ProjectDataPath).Projects.OrderByDescending(x => x.Date);
                m_Projects.Clear();

                foreach (var project in projects)
                {
                    if (Directory.Exists(project.FullPath))
                    {
                        project.Icon = File.ReadAllBytes($@"{project.FullPath}\Icon.png");
                        project.ScreenShot = File.ReadAllBytes($@"{project.FullPath}\ScreenShot.png");
                        m_Projects.Add(project);
                    }
                }
            }
        }
        private static void WriteProjectData()
        {
            var projects = m_Projects.OrderBy(x => x.Date).ToList();
            ImSerializer.ToFileXml(new ProjectDataList() { Projects = projects }, m_ProjectDataPath);
        }
        public static Project Open(ProjectData data)
        {
            ReadProjectData();
            var project = m_Projects.FirstOrDefault(x => x.FullPath == data.FullPath);
            if(project != null)
            {
                project.Date = DateTime.Now;
            }
            else
            {
                project = data;
                project.Date = DateTime.Now;
                m_Projects.Add(project);
            }

            WriteProjectData();

            return Project.Load(project.PathToFile);
        }

       
    }
}
