using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace WPFDesigner_XML.Common.Models.UIModels
{
    public class VisualStateDefinitionModel: MobiseModel
    {
        /// <summary>
        /// The class style
        /// </summary>
        private string _classStyle;

        /// <summary>
        /// The is default
        /// </summary>
        private bool _isdefault;

        /// <summary>
        /// Gets or sets a value indicating whether this instance is default.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is default; otherwise, <c>false</c>.
        /// </value>
        public bool IsDefault
        {
            get 
            { 
                return this._isdefault; 
            }
            set 
            {
                this._isdefault = value;
                this.NotifyPropertyChanged("IsDefault");
            }
        }
        
        /// <summary>
        /// Gets or sets the class style.
        /// </summary>
        /// <value>
        /// The class style.
        /// </value>
        public string ClassStyle
        {
            get 
            { 
                return _classStyle; 
            }
            set 
            { 
                this._classStyle = value;
                this.NotifyPropertyChanged("ClassStyle");
            }
        }
        
        /// <summary>
        /// Initializes a new instance of the <see cref="VisualStateDefinitionModel"/> class.
        /// </summary>
        public VisualStateDefinitionModel()
            : base()
        {
        }

        #region Mobise Model

        /// <summary>
        /// To the XML.
        /// </summary>
        /// <returns></returns>
        public override System.Xml.Linq.XElement ToXml()
        {
            return this.ToXml(new XElement("visualState"), true);
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
            baseElement.SetAttributeValue("class", this.ClassStyle);
            baseElement.SetAttributeValue("default", this.IsDefault.ToString());
            return baseElement;
        }

        /// <summary>
        /// Froms the XML.
        /// </summary>
        /// <param name="xml">The XML.</param>
        public override void FromXml(System.Xml.Linq.XElement xml)
        {
            if (xml.HasAttributes)
            {
                this.Name = xml.Attribute("name") != null ? xml.Attribute("name").Value : string.Empty;
                this.ClassStyle = xml.Attribute("class") != null ? xml.Attribute("class").Value : string.Empty;
                this.IsDefault = xml.Attribute("default") != null ? bool.Parse(xml.Attribute("default").Value) : false;
            }
        }

        #endregion
    }
}
