

namespace WPFDesigner_XML.Common.Models.Entity
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Xml.Linq;

    /// <summary>
    /// This class will store extra information about the attributes.
    /// </summary>
    public class AttributeInfo : MobiseModel
    {
        /// <summary>
        /// The level of the attribute.
        /// </summary>
        private AttributeLevel attributeLevel;

        /// <summary>
        /// Flag to mark wherever or not the attribute is editable.
        /// </summary>
        private bool isEditable;

        /// <summary>
        /// Flag to mark wherever or not the attribute is indexed.
        /// </summary>
        private bool isIndexed;

        /// <summary>
        /// Flag whatever the value is unique.
        /// </summary>
        private bool isUnique;

        /// <summary>
        /// Flag to mark wherever or not the attribute is a primary key.
        /// </summary>
        private bool isPrimaryKey;

        /// <summary>
        /// The present in SQLite DB
        /// </summary>
        private bool presentInSQLiteDB;

        /// <summary>
        /// Flag to mark wherever or not the attribute is a primary key on client side.
        /// </summary>
        private bool isClientKey;

        /// <summary>
        /// The default value of the field
        /// </summary>
        private string defaultValue;

        /// <summary>
        /// The description of the field
        /// </summary>
        private string description;

        /// <summary>
        /// The lookup script of the field
        /// </summary>
        private string lookupScript;

        /// <summary>
        /// Gets or sets the attribute level.
        /// </summary>
        /// <value>The attribute level.</value>
        public AttributeLevel AttributeLevel
        {
            get
            {
                return this.attributeLevel;
            }

            set
            {
                if (this.attributeLevel != value)
                {
                    this.attributeLevel = value;
                    this.NotifyPropertyChanged("AttributeLevel");
                }
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is editable.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is editable; otherwise, <c>false</c>.
        /// </value>
        public bool IsEditable
        {
            get
            {
                return this.isEditable;
            }

            set
            {
                if (this.isEditable != value)
                {
                    this.isEditable = value;
                    this.NotifyPropertyChanged("IsEditable");
                }
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is indexed.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is indexed; otherwise, <c>false</c>.
        /// </value>
        public bool IsIndexed
        {
            get
            {
                return this.isIndexed;
            }

            set
            {
                if (this.isIndexed != value)
                {
                    this.isIndexed = value;
                    this.NotifyPropertyChanged("IsIndexed");
                }
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is unique.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is unique; otherwise, <c>false</c>.
        /// </value>
        public bool IsUnique
        {
            get
            {
                return this.isUnique;
            }

            set
            {
                if (this.isUnique != value)
                {
                    this.isUnique = value;
                    this.NotifyPropertyChanged("IsUnique");
                }
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is primary key.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is primary key; otherwise, <c>false</c>.
        /// </value>
        public bool IsPrimaryKey
        {
            get
            {
                return this.isPrimaryKey;
            }

            set
            {
                if (this.isPrimaryKey != value)
                {
                    this.isPrimaryKey = value;
                    this.NotifyPropertyChanged("IsPrimaryKey");
                }
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether [present in SQLite DB].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [present in SQLite DB]; otherwise, <c>false</c>.
        /// </value>
        public bool PresentInSQLiteDB
        {
            get
            {
                return this.presentInSQLiteDB;
            }

            set
            {
                this.presentInSQLiteDB = value;
                this.NotifyPropertyChanged("PresentInSQLiteDB");
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is primary key on client side.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is primary key on client side; otherwise, <c>false</c>.
        /// </value>
        public bool IsClientKey
        {
            get
            {
                return this.isClientKey;
            }

            set
            {
                if (this.isClientKey != value)
                {
                    this.isClientKey = value;
                    this.NotifyPropertyChanged("IsClientKey");
                }
            }
        }

        /// <summary>
        /// Gets or sets the default value.
        /// </summary>
        /// <value>
        /// The default value.
        /// </value>
        public string DefaultValue
        {
            get
            {
                return this.defaultValue;
            }

            set
            {
                if (this.defaultValue != value)
                {
                    this.defaultValue = value;
                    this.NotifyPropertyChanged("DefaultValue");
                }
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
                return this.description;
            }

            set
            {
                if (this.description != value)
                {
                    this.description = value;
                    this.NotifyPropertyChanged("Description");
                }
            }
        }

        /// <summary>
        /// Gets or sets the lookup script.
        /// </summary>
        /// <value>
        /// The lookup script.
        /// </value>
        public string LookupScript
        {
            get
            {
                return this.lookupScript;
            }

            set
            {
                if (this.lookupScript != value)
                {
                    this.lookupScript = value;
                    this.NotifyPropertyChanged("LookupScript");
                }
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AttributeInfo"/> class.
        /// </summary>
        public AttributeInfo() : base()
        {
            this.PresentInSQLiteDB = true;
            this.IsIndexed = false;
            this.IsEditable = true;
            this.IsPrimaryKey = false;
            this.IsUnique = false;
            this.IsClientKey = false;
            this.DefaultValue = string.Empty;
            this.Description = string.Empty;
            this.LookupScript = string.Empty;
            this.AttributeLevel = AttributeLevel.Level0;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AttributeInfo"/> class.
        /// </summary>
        /// <param name="xmlAttributeInfo">The XML attribute info.</param>
        public AttributeInfo(XElement xmlAttributeInfo)
            : this()
        {
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
            this.PresentInSQLiteDB = xmlAttributeInfo.Attribute("inSQLiteDB") != null ? Convert.ToBoolean(xmlAttributeInfo.Attribute("inSQLiteDB").Value) : true;
            this.IsPrimaryKey = xmlAttributeInfo.Attribute("isPrimaryKey") != null ? Convert.ToBoolean(xmlAttributeInfo.Attribute("isPrimaryKey").Value) : false;
            this.IsIndexed = xmlAttributeInfo.Attribute("indexed") != null ? xmlAttributeInfo.Attribute("indexed").Value.ToUpperInvariant().Equals("YES") ? true : false : false;
            this.IsEditable = xmlAttributeInfo.Attribute("isEditable") != null ? Convert.ToBoolean(xmlAttributeInfo.Attribute("isEditable").Value) : false;
            this.AttributeLevel = xmlAttributeInfo.Attribute("attributeLevel") != null ? (AttributeLevel)Enum.Parse(typeof(AttributeLevel), xmlAttributeInfo.Attribute("attributeLevel").Value) : AttributeLevel.Level0;
            this.IsClientKey = xmlAttributeInfo.Attribute("isClientKey") != null ? Convert.ToBoolean(xmlAttributeInfo.Attribute("isClientKey").Value) : false;
            this.DefaultValue = xmlAttributeInfo.Attribute("defaultValue") != null ? xmlAttributeInfo.Attribute("defaultValue").Value : string.Empty;
            this.Description = xmlAttributeInfo.Attribute("description") != null ? xmlAttributeInfo.Attribute("description").Value : string.Empty;
            this.LookupScript = xmlAttributeInfo.Attribute("lookupScript") != null ? xmlAttributeInfo.Attribute("lookupScript").Value : string.Empty;
        }

        /// <summary>
        /// Converts the current object to its XCode XML representation.
        /// </summary>
        /// <returns>
        /// An XML representation of the current object.
        /// </returns>
        public virtual XAttribute ToXCD()
        {
            return new XAttribute("indexed", this.IsClientKey ? "YES" : (this.IsIndexed ? "YES" : "NO"));
        }

        /// <summary>
        /// Converts the current object to its Mobise Studio XCode XML representation.
        /// </summary>
        /// <returns>
        /// An XML representation of the current object.
        /// </returns>
        public virtual IList<XAttribute> ToRPXCD()
        {
            IList<XAttribute> attributes = new List<XAttribute>() { this.ToXCD() };
            if (this.IsEditable)
            {
                attributes.Add(new XAttribute("isEditable", this.IsEditable));
                attributes.Add(new XAttribute("isPrimaryKey", this.IsPrimaryKey));
                attributes.Add(new XAttribute("attributeLevel", this.AttributeLevel));
                attributes.Add(new XAttribute("inSQLiteDB", this.PresentInSQLiteDB));
                if (this.IsClientKey)
                {
                    attributes.Add(new XAttribute("isClientKey", this.IsClientKey));
                }
                if (!String.IsNullOrEmpty(this.DefaultValue))
                {
                    attributes.Add(new XAttribute("defaultValue", this.DefaultValue));
                }
                if (!String.IsNullOrEmpty(this.Description))
                {
                    attributes.Add(new XAttribute("description", this.Description));
                }
                if (!String.IsNullOrEmpty(this.LookupScript))
                {
                    attributes.Add(new XAttribute("lookupScript", this.LookupScript));
                }
            }

            return attributes;
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
}
