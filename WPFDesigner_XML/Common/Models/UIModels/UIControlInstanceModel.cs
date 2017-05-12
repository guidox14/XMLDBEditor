using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using WPFDesigner_XML.Common.Converters;

namespace WPFDesigner_XML.Common.Models.UIModels
{
    public class UIControlInstanceModel : MobiseModel
    {
        public UIControlInstanceModel(ScreenModel parent, UIControlDefinitionModel definition)
            : this(definition)
        {
            this.Parent = parent;
            this.ParentScreen = parent;
        }

        public UIControlInstanceModel(MobiseModel parent, ScreenModel parentScreen, UIControlDefinitionModel definition)
            : this(definition)
        {
            this.Parent = parent;
            this.ParentScreen = parentScreen;
        }

        public UIControlInstanceModel(UIControlDefinitionModel definition)
            : base()
        {
            this.mControlDefinition = definition;
            this.mAttributes = new Dictionary<AttributeModel, object>();
            this.mAttributesIndex = new Dictionary<string, AttributeModel>();
            this.LoadControlFromDefinition();
        }

        private UIControlDefinitionModel mControlDefinition;
        public UIControlDefinitionModel ControlDefinition
        {
            get
            {
                return this.mControlDefinition;
            }
        }

        // collection of attributes and his value
        private Dictionary<string, AttributeModel> mAttributesIndex;
        private Dictionary<AttributeModel, object> mAttributes;
        public Dictionary<AttributeModel, object> Attributes
        {
            get
            {
                return this.mAttributes;
            }
            private set
            {
                this.mAttributes = value;
                this.NotifyPropertyChanged("Attributes");
            }
        }

        // current style class
        /// <summary>
        /// Gets or sets the style class.
        /// </summary>
        /// <value>
        /// The style class.
        /// </value>
        public string StyleClass
        {
            get
            {
                return this["class"] == null ? string.Empty : this["class"].ToString();
            }
            set
            {
                string styleClassValue = value;
                if (this.ParentScreen != null && this.ParentScreen.ParentProject.RenamedStyles.ContainsKey(value))
                    styleClassValue = this.ParentScreen.ParentProject.RenamedStyles[value];
                else if (this is ScreenModel && ((ScreenModel)this).ParentProject != null && ((ScreenModel)this).ParentProject.RenamedStyles.ContainsKey(value))
                    styleClassValue = ((ScreenModel)this).ParentProject.RenamedStyles[value];

                this["class"] = styleClassValue;
                this.NotifyPropertyChanged("StyleClass");
            }
        }

        /// <summary> 
        /// Gets or sets the style.
        /// </summary>
        /// <value>
        /// The style.
        /// </value>
        public string Style
        {
            get
            {
                return this["style"] == null ? string.Empty : this["style"].ToString();
            }
            set
            {
                this["style"] = value;
                this.NotifyPropertyChanged("Style");
            }
        }

        /// <summary>
        /// Gets the error message for the property with the given name.
        /// </summary>
        /// <param name="attributeName">Name of the attribute.</param>
        /// <returns></returns>
        public object this[string attributeName]
        {
            get
            {
                AttributeModel attribute = this.mAttributesIndex.ContainsKey(attributeName.ToLowerInvariant()) ? this.mAttributesIndex[attributeName.ToLowerInvariant()] : null; // this.ControlDefinition.AllAttributes.FirstOrDefault(a => a.Name.ToLowerInvariant() == attributeName.ToLowerInvariant());
                if (attribute != null)
                {
                    return mAttributes[attribute];
                }
                else
                {
                    return null;
                }
            }
            set
            {
                AttributeModel attribute = this.mAttributesIndex.ContainsKey(attributeName.ToLowerInvariant()) ? this.mAttributesIndex[attributeName.ToLowerInvariant()] : null;
                if (attribute != null)
                {
                    if (this.onAttributeValueChanged(attribute, value))
                    {
                        //return; // don't do notify property change of Item if notify attribute change.
                    }
                }
                this.NotifyPropertyChanged("Item");
            }
        }

        /// <summary>
        /// The parent screen
        /// </summary>
        private ScreenModel _parentScreen;

        /// <summary>
        /// Gets or sets the parent screen.
        /// </summary>
        /// <value>
        /// The parent screen.
        /// </value>
        public ScreenModel ParentScreen
        {
            get
            {
                return _parentScreen;
            }
            set
            {
                _parentScreen = value;
                this.NotifyPropertyChanged("ParentScreen");
            }
        }

        /// <summary>
        /// On the attribute value changed.
        /// </summary>
        /// <param name="attribute">The attribute.</param>
        /// <param name="newValue">The new value.</param>
        public bool onAttributeValueChanged(AttributeModel attribute, object newValue)
        {
            if (this.Attributes.ContainsKey(attribute))
            {
                this.Attributes[attribute] = newValue;
                this.NotifyPropertyChanged(attribute.Name);
                return true;
            }
            return false;
        }

        #region Factory

        private void LoadControlFromDefinition()
        {
            if (this.mControlDefinition != null)
            {
                // create dictionary of attributes
                this.LoadAttributesFromDefinition();

                // set default class
                this.StyleClass = this.mControlDefinition.DefaultStyleClass;
                this.Style = "";
                // set icon
                //this.IconUrl = this.mControlDefinition.IconUrl;
            }
        }

        private void LoadDefaultAttributesForDesigner()
        {

        }

        private void LoadAttributesFromDefinition()
        {
            // validate default attributes: width, height, left, top, id
            // todo:

            List<AttributeModel> allAttributes = this.mControlDefinition.AllAttributes;
            foreach (AttributeModel attribute in allAttributes)
            {
                // add it with the default value
                if (this.mAttributesIndex.ContainsKey(attribute.Name.ToLowerInvariant()))
                {
                    throw new ArgumentException(string.Format("The attribute {0} already exist in the control type {1}", attribute.Name, this.mControlDefinition.Name), "Attributes");
                }

                this.mAttributesIndex.Add(attribute.Name.ToLowerInvariant(), attribute);
                this.mAttributes.Add(attribute, attribute.DefaultValue);
                if (attribute.AttributeType.ToLowerInvariant() == "collection")
                {
                    this.mAttributes[attribute] = new TrulyObservableCollection<UIControlInstanceModel>();
                }
            }
        }

        #endregion

        #region MobiseModel Base

        /// <summary>
        /// Gets the name of the attribute model by.
        /// </summary>
        /// <param name="attribute">The attribute.</param>
        /// <returns></returns>
        public AttributeModel GetAttributeModelByName(string attribute)
        {
            return this.mAttributesIndex[attribute.ToLowerInvariant()];
        }

        /// <summary>
        /// To the XML.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public override XElement ToXml()
        {
            XElement controlDefinitionElement = new XElement("item");
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
            baseElement.SetAttributeValue("mobiseID", this.MobiseObjectID);
            baseElement.SetAttributeValue("type", this.ControlDefinition.Name);
            if (!string.IsNullOrEmpty(this.Name))
                baseElement.SetAttributeValue("name", this.Name);

            foreach (var key in mAttributes.Keys)
            {
                this.SerializeAttributeValue(baseElement, key);
            }

            return baseElement;
        }

        /// <summary>
        /// Serializes the attribute value.
        /// </summary>
        /// <param name="baseElement">The base element.</param>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        private void SerializeAttributeValue(XElement baseElement, AttributeModel key)
        {
            string value = string.Empty;
            object typedValue = mAttributes[key];
            if (typedValue != null)
            {
                switch (key.AttributeType.ToLowerInvariant())
                {
                    case "collection": // only collections are special cases
                        {
                            TrulyObservableCollection<UIControlInstanceModel> collection = typedValue as TrulyObservableCollection<UIControlInstanceModel>;
                            if (collection.Count > 0)
                            {
                                AddToXMLChildElementsByID(baseElement, key.Name, collection);
                            }
                        }
                        break;

                    default:
                        {
                            AttributeValueConverter valueConverter = new AttributeValueConverter();
                            value = valueConverter.Convert(typedValue, typeof(string), key.AttributeType, System.Globalization.CultureInfo.InvariantCulture) as string;
                        }
                        break;
                }
            }
            else
            {
                value = string.Empty;
            }

            if (!string.IsNullOrEmpty(value))
            {
                baseElement.SetAttributeValue(key.Name.ToLowerInvariant(), value);
            }

        }

         public void FromXml(XElement xml, string newId)
        {
            this.FromXml(xml);
            this.mMobiseObjectID = newId;
        }

        /// <summary>
        /// Froms the XML.
        /// </summary>
        /// <param name="xml">The XML.</param>
        /// <exception cref="System.NotImplementedException"></exception>
        public override void FromXml(XElement xml)
        {
            if (xml.Attribute("name") != null)
                this.Name = xml.Attribute("name").Value;

            if (xml.Attribute("mobiseID") != null && !(this is ScreenModel))
                this.mMobiseObjectID = xml.Attribute("mobiseID").Value;

            ////if (xml.Attribute("class") != null)
            ////  this.StyleClass = xml.Attribute("class").Value;

            ////if (xml.Attribute("style") != null)
            ////    this.Style = xml.Attribute("style").Value;

            foreach (var attribute in xml.Attributes())
            {
                // don't load any attribute call type name or mobiseID because this are not part of the dynamic attributes
                if (attribute.Name.LocalName.ToLowerInvariant() != "type" && attribute.Name.LocalName.ToLowerInvariant() != "name" && attribute.Name.LocalName.ToLowerInvariant() != "mobiseid")
                {
                    var controlAttribute = this.mAttributesIndex.ContainsKey(attribute.Name.LocalName.ToLowerInvariant()) ? this.mAttributesIndex[attribute.Name.LocalName.ToLowerInvariant()] : null;//this.mControlDefinition.AllAttributes.FirstOrDefault(a => a.Name.ToLowerInvariant() == attribute.Name.LocalName.ToLowerInvariant());
                    if (controlAttribute != null)
                    {
                        this.mAttributes[controlAttribute] = controlAttribute.ParseValue(attribute.Value);
                    }
                }
            }

            if (xml.HasElements)
            {
                foreach (XElement childCollection in xml.Elements())
                {
                    var controlAttribute = this.mAttributesIndex.ContainsKey(childCollection.Name.LocalName.ToLowerInvariant()) ? this.mAttributesIndex[childCollection.Name.LocalName.ToLowerInvariant()] : null;//this.mControlDefinition.AllAttributes.FirstOrDefault(a => a.Name == mControlDefinition.ContainerProperty);
                    if (controlAttribute != null)
                    {
                        TrulyObservableCollection<UIControlInstanceModel> childItems = new TrulyObservableCollection<UIControlInstanceModel>();
                        if (childItems != null)
                        {
                            ReadFromXMLChildItems(xml, childCollection.Name.LocalName, childItems);
                        }
                        this.mAttributes[controlAttribute] = childItems;
                    }
                }
            }
        }

        protected void ReadFromXMLChildItems(XElement xml, string childColletionTagName, ICollection<UIControlInstanceModel> childCollection)
        {
            ScreenModel screen = null;
            if (!(this is ScreenModel))
            {
                System.Diagnostics.Debug.Assert(this.ParentScreen != null, "Parent Screen can't be null");
                screen = this.ParentScreen;
            }
            else
            {
                screen = this as ScreenModel;
            }

            System.Diagnostics.Debug.Assert(screen.ParentProject != null, "Parent project can't be null");
            System.Diagnostics.Debug.Assert(screen.ParentProject.mobiseConfiguration != null, "Mobise configuration can't be null");
            System.Diagnostics.Debug.Assert(screen.ParentProject.mobiseConfiguration.Controls != null, "Mobise configuration control definitions can't be null");
            System.Diagnostics.Debug.Assert(screen.ParentProject.mobiseConfiguration.ControllerDefinitons != null, "Mobise configuration controller definitions can't be null");

            XElement childColletionTag = xml.Element(childColletionTagName);
            if (childColletionTag != null && childColletionTag.HasElements)
            {
                // Parallel.ForEach(controls.Elements("control"), (element) =>
                foreach (XElement element in childColletionTag.Elements())
                {
                    string childItemID = element.Attribute("mobiseID") != null ? element.Attribute("mobiseID").Value : string.Empty;
                    UIControlInstanceModel newControl = childCollection.FirstOrDefault(ctrl => ctrl.MobiseObjectID == childItemID);
                    if (newControl == null)
                    {
                        string controlType = element.Attribute("type") != null ? element.Attribute("type").Value : string.Empty;

                        UIControlDefinitionModel definition = null;
                        definition = screen.ParentProject.mobiseConfiguration.Controls.FirstOrDefault(def => def.Name.Equals(controlType, StringComparison.OrdinalIgnoreCase));
                        
                        if (definition != null)
                        {
                            newControl = new UIControlInstanceModel(definition);
                            
                            if (this is ScreenModel)
                            {
                                newControl.ParentScreen = this as ScreenModel;
                            }
                            else
                            {
                                newControl.ParentScreen = this.ParentScreen;
                            }
                            childCollection.Add(newControl);
                        }
                    }
                    newControl.FromXml(element);

                };

            }
        }
        #endregion

    }
}
