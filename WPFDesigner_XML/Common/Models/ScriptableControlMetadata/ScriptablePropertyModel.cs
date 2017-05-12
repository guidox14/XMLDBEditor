using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace WPFDesigner_XML.Common.Models.ScriptableControlMetadata
{
    public class ScriptablePropertyModel: MobiseModel
    {
        #region properties
        private string _description;

        private string _propertyType;

        private string _getter;

        /// <summary>
        /// Gets or sets the getter.
        /// </summary>
        /// <value>
        /// The getter.
        /// </value>
        public string Getter
        {
            get { return _getter; }
            set { _getter = value; }
        }
        

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

        /// <summary>
        /// Initializes a new instance of the <see cref="ScriptablePropertyModel"/> class.
        /// </summary>
        /// <param name="parent">The parent.</param>
        public ScriptablePropertyModel(MobiseModel parent)
            : base(parent)
        {
        }

        /// <summary>
        /// To the XML.
        /// </summary>
        /// <returns></returns>
        public override System.Xml.Linq.XElement ToXml()
        {
            return this.ToXml(new XElement("Property"), true);
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
            if (!string.IsNullOrEmpty(this.Description))
                baseElement.SetAttributeValue("description", this.Description);

            if (!string.IsNullOrEmpty(this.PropertyType))
                baseElement.SetAttributeValue("type", this.PropertyType);

            if (!string.IsNullOrEmpty(this.Getter))
                baseElement.SetAttributeValue("getter", this.Getter);

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
            this.Getter = xml.Attribute("getter") != null ? xml.Attribute("getter").Value : string.Empty;
        }
    }
}
