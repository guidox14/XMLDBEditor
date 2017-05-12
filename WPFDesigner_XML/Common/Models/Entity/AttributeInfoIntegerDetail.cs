
namespace WPFDesigner_XML.Common.Models.Entity
{
    using Microsoft.XmlTemplateDesigner;
    using System;
    using System.Collections.Generic;
    using System.Xml.Linq;

    /// <summary>
    /// 
    /// </summary>
    public class AttributeInfoIntegerDetail : MobiseModel
    {
        #region Fields

        /// <summary>
        /// Marks wherever the Attribute data must be validated.
        /// </summary>
        private bool validate;

        /// <summary>
        /// The min supported value.
        /// </summary>
        private double minValue;

        /// <summary>
        /// The max supported value.
        /// </summary>
        private double maxValue;

        /// <summary>
        /// The step is an increment value for the percentage.
        /// </summary>
        private int step;

        /// <summary>
        /// List of available money symbols.
        /// </summary>
        private MoneySymbol moneySymbol;

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

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="AttributeInfoIntegerDetail"/> is validate.
        /// </summary>
        /// <value><c>true</c> if validate; otherwise, <c>false</c>.</value>
        public bool Validate
        {
            get
            {
                return this.validate;
            }

            set
            {
                if (this.validate != value)
                {
                    this.validate = value;
                    this.NotifyPropertyChanged("Validate");
                }
            }
        }

        /// <summary>
        /// Gets or sets the min value.
        /// </summary>
        /// <value>The min value.</value>
        public double MinValue
        {
            get
            {
                return this.minValue;
            }

            set
            {
                if (this.minValue != value)
                {
                    this.minValue = value;
                    this.NotifyPropertyChanged("MinValue");
                }
            }
        }

        /// <summary>
        /// Gets or sets the max value.
        /// </summary>
        /// <value>The max value.</value>
        public double MaxValue
        {
            get
            {
                return this.maxValue;
            }

            set
            {
                if (this.maxValue != value)
                {
                    this.maxValue = value;
                    this.NotifyPropertyChanged("MaxValue");
                }
            }
        }

        /// <summary>
        /// Gets or sets the step.
        /// </summary>
        /// <value>The step.</value>
        public int Step
        {
            get
            {
                return this.step;
            }

            set
            {
                if (this.step != value)
                {
                    this.step = value;
                    this.NotifyPropertyChanged("Step");
                }
            }
        }

        /// <summary>
        /// Gets or sets the money symbols.
        /// </summary>
        /// <value>The money symbols.</value>
        public MoneySymbol MoneySymbol
        {
            get
            {
                return this.moneySymbol;
            }

            set
            {
                if (this.moneySymbol != value)
                {
                    this.moneySymbol = value;
                    this.NotifyPropertyChanged("MoneySymbol");
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
                this.sortByValue = value;
                this.sortAlphabetically = !value;
                this.NotifyPropertyChanged("SortByValue");
                this.NotifyPropertyChanged("SortAlphabetically");
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
                this.sortAlphabetically = value;
                this.sortByValue = !value;
                this.NotifyPropertyChanged("SortAlphabetically");
                this.NotifyPropertyChanged("SortByValue");
            }
        }

        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="AttributeInfoIntegerDetail"/> class.
        /// </summary>
        public AttributeInfoIntegerDetail() : base()
        {
            this.InitializeProperties();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AttributeInfoIntegerDetail"/> class.
        /// </summary>
        /// <param name="attributeInfoDetailXml">The attribute info detail XML.</param>
        public AttributeInfoIntegerDetail(XElement attributeInfoDetailXml)
            : this()
        {
            if (attributeInfoDetailXml != null)
            {
                this.ProcessXml(attributeInfoDetailXml);
            }
        }

        /// <summary>
        /// Processes the XML.
        /// </summary>
        /// <param name="attributeInfoDetailXml">The attribute info detail XML.</param>
        private void ProcessXml(XElement attributeInfoDetailXml)
        {
            this.Validate = attributeInfoDetailXml.Attribute("validate") != null ? Convert.ToBoolean(attributeInfoDetailXml.Attribute("validate").Value) : false;
            this.MinValue = attributeInfoDetailXml.Attribute("minValue") != null ? Convert.ToDouble(attributeInfoDetailXml.Attribute("minValue").Value) : 0;
            this.MaxValue = attributeInfoDetailXml.Attribute("maxValue") != null ? Convert.ToDouble(attributeInfoDetailXml.Attribute("maxValue").Value) : 100;
            this.MoneySymbol = attributeInfoDetailXml.Attribute("moneySymbol") != null ? (MoneySymbol)Enum.Parse(typeof(MoneySymbol), attributeInfoDetailXml.Attribute("moneySymbol").Value) : MoneySymbol.None;
            this.Step = attributeInfoDetailXml.Attribute("step") != null ? Convert.ToInt32(attributeInfoDetailXml.Attribute("step").Value) : 5;
            this.LookupTableName = attributeInfoDetailXml.Attribute("lookupTableName") != null ? attributeInfoDetailXml.Attribute("lookupTableName").Value : string.Empty;
            this.LookupTableValue = attributeInfoDetailXml.Attribute("lookupTableValue") != null ? Convert.ToInt32(attributeInfoDetailXml.Attribute("lookupTableValue").Value) : 0;
            this.SortByValue = attributeInfoDetailXml.Attribute("sortByValue") != null ? Convert.ToBoolean(attributeInfoDetailXml.Attribute("sortByValue").Value) : false;
            this.SortAlphabetically = attributeInfoDetailXml.Attribute("sortAlphabetically") != null ? Convert.ToBoolean(attributeInfoDetailXml.Attribute("sortAlphabetically").Value) : false;
        }

        /// <summary>
        /// Converts the current object to its Mobise Studio XCode XML representation.
        /// </summary>
        /// <returns>
        /// An XML representation of the current object.
        /// </returns>
        public IList<XAttribute> ToRPXCD(IntegerInfoType infoType)
        {
            IList<XAttribute> attributes = new List<XAttribute>();

            if (this.Validate)
            {
                attributes.Add(new XAttribute("validate", this.Validate));
                attributes.Add(new XAttribute("minValue", this.MinValue));
                attributes.Add(new XAttribute("maxValue", this.MaxValue));
            }

            switch (infoType)
            {
                case IntegerInfoType.Lookup:
                    return new List<XAttribute>() 
                    {
                        new XAttribute("isLookup", true),
                        new XAttribute("lookupTableName", string.IsNullOrEmpty(this.LookupTableName) ? string.Empty : this.LookupTableName),
                        new XAttribute("lookupTableValue", this.LookupTableValue),
                        new XAttribute("sortByValue", this.SortByValue),
                        new XAttribute("sortAlphabetically", this.SortAlphabetically)
                    };

                case IntegerInfoType.Money:
                    attributes.Add(new XAttribute("moneySymbol", this.MoneySymbol));
                    return attributes;

                case IntegerInfoType.Numeric:
                    return attributes;

                case IntegerInfoType.Percentage:
                    attributes.Add(new XAttribute("step", this.Step));
                    return attributes;

                default:
                    return attributes;
            }
        }

        /// <summary>
        /// Initializes the properties.
        /// </summary>
        private void InitializeProperties()
        {
            this.Validate = false;
            this.MinValue = 0;
            this.MaxValue = 100;
            this.Step = 5;
            this.MoneySymbol = MoneySymbol.None;
            this.SortByValue = true;
            this.LookupTableValue = int.MinValue;
        }

        #region MobiseModel Base

        public override XElement ToXml()
        {
            throw new NotImplementedException();
        }

        public override XElement ToXml(XElement baseElement, bool AddCommonAttributes)
        {
            throw new NotImplementedException();
        }

        public override void FromXml(XElement xml)
        {
            throw new NotImplementedException();
        }


        #endregion
    }

    /// <summary>
    /// The different available money symbols.
    /// </summary>
    public enum MoneySymbol
    {
        /// <summary>
        /// Do not use a symbol.
        /// </summary>
        [LocalizableDescription(@"Symbol_None", typeof(Resources))]
        None = 0,

        /// <summary>
        /// The dollar $ symbol.
        /// </summary>
        /// 
        [LocalizableDescription(@"Symbol_Dollar", typeof(Resources))]
        Dollar = 1
    }

    /// <summary>
    /// The different valid options for the Integer validation types.
    /// </summary>
    public enum IntegerInfoType
    {
        /// <summary>
        /// The integer value is a numeric value.
        /// </summary>
        Numeric = 0,

        /// <summary>
        /// The integer value is a percentage value.
        /// </summary>
        Percentage = 1,

        /// <summary>
        /// The integer value is a monetary value.
        /// </summary>
        Money = 2,

        /// <summary>
        /// The integer value comes from  a look up table.
        /// </summary>
        Lookup = 3
    }

}
