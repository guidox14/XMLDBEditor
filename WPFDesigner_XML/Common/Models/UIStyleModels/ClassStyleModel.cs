using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace WPFDesigner_XML.Common.Models.UIStyleModels
{
    public static class DictionaryHelper
    {
        public static bool DictionaryEqual<TKey, TValue>(this IDictionary<TKey, TValue> first, IDictionary<TKey, TValue> second)
        {
            if (first == second) return true;
            if ((first == null) || (second == null)) return false;
            if (first.Count != second.Count) return false;

            var comparer = EqualityComparer<TValue>.Default;

            foreach (KeyValuePair<TKey, TValue> kvp in first)
            {
                TValue secondValue;
                if (!second.TryGetValue(kvp.Key, out secondValue)) return false;
                if (!comparer.Equals(kvp.Value, secondValue)) return false;
            }
            return true;
        }
    }
    /// <summary>
    /// contains the class style information for a given control type in a given target if apply
    /// </summary>
    public class ClassStyleModel: MobiseModel, ICloneable
    {

        /// <summary>
        /// The target control type
        /// </summary>
        private string _controlType;

        /// <summary>
        /// The target device type
        /// </summary>
        private string _target;

        /// <summary>
        /// The is default style
        /// </summary>
        private bool _isDefault;

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
                return _isDefault; 
            }
            set 
            { 
                _isDefault = value;
                this.NotifyPropertyChanged("IsDefault");
            }
        }
        public bool Compare(ClassStyleModel style)
        {
            // Get the keys of the collection
            return DictionaryHelper.DictionaryEqual<string, string>(style.DictAttributeList, this.DictAttributeList );

        }

        /// <summary>
        /// The style attributes
        /// </summary>
        private Dictionary<string, string> _styleAttributes = new Dictionary<string,string>();

        public Dictionary<string, string> DictAttributeList
        {
            get
            {
                return this._styleAttributes;
            }
        }
        /// <summary>
        /// Gets the style attribute list.
        /// </summary>
        /// <value>
        /// The style attribute list.
        /// </value>
        public Dictionary<string, string>.KeyCollection StyleAttributeList
        {
            get
            {
                return this._styleAttributes.Keys;
            }
        }

        /// <summary>
        /// Gets or sets the <see cref="System.String"/> with the specified key.
        /// </summary>
        /// <value>
        /// The <see cref="System.String"/>.
        /// </value>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        public string this[string key]
        {
            get
            {
                switch (key.ToLowerInvariant())
                {
                    case "name":
                        return this.Name;
                    case "controltype":
                        return this.ControlType;
                    case "target":
                        return this.Target;
                    default:
                        if (_styleAttributes.ContainsKey(key.ToLowerInvariant()))
                            return _styleAttributes[key.ToLowerInvariant()];
                        else
                            return null;
                }
               
            }
            set
            {
                switch (key.ToLowerInvariant())
                {
                    case "name":
                        this.Name = value;
                        break;
                    case "controltype":
                        this.ControlType = value;
                        break;
                    case "target":
                        this.Target = value;
                        break;
                    default:
                        _styleAttributes[key.ToLowerInvariant()] = value;
                        break;
                }
                this.NotifyPropertyChanged(key);
            }
        }

        /// <summary>
        /// Gets or sets the target.
        /// </summary>
        /// <value>
        /// The target.
        /// </value>
        public string Target
        {
            get 
            { 
                return this._target; 
            }
            set 
            {
                this._target = value;
                this.NotifyPropertyChanged("Target");
            }
        }
        
        /// <summary>
        /// Gets or sets the type of the control.
        /// </summary>
        /// <value>
        /// The type of the control.
        /// </value>
        public string ControlType
        {
            get 
            { 
                return this._controlType; 
            }
            set 
            { 
                this._controlType = value;
                this.NotifyPropertyChanged("ControlType");
            }
        }
        

       // public TrulyObservableCollection<StyleNodeModel> attributes;

        /// <summary>
        /// Initializes a new instance of the <see cref="ClassStyleModel"/> class.
        /// </summary>
        public ClassStyleModel(MobiseModel parent)
            : base(parent)
        {
        }

        #region Mobise Model
        /// <summary>
        /// To the XML.
        /// </summary>
        /// <returns></returns>
        public override System.Xml.Linq.XElement ToXml()
        {
            XElement element = new XElement("class");
            return this.ToXml(element, true);
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
            baseElement.SetAttributeValue("controlType", this.ControlType);
            baseElement.SetAttributeValue("target", this.Target);
            baseElement.SetAttributeValue("isDefault", this.IsDefault);
            StringBuilder value = new StringBuilder();
            value.AppendLine();
            foreach (string key in _styleAttributes.Keys)
            {
                string serializedAttribute = string.Format("{0}:{1};", key, _styleAttributes[key]);
                value.Append("          ");
                value.AppendLine(serializedAttribute); 
            }
            value.Append("      ");
            baseElement.SetValue(value.ToString());
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
                this.ControlType = xml.Attribute("controlType") != null ? xml.Attribute("controlType").Value : string.Empty;
                this.Target = xml.Attribute("target") != null ? xml.Attribute("target").Value : string.Empty;
                this.IsDefault = xml.Attribute("isDefault") != null ? bool.Parse(xml.Attribute("isDefault").Value) : false; 
            }
            if (!string.IsNullOrEmpty(xml.Value))
            {
                string[] attributes = xml.Value.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
                for (int i = 0; i < attributes.Length; i++)
                {
                    string[] components = attributes[i].Split(new char[] { ':' }, StringSplitOptions.RemoveEmptyEntries);
                    if (components.Length == 2)
                    {
                        this[components[0].Trim()] = components[1].Trim();
                    }
                }
            }
        }
        #endregion


        public object Clone()
        {
            ClassStyleModel newStyleMdl = new ClassStyleModel(this.Parent);
            newStyleMdl.FromXml(this.ToXml());
            return newStyleMdl;
        }

        public ClassStyleModel ApplyInlineStyle(string inlineStyle)
        {
            ClassStyleModel newStyleMdl = new ClassStyleModel(this.Parent);
            newStyleMdl.FromXml(this.ToXml());

            if (!string.IsNullOrEmpty(inlineStyle))
            {
                string[] attributes = inlineStyle.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
                for (int i = 0; i < attributes.Length; i++)
                {
                    string[] components = attributes[i].Split(new char[] { ':' }, StringSplitOptions.RemoveEmptyEntries);
                    if (components.Length == 2)
                    {
                        newStyleMdl[components[0].Trim()] = components[1].Trim();
                    }
                }
            }

            return newStyleMdl;
        }
    }
}
