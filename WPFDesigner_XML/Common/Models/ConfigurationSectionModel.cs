using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace WPFDesigner_XML.Common.Models
{
    public class ConfigurationSectionModel : MobiseModel
    {
         public ConfigurationSectionModel() : base()
        {
            this.mSettings = new TrulyObservableCollection<SettingModel>();
        }

          /// <summary>
        /// Initializes a new instance of the <see cref="File"/> class.
        /// </summary>
        /// <param name="xmlModel">The XML model.</param>
         public ConfigurationSectionModel(XElement xmlModel)
            : this()
        { 
            if (xmlModel != null)
            {
                this.FromXml(xmlModel);
            }
        }

        private ConfigurationSectionTypes mSectionType;
        public ConfigurationSectionTypes SectionType
        {
            get
            {
                return this.mSectionType;
            }
            set
            {
                this.mSectionType = value;
                this.NotifyPropertyChanged("SectionType");
            }
        }

        private TrulyObservableCollection<SettingModel> mSettings;
        public TrulyObservableCollection<SettingModel> Settings
        {
            get
            {
                return this.mSettings;
            }
            private set
            {
                this.mSettings = value;
                this.NotifyPropertyChanged("Settings");
            }
        }

        #region MobiseModel Base

        public override XElement ToXml()
        {
            XElement configSection = new XElement("ConfigurationSection");
            configSection.SetAttributeValue("mobiseID", this.MobiseObjectID);
            configSection.SetAttributeValue("name", this.Name);

            XElement settingsElement = new XElement("Settings");
            configSection.Add(settingsElement);

            foreach (SettingModel setting in this.Settings)
            {
                settingsElement.Add(setting.ToXml());
            }

            return configSection;
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
                this.SectionType = xml.Attribute("type") != null ? (ConfigurationSectionTypes)Enum.Parse(typeof(FileType), xml.Attribute("type").Value) : ConfigurationSectionTypes.Application;
            }

            foreach (var setting in xml.Elements("Settings").Elements("Setting"))
            {
                this.Settings.Add(new SettingModel(setting));
            }
        }


        #endregion
    }

    public enum ConfigurationSectionTypes
    {
        Application,
        User,
        Theme,
        UI
    }
}
