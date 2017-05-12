using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace WPFDesigner_XML.Common.Models.UIStyleModels
{
    public class StyleSchemaMappingModel: MobiseModel
    {
        /// <summary>
        /// The control type
        /// </summary>
        private string _controlType;

        /// <summary>
        /// The group name
        /// </summary>
        private string _groupName;
        
        /// <summary>
        /// Gets or sets the name of the group.
        /// </summary>
        /// <value>
        /// The name of the group.
        /// </value>
        public string GroupName
        {
            get 
            { 
                return this._groupName; 
            }
            set 
            {
                this._groupName = value;
                this.NotifyPropertyChanged("GroupName");
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
                return _controlType; 
            }
            set 
            { 
                _controlType = value;
                this.NotifyPropertyChanged("ControlType");
            }
        }


        /// <summary>
        /// Gets or sets the exceptions.
        /// </summary>
        /// <value>
        /// The exceptions.
        /// </value>
        public Dictionary<string, List<string>> Exceptions { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="StyleSchemaMappingModel"/> class.
        /// </summary>
        public StyleSchemaMappingModel(MobiseModel parent)
            : base(parent)
        {
            this.Exceptions = new Dictionary<string, List<string>>();
        }

        #region MobiseModel

        /// <summary>
        /// To the XML.
        /// </summary>
        /// <returns></returns>
        public override System.Xml.Linq.XElement ToXml()
        {
            XElement element = new XElement("mapping");
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
            System.Diagnostics.Debug.Assert(baseElement != null, "The XElement shouldn't be null");
            baseElement.SetAttributeValue("controlType", this.ControlType);
            baseElement.SetAttributeValue("refGroupName", this.GroupName);

            if (this.Exceptions.Count > 0)
            {
                StringBuilder exeptionsString = new StringBuilder();
                foreach (var key in this.Exceptions.Keys)
                {
                    if (exeptionsString.Length > 0)
                    {
                        exeptionsString.Append(", ");
                    }
                    exeptionsString.Append(key);
                    exeptionsString.Append(".");
                    exeptionsString.Append(this.Exceptions[key]);
                }
                baseElement.SetAttributeValue("except", exeptionsString.ToString());
            }

            return baseElement;
        }

        /// <summary>
        /// Froms the XML.
        /// </summary>
        /// <param name="xml">The XML.</param>
        public override void FromXml(System.Xml.Linq.XElement xml)
        {
            System.Diagnostics.Debug.Assert(xml != null, "The XElement shouldn't be null");
            if (xml.HasAttributes)
            {
                this.ControlType = xml.Attribute("controlType") != null ? xml.Attribute("controlType").Value.ToLowerInvariant() : null;
                this.GroupName = xml.Attribute("refGroupName") != null ? xml.Attribute("refGroupName").Value : string.Empty;
                string exceptions = xml.Attribute("except") != null ? xml.Attribute("except").Value : string.Empty;
                this.ParseExceptions(exceptions);
            }
        }

        /// <summary>
        /// Parses the exceptions.
        /// </summary>
        /// <param name="exceptValue">The except value.</param>
        private void ParseExceptions(string exceptValue)
        {
            if (!string.IsNullOrEmpty(exceptValue))
            {
                try
                {
                    string[] differentExceptions = exceptValue.Split(',');
                    foreach (string excep in differentExceptions)
                    {
                        string temp = excep.Trim();
                        string[] values = temp.Split('.');
                        if (values.Count() == 2)
                        {
                            string element = values[0];
                            string attribute = values[1];

                            if (this.Exceptions.ContainsKey(element))
                            {
                                if (!this.Exceptions[element].Contains(attribute))
                                {
                                    this.Exceptions[element].Add(attribute);
                                }
                            }
                            else
                            {
                                List<string> val = new List<string>();
                                val.Add(attribute);
                                this.Exceptions.Add(element, val);
                            }
                        }
                    }
                }
                catch (Exception ex) { }
            }
        }

        #endregion
    }
}
