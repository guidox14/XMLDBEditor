

namespace WPFDesigner_XML.Common.Models.Entity
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Xml.Linq;

    /// <summary>
    /// Extension of the AttributeInfo class to handle the info for the string attribute type.
    /// </summary>
    public class AttributeInfoString : AttributeInfo
    {
        #region Fields

        /// <summary>
        /// Mark wherever the attribute info comes from a look up table.
        /// </summary>
        private bool isLookup;

        /// <summary>
        /// The name of the lookup table.
        /// </summary>
        private string lookupTableName;

        /// <summary>
        /// The lookup key value.
        /// </summary>
        private int lookupTableValue;

        /// <summary>
        /// Mark wherever the values will be sorted by the default value of the data key.
        /// </summary>
        private bool sortByValue;

        /// <summary>
        /// Mark wherever the values will be sorted alphabetically.
        /// </summary>
        private bool sortAlphabetically;

        /// <summary>
        /// Value that sets the max value of characters.
        /// </summary>
        private int maxChars;

        /// <summary>
        /// Flag if the attribute can be empty.
        /// </summary>
        private bool canBeEmpty;

        /// <summary>
        /// Flag if the attribute will use a regular expression.
        /// </summary>
        private bool useRegex;

        /// <summary>
        /// the regular expression needed to validate the input.
        /// </summary>
        private string regexValue;

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets a value indicating whether this instance is lookup.
        /// </summary>
        /// <value><c>true</c> if this instance is lookup; otherwise, <c>false</c>.</value>
        public bool IsLookup
        {
            get
            {
                return this.isLookup;
            }

            set
            {
                if (this.isLookup != value)
                {
                    this.isLookup = value;
                    if (!this.isLookup)
                    {
                        this.SortByValue = true;
                        this.LookupTableValue = int.MinValue;
                        this.lookupTableName = string.Empty;
                    }
                    else 
                    {
                        this.MaxChars = 32;
                        this.CanBeEmpty = true;
                        this.UseRegex = false;
                        this.RegexValue = string.Empty;
                    }

                    this.NotifyPropertyChanged("IsLookup");
                }
            }
        }

        /// <summary>
        /// Gets or sets the name of the lookup table.
        /// </summary>
        /// <value>The name of the lookup table.</value>
        public string LookupTableName
        {
            get
            {
                return this.lookupTableName;
            }

            set
            {
                if (this.lookupTableName != value)
                {
                    this.lookupTableName = value;
                    this.NotifyPropertyChanged("LookupTableName");
                }
            }
        }

        /// <summary>
        /// Gets or sets the lookup table value.
        /// </summary>
        /// <value>The lookup table value.</value>
        public int LookupTableValue
        {
            get
            {
                return this.lookupTableValue;
            }

            set
            {
                if (this.lookupTableValue != value)
                {
                    this.lookupTableValue = value;
                    this.NotifyPropertyChanged("LookupTableValue");
                }
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether to sort by value.
        /// </summary>
        /// <value><c>true</c> if must sort by value; otherwise, <c>false</c>.</value>
        public bool SortByValue
        {
            get
            {
                return this.sortByValue;
            }

            set
            {
                if (this.sortByValue != value)
                {
                    this.sortByValue = value;
                    this.sortAlphabetically = !value;
                    this.NotifyPropertyChanged("SortByValue");
                    this.NotifyPropertyChanged("SortAlphabetically");
                }
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether to sort alphabetically.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if must sort alphabetically; otherwise, <c>false</c>.
        /// </value>
        public bool SortAlphabetically
        {
            get
            {
                return this.sortAlphabetically;
            }

            set
            {
                if (this.sortAlphabetically != value)
                {
                    this.sortAlphabetically = value;
                    this.sortByValue = !value;
                    this.NotifyPropertyChanged("SortAlphabetically");
                    this.NotifyPropertyChanged("SortByValue");
                }
            }
        }


        /// <summary>
        /// Gets or sets the max chars.
        /// </summary>
        /// <value>The max chars.</value>
        public int MaxChars
        {
            get
            {
                return this.maxChars;
            }

            set
            {
                if (this.maxChars != value)
                {
                    this.maxChars = value;
                    this.NotifyPropertyChanged("MaxChars");
                }
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance can be empty.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance can be empty; otherwise, <c>false</c>.
        /// </value>
        public bool CanBeEmpty
        {
            get
            {
                return this.canBeEmpty;
            }

            set
            {
                if (this.canBeEmpty != value)
                {
                    this.canBeEmpty = value;
                    this.NotifyPropertyChanged("CanBeEmpty");
                }
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether [use regex].
        /// </summary>
        /// <value><c>true</c> if [use regex]; otherwise, <c>false</c>.</value>
        public bool UseRegex
        {
            get
            {
                return this.useRegex;
            }

            set
            {
                if (this.useRegex != value)
                {
                    this.useRegex = value;
                    this.NotifyPropertyChanged("UseRegex");
                }
            }
        }

        /// <summary>
        /// Gets or sets the regex value.
        /// </summary>
        /// <value>The regex value.</value>
        public string RegexValue
        {
            get
            {
                return this.regexValue;
            }

            set
            {
                if (this.regexValue != value)
                {
                    this.regexValue = value;
                    this.NotifyPropertyChanged("RegexValue");
                }
            }
        }

        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="AttributeInfoString"/> class.
        /// </summary>
        public AttributeInfoString()
            : base()
        {
            this.InitializeProperties();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AttributeInfoString"/> class.
        /// </summary>
        /// <param name="xmlAttributeInfo">The XML attribute info.</param>
        public AttributeInfoString(XElement xmlAttributeInfo)
            : base(xmlAttributeInfo)
        {
            this.InitializeProperties();
            if (xmlAttributeInfo != null)
            {
                this.ProcessXml(xmlAttributeInfo);
            }
        }

        /// <summary>
        /// Processes the XML.
        /// </summary>
        /// <param name="xmlAttributeInfo">The XML attribute info.</param>
        private void ProcessXml(XElement xmlAttributeInfo)
        {
            this.IsLookup = xmlAttributeInfo.Attribute("isLookup") != null ? Convert.ToBoolean(xmlAttributeInfo.Attribute("isLookup").Value) : false;
            this.LookupTableName = xmlAttributeInfo.Attribute("lookupTableName") != null ? xmlAttributeInfo.Attribute("lookupTableName").Value : string.Empty;
            this.LookupTableValue = xmlAttributeInfo.Attribute("lookupTableValue") != null ? Convert.ToInt32(xmlAttributeInfo.Attribute("lookupTableValue").Value) : 0;
            this.SortByValue = xmlAttributeInfo.Attribute("sortByValue") != null ? Convert.ToBoolean(xmlAttributeInfo.Attribute("sortByValue").Value) : false;
            this.SortAlphabetically = xmlAttributeInfo.Attribute("sortAlphabetically") != null ? Convert.ToBoolean(xmlAttributeInfo.Attribute("sortAlphabetically").Value) : false;
            this.MaxChars = xmlAttributeInfo.Attribute("maxChars") != null ? Convert.ToInt32(xmlAttributeInfo.Attribute("maxChars").Value) : 32;
            this.CanBeEmpty = xmlAttributeInfo.Attribute("canBeEmpty") != null ? Convert.ToBoolean(xmlAttributeInfo.Attribute("canBeEmpty").Value) : true;
            this.UseRegex = xmlAttributeInfo.Attribute("useRegex") != null ? Convert.ToBoolean(xmlAttributeInfo.Attribute("useRegex").Value) : false;
            this.RegexValue = xmlAttributeInfo.Attribute("regexValue") != null ? xmlAttributeInfo.Attribute("regexValue").Value : string.Empty;
        }

        /// <summary>
        /// Converts the current object to its Mobise Studio XCode XML representation.
        /// </summary>
        /// <returns>
        /// An XML representation of the current object.
        /// </returns>
        public override IList<XAttribute> ToRPXCD()
        {
            IList<XAttribute> attributes = new List<XAttribute>(base.ToRPXCD());

            if (this.IsLookup)
            {
                attributes.Add(new XAttribute("isLookup", this.IsLookup));
                attributes.Add(new XAttribute("lookupTableName", this.LookupTableName));
                attributes.Add(new XAttribute("lookupTableValue", this.LookupTableValue));
                attributes.Add(new XAttribute("sortByValue", this.SortByValue));
                attributes.Add(new XAttribute("sortAlphabetically", this.SortAlphabetically));

                return attributes;
            }
            else
            {
                attributes.Add(new XAttribute("maxChars", this.MaxChars));
                attributes.Add(new XAttribute("canBeEmpty", this.CanBeEmpty));

                if (this.UseRegex)
                {
                    attributes.Add(new XAttribute("useRegex", this.UseRegex));
                    attributes.Add(new XAttribute("regexValue", this.RegexValue));
                }

                return attributes;
            }
        }

        /// <summary>
        /// Converts the current object to its XCode XML representation.
        /// </summary>
        /// <returns>
        /// An XML representation of the current object.
        /// </returns>
        public override XAttribute ToXCD()
        {
            return base.ToXCD();
        }

        /// <summary>
        /// Initializes the properties.
        /// </summary>
        private void InitializeProperties()
        {
            this.MaxChars = 32;
            this.CanBeEmpty = true;
            this.UseRegex = false;
            this.RegexValue = string.Empty;
            this.SortByValue = true;
            this.LookupTableValue = int.MinValue;
        }
    }
}
