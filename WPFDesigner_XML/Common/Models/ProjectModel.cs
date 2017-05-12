using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Xml.Linq;
using WPFDesigner_XML.Common.Models.UIModels;
using WPFDesigner_XML.Common.Models.UIStyleModels;

namespace WPFDesigner_XML.Common.Models
{
    public class ProjectModel : FileSystemObjectModel
    {
        internal MobiseConfiguration mobiseConfiguration = null;

        /// <summary>
        /// The MB sync URL
        /// </summary>
        private string _MBSyncUrl;

        /// <summary>
        /// The MB sync URL
        /// </summary>
        private string _MBTestSyncUrl;

        /// <summary>
        /// Contains the changed style names.
        /// </summary>
        public Dictionary<string, string> RenamedStyles { get; set; }

        /// <summary>
        /// Gets or sets my property.
        /// </summary>
        /// <value>
        /// My property.
        /// </value>
        public string MBSyncUrl
        {
            get 
            { 
                return _MBSyncUrl; 
            }
            set 
            { 
                _MBSyncUrl = value;
                this.NotifyPropertyChanged("MBSyncUrl");
            }
        }

        /// <summary>
        /// Gets or sets my property.
        /// </summary>
        /// <value>
        /// My property.
        /// </value>
        public string MBTestSyncUrl
        {
            get
            {
                return _MBTestSyncUrl;
            }
            set
            {
                _MBTestSyncUrl = value;
                this.NotifyPropertyChanged("MBTestSyncUrl");
            }
        }
        

        /// <summary>
        /// Initializes a new instance of the <see cref="Project" /> class.
        /// </summary>
        public ProjectModel(string applicationVersion, MobiseConfiguration configuration): base (null)
        {
            this.RenamedStyles = new Dictionary<string, string>();
            this.mobiseConfiguration = configuration;
            this.FileSystemObjectModelType = FileSystemObjectType.File;
            this.FileType = Common.Models.FileType.Project;
            this.FolderType = Common.Models.FolderType.Project;
            this.MBSyncUrl = "http://107.20.134.125:1414/MBSyncService.svc";
            this.MBTestSyncUrl = "http://107.20.134.125:1414/TestSyncService.svc";

            this.Models = new List<DatabaseModel>();
            this.References = new TrulyObservableCollection<ReferencesModel>();
            this.Targets = new TrulyObservableCollection<TargetModel>();
            this.ConfigurationSections = new TrulyObservableCollection<ConfigurationSectionModel>();
            this.Folders = new TrulyObservableCollection<FolderModel>();
            this.Files = new List<FileModel>();
            this.Image = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(ImagesResources.Project.GetHbitmap(), IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
            this.Version = applicationVersion;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Project"/> class.
        /// </summary>
        /// <param name="xmlModel">The XML model.</param>
        public ProjectModel(XElement xmlModel, string applicationVersion, MobiseConfiguration configuration)
            : this(applicationVersion, configuration)
        {
            if (xmlModel != null)
            {
                this.FromXml(xmlModel);
            }
        }

        #region Project

        /// <summary>
        /// Gets or sets the image.
        /// </summary>
        /// <value>
        /// The image.
        /// </value>
        public ImageSource Image { get; set; }

        public int ProjectType { get; set; }

        private ProjectThemesModel currentProjectThemeModel = null;
        private FileModel currentThemesFileModel = null;
        public TrulyObservableCollection<ThemeModel> Themes 
        {
            get
            {
                if (currentProjectThemeModel == null && this.mobiseConfiguration != null)
                {
                    List<FileModel> filesList = this.SearchFilesOfType(FileType.Style);
                    if (filesList.Count > 0)
                    {
                        currentThemesFileModel = filesList[0];

                        ProjectThemesModel defaultThemes = new ProjectThemesModel();
                        foreach (var t in this.mobiseConfiguration.Themes) defaultThemes.Themes.Add(t);
                        foreach (var s in this.mobiseConfiguration.StyleAttributeDefaultValues) defaultThemes.StyleAttributeDefaultValues.Add(s);

                        currentProjectThemeModel = currentThemesFileModel.LoadModel(defaultThemes) as ProjectThemesModel;
                    }
                }
                return currentProjectThemeModel.Themes;
            }
        }

        /// <summary>
        /// Gets or sets the theme name.
        /// </summary>
        /// <value>
        /// the Theme name.
        /// </value>
        public string ThemeName { get; set; }

        /// <summary>
        /// Gets the project theme.
        /// </summary>
        /// <value>
        public ThemeModel ProjectTheme 
        {
            get
            {
                if (!string.IsNullOrEmpty(this.ThemeName))
                {
                    return this.Themes.Where(th => th.Name.ToLower() == this.ThemeName.ToLower()).FirstOrDefault();
                }
                else if (this.Themes != null)
                {
                    return this.Themes.FirstOrDefault();
                }
                return null;
            }
        }

        private int mTagCount;
        public int TagCount
        {
            get
            {
                return mTagCount;
            }
            set
            {
                mTagCount = value;
            }
        }

        public virtual void IncreaseTagCoun()
        {
            this.TagCount += 1;
        }

        public virtual void DecreaseTagCount()
        {
            this.TagCount -= 1;
        }

        private string mVersion;
        public string Version
        {
            get
            {
                return mVersion;
            }
            set
            {
                this.mVersion = value;
                this.NotifyPropertyChanged("Version");
            }
        }

        public void SaveThemes()
        {
            currentThemesFileModel.SaveModel(this.currentProjectThemeModel);
        }

        public virtual ProjectVersionStatus GetVersionStatus(string ApplicationVersion)
        {
            string[] versionNumbers = this.Version.Split('.');
            if (versionNumbers.Count() == 3)
            {
                int mayorVersion = int.Parse(versionNumbers[0]);
                int middleVersion = int.Parse(versionNumbers[1]);
                int minorVersion = int.Parse(versionNumbers[2]);

                string[] currentVersionNumbers = ApplicationVersion.Split('.');
                int mayorVersionActual = int.Parse(currentVersionNumbers[0]);
                int middleVersionActual = int.Parse(currentVersionNumbers[1]);
                int minorVersionActual = int.Parse(currentVersionNumbers[2]);

                if (mayorVersion == mayorVersionActual && middleVersion == middleVersionActual && minorVersion == minorVersionActual)
                {
                    return ProjectVersionStatus.same;
                }
                else
                {
                    if (mayorVersion <= mayorVersionActual)
                    {
                        if (mayorVersion == mayorVersionActual)
                        {
                            if (middleVersion <= middleVersionActual)
                            {
                                if (middleVersion == middleVersionActual)
                                {
                                    if (minorVersion > minorVersionActual)
                                    {

                                        return ProjectVersionStatus.newer;
                                    }
                                    else
                                    {
                                        return ProjectVersionStatus.older;
                                    }
                                }
                            }
                            else
                            {
                                return ProjectVersionStatus.newer;
                            }
                        }
                        else
                        {
                            return ProjectVersionStatus.older;
                        }
                    }
                    else
                    {
                        return ProjectVersionStatus.newer;
                    }
                }
            }


            return ProjectVersionStatus.unknown;

        }

        private TrulyObservableCollection<FolderModel> mFolders;
        public TrulyObservableCollection<FolderModel> Folders
        {
            get
            {
                return mFolders;
            }
            set
            {
                this.mFolders = value;
                this.NotifyPropertyChanged("Folders");
            }
        }

        private List<FileModel> mFiles;
        public List<FileModel> Files
        {
            get
            {
                return mFiles;
            }
            set
            {
                this.mFiles = value;
                this.NotifyPropertyChanged("Files");
            }
        }

        private List<DatabaseModel> mModels;
        public List<DatabaseModel> Models
        {
            get
            {
                return mModels;
            }
            set
            {
                this.mModels = value;
                this.NotifyPropertyChanged("Models");
            }
        }

        private TrulyObservableCollection<ReferencesModel> mReferences;
        public TrulyObservableCollection<ReferencesModel> References
        {
            get
            {
                return mReferences;
            }
            set
            {
                this.mReferences = value;
                this.NotifyPropertyChanged("References");
            }
        }

        private TrulyObservableCollection<TargetModel> mTargets;
        public TrulyObservableCollection<TargetModel> Targets
        {
            get
            {
                return mTargets;
            }
            set
            {
                this.mTargets = value;
                this.NotifyPropertyChanged("Targets");
            }
        }

        private TrulyObservableCollection<ConfigurationSectionModel> mConfigurationSections;
        public TrulyObservableCollection<ConfigurationSectionModel> ConfigurationSections
        {
            get
            {
                return mConfigurationSections;
            }
            set
            {
                this.mConfigurationSections = value;
                this.NotifyPropertyChanged("ConfigurationSections");
            }
        }

        public TrulyObservableCollection<ScreenModel> AllScreens
        {
            get
            {
                throw new NotImplementedException();
            }
        }


        #endregion

        #region file search

        /// <summary>
        /// Searches for files of the given type, and return a list of the matched files.
        /// </summary>
        /// <param name="type">The type of file to search for.</param>
        /// <returns>return a list of FileModel's of the given type</returns>
        public List<FileModel> SearchFilesOfType(FileType type)
        {
            List<FileModel> fileList = new List<FileModel>();
            var queryFiles = from file in this.Files where file.FileType == type select file;
            fileList.AddRange(queryFiles);
            foreach (var folder in this.Folders)
            {
                fileList.AddRange(folder.SearchFilesOfType(type));
            }

            return fileList;
        }

        #endregion

        #region MobiseModel Base

        public override XElement ToXml()
        {
            XElement entity = new XElement("project");
            this.ToXml(entity, true);
            return entity;
        }

        public override XElement ToXml(XElement entity, bool AddCommonAttributes)
        {
            if (AddCommonAttributes)
            {
                entity.SetAttributeValue("mobiseID", this.MobiseObjectID);
                entity.SetAttributeValue("name", this.Name);
            }

            entity.SetAttributeValue("tagCount", this.TagCount);
            entity.SetAttributeValue("projectType", this.ProjectType.ToString());
            entity.SetAttributeValue("version", this.Version);
            entity.SetAttributeValue("theme", this.ThemeName);
            entity.SetAttributeValue("mbsyncUrl", this.MBSyncUrl);
            entity.SetAttributeValue("mbtestsyncUrl", this.MBTestSyncUrl);

            if (this.Folders != null)
            {
                foreach (FolderModel folder in this.Folders)
                {
                    entity.Add(folder.ToXml());
                }
            }

            if (this.Files != null)
            {
                foreach (FileModel file in this.Files)
                {
                    entity.Add(file.ToXml());
                }
            }

            if (this.References != null)
            {
                foreach (ReferencesModel references in this.References)
                {
                    entity.Add(references.ToXml());
                }
            }

            if (this.Targets != null)
            {
                foreach (TargetModel target in this.Targets)
                {
                    entity.Add(target.ToXml());
                }
            }

            XElement configurationSectionsElement = new XElement("ConfigurationSections");
            if (this.ConfigurationSections != null)
            {
                foreach (ConfigurationSectionModel configSection in this.ConfigurationSections)
                {
                    configurationSectionsElement.Add(configSection.ToXml());
                }
            }
            entity.Add(configurationSectionsElement);

            return entity;
        }

        public override void FromXml(XElement xml)
        {
            if (xml.HasAttributes)
            {
                this.mMobiseObjectID = xml.Attribute("mobiseID") != null ? xml.Attribute("mobiseID").Value : Guid.NewGuid().ToString();
                this.Name = xml.Attribute("name") != null ? xml.Attribute("name").Value : string.Empty;
                this.TagCount = xml.Attribute("tagCount") != null ? int.Parse(xml.Attribute("tagCount").Value) : 0;
                this.Version = xml.Attribute("version") != null ? xml.Attribute("version").Value : "0.0.1";
                this.ProjectType = xml.Attribute("projectType") != null ? int.Parse(xml.Attribute("projectType").Value) : 0;
                this.ThemeName = xml.Attribute("theme") != null ? xml.Attribute("theme").Value : string.Empty;
                this.MBSyncUrl = xml.Attribute("mbsyncUrl") != null ? xml.Attribute("mbsyncUrl").Value : string.Empty;
                this.MBTestSyncUrl = xml.Attribute("mbtestsyncUrl") != null ? xml.Attribute("mbtestsyncUrl").Value : string.Empty;
            }

            if (xml.HasElements)
            {
                foreach (var folder in xml.Elements("folder"))
                {
                    this.Folders.Add(new FolderModel(folder, this));
                }
                
                // this.Folders = new TrulyObservableCollection<FolderModel>(this.Folders.OrderBy(f => f.Name).ToList());

                foreach (var file in xml.Elements("lookupTable"))
                {
                    this.Files.Add(new FileModel(file, this));
                }
                // this.Files = new List<FileModel>(this.Files.OrderBy(f => f.Name).ToList());

                foreach (var reference in xml.Elements("references"))
                {
                    this.References.Add(new ReferencesModel(reference));
                }

                foreach (var target in xml.Elements("targets"))
                {
                    this.Targets.Add(new TargetModel(target));
                }

                foreach (var configSetting in xml.Elements("ConfigurationSections").Elements("ConfigurationSection"))
                {
                    this.ConfigurationSections.Add(new ConfigurationSectionModel(configSetting));
                }
            }
        }

        #endregion

    }

    public enum ProjectVersionStatus
    {
        older,
        same,
        newer,
        unknown
    }
}
