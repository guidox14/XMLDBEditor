using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace WPFDesigner_XML.Common.Models.UIModels
{
    public class AttributeCategoryModel : MobiseModel
    {
        
        /// <summary>
        /// The _order
        /// </summary>
        private int _order;

        public TrulyObservableCollection<AttributeModel> ControlAttributes { get; private set; }

        public AttributeCategoryModel(MobiseModel parent)
            : base(parent)
        {
            this.Name = string.Empty;
            this.ControlAttributes = new TrulyObservableCollection<AttributeModel>();
        }

        #region MobiseModel Base

        public override XElement ToXml()
        {
            //<AttributeCategory name="Common" order="0">
            XElement element = new XElement("AttributeCategory");
            return this.ToXml(element, true);
        }

        public override XElement ToXml(XElement baseElement, bool AddCommonAttributes)
        {
            baseElement.SetAttributeValue("name", this.Name);
            
            foreach (var item in this.ControlAttributes)
            {
                XElement controlAttributeElement = baseElement.Elements().FirstOrDefault(c => c.Attribute("name") != null && c.Attribute("name").Value == item.Name);
                if (controlAttributeElement == null)
                {
                    baseElement.Add(item.ToXml());
                }
                else
                {
                    item.ToXml(controlAttributeElement, true);
                }
            }

            return baseElement;
        }

        public override void FromXml(XElement xml)
        {
            this.Name = xml.Attribute("name") != null ? xml.Attribute("name").Value : string.Empty;
           
            if (xml.HasElements)
            {
                int currentElementsCount = this.ControlAttributes.Count;
                foreach (XElement item in xml.Elements())
                {
                    bool created = false;
                    string attributeName = item.Attribute("name") != null ? item.Attribute("name").Value : string.Empty;
                    AttributeModel attribute = this.ControlAttributes.FirstOrDefault(a => a.Name.ToLowerInvariant() == attributeName.ToLowerInvariant());
                    if (attribute == null)
                    {
                        attribute = new AttributeModel(this);
                        created = true;
                    }
                    attribute.FromXml(item);
                    if (created)
                    {
                        this.ControlAttributes.Add(attribute);
                    }
                }

            }
        }


        #endregion
    }
}
