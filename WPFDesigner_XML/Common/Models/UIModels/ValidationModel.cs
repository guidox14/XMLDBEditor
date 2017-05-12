using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace WPFDesigner_XML.Common.Models.UIModels
{
    public class ValidationModel : MobiseModel
    {
       
        /// <summary>
        /// The value
        /// </summary>
        private string value;
        
        /// <summary>
        /// The collection property
        /// </summary>
        private string collectionProperty;

        /// <summary>
        /// The value collection property
        /// </summary>
        private string valueCollectionProperty;
        
        /// <summary>
        /// Gets or sets the value collection property.
        /// </summary>
        /// <value>
        /// The value collection property.
        /// </value>
        public string ValueCollectionProperty
        {
            get { return valueCollectionProperty; }
            set { valueCollectionProperty = value; }
        }
        
        /// <summary>
        /// Gets or sets the collection property.
        /// </summary>
        /// <value>
        /// The collection property.
        /// </value>
        public string CollectionProperty
        {
            get { return collectionProperty; }
            set { collectionProperty = value; }
        }
        


        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        /// <value>
        /// The value.
        /// </value>
        public string Value
        {
            get 
            { 
                return value; 
            }
            set 
            { 
                this.value = value;
                this.NotifyPropertyChanged("Value");
            }
        }
        
        /// <summary>
        /// Initializes a new instance of the <see cref="ValidationModel"/> class.
        /// </summary>
        public ValidationModel(MobiseModel parent)
            : base(parent)
        {
        }

        #region MobiseModel Base

        public override XElement ToXml()
        {
            XElement element = new XElement("validation");
            return this.ToXml(element, true);
        }

        public override XElement ToXml(XElement baseElement, bool AddCommonAttributes)
        {
            baseElement.SetAttributeValue("name", this.Name);
            baseElement.SetAttributeValue("value", this.Value);
            
            if (!string.IsNullOrEmpty(this.CollectionProperty))
            {
                baseElement.SetAttributeValue("collectionProperty", this.Value);
            }

            if (!string.IsNullOrEmpty(this.ValueCollectionProperty))
            {
                baseElement.SetAttributeValue("valueCollectionProperty", this.Value);
            }
            return baseElement;
        }

        public override void FromXml(XElement xml)
        {
            this.Name = xml.Attribute("name") != null ? xml.Attribute("name").Value : string.Empty;
            this.Value = xml.Attribute("value") != null ? xml.Attribute("value").Value : string.Empty;

            this.CollectionProperty = xml.Attribute("collectionProperty") != null ? xml.Attribute("collectionProperty").Value : string.Empty;
            this.ValueCollectionProperty = xml.Attribute("valueCollectionProperty") != null ? xml.Attribute("valueCollectionProperty").Value : string.Empty;
        }
        
        #endregion
    }
}
