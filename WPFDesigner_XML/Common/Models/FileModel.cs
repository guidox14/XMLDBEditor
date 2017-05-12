
using System;
using System.Drawing;
using System.Linq;
using System.Windows;
using System.Windows.Media.Imaging;
using System.Xml.Linq;
using System.Threading;
using WPFDesigner_XML.Common.Models.UIStyleModels;
using WPFDesigner_XML.Common.Models.ControllerModels;
using WPFDesigner_XML.Common.Models.UIModels;

namespace WPFDesigner_XML.Common.Models
{
    /// <summary>
    /// File Show
    /// </summary>
    public enum FileVisibility
    {
        Visible,
        Hidden
    }

    public class FileModel : FileSystemObjectModel
    {
       
        /// <summary>
        /// Initializes a new instance of the <see cref="File" /> class.
        /// </summary>
        public FileModel(ProjectModel project)
            : base(project)
        {
            this.FileType = FileType.None;
            this.FileSystemObjectModelType = FileSystemObjectType.File;
            this.PictureImage = null;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="File"/> class.
        /// </summary>
        /// <param name="xmlModel">The XML model.</param>
        public FileModel(XElement xmlModel, ProjectModel project)
            : base(project)
        {
            this.FileType = FileType.None;
            this.FileSystemObjectModelType = FileSystemObjectType.File;

            if (xmlModel != null)
            {
                this.FromXml(xmlModel);
            }
        }

        #region Properties



        /// <summary>
        /// Gets or sets the picture image.
        /// </summary>
        /// <value>
        /// The picture image.
        /// </value>
        public System.Windows.Media.ImageSource PictureImage { get; set; }

        #endregion

        #region Helpers

        /// <summary>
        /// Loads the image.
        /// </summary>
        public void LoadImage(string projectPath)
        {
            if (this.PictureImage == null)
            {
                try
                {
                    string path = System.IO.Path.Combine(projectPath, this.Path);
                    Bitmap image = new Bitmap(path);
                    System.Drawing.Size size = new System.Drawing.Size(200, 100);
                    image = ResizeImage(image, size);
                    this.PictureImage = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(image.GetHbitmap(), IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
                }
                catch (Exception ex)
                {
                    //Do nothing
                }
            }
        }
        public static Bitmap ResizeImage(Bitmap imgToResize, System.Drawing.Size size)
        {
            // no need to resize
            if (imgToResize.Size.Width <= size.Width && imgToResize.Size.Height <= size.Height)
                return imgToResize;

            int sourceWidth = imgToResize.Width;
            int sourceHeight = imgToResize.Height;

            //Consider vertical pics
            if (sourceWidth < sourceHeight)
            {
                int buff = size.Width;

                size.Width = size.Height;
                size.Height = buff;
            }

            float nPercent = 0, nPercentW = 0, nPercentH = 0;

            nPercentW = ((float)size.Width / (float)sourceWidth);
            nPercentH = ((float)size.Height / (float)sourceHeight);
            if (nPercentH < nPercentW)
            {
                nPercent = nPercentH;
            }
            else
            {
                nPercent = nPercentW;
            }

            int destWidth = (int)(sourceWidth * nPercent);
            int destHeight = (int)(sourceHeight * nPercent);


            try
            {

                Bitmap b = new Bitmap(destWidth, destHeight);
                using (Graphics g = Graphics.FromImage((Image)b))
                {
                    g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                    g.DrawImage(imgToResize, 0, 0, destWidth, destHeight);
                }

                return b;
            }
            catch
            {

            }
            return imgToResize;
        }
        /// <summary>
        /// Loads the image.
        /// </summary>
        public void LoadImage()
        {
            if (this.ParentProject != null)
            {
                this.LoadImage(this.ParentProject.Path);
            }
        }

        /// <summary>
        /// Loads the model.
        /// </summary>
        /// <returns></returns>
        public MobiseModel LoadModel()
        {
            System.Diagnostics.Debug.Assert(this.ParentProject != null, "Parent Project shouldn't be null");
            System.Diagnostics.Debug.Assert(this.ParentProject.mobiseConfiguration != null, "Mobise configuration shouldn't be null");

            string filePath = System.IO.Path.Combine(this.ParentProject.Path, this.Path);
            MobiseModel model = null;
            switch (this.FileType)
            {
                case Models.FileType.Model:
                    model = new DatabaseModel(this.ParentProject);
                    model.Name = this.Name;
                    ((DatabaseModel)model).Path = filePath;
                    break;
                case Models.FileType.Style:
                    model = new ProjectThemesModel();
                    model.Name = this.Name;
                    break;
                case FileType.ScreenDefinition:
                    var def = this.ParentProject.mobiseConfiguration.Controls.FirstOrDefault(c => c.Name.Equals("screen", StringComparison.OrdinalIgnoreCase));
                    model = new ScreenModel(def, this.ParentProject);
                    model.Name = this.Name;
                    model.MobiseObjectID = this.MobiseObjectID;
                    break;
                case FileType.CustomControl:
                    def = this.ParentProject.mobiseConfiguration.Controls.FirstOrDefault(c => c.Name.Equals("view", StringComparison.OrdinalIgnoreCase));
                    model = new ScreenModel(def, this.ParentProject);
                    model.Name = this.Name;
                    model.MobiseObjectID = this.MobiseObjectID;
                    break;
                case FileType.Controller:
                    {
                        string modelpath = System.IO.Path.Combine(this.ParentProject.Path, this.Path);
                        if (System.IO.File.Exists(modelpath))
                        {
                            XDocument doc = XDocument.Load(filePath);
                            string controllerType = doc.Root.Attribute("type") != null ? doc.Root.Attribute("type").Value : string.Empty;
                            var cntDef = this.ParentProject.mobiseConfiguration.ControllerDefinitons.FirstOrDefault(c => c.Name.Equals(controllerType, StringComparison.OrdinalIgnoreCase));
                            if (cntDef != null)
                            {
                                model = new ControllerModel(cntDef, this.ParentProject);
                            }
                        }
                        
                    }
                    break;

                default:
                    model = null;
                    break;
            }

            return this.LoadModel(model);

        }

        public MobiseModel LoadModel(MobiseModel baseModel)
        {
            string filePath = System.IO.Path.Combine(this.ParentProject.Path, this.Path);
            if (baseModel != null)
            {
                if (System.IO.File.Exists(filePath))
                {
                    baseModel.LoadDate = System.IO.File.GetLastWriteTime(filePath);
                    for (int count = 0; count < 3; count++)
                    {
                        try
                        {
                            XDocument doc = XDocument.Load(filePath);
                            baseModel.FromXml(doc.Root);
                            break;
                        }
                        catch (Exception ex)
                        {
                            Thread.Sleep(100);
                        }
                    }

                }
            }

            return baseModel;
        }

        public void SaveModel(MobiseModel model)
        {

            string filePath = System.IO.Path.Combine(this.ParentProject.Path, this.Path);
            if (model != null)
            {
                if (model.LoadDate != null && model.LoadDate.Value < System.IO.File.GetLastWriteTime(filePath))
                {
                    if (MessageBox.Show("The existing project is newer than the current version. Are you sure you want to override it?", "Warning!", MessageBoxButton.YesNo) == MessageBoxResult.No)
                    {
                        return; // cancel save
                    }
                }
                XDocument doc = new XDocument();
                doc.Add(model.ToXml());
                for (int i = 0; i < 3; i++)
                {
                    try
                    {
                        doc.Save(filePath);
                        break;
                    }
                    catch (Exception ex)
                    {
                        Thread.Sleep(100);
                    }
                }
                model.LoadDate = System.IO.File.GetLastWriteTime(filePath);
            }
        }

        #endregion

        #region MobiseModel Base

        public override XElement ToXml()
        {
            XElement entity = new XElement("file");
            entity.SetAttributeValue("mobiseID", this.MobiseObjectID);
            entity.SetAttributeValue("name", this.Name);
            entity.SetAttributeValue("path", this.Path);
            entity.SetAttributeValue("type", this.FileType.ToString());

            if (this.FileVisibility == FileVisibility.Hidden)
            {
                entity.SetAttributeValue("show", this.FileVisibility.ToString());
            }

            return entity;
        }

        public override XElement ToXml(XElement baseElement, bool AddCommonAttributes)
        {
            throw new NotImplementedException();
        }

        public override void FromXml(XElement xml)
        {
            if (xml.HasAttributes)
            {
                this.mMobiseObjectID = xml.Attribute("mobiseID") != null ? xml.Attribute("mobiseID").Value : Guid.NewGuid().ToString();
                this.Name = xml.Attribute("name") != null ? xml.Attribute("name").Value : string.Empty;
                this.Path = xml.Attribute("path") != null ? xml.Attribute("path").Value : string.Empty;
                this.FileType = xml.Attribute("type") != null ? (FileType)Enum.Parse(typeof(FileType), xml.Attribute("type").Value) : FileType.None;
                this.FileVisibility = xml.Attribute("show") != null ? (FileVisibility)Enum.Parse(typeof(FileVisibility), xml.Attribute("show").Value) : FileVisibility.Visible;
            }
        }

        #endregion

        public void Rename(string newName)
        {
            try
            {
                if (System.IO.File.Exists(System.IO.Path.Combine(ParentProject.Path, this.Path.Replace(this.Name, newName))))
                {
                  //  MessageBox.Show("There is another item with the same name.", "Error Renaming", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK);
                    throw new Exception("There is another item with the same name.");
                }
                else
                {
                    if (System.IO.File.Exists(System.IO.Path.Combine(ParentProject.Path, this.Path)))
                    {
                        System.IO.File.Move(System.IO.Path.Combine(ParentProject.Path, this.Path), System.IO.Path.Combine(ParentProject.Path, this.Path.Replace(this.Name, newName)));
                    }
                    this.Path = this.Path.Replace(this.Name, newName);
                    this.Name = newName;
                }
            }
            catch
            {
                throw new Exception("Invalid name or file is read only.");
            }
        }

        public bool Exists()
        {
            return System.IO.File.Exists(System.IO.Path.Combine(this.ParentProject.Path, this.Path));
        }
    }
}
