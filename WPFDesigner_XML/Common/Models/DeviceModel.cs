using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace WPFDesigner_XML.Common.Models
{
    public class DeviceModel : MobiseModel
    {
           public DeviceModel() : base()
        {
            this.mScreenResolutions = new TrulyObservableCollection<ScreenResolutionModel>();
        }

          /// <summary>
        /// Initializes a new instance of the <see cref="File"/> class.
        /// </summary>
        /// <param name="xmlModel">The XML model.</param>
           public DeviceModel(XElement xmlModel)
            : this()
        { 
            if (xmlModel != null)
            {
                this.FromXml(xmlModel);
            }
        }


        private bool mSupportPortrait;
        public bool SupportPortrait
        {
            get
            {
                return this.mSupportPortrait;
            }
            set
            {
                this.mSupportPortrait = value;
                this.NotifyPropertyChanged("SupportPortrait");
            }
        }

        private bool mSupportLandscape;
        public bool SupportLandscape
        {
            get
            {
                return this.mSupportLandscape;
            }
            set
            {
                this.mSupportLandscape = value;
                this.NotifyPropertyChanged("SupportLandscape");
            }
        }

        private TrulyObservableCollection<ScreenResolutionModel> mScreenResolutions;
        public TrulyObservableCollection<ScreenResolutionModel> ScreenResolutions
        {
            get
            {
                return this.mScreenResolutions;
            }
            private set
            {
                this.mScreenResolutions = value;
                this.NotifyPropertyChanged("ScreenResolution");
            }
        }

        private DeviceSettingModel mSettings;
        public DeviceSettingModel Settings
        {
            get
            {
                return this.mSettings;
            }
            set
            {
                this.mSettings = value;
                this.NotifyPropertyChanged("Settings");
            }
        }
         

        #region MobiseModel Base

        public override XElement ToXml()
        {
            XElement device = new XElement("device");
            device.SetAttributeValue("mobiseID", this.MobiseObjectID);
            device.SetAttributeValue("name", this.Name);
            device.SetAttributeValue("SupportPortrait", this.SupportPortrait.ToString());
            device.SetAttributeValue("SupportLandscape", this.SupportLandscape.ToString());

            XElement resolutionsElement = new XElement("ScreenResolutions");
            device.Add(resolutionsElement);

            foreach (ScreenResolutionModel screenResolution in this.ScreenResolutions)
            {
                resolutionsElement.Add(screenResolution.ToXml());
            }

            return device;
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
                this.SupportPortrait = xml.Attribute("SupportPortrait") != null ? bool.Parse(xml.Attribute("SupportPortrait").Value) : false;
                this.SupportLandscape = xml.Attribute("SupportLandscape") != null ? bool.Parse(xml.Attribute("SupportLandscape").Value) : false;
            }

            foreach (var screenresolution in xml.Elements("ScreenResolutions").Elements("ScreenResolution"))
            {
                this.ScreenResolutions.Add(new ScreenResolutionModel(screenresolution));
            }
        }


        #endregion
    }

    public enum DeviceOrientation
    {
        Portrait,
        Landscape
    }
}
