using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace WPFDesigner_XML.Common.Models.ScriptableControlMetadata
{
    public class FunctionParamModel: MobiseModel
    {
        #region properties
        private string _description;

        private string _propertyType;

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        /// <value>
        /// The description.
        /// </value>
        public string Description
        {
            get
            {
                return _description;
            }
            set
            {
                _description = value;
                this.NotifyPropertyChanged("Description");
            }
        }

        /// <summary>
        /// Gets or sets the type of the property.
        /// </summary>
        /// <value>
        /// The type of the property.
        /// </value>
        public string PropertyType
        {
            get
            {
                return _propertyType;
            }
            set
            {
                _propertyType = value;
                this.NotifyPropertyChanged("PropertyType");
            }
        }
        #endregion
         public FunctionParamModel(MobiseModel parent)
            : base(parent)
        {
        }


        /// <summary>
        /// To the XML.
        /// </summary>
        /// <returns></returns>
        public override System.Xml.Linq.XElement ToXml()
        {
            return this.ToXml(new XElement("Param"), true);
        }

        /// <summary>
        /// To the XML.
        /// </summary>
        /// <param name="baseElement">The base element.</param>
        /// <param name="AddCommonAttributes">if set to <c>true</c> [add common attributes].</param>
        /// <returns></returns>
        public override System.Xml.Linq.XElement ToXml(System.Xml.Linq.XElement baseElement, bool AddCommonAttributes)
        {
            baseElement.SetAttributeValue("name", this.Name);
            baseElement.SetAttributeValue("description", this.Description);
            baseElement.SetAttributeValue("type", this.PropertyType);
            
            return baseElement;
        }

        /// <summary>
        /// Froms the XML.
        /// </summary>
        /// <param name="xml">The XML.</param>
        public override void FromXml(System.Xml.Linq.XElement xml)
        {
            this.Name = xml.Attribute("name") != null ? xml.Attribute("name").Value : string.Empty;
            this.Description = xml.Attribute("description") != null ? xml.Attribute("description").Value : string.Empty;
            this.PropertyType = xml.Attribute("type") != null ? xml.Attribute("type").Value : string.Empty;
        }
    }
}
