using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Media.Imaging;
using System.Xml.Linq;

namespace WPFDesigner_XML.Common.Models
{
    public class FolderModel : FileSystemObjectModel
    { 
          /// <summary>
        /// Initializes a new instance of the <see cref="Folder" /> class.
        /// </summary>
        public FolderModel(ProjectModel project)
            : base(project)
        {
            this.Folders = new List<FolderModel>();
            this.Files = new List<FileModel>();
            this.FolderType = FolderType.None;
            this.FileType = Models.FileType.None;
            this.FileSystemObjectModelType = FileSystemObjectType.Folder;
            this.Image = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(ImagesResources.Folder.GetHbitmap(), IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Folder"/> class.
        /// </summary>
        /// <param name="xmlModel">The XML model.</param>
        public FolderModel(XElement xmlModel, ProjectModel project)
            : this(project)
        {
            if (xmlModel != null)
            {
                this.FromXml(xmlModel);
            }
        }

        #region Properties
         
        /// <summary>
        /// Gets or sets the folders.
        /// </summary>
        /// <value>
        /// The folders.
        /// </value>
        public List<FolderModel> Folders { get; internal set; }

        /// <summary>
        /// Gets or sets the files.
        /// </summary>
        /// <value>
        /// The files.
        /// </value>
        public List<FileModel> Files { get; internal set; }

        #endregion

        #region Helpers

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
            this.Folders.ForEach(f => fileList.AddRange(f.SearchFilesOfType(type)));
            return fileList;
        }

        #endregion

        #region MobiseModel Base

        public override XElement ToXml()
        {
            XElement entity = new XElement("folder");
            return this.ToXml(entity, true);
        }

        public override XElement ToXml(XElement baseElement, bool AddCommonAttributes)
        {
            baseElement.SetAttributeValue("mobiseID", this.MobiseObjectID);
            baseElement.SetAttributeValue("name", this.Name);
            baseElement.SetAttributeValue("path", this.Path);
            baseElement.SetAttributeValue("type", this.FolderType.ToString());

            if (this.Folders != null)
            {
                foreach (FolderModel folder in this.Folders)
                {
                    baseElement.Add(folder.ToXml());
                }
            }

            if (this.Files != null)
            {
                foreach (FileModel file in this.Files)
                {
                    baseElement.Add(file.ToXml());
                }
            }

            return baseElement;
        }

        public override void FromXml(XElement xml)
        {
            if (xml.HasAttributes)
            {
                this.mMobiseObjectID = xml.Attribute("mobiseID") != null ? xml.Attribute("mobiseID").Value : Guid.NewGuid().ToString();
                this.Name = xml.Attribute("name") != null ? xml.Attribute("name").Value : string.Empty;
                this.Path = xml.Attribute("path") != null ? xml.Attribute("path").Value : string.Empty;
                this.FolderType = xml.Attribute("type") != null ? (FolderType)Enum.Parse(typeof(FolderType), xml.Attribute("type").Value) : FolderType.None;
            }

            if (xml.HasElements)
            {
                foreach (var folder in xml.Elements("folder"))
                {
                    this.Folders.Add(new FolderModel(folder, this.ParentProject) { ParentFolder = this, ParentProject = this.ParentProject });
                }

                foreach (var file in xml.Elements("file"))
                {
                    this.Files.Add(new FileModel(file, this.ParentProject) { ParentFolder = this, ParentProject = this.ParentProject });
                }
            }
        }


        #endregion
    }
}
