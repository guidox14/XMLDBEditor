using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace WPFDesigner_XML.Common.Models
{
    public class SettingModel : MobiseModel
    {
        private string mValue;
        public string Value
        {
            get
            {
                return mValue;
            }
            set
            {
                if (mValue != value)
                {
                    mValue = value;
                    this.NotifyPropertyChanged("Value");
                }
            }
        }

        public SettingModel() : base()
        {
            
        }

          /// <summary>
        /// Initializes a new instance of the <see cref="File"/> class.
        /// </summary>
        /// <param name="xmlModel">The XML model.</param>
         public SettingModel(XElement xmlModel)
            : this()
        { 
            if (xmlModel != null)
            {
                this.FromXml(xmlModel);
            }
        }

        #region MobiseModel Base

        public override XElement ToXml()
        {
            XElement settingElement = new XElement("Setting");
            settingElement.SetAttributeValue("mobiseID", this.MobiseObjectID);
            settingElement.SetAttributeValue("name", this.Name);
            settingElement.SetAttributeValue("value", this.Value);

            return settingElement;
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
                this.Value = xml.Attribute("value") != null ? xml.Attribute("value").Value : string.Empty;
            } 
        }


        #endregion
    }
}
