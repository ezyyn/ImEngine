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
    public class ProjectTemplate
    {
        [DataMember]
        public string ProjectType { get; set; }
        [DataMember]
        public string ProjectFile { get; set; }
        public string ProjectFilePath { get; set; }
        [DataMember]
        public List<string> Folders { get; set; }
        public byte[] Icon { get; set; }
        public byte[] ScreenShot { get; set; }
        public string IconFilePath { get; set; }
        public string ScreenShotFilePath { get; set; }
    }


    class NewProject : ViewModelBase
    {
        private string m_ProjectName = "NewProject";
        private string m_ProjectPath = $@"{Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)}\ImEngineProjects\";
        private readonly string m_TemplatePath = $@"{Environment.CurrentDirectory}\ProjectTemplates\";
        private bool m_IsValid;
        private string m_ErrorMsg;

        private ObservableCollection<ProjectTemplate> m_ProjectTemplates = new ObservableCollection<ProjectTemplate>();
        public NewProject()
        {
            ProjectTemplates = new ReadOnlyObservableCollection<ProjectTemplate>(m_ProjectTemplates);

            try
            {
                var templates = Directory.GetFiles(m_TemplatePath, "template.xml", SearchOption.AllDirectories);
                Debug.Assert(templates.Any());

                foreach (var file in templates)
                {
                    var template = ImSerializer.FromFileXml<ProjectTemplate>(file);

                    template.IconFilePath = Path.GetFullPath(Path.Combine(Path.GetDirectoryName(file), "Icon.png"));
                    template.Icon = File.ReadAllBytes(template.IconFilePath);

                    template.ScreenShotFilePath = Path.GetFullPath(Path.Combine(Path.GetDirectoryName(file), "ScreenShot.png"));
                    template.ScreenShot = File.ReadAllBytes(template.ScreenShotFilePath);

                    template.ProjectFilePath = Path.GetFullPath(Path.Combine(Path.GetDirectoryName(file), template.ProjectFile));

                    m_ProjectTemplates.Add(template);
                }

                ValidateProject();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                // TODO: better logging system
            }
        }

        public string CreateProject(ProjectTemplate template)
        {
            if(!ValidateProject())
            {
                return string.Empty;
            }

            if (!ProjectPath.EndsWith(Path.DirectorySeparatorChar.ToString()))
            {
                ProjectPath += @"\";
            }
            var path = $@"{ProjectPath}{ProjectName}\";

            try
            {
                if(!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                
                foreach(var folder in template.Folders)
                {
                    Directory.CreateDirectory(Path.GetFullPath(Path.Combine(Path.GetDirectoryName(path), folder)));
                }

                var dirInfo = new DirectoryInfo(path + @".ImEngine\");
                dirInfo.Attributes |= FileAttributes.Hidden;
                File.Copy(template.IconFilePath, Path.GetFullPath(Path.Combine(dirInfo.FullName, "Icon.png")));
                File.Copy(template.ScreenShotFilePath, Path.GetFullPath(Path.Combine(dirInfo.FullName, "ScreenShot.png")));

                var projectXml = File.ReadAllText(template.ProjectFilePath);

                projectXml = ImFormatter.Format(projectXml, "{}", ProjectName, ProjectPath);

                var projectPath = Path.GetFullPath(Path.Combine(path, $"{ProjectName}{Project.Extension}"));
                File.WriteAllText(projectPath, projectXml);

                return path;
            }
            catch(Exception ex)
            {
                Debug.WriteLine(ex.Message);

                return string.Empty;
            }
        }

        private bool ValidateProject()
        {
            var path = ProjectPath;
            
            if(!path.EndsWith(Path.DirectorySeparatorChar.ToString()))
            {
                path += @"\"; 
            }
            path += $@"{ProjectName}\";

            m_IsValid = false;
            if (string.IsNullOrWhiteSpace(ProjectName.Trim()))
            {
                ErrorMsg = "Type in a project name.";
            }
            else if (ProjectName.IndexOfAny(Path.GetInvalidFileNameChars()) != -1)
            {
                ErrorMsg = "Invalid characters in project name.";
            }
            else if (string.IsNullOrWhiteSpace(ProjectPath.Trim()))
            {
                ErrorMsg = "Select a valid project folder.";
            }
            else if (ProjectPath.IndexOfAny(Path.GetInvalidPathChars()) != -1)
            {
                ErrorMsg = "Select a valid project folder.";
            }
            else if(Directory.Exists(path) && Directory.EnumerateFileSystemEntries(path).Any())
            {
                ErrorMsg = "Selected project folder already exists and is not empty.";
            }
            else
            {
                ErrorMsg = string.Empty;
                IsValid = true;
            }

            return IsValid;
        }

        public ReadOnlyObservableCollection<ProjectTemplate> ProjectTemplates
        {
            get;
        }

        public string ProjectName
        {
            get => m_ProjectName;
            set
            {
                if (m_ProjectName != value)
                {
                    m_ProjectName = value;
                    ValidateProject();
                    OnPropertyChange(nameof(ProjectName));
                }
            }
        }
        public string ProjectPath
        {
            get => m_ProjectPath;
            set
            {
                if (m_ProjectPath != value)
                {
                    m_ProjectPath = value;
                    ValidateProject();
                    OnPropertyChange(nameof(ProjectPath));
                }
            }
        }
        public bool IsValid
        {
            get => m_IsValid;

            set
            {
                if(m_IsValid != value)
                {
                    m_IsValid = value;
                    OnPropertyChange(nameof(IsValid));
                }
            }
        }

        public string ErrorMsg
        {
            get => m_ErrorMsg;

            set
            {
                if (m_ErrorMsg != value)
                {
                    m_ErrorMsg = value;
                    OnPropertyChange(nameof(ErrorMsg));
                }
            }
        }
    }
}
