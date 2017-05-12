using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace WPFDesigner_XML.Common.Models
{
    public class PluginModel : FolderModel
    { 
         public PluginModel(ProjectModel project) : base(project)
        {
            this.FileSystemObjectModelType = FileSystemObjectType.Folder;
            this.FileType = Models.FileType.None;
            this.FolderType = Models.FolderType.Plugin;
        }

          /// <summary>
        /// Initializes a new instance of the <see cref="File"/> class.
        /// </summary>
        /// <param name="xmlModel">The XML model.</param>
         public PluginModel(XElement xmlModel, ProjectModel project)
            : this(project)
        { 
            if (xmlModel != null)
            {
                this.FromXml(xmlModel);
            }
        }

        #region MobiseModel Base

        public override XElement ToXml()
        {
            XElement pluginElement = new XElement("Plugin");
            pluginElement.SetAttributeValue("mobiseID", this.MobiseObjectID);
            pluginElement.SetAttributeValue("name", this.Name);
            pluginElement.SetAttributeValue("path", this.Path);
            pluginElement.SetAttributeValue("visibility", this.FileVisibility);


            return pluginElement;
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
                //this.FolderType = xml.Attribute("type") != null ? (FolderType)Enum.Parse(typeof(FolderType), xml.Attribute("type").Value) : FolderType.None;
                this.FileVisibility = xml.Attribute("visibility") != null ? (FileVisibility)Enum.Parse(typeof(FileVisibility), xml.Attribute("visibility").Value) : FileVisibility.Visible;
            } 
        }


        #endregion
    }
}
