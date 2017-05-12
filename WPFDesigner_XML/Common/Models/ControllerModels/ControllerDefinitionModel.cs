using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Xml.Linq;
using WPFDesigner_XML.Common.Models.UIModels;

namespace WPFDesigner_XML.Common.Models.ControllerModels
{
    public class ControllerDefinitionModel: UIControlDefinitionModel
    {

        /// <summary>
        /// Initializes a new instance of the <see cref="ControllerDefinitionModel"/> class.
        /// </summary>
        /// <param name="parent"></param>
        public ControllerDefinitionModel(MobiseModel parent)
            : base(parent, false)
        {
        }

        /// <summary>
        /// To the XML.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public override XElement ToXml()
        {
            XElement controlDefinitionElement = new XElement("controllerDefinition");
            return this.ToXml(controlDefinitionElement, true);
        }

        /// <summary>
        /// To the XML.
        /// </summary>
        /// <param name="baseElement">The base element.</param>
        /// <param name="AddCommonAttributes">if set to <c>true</c> [add common attributes].</param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public override XElement ToXml(XElement baseElement, bool AddCommonAttributes)
        {
            baseElement.SetAttributeValue("name", this.Name);
            baseElement.SetAttributeValue("description", this.Description);

            baseElement.SetAttributeValue("iconUrl", this.IconUrl);
    
            XElement designerAttributesElement = baseElement.Element("designerAttributes");
            if (designerAttributesElement == null)
            {
                designerAttributesElement = new XElement("designerAttributes");
                baseElement.Add(designerAttributesElement);
            }
            foreach (string key in this.DesignerAttributes.Keys)
            {
                XElement designerAttributeElement = designerAttributesElement.Elements().FirstOrDefault(e => e.Attribute("name") != null && e.Attribute("name").Value == key);
                if (designerAttributeElement == null)
                {
                    designerAttributeElement = new XElement("designerAttribute");
                    designerAttributesElement.Add(designerAttributeElement);
                }
                designerAttributeElement.SetAttributeValue("name", key);
                designerAttributeElement.SetAttributeValue("value", this.DesignerAttributes[key]);

            }


            XElement AttributeCategoriesElement = baseElement.Element("AttributeCategories");
            if (AttributeCategoriesElement == null)
            {
                AttributeCategoriesElement = new XElement("AttributeCategories");
                baseElement.Add(AttributeCategoriesElement);
            }
            foreach (AttributeCategoryModel item in this.AttributeCategories)
            {
                XElement categoryElement = AttributeCategoriesElement.Elements().FirstOrDefault(e => e.Attribute("name") != null && e.Attribute("name").Value == item.Name);
                if (categoryElement != null)
                    item.ToXml(categoryElement, true);
                else
                    AttributeCategoriesElement.Add(item.ToXml());
            }
            
            return baseElement;
        }

        /// <summary>
        /// Froms the XML.
        /// </summary>
        /// <param name="xml">The XML.</param>
        /// <exception cref="System.NotImplementedException"></exception>
        public override void FromXml(XElement xml)
        {
            this.Name = xml.Attribute("name") != null ? xml.Attribute("name").Value : string.Empty;
            this.Description = xml.Attribute("description") != null ? xml.Attribute("description").Value : string.Empty;
            this.IconUrl = xml.Attribute("iconUrl") != null ? xml.Attribute("iconUrl").Value : string.Empty;
            
            XElement designerAttributesElement = xml.Element("designerAttributes");
            if (designerAttributesElement != null && designerAttributesElement.HasElements)
            {
                foreach (XElement element in designerAttributesElement.Elements())
                {
                    string key = element.Attribute("name") != null ? element.Attribute("name").Value.ToLowerInvariant() : string.Empty;
                    string value = element.Attribute("value") != null ? element.Attribute("value").Value : string.Empty;
                    if (!string.IsNullOrEmpty(key))
                    {
                        this.DesignerAttributes[key] = value;
                    }
                }
            }

            XElement AttributeCategoriesElement = xml.Element("AttributeCategories");
            if (AttributeCategoriesElement != null && AttributeCategoriesElement.HasElements)
            {
                int numOfCategories = this.AttributeCategories.Count();
                foreach (XElement item in AttributeCategoriesElement.Elements())
                {
                    bool created = false;
                    string categoryName = item.Attribute("name") != null ? item.Attribute("name").Value : string.Empty;
                    AttributeCategoryModel category = this.AttributeCategories.FirstOrDefault(c => c.Name == categoryName);
                    if (category == null)
                    {
                        created = true;
                        category = new AttributeCategoryModel(this);
                        category.FromXml(item);
                        this.AttributeCategories.Add( category);
                    }
                    else
                    {
                        category.FromXml(item);
                    }

                }
            }
        }

    }
}
