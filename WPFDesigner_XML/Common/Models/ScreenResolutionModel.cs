using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace WPFDesigner_XML.Common.Models
{
    public class ScreenResolutionModel : MobiseModel
    {
        private int mScreenWidth;
        public int ScreenWidth
        {
            get
            {
                return this.mScreenWidth;
            }
            set
            {
                this.mScreenWidth = value;
                this.NotifyPropertyChanged("ScreenWidth");
            }
        }

        private int mScreenHeight;
        public int ScreenHeight
        {
            get
            {
                return this.mScreenHeight;
            }
            set
            {
                this.mScreenHeight = value;
                this.NotifyPropertyChanged("ScreenHeight");
            }
        }

         public ScreenResolutionModel() : base()
        {
             
        }

          /// <summary>
        /// Initializes a new instance of the <see cref="File"/> class.
        /// </summary>
        /// <param name="xmlModel">The XML model.</param>
         public ScreenResolutionModel(XElement xmlModel)
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
            XElement screenResolution = new XElement("ScreenResolution");
            screenResolution.SetAttributeValue("mobiseID", this.MobiseObjectID);
            screenResolution.SetAttributeValue("name", this.Name);
            screenResolution.SetAttributeValue("Width", this.ScreenWidth.ToString());
            screenResolution.SetAttributeValue("Height", this.ScreenHeight.ToString());

            return screenResolution;
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
                this.ScreenHeight = xml.Attribute("Width") != null ? int.Parse(xml.Attribute("Width").Value) : 200;
                this.ScreenWidth = xml.Attribute("Height") != null ? int.Parse(xml.Attribute("Height").Value) : 200;
            }

        }


        #endregion
    }
}
