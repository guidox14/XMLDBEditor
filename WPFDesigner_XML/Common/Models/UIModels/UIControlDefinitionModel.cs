using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Xml.Linq;

namespace WPFDesigner_XML.Common.Models.UIModels
{
    /// <summary>
    /// Contains the metadata that defines a ui control like a texbox, label, table, grid, etc
    /// </summary>
    public class UIControlDefinitionModel : MobiseModel
    {
        #region fields

        /// <summary>
        /// The _description
        /// </summary>
        private string _description;

        /// <summary>
        /// The _icon URL
        /// </summary>
        private string _iconUrl;

        /// <summary>
        /// The _design time control type
        /// </summary>
        private Type _designTimeControlType = null;
        public Type DesignTimeControlType
        {
            get
            {
                if (_designTimeControlType == null)
                {
                    if (this.DesignerAttributes.ContainsKey("renderer"))
                    {
                        this._designTimeControlType = Type.GetType(this.DesignerAttributes["renderer"]);
                    }
                }
                return this._designTimeControlType;
            }
            set
            {
                this._designTimeControlType = value;
                this.NotifyPropertyChanged("DesignTimeControlType");
            }
        }

        /// <summary>
        /// The _default style class
        /// </summary>
        private string _defaultStyleClass;

        /// <summary>
        /// The _container type
        /// </summary>
        private string _containerType;

        /// <summary>
        /// The container property
        /// </summary>
        private string _containerProperty;

        /// <summary>
        /// The container property types
        /// </summary>
        private string _containerPropertyTypes;

        /// <summary>
        /// store the value indicating if the control should be show in the toolbox
        /// </summary>
        private bool _browsable;
        #endregion

        #region All Attributes

        /// <summary>
        /// Gets all attributes.
        /// </summary>
        /// <value>
        /// All attributes.
        /// </value>
        public List<AttributeModel> AllAttributes
        {
            get
            {
                List<AttributeModel> result = new List<AttributeModel>();

                foreach (AttributeCategoryModel category in this.AttributeCategories)
                {
                    foreach (AttributeModel att in category.ControlAttributes)
                    {
                        result.Add(att);
                    }
                }

                return result;

            }
        }

        #endregion

        #region MobiseModel Base

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="UIControlDefinitionModel"/> is browsable.
        /// </summary>
        /// <value>
        ///   <c>true</c> if browsable; otherwise, <c>false</c>.
        /// </value>
        public bool Browsable
        {
            get { return _browsable; }
            set { _browsable = value; }
        }

        /// <summary>
        /// Gets or sets the container property types.
        /// </summary>
        /// <value>
        /// The container property types.
        /// </value>
        public string ContainerPropertyTypes
        {
            get
            {
                return _containerPropertyTypes;
            }
            set
            {
                _containerPropertyTypes = value;
                this.NotifyPropertyChanged("ContainerPropertyTypes");
            }
        }

        /// <summary>
        /// Gets or sets the container property.
        /// </summary>
        /// <value>
        /// The container property.
        /// </value>
        public string ContainerProperty
        {
            get
            {
                return _containerProperty;
            }
            set
            {
                _containerProperty = value;
                this.NotifyPropertyChanged("ContainerProperty");
            }
        }

        /// <summary>
        /// The icon cache
        /// </summary>
        private ImageSource iconCache = null;

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
                return this._description;
            }
            set
            {
                this._description = value;
                this.NotifyPropertyChanged("Description");
            }
        }

        /// <summary>
        /// Gets or sets the icon URL.
        /// </summary>
        /// <value>
        /// The icon URL.
        /// </value>
        public string IconUrl
        {
            get
            {
                return this._iconUrl;
            }
            set
            {
                this._iconUrl = value;
                this.NotifyPropertyChanged("IconUrl");
            }
        }

        /// <summary>
        /// Gets or sets the default style class.
        /// </summary>
        /// <value>
        /// The default style class.
        /// </value>
        public string DefaultStyleClass
        {
            get
            {
                return this._defaultStyleClass;
            }
            set
            {
                this._defaultStyleClass = value;
                this.NotifyPropertyChanged("DefaultStyleClass");
            }
        }

        /// <summary>
        /// Gets or sets the type of the container.
        /// </summary>
        /// <value>
        /// The type of the container.
        /// </value>
        public string ContainerType
        {
            get
            {
                return this._containerType;
            }
            set
            {
                this._containerType = value;
                this.NotifyPropertyChanged("ContainerType");
            }
        }

        /// <summary>
        /// Gets the designer attributes.
        /// </summary>
        /// <value>
        /// The designer attributes.
        /// </value>
        public Dictionary<string, string> DesignerAttributes { get; private set; }

        /// <summary>
        /// Gets the attribute categories.
        /// </summary>
        /// <value>
        /// The attribute categories.
        /// </value>
        public TrulyObservableCollection<AttributeCategoryModel> AttributeCategories { get; private set; }

        public TrulyObservableCollection<VisualStateDefinitionModel> VisualStates { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="UIControlDefinitionModel"/> class.
        /// </summary>
        public UIControlDefinitionModel(MobiseModel parent)
            : this(parent, true)
        {
           
        }

        public UIControlDefinitionModel(MobiseModel parent, bool addDefaultAttributes)
            : base(parent)
        {
            this.AttributeCategories = new TrulyObservableCollection<AttributeCategoryModel>();
            this.DesignerAttributes = new Dictionary<string, string>();
            this.VisualStates = new TrulyObservableCollection<VisualStateDefinitionModel>();
            if (addDefaultAttributes)
            {
                this.CreateDefaultAttributes();
            }
        }

        private void CreateDefaultAttributes()
        {
            MobiseConfiguration parent = this.Parent as MobiseConfiguration;
            if (parent != null)
            {
                foreach (AttributeCategoryModel category in parent.baseControlAttributes)
                {
                    AttributeCategoryModel newCategory = new AttributeCategoryModel(this);
                    newCategory.FromXml(category.ToXml());
                    this.AttributeCategories.Add(newCategory);
                }
            }
        }

        /// <summary>
        /// To the XML.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public override XElement ToXml()
        {
            XElement controlDefinitionElement = new XElement("control");
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
            baseElement.SetAttributeValue("containerType", this.ContainerType);
            baseElement.SetAttributeValue("class", this.DefaultStyleClass);

            if (!this.ContainerType.Equals("none", StringComparison.OrdinalIgnoreCase))
            {
                if (!string.IsNullOrEmpty(this.ContainerPropertyTypes))
                    baseElement.SetAttributeValue("containerType", this.ContainerPropertyTypes);
                if (!string.IsNullOrEmpty(this.ContainerProperty))
                    baseElement.SetAttributeValue("containerType", this.ContainerProperty);
            }

            baseElement.SetAttributeValue("browsable", this.Browsable);

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

            XElement VisualStatesDefinitionsElement = baseElement.Element("VisualStates");
            if (VisualStatesDefinitionsElement == null)
            {
                VisualStatesDefinitionsElement = new XElement("VisualStates");
                baseElement.Add(VisualStatesDefinitionsElement);
            }
            foreach (VisualStateDefinitionModel item in this.VisualStates)
            {
                XElement VisualStateDefinitionElement = VisualStatesDefinitionsElement.Elements().FirstOrDefault(e => e.Attribute("name") != null && e.Attribute("name").Value == item.Name);
                if (VisualStateDefinitionElement != null)
                    item.ToXml(VisualStateDefinitionElement, true);
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
            this.ContainerType = xml.Attribute("ContainerType") != null ? xml.Attribute("ContainerType").Value : string.Empty;
            this.ContainerPropertyTypes = xml.Attribute("ContainerPropertyTypes") != null ? xml.Attribute("ContainerPropertyTypes").Value : string.Empty;
            this.ContainerProperty = xml.Attribute("ContainerProperty") != null ? xml.Attribute("ContainerProperty").Value : string.Empty;
            this.Browsable = xml.Attribute("browsable") != null ? bool.Parse(xml.Attribute("browsable").Value) : true;
            if (xml.Attribute("class") != null)
                this.DefaultStyleClass = xml.Attribute("class").Value;

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

            if (this.DesignerAttributes.ContainsKey("portraitwidth") || this.DesignerAttributes.ContainsKey("portraitheight"))
            {
                double width = 100, height = 40;
                width = this.DesignerAttributes.ContainsKey("portraitwidth") ? double.Parse(this.DesignerAttributes["portraitwidth"]) : 100;
                height = this.DesignerAttributes.ContainsKey("portraitheight") ? double.Parse(this.DesignerAttributes["portraitheight"]) : 40;
                this.AttributeCategories[1].ControlAttributes[0].DefaultValue = new Rect(0, 0, width, height);
                if (this.Name.Equals("screen", StringComparison.OrdinalIgnoreCase) || this.Name.Equals("view", StringComparison.OrdinalIgnoreCase))
                {
                    this.AttributeCategories[1].ControlAttributes[1].DefaultValue = new Rect(0, 0, height, width);
                }
                else
                {
                    this.AttributeCategories[1].ControlAttributes[1].DefaultValue = new Rect(0, 0, width, height);
                }
            }

            if (this.DesignerAttributes.ContainsKey("landscapewidth") || this.DesignerAttributes.ContainsKey("landscapeheight"))
            {
                double width = 100, height = 40;
                width = this.DesignerAttributes.ContainsKey("landscapewidth") ? double.Parse(this.DesignerAttributes["landscapewidth"]) : 100;
                height = this.DesignerAttributes.ContainsKey("landscapeheight") ? double.Parse(this.DesignerAttributes["landscapeheight"]) : 40;
                this.AttributeCategories[1].ControlAttributes[1].DefaultValue = new Rect(0, 0, width, height);
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
                        this.AttributeCategories.Add(category);
                    }
                    else
                    {
                        category.FromXml(item);
                    }

                }
            }

            XElement VisualStatesDefinitionsElement = xml.Element("VisualStates");
            if (VisualStatesDefinitionsElement != null && VisualStatesDefinitionsElement.HasElements)
            {
                foreach (XElement item in VisualStatesDefinitionsElement.Elements())
                {
                    string stateName = item.Attribute("name") != null ? item.Attribute("name").Value : string.Empty;
                    VisualStateDefinitionModel visualState = this.VisualStates.FirstOrDefault(c => c.Name == stateName);
                    if (visualState == null)
                    {
                        visualState = new VisualStateDefinitionModel();
                        this.VisualStates.Add(visualState);
                    }
                    visualState.FromXml(item);
                }
            }
        }


        #endregion

        #region Factory

        public UIControlInstanceModel CreateInstance()
        {
            return new UIControlInstanceModel(this);
        }

        public static UIControlInstanceModel CreateInstance(UIControlDefinitionModel definition)
        {
            return new UIControlInstanceModel(definition);
        }

        #endregion
    }
}
