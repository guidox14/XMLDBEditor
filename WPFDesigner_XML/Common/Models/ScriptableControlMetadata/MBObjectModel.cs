using System.Xml.Linq;

namespace WPFDesigner_XML.Common.Models.ScriptableControlMetadata
{
    /// <summary>
    /// Contains the metadata that defines a scriptable part of a ui control like a mbTexbox, mbLabel, etc...
    /// </summary>
    public class MBObjectModel : MobiseModel
    {
        #region properties
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
        /// Gets the properties.
        /// </summary>
        /// <value>
        /// The properties.
        /// </value>
        public TrulyObservableCollection<ScriptablePropertyModel> Properties { get; private set; }

        /// <summary>
        /// Gets the functions.
        /// </summary>
        /// <value>
        /// The functions.
        /// </value>
        public TrulyObservableCollection<ScriptableFunctionModel> Functions { get; private set; }
        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="MBObjectModel"/> class.
        /// </summary>
        public MBObjectModel():base()
        {
            this.Properties = new TrulyObservableCollection<ScriptablePropertyModel>();
            this.Functions = new TrulyObservableCollection<ScriptableFunctionModel>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MBObjectModel"/> class.
        /// </summary>
        /// <param name="parent">The parent.</param>
        public MBObjectModel(MobiseModel parent)
            : base(parent)
        {
            this.Properties = new TrulyObservableCollection<ScriptablePropertyModel>();
            this.Functions = new TrulyObservableCollection<ScriptableFunctionModel>();
        }

        /// <summary>
        /// To the XML.
        /// </summary>
        /// <returns></returns>
        public override System.Xml.Linq.XElement ToXml()
        {
            return this.ToXml(new XElement("MBObject"), true);
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

            if (this.Properties.Count > 0)
                AddToXMLChildElementsByName(baseElement, "Properties", Properties);

            if (this.Functions.Count > 0)
                AddToXMLChildElementsByID(baseElement, "Functions", Functions);

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

            ReadFromXMLChildItemsByName<ScriptablePropertyModel>(xml, "Properties", this.Properties, this);
            ReadFromXMLChildItemsByID<ScriptableFunctionModel>(xml, "Functions", this.Functions, this);
        }

    }
}
