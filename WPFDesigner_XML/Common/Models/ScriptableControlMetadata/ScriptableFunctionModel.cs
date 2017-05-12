using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace WPFDesigner_XML.Common.Models.ScriptableControlMetadata
{
    /// <summary>
    /// Scriptable function model
    /// </summary>
    public class ScriptableFunctionModel : MobiseModel
    {
        #region properties
        private string _returnType;

        private string _description;

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
        /// Gets or sets the type of the return.
        /// </summary>
        /// <value>
        /// The type of the return.
        /// </value>
        public string ReturnType
        {
            get
            {
                return _returnType;
            }
            set
            {
                _returnType = value;
                this.NotifyPropertyChanged("ReturnType");
            }
        }

        public TrulyObservableCollection<FunctionParamModel> Parameters { get; internal set; }

        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="ScriptableFunctionModel"/> class.
        /// </summary>
        public ScriptableFunctionModel()
            : base()
        {
            this.Parameters = new TrulyObservableCollection<FunctionParamModel>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ScriptableFunctionModel"/> class.
        /// </summary>
        /// <param name="parent">The parent.</param>
        public ScriptableFunctionModel(MobiseModel parent)
            : base(parent)
        {
            this.Parameters = new TrulyObservableCollection<FunctionParamModel>();
        }

        /// <summary>
        /// To the XML.
        /// </summary>
        /// <returns></returns>
        public override System.Xml.Linq.XElement ToXml()
        {
            return this.ToXml(new XElement("Function"), true);
        }

        /// <summary>
        /// To the XML.
        /// </summary>
        /// <param name="baseElement">The base element.</param>
        /// <param name="AddCommonAttributes">if set to <c>true</c> [add common attributes].</param>
        /// <returns></returns>
        public override XElement ToXml(XElement baseElement, bool AddCommonAttributes)
        {
            baseElement.SetAttributeValue("name", this.Name);
            baseElement.SetAttributeValue("description", this.Description);
            baseElement.SetAttributeValue("return", this.ReturnType);

            AddToXMLChildElementsByName(baseElement, "Params", this.Parameters);

            return baseElement;
        }

        /// <summary>
        /// Froms the XML.
        /// </summary>
        /// <param name="xml">The XML.</param>
        public override void FromXml(XElement xml)
        {
            this.Name = xml.Attribute("name") != null ? xml.Attribute("name").Value : string.Empty;
            this.Description = xml.Attribute("description") != null ? xml.Attribute("description").Value : string.Empty;
            this.ReturnType = xml.Attribute("return") != null ? xml.Attribute("return").Value : string.Empty;

            ReadFromXMLChildItemsByName(xml, "Params", this.Parameters, this);
        }
    }
}
