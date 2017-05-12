

namespace WPFDesigner_XML.Common.Models.Entity
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Xml.Linq;

    /// <summary>
    /// Extension of the AttributeInfo class to handle the info for the double attribute type.
    /// </summary>
    public class AttributeInfoDouble : AttributeInfo
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
        /// The allowed number of decimal values.
        /// </summary>
        private int decimals;

        /// <summary>
        /// List of available money symbols.
        /// </summary>
        private MoneySymbol moneySymbol;

        #endregion

        #region Properties

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
        /// Gets or sets the decimals.
        /// </summary>
        /// <value>The decimals.</value>
        public int Decimals
        {
            get
            {
                return this.decimals;
            }

            set
            {
                if (this.decimals != value)
                {
                    this.decimals = value;
                    this.NotifyPropertyChanged("Decimals");
                }
            }
        }

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

        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="AttributeInfoDouble"/> class.
        /// </summary>
        public AttributeInfoDouble()
            : base()
        {
            this.InitializeProperties();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AttributeInfoDouble"/> class.
        /// </summary>
        /// <param name="xmlAttribute">The XML attribute.</param>
        public AttributeInfoDouble(XElement xmlAttributeInfo)
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
            this.Decimals = xmlAttributeInfo.Attribute("decimals") != null ? Convert.ToInt32(xmlAttributeInfo.Attribute("decimals").Value) : 2;
            this.MoneySymbol = xmlAttributeInfo.Attribute("moneySymbol") != null ? (MoneySymbol)Enum.Parse(typeof(MoneySymbol), xmlAttributeInfo.Attribute("moneySymbol").Value) : MoneySymbol.None;
            this.Validate = xmlAttributeInfo.Attribute("validate") != null ? Convert.ToBoolean(xmlAttributeInfo.Attribute("validate").Value) : false;
            this.minValue = xmlAttributeInfo.Attribute("minValue") != null ? Convert.ToDouble(xmlAttributeInfo.Attribute("minValue").Value) : 0;
            this.maxValue = xmlAttributeInfo.Attribute("maxValue") != null ? Convert.ToDouble(xmlAttributeInfo.Attribute("maxValue").Value) : 100;
        }

        /// <summary>
        /// Converts the current object to its Mobise Studio XCode XML representation.
        /// </summary>
        /// <returns>
        /// An XML representation of the current object.
        /// </returns>
        public override IList<XAttribute> ToRPXCD()
        {
            IList<XAttribute> attributes = new List<XAttribute>(base.ToRPXCD()) { 
                new XAttribute("decimals", this.Decimals),
                new XAttribute("moneySymbol", this.MoneySymbol)
            };

            if (this.Validate)
            {
                attributes.Add(new XAttribute("validate", this.Validate));
                attributes.Add(new XAttribute("minValue", this.MinValue));
                attributes.Add(new XAttribute("maxValue", this.MaxValue));
            }

            return attributes;
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
            this.Decimals = 2;
            this.MoneySymbol = MoneySymbol.None;
            this.Validate = false;
            this.MinValue = 0;
            this.MaxValue = 100;
        }
    }
}
