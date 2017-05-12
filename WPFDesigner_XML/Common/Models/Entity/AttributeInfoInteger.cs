

namespace WPF.Common.Models.Entity
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Xml.Linq;
    using WPFDesigner_XML.Common.Models.Entity;

    /// <summary>
    /// Extension of the AttributeInfo class to handle the info for the integer attribute type.
    /// </summary>
    public class AttributeInfoInteger : AttributeInfo
    {
        #region Fields

        /// <summary>
        /// Flag whatever the integer amount is a percentage amount.
        /// </summary>
        private bool isPercentage;

        /// <summary>
        /// Flag whatever the integer amount is a numeric amount.
        /// </summary>
        private bool isNumeric;

        /// <summary>
        /// Flag whatever the integer amount is a money amount.
        /// </summary>
        private bool isMoney;

        /// <summary>
        /// Flag whatever the integer amount is a from a lookup table.
        /// </summary>
        private bool isLookup;

        /// <summary>
        /// Enum to classify the different info types.
        /// </summary>
        private IntegerInfoType infoType;

        /// <summary>
        /// Extra information about the attribute.
        /// </summary>
        private AttributeInfoIntegerDetail attributeInfoDetail;

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets a value indicating whether this instance is percentage.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is percentage; otherwise, <c>false</c>.
        /// </value>
        public bool IsPercentage
        {
            get
            {
                return this.isPercentage;
            }

            set
            {
                if (this.isPercentage != value)
                {
                    this.isPercentage = value;
                    this.isNumeric = !value;
                    this.isMoney = !value;
                    this.isLookup = !value;

                    if (value)
                    {
                        this.infoType = IntegerInfoType.Percentage;
                    }

                    this.NotifyPropertyChanged("IsPercentage");
                    this.NotifyPropertyChanged("IsNumeric");
                    this.NotifyPropertyChanged("IsMoney");
                    this.NotifyPropertyChanged("IsLookup");
                }
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is numeric.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is numeric; otherwise, <c>false</c>.
        /// </value>
        public bool IsNumeric
        {
            get
            {
                return this.isNumeric;
            }

            set
            {
                if (this.isNumeric != value)
                {
                    this.isNumeric = value;
                    this.isPercentage = !value;
                    this.isMoney = !value;
                    this.isLookup = !value;

                    if (value)
                    {
                        this.infoType = IntegerInfoType.Numeric;
                    }

                    this.NotifyPropertyChanged("IsPercentage");
                    this.NotifyPropertyChanged("IsNumeric");
                    this.NotifyPropertyChanged("IsMoney");
                    this.NotifyPropertyChanged("IsLookup");
                }
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is money.
        /// </summary>
        /// <value><c>true</c> if this instance is money; otherwise, <c>false</c>.</value>
        public bool IsMoney
        {
            get
            {
                return this.isMoney;
            }

            set
            {
                if (this.isMoney != value)
                {
                    this.isMoney = value;
                    this.isNumeric = !value;
                    this.isPercentage = !value;
                    this.isLookup = !value;

                    if (value)
                    {
                        this.infoType = IntegerInfoType.Money;
                    }

                    this.NotifyPropertyChanged("IsPercentage");
                    this.NotifyPropertyChanged("IsNumeric");
                    this.NotifyPropertyChanged("IsMoney");
                    this.NotifyPropertyChanged("IsLookup");
                }
            }
        }

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
                    this.isMoney = !value;
                    this.isNumeric = !value;
                    this.isPercentage = !value;

                    if (value)
                    {
                        this.infoType = IntegerInfoType.Lookup;
                    }

                    this.NotifyPropertyChanged("IsPercentage");
                    this.NotifyPropertyChanged("IsNumeric");
                    this.NotifyPropertyChanged("IsMoney");
                    this.NotifyPropertyChanged("IsLookup");
                }
            }
        }

        /// <summary>
        /// Gets or sets the attribute info detail.
        /// </summary>
        /// <value>The attribute info detail.</value>
        public AttributeInfoIntegerDetail AttributeInfoDetail
        {
            get
            {
                return this.attributeInfoDetail;
            }

            set
            {
                if (this.attributeInfoDetail != value)
                {
                    this.attributeInfoDetail = value;
                    this.NotifyPropertyChanged("AttributeInfoDetail");
                }
            }
        }

        /// <summary>
        /// Gets or sets the type of the info.
        /// </summary>
        /// <value>The type of the info.</value>
        private IntegerInfoType InfoType
        {
            get
            {
                return this.infoType;
            }

            set
            {
                this.infoType = value;
                switch (this.infoType)
                {
                    case IntegerInfoType.Lookup:
                        this.IsLookup = true;
                        break;

                    case IntegerInfoType.Money:
                        this.IsMoney = true;
                        break;

                    case IntegerInfoType.Numeric:
                        this.IsNumeric = true;
                        break;

                    case IntegerInfoType.Percentage:
                        this.IsPercentage = true;
                        break;
                }
            }
        }

        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="AttributeInfoInteger"/> class.
        /// </summary>
        public AttributeInfoInteger()
            : base()
        {
            this.IsNumeric = true;
            this.AttributeInfoDetail = new AttributeInfoIntegerDetail();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AttributeInfoInteger"/> class.
        /// </summary>
        /// <param name="xmlAttributeInfo">The XML attribute info.</param>
        public AttributeInfoInteger(XElement xmlAttributeInfo)
            : base(xmlAttributeInfo)
        {
            if (xmlAttributeInfo != null)
            {
                this.ProcessXml(xmlAttributeInfo);
                this.AttributeInfoDetail = new AttributeInfoIntegerDetail(xmlAttributeInfo);
            }
        }

        /// <summary>
        /// Processes the XML.
        /// </summary>
        /// <param name="xmlAttributeInfo">The XML attribute info.</param>
        private void ProcessXml(XElement xmlAttributeInfo)
        {
            this.InfoType = xmlAttributeInfo.Attribute("infoType") != null ? (IntegerInfoType)Enum.Parse(typeof(IntegerInfoType), xmlAttributeInfo.Attribute("infoType").Value) : IntegerInfoType.Numeric;
        }

        /// <summary>
        /// Converts the current object to its Mobise Studio XCode XML representation.
        /// </summary>
        /// <returns>
        /// An XML representation of the current object.
        /// </returns>
        public override IList<XAttribute> ToRPXCD()
        {
            IList<XAttribute> attributeList = new List<XAttribute>(base.ToRPXCD()) { 
                new XAttribute("infoType", this.InfoType)
            };

            return attributeList.Concat(AttributeInfoDetail.ToRPXCD(this.InfoType)).ToList();
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
    }
}
