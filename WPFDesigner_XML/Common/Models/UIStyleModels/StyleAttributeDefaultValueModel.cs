using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WPFDesigner_XML.Common.Models.UIStyleModels
{
    public class StyleAttributeDefaultValueModel: MobiseModel
    {

        private string _value;

        public string Value
        {
            get 
            { 
                return this._value; 
            }
            set 
            { 
                this._value = value;
                this.NotifyPropertyChanged("Value");
            }
        }
        
        public StyleAttributeDefaultValueModel(MobiseModel parent)
            : base(parent)
        {
        }
        #region Mobise Model
        public override System.Xml.Linq.XElement ToXml()
        {
            //throw new NotImplementedException();
            return this.ToXml(new System.Xml.Linq.XElement("defaultValue"), true);
        }

        public override System.Xml.Linq.XElement ToXml(System.Xml.Linq.XElement baseElement, bool AddCommonAttributes)
        {
            baseElement.SetAttributeValue("name", this.Name);
            baseElement.SetAttributeValue("value", this.Value);
            return baseElement;
        }

        public override void FromXml(System.Xml.Linq.XElement xml)
        {
            this.Name = xml.Attribute("name") != null ? xml.Attribute("name").Value : string.Empty;
            this.Value = xml.Attribute("value") != null ? xml.Attribute("value").Value : string.Empty;

        }
        #endregion
    }
}
