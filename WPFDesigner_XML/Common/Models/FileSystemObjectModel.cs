using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Xml.Linq;
 

namespace WPFDesigner_XML.Common.Models
{

    public class FileSystemObjectModel : MobiseModel
    {
        /// <summary>
        /// Gets or sets the parent folder.
        /// </summary>
        /// <value>
        /// The parent folder.
        /// </value>
        public FolderModel ParentFolder { get; set; }

        public ProjectModel ParentProject { get; set; }

        public FileSystemObjectModel(ProjectModel project)
            : base()
        { 
            this.FileType = FileType.None;
            this.FileSystemObjectModelType = FileSystemObjectType.File;
            this.ParentProject = project;
        }

        private FileVisibility mVisibility;
        public FileVisibility FileVisibility
        {
            get
            {
                return this.mVisibility;
            }
            set
            {
                this.mVisibility = value;
                this.NotifyPropertyChanged("FileVisibility");
            }

        }

        /// <summary>
        /// Gets or sets the path.
        /// </summary>
        /// <value>
        /// The path.
        /// </value>
        public string Path { get; set; }

        /// <summary>
        /// Gets or sets the image.
        /// </summary>
        /// <value>
        /// The image.
        /// </value>
        public ImageSource Image { get; set; }

        /// <summary>
        /// The type of the file
        /// </summary>
        public FileSystemObjectType FileSystemObjectModelType { get; set; }

        private FileType mFileType;
        /// <summary>
        /// The type of the file
        /// </summary>
        public FileType FileType
        {
            get
            {
                return this.mFileType;
            }
            set
            {
                this.mFileType = value;
                this.SetImageFromType();
            }
        }

        /// <summary>
        /// Gets or sets the type of the folder.
        /// </summary>
        /// <value>
        /// The type of the folder.
        /// </value>
        public FolderType FolderType
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the current last modified date of the file or folder.
        /// </summary>
        public DateTime LastModifiedDate
        {
            get
            {
                string filePath = System.IO.Path.Combine(this.ParentProject.Path, this.Path);
                if (!String.IsNullOrEmpty(filePath))
                {
                    return System.IO.File.GetLastWriteTime(filePath);
                }
                return DateTime.Now;
            }
            
        }

        /// <summary>
        /// Sets the type of the image from.
        /// </summary>
        private void SetImageFromType()
        {
            Bitmap bitmap = null;

            switch (this.FileType)
            {
                case FileType.None:
                    bitmap = ImagesResources.File;
                    break;
                case FileType.Model:
                    bitmap = ImagesResources.Model;
                    break;
                case FileType.SQLite:
                    bitmap = ImagesResources.Database;
                    break;
                case FileType.ScreenDefinition:
                    bitmap = ImagesResources.UIDesigner;
                    break;
                case FileType.CustomControl:
                    bitmap = ImagesResources.CustomControl;
                    break;
                case FileType.Resource:
                    bitmap = ImagesResources.Resource;
                    break;
                case FileType.Mapping:
                    bitmap = ImagesResources.Mapping;
                    break;

                case FileType.Dll:
                    bitmap = ImagesResources.Dll;
                    break;
            }

            if (bitmap != null)
            {
                this.Image = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(bitmap.GetHbitmap(), IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
            }
        }
     
        #region MobiseModel Base

        public override XElement ToXml()
        {
            throw new NotImplementedException();
        }

        public override XElement ToXml(XElement baseElement, bool AddCommonAttributes)
        {
            throw new NotImplementedException();
        }

        public override void FromXml(XElement xml)
        {
            throw new NotImplementedException();
        }


        #endregion
    }

    /// <summary>
    /// The folder types
    /// </summary>
    public enum FileSystemObjectType
    {
        File,
        Folder 
    }

    /// <summary>
    /// The file types
    /// </summary>
    public enum FileType
    {
        None,
        File,
        Model,
        ScreenDefinition,
        CustomControl,
        Controller,
        SQLite,
        Resource,
        Style,
        MenuDefinition,
        ControllerDefinition,
        SearchDefinition,
        Mapping,
        Dll,
        Plugin,
        Project
    }

    /// <summary>
    /// The folder types
    /// </summary>
    public enum FolderType
    {
        None,
        Model,
        LookupTableDB,
        UIDesginer,
        Controller,
        Resources,
        Mapping,
        Dll, 
        Project,
        Plugin
    }
}
