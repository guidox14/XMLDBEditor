

namespace WPFDesigner_XML.Common.Models.Entity
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Xml.Linq;

    public class AttributeInfoDate : AttributeInfo
    {
        #region Fields

        /// <summary>
        /// Mark if the value is time only.
        /// </summary>
        private bool isTimeOnly;

        /// <summary>
        /// Mark if the value is date only.
        /// </summary>
        private bool isDateOnly;


        /// <summary>
        /// Mark if requires validation.
        /// </summary>
        private bool validate;

        /// <summary>
        /// Min value for validation.
        /// </summary>
        private DateTime minDate;

        /// <summary>
        /// Max value for validation.
        /// </summary>
        private DateTime maxDate;

        /// <summary>
        /// Mark that the min date will be current run time.
        /// </summary>
        private bool isMinDateNow;

        /// <summary>
        /// Mark that the max date will be current run time.
        /// </summary>
        private bool isMaxDateNow;

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets a value indicating whether this instance is time only.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is time only; otherwise, <c>false</c>.
        /// </value>
        public bool IsTimeOnly
        {
            get
            {
                return this.isTimeOnly;
            }

            set
            {
                if (this.isTimeOnly != value)
                {
                    this.isTimeOnly = value;
                    this.isDateOnly = value ? !value : value;
                    this.NotifyPropertyChanged("IsTimeOnly");
                    this.NotifyPropertyChanged("IsDateOnly");
                }
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is date only.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is date only; otherwise, <c>false</c>.
        /// </value>
        public bool IsDateOnly
        {
            get
            {
                return this.isDateOnly;
            }

            set
            {
                if (this.isDateOnly != value)
                {
                    this.isDateOnly = value;
                    this.isTimeOnly = value ? !value : value;
                    this.NotifyPropertyChanged("IsDateOnly");
                    this.NotifyPropertyChanged("IsTimeOnly");
                }
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="AttributeInfoDate"/> is validate.
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
        /// Gets or sets the min date.
        /// </summary>
        /// <value>The min date.</value>
        public DateTime MinDate
        {
            get
            {
                return this.minDate;
            }

            set
            {
                if (this.minDate != value)
                {
                    this.minDate = value;
                    this.NotifyPropertyChanged("MinDate");
                }
            }
        }

        /// <summary>
        /// Gets or sets the max date.
        /// </summary>
        /// <value>The max date.</value>
        public DateTime MaxDate
        {
            get
            {
                return this.maxDate;
            }

            set
            {
                if (this.maxDate != value)
                {
                    this.maxDate = value;
                    this.NotifyPropertyChanged("MaxDate");
                }
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is min date now.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is min date now; otherwise, <c>false</c>.
        /// </value>
        public bool IsMinDateNow
        {
            get
            {
                return this.isMinDateNow;
            }

            set
            {
                if (this.isMinDateNow != value)
                {
                    this.isMinDateNow = value;
                    this.NotifyPropertyChanged("IsMinDateNow");
                }
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is max date now.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is max date now; otherwise, <c>false</c>.
        /// </value>
        public bool IsMaxDateNow
        {
            get
            {
                return this.isMaxDateNow;
            }

            set
            {
                if (this.isMaxDateNow != value)
                {
                    this.isMaxDateNow = value;
                    this.NotifyPropertyChanged("IsMaxDateNow");
                }
            }
        }

        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="AttributeInfoDate"/> class.
        /// </summary>
        public AttributeInfoDate()
            : base()
        {
            this.InitializeProperties();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AttributeInfoDate"/> class.
        /// </summary>
        /// <param name="xmlAttributeInfo">The XML attribute info.</param>
        public AttributeInfoDate(XElement xmlAttributeInfo)
            : base(xmlAttributeInfo)
        {
            this.InitializeProperties();
            if (xmlAttributeInfo != null)
            {
                this.ProcessXml(xmlAttributeInfo);
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
        /// Converts the current object to its Mobise Studio XCode XML representation.
        /// </summary>
        /// <returns>
        /// An XML representation of the current object.
        /// </returns>
        public override IList<XAttribute> ToRPXCD()
        {
            IList<XAttribute> attributes = new List<XAttribute>(base.ToRPXCD()) { 
                new XAttribute("isTimeOnly", this.IsTimeOnly), 
                new XAttribute("isDateOnly", this.IsDateOnly)
            };

            if (this.Validate)
            {
                attributes.Add(new XAttribute("validate", this.Validate));
                attributes.Add(this.IsMinDateNow ? new XAttribute("isMinDateNow", this.IsMinDateNow) : new XAttribute("minDate", this.MinDate.ToUniversalTime()));
                attributes.Add(this.IsMaxDateNow ? new XAttribute("isMaxDateNow", this.IsMaxDateNow) : new XAttribute("maxDate", this.MaxDate.ToUniversalTime()));
            }

            return attributes;
        }

        /// <summary>
        /// Processes the XML.
        /// </summary>
        /// <param name="xmlAttributeInfo">The XML attribute info.</param>
        private void ProcessXml(XElement xmlAttributeInfo)
        {
            this.IsTimeOnly = xmlAttributeInfo.Attribute("isTimeOnly") != null ? Convert.ToBoolean(xmlAttributeInfo.Attribute("isTimeOnly").Value) : false;
            this.IsDateOnly = xmlAttributeInfo.Attribute("isDateOnly") != null ? Convert.ToBoolean(xmlAttributeInfo.Attribute("isDateOnly").Value) : false;
            this.Validate = xmlAttributeInfo.Attribute("validate") != null ? Convert.ToBoolean(xmlAttributeInfo.Attribute("validate").Value) : false;
            this.IsMinDateNow = xmlAttributeInfo.Attribute("isMinDateNow") != null ? Convert.ToBoolean(xmlAttributeInfo.Attribute("isMinDateNow").Value) : false;
            this.IsMaxDateNow = xmlAttributeInfo.Attribute("isMaxDateNow") != null ? Convert.ToBoolean(xmlAttributeInfo.Attribute("isMaxDateNow").Value) : false;
            this.MinDate = xmlAttributeInfo.Attribute("minDate") != null ? Convert.ToDateTime(xmlAttributeInfo.Attribute("minDate").Value).ToLocalTime() : DateTime.Now;
            this.MaxDate = xmlAttributeInfo.Attribute("maxDate") != null ? Convert.ToDateTime(xmlAttributeInfo.Attribute("maxDate").Value).ToLocalTime() : DateTime.Now;
        }

        /// <summary>
        /// Initializes the properties.
        /// </summary>
        private void InitializeProperties()
        {
            this.MaxDate = DateTime.Now;
            this.MinDate = DateTime.Now;
            this.IsDateOnly = true;
            this.IsTimeOnly = false;
            this.IsMaxDateNow = false;
            this.IsMinDateNow = false;
            this.Validate = false;
        }
    }
}
