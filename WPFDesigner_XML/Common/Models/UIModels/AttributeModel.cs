using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Xml.Linq;
using WPFDesigner_XML.Common.Converters;

namespace WPFDesigner_XML.Common.Models.UIModels
{
    public class AttributeModel : MobiseModel
    {
       
        /// <summary>
        /// The attribute type
        /// </summary>
        private string _attributeType;

        /// <summary>
        /// The _editor type
        /// </summary>
        private string _editorType;

        /// <summary>
        /// The _description
        /// </summary>
        private string _description;

        /// <summary>
        /// The browsable
        /// </summary>
        private bool _browsable = true;

        /// <summary>
        /// The child type
        /// </summary>
        private string _childType;

        /// <summary>
        /// Gets or sets the type of the child.
        /// </summary>
        /// <value>
        /// The type of the child.
        /// </value>
        public string ChildType
        {
            get
            {
                return _childType;
            }
            set
            {
                _childType = value;
                this.NotifyPropertyChanged("ChildType");
            }
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
                return this._description;
            }
            set
            {
                this._description = value;
                this.NotifyPropertyChanged("Description");
            }
        }

        /// <summary>
        /// Gets or sets the type of the attribute.
        /// </summary>
        /// <value>
        /// The type of the attribute.
        /// </value>
        public string AttributeType
        {
            get
            {
                return this._attributeType;
            }
            set
            {
                this._attributeType = value;
                this.NotifyPropertyChanged("PropertyType");
            }
        }

        /// <summary>
        /// Gets or sets the type of the editor.
        /// </summary>
        /// <value>
        /// The type of the editor.
        /// </value>
        public string EditorType
        {
            get
            {
                return this._editorType;
            }
            set
            {
                this._editorType = value;
                this.NotifyPropertyChanged("EditorType");
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="AttributeModel"/> is browsable.
        /// </summary>
        /// <value>
        ///   <c>true</c> if browsable; otherwise, <c>false</c>.
        /// </value>
        public bool Browsable
        {
            get
            {
                return _browsable;
            }
            set
            {
                this._browsable = value;
                this.NotifyPropertyChanged("Browsable");
            }
        }


        private object _defaultValue;
        /// <summary>
        /// Gets or sets the default Value.
        /// </summary>
        /// <value>
        /// The description.
        /// </value>
        public object DefaultValue
        {
            get
            {
                return this._defaultValue;
            }
            set
            {
                this._defaultValue = value;
                this.NotifyPropertyChanged("DefaultValue");
            }
        }

        public TrulyObservableCollection<ValidationModel> Validations { get; private set; }

        public AttributeModel(AttributeCategoryModel parent)
            : base(parent)
        {
            this.Validations = new TrulyObservableCollection<ValidationModel>();
        }

        #region MobiseModel Base

        public override XElement ToXml()
        {
            XElement attributeElement = new XElement("attribute");
            return ToXml(attributeElement, true);
        }

        public override XElement ToXml(XElement baseElement, bool AddCommonAttributes)
        {
            baseElement.SetAttributeValue("name", this.Name);
            baseElement.SetAttributeValue("description", this.Description);
            baseElement.SetAttributeValue("type", this.AttributeType);

            if (!string.IsNullOrEmpty(this.EditorType))
            {
                baseElement.SetAttributeValue("editorType", this.EditorType);
            }

            baseElement.SetAttributeValue("browsable", this.Browsable.ToString());

            if (!string.IsNullOrEmpty(this.ChildType))
            {
                baseElement.SetAttributeValue("childType", this.ChildType);
            }
            
            AttributeValueConverter converter = new Converters.AttributeValueConverter();
            baseElement.SetAttributeValue("defaultValue", converter.Convert(this.DefaultValue, typeof(string), this.AttributeType, CultureInfo.InvariantCulture));

            if (this.Validations.Count > 0)
            {
                XElement validations = baseElement.Element("Validations");
                if (validations == null)
                {
                    validations = new XElement("Validations");
                    baseElement.Add(validations);
                }
                foreach (ValidationModel item in this.Validations)
                {
                    XElement oldValidation = validations.Elements().FirstOrDefault(val => val.Attribute("name") != null && val.Attribute("name").Value == item.Name);
                    if (oldValidation != null)
                    {
                        item.ToXml(oldValidation, true);
                    }
                    else
                    {
                        validations.Add(item.ToXml());
                    }
                }
            }

            return baseElement;
        }

        public override void FromXml(XElement xml)
        {
            this.Name = xml.Attribute("name") != null ? xml.Attribute("name").Value : string.Empty;
            this.Description = xml.Attribute("description") != null ? xml.Attribute("description").Value : string.Empty;
            this.EditorType = xml.Attribute("editorType") != null ? xml.Attribute("editorType").Value : string.Empty;
            this.ChildType = xml.Attribute("childType") != null ? xml.Attribute("childType").Value : string.Empty;
            this.AttributeType = xml.Attribute("type") != null ? xml.Attribute("type").Value : string.Empty;
            this.DefaultValue = xml.Attribute("defaultValue") != null ? this.ParseValue(xml.Attribute("defaultValue").Value) : null;

            if (xml.Attribute("browsable") != null && !string.IsNullOrEmpty(xml.Attribute("browsable").Value))
            {
                bool browsable = true;
                if (bool.TryParse(xml.Attribute("browsable").Value, out browsable))
                {
                    this.Browsable = browsable;
                }
            }

            if (xml.HasElements)
            {
                XElement validations = xml.Element("Validations");
                if (validations != null)
                {
                    foreach (XElement item in validations.Elements())
                    {
                        string validationType = item.Attribute("name") != null ? item.Attribute("name").Value : string.Empty;
                        ValidationModel validation = this.Validations.FirstOrDefault(val => val.Name == validationType);
                        if (validation == null)
                        {
                            validation = new ValidationModel(this);
                            this.Validations.Add(validation);
                        }
                        validation.FromXml(item);
                    }
                }
            }
        }


        #endregion

        /// <summary>
        /// Parses the value.
        /// </summary>
        /// <param name="stringValue">The string value.</param>
        /// <returns></returns>
        public object ParseValue(string stringValue)
        {
            switch (this.AttributeType.ToLowerInvariant())
            {
                case "int":
                    int intValue = 0;
                    if (int.TryParse(stringValue, out intValue))
                    {
                        return intValue;
                    }
                    else
                    {
                        throw new InvalidCastException(string.Format("The value {0} can't be cast to {1} for the attribute {2}", stringValue, typeof(int).Name, this.Name));
                    }
                case "double":
                    double doubleValue = 0;
                    if (double.TryParse(stringValue, out doubleValue))
                    {
                        return doubleValue;
                    }
                    else
                    {
                        throw new InvalidCastException(string.Format("The value {0} can't be cast to {1} for the attribute {2}", stringValue, typeof(int).Name, this.Name));
                    }
                case "bool":
                case "boolean":
                        bool boolValue = false;
                        if (bool.TryParse(stringValue, out boolValue))
                        {
                            return boolValue;
                        }
                        else
                        {
                            throw new InvalidCastException(string.Format("The value {0} can't be cast to {1} for the attribute {2}", stringValue, typeof(int).Name, this.Name));
                        }
                case "datetime":
                    DateTime datetimeValue = DateTime.MinValue;
                    if (DateTime.TryParse(stringValue, out datetimeValue))
                    {
                        return datetimeValue;
                    }
                    else
                    {
                        throw new InvalidCastException(string.Format("The value {0} can't be cast to {1} for the attribute {2}", stringValue, typeof(int).Name, this.Name));
                    }
                case "regex":
                    Regex regex = new Regex(stringValue);
                    return regex;
                case "Collection":
                    return new TrulyObservableCollection<UIControlInstanceModel>();
                case "rect":
                    Rect rectangle = ParseRectangle(stringValue);
                    return rectangle;
                case "inset":
                    Rect inset = ParseInset(stringValue);
                    return inset;
            }

            return stringValue;
        }
        public static Rect ParseInset(string stringValue)
        {
            double top = 0, left = 0, right = 0, bottom = 0;

            if (stringValue.StartsWith("{") && stringValue.EndsWith("}"))
            {
                string rectString = stringValue.Substring(1, stringValue.Length - 2); // remove side elements

                String []values = rectString.Split(',');

                if (values.Length == 4)
                {
                    double.TryParse(values[0], NumberStyles.Number, CultureInfo.InvariantCulture, out top);
                    double.TryParse(values[1], NumberStyles.Number, CultureInfo.InvariantCulture, out left);
                    double.TryParse(values[2], NumberStyles.Number, CultureInfo.InvariantCulture, out bottom);
                    double.TryParse(values[3], NumberStyles.Number, CultureInfo.InvariantCulture, out right);
                }
            }

            return new Rect(left,top,right,bottom);
        }
        public static Rect ParseRectangle(string stringValue)
        {
            double x = 0, y = 0, width = 0, height = 0;

            if (stringValue.StartsWith("{") && stringValue.EndsWith("}"))
            {
                string rectString = stringValue.Substring(1, stringValue.Length - 2); // remove side elements
                Regex regx = new Regex(@"[{]\s*[0-9]*([.][0-9]*|)\s*,\s*[0-9]*([.][0-9]*|)\s*[}]");
                var matches = regx.Matches(rectString);
                if (matches.Count > 0)
                {
                    string[] values = matches[0].Value.Substring(1, matches[0].Value.Length - 2).Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                    if (values.Length == 2)
                    {
                        double.TryParse(values[0], NumberStyles.Number, CultureInfo.InvariantCulture, out x);
                        double.TryParse(values[1], NumberStyles.Number, CultureInfo.InvariantCulture, out y);
                    }

                    if (matches.Count == 2)
                    {
                        values = matches[1].Value.Substring(1, matches[1].Value.Length - 2).Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                        if (values.Length == 2)
                        {
                            double.TryParse(values[0], NumberStyles.Number, CultureInfo.InvariantCulture, out width);
                            double.TryParse(values[1], NumberStyles.Number, CultureInfo.InvariantCulture, out height);
                        }
                    }
                }
            }

            return new Rect(x, y, width, height);
        }
    }
}
