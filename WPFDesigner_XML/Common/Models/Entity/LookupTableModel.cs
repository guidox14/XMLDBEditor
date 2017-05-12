

namespace WPFDesigner_XML.Common.Models.Entity
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Xml.Linq;
 

    /// <summary>
    /// This class contains the structure for the lookup table objects.
    /// </summary>
    public class LookupTableModel : MobiseModel
    {
        #region Fields

        /// <summary>
        /// The path of the database.
        /// </summary>
        private string databasePath;

        /// <summary>
        /// The name of the database.
        /// </summary>
        private string databaseName;

        /// <summary>
        /// The table name;
        /// </summary>
        private string tableName;

        /// <summary>
        /// The Lookup table name.
        /// </summary>
        private string lookupTableName;

        /// <summary>
        /// The name of the column that contains the key;
        /// </summary>
        private string lookupKey;

        /// <summary>
        /// The name of the column that contains the value.
        /// </summary>
        private string lookupValue;

        /// <summary>
        /// The name of the column that contains the description.
        /// </summary>
        private string lookupDescription;

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the database path.
        /// </summary>
        /// <value>The database path.</value>
        public string DatabasePath
        {
            get
            {
                return this.databasePath;
            }

            set
            {
                if (this.databasePath != value)
                {
                    this.databasePath = value;
                    this.NotifyPropertyChanged("DatabasePath");
                }
            }
        }

        /// <summary>
        /// Gets or sets the name of the database.
        /// </summary>
        /// <value>The name of the database.</value>
        public string DatabaseName
        {
            get
            {
                return this.databaseName;
            }

            set
            {
                if (this.databaseName != value)
                {
                    this.databaseName = value;
                    this.NotifyPropertyChanged("DatabaseName");
                }
            }
        }

        /// <summary>
        /// Gets or sets the name of the table.
        /// </summary>
        /// <value>The name of the table.</value>
        public string TableName
        {
            get
            {
                return this.tableName;
            }

            set
            {
                if (this.tableName != value)
                {
                    this.tableName = value;
                    this.NotifyPropertyChanged("TableName");
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
        /// Gets or sets the lookup key.
        /// </summary>
        /// <value>The lookup key.</value>
        public string LookupKey
        {
            get
            {
                return this.lookupKey;
            }

            set
            {
                if (this.lookupKey != value)
                {
                    this.lookupKey = value;
                    this.NotifyPropertyChanged("LookupKey");
                }
            }
        }

        /// <summary>
        /// Gets or sets the lookup value.
        /// </summary>
        /// <value>The lookup value.</value>
        public string LookupValue
        {
            get
            {
                return this.lookupValue;
            }

            set
            {
                if (this.lookupValue != value)
                {
                    this.lookupValue = value;
                    this.NotifyPropertyChanged("LookupValue");
                }
            }
        }

        /// <summary>
        /// Gets or sets the lookup description.
        /// </summary>
        /// <value>The lookup description.</value>
        public string LookupDescription
        {
            get
            {
                return this.lookupDescription;
            }

            set
            {
                if (this.lookupDescription != value)
                {
                    this.lookupDescription = value;
                    this.NotifyPropertyChanged("LookupDescription");
                }
            }
        }

        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="LookupTable"/> class.
        /// </summary>
        public LookupTableModel() : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LookupTable"/> class.
        /// </summary>
        /// <param name="xmlLookupTable">The XML lookup table.</param>
        public LookupTableModel(XElement xmlLookupTable)
            : this()
        {
            if (xmlLookupTable != null)
            {
                this.ProcessXml(xmlLookupTable);
            }
        }

        /// <summary>
        /// Processes the XML.
        /// </summary>
        /// <param name="xmlLookupTable">The XML lookup table.</param>
        private void ProcessXml(XElement xmlLookupTable)
        {
            if (xmlLookupTable.HasAttributes)
            {
                this.LookupTableName = xmlLookupTable.Attribute("name") != null ? xmlLookupTable.Attribute("name").Value : string.Empty;
                this.DatabasePath = xmlLookupTable.Attribute("databasePath") != null ? xmlLookupTable.Attribute("databasePath").Value : string.Empty;
                this.DatabaseName = xmlLookupTable.Attribute("databaseName") != null ? xmlLookupTable.Attribute("databaseName").Value : string.Empty;
                this.TableName = xmlLookupTable.Attribute("tableName") != null ? xmlLookupTable.Attribute("tableName").Value : string.Empty;
                this.LookupKey = xmlLookupTable.Attribute("keyColumn") != null ? xmlLookupTable.Attribute("keyColumn").Value : string.Empty;
                this.LookupValue = xmlLookupTable.Attribute("valueColumn") != null ? xmlLookupTable.Attribute("valueColumn").Value : string.Empty;
                this.LookupDescription = xmlLookupTable.Attribute("descriptionColumn") != null ? xmlLookupTable.Attribute("descriptionColumn").Value : string.Empty;
            }
        }

        /// <summary>
        /// Converts the current object to its Mobise Studio XCode XML representation.
        /// </summary>
        /// <returns>
        /// An XML representation of the current object.
        /// </returns>
        public XElement ToRPXCD()
        {
            XElement lookUpElement = new XElement("lookupTable");
            lookUpElement.SetAttributeValue("name", this.LookupTableName);
            lookUpElement.SetAttributeValue("databasePath", this.DatabasePath);
            lookUpElement.SetAttributeValue("databaseName", this.DatabaseName);
            lookUpElement.SetAttributeValue("tableName", this.TableName);
            lookUpElement.SetAttributeValue("keyColumn", this.LookupKey);
            lookUpElement.SetAttributeValue("valueColumn", this.LookupValue);
            lookUpElement.SetAttributeValue("descriptionColumn", this.LookupDescription);
            return lookUpElement;
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

        /// <summary>
        /// An instance representing the data from the lookup table.
        /// </summary>
        public class LookupTableInstance
        {
            /// <summary>
            /// Gets or sets the key.
            /// </summary>
            /// <value>The key.</value>
            public int Key { get; set; }

            /// <summary>
            /// Gets or sets the value.
            /// </summary>
            /// <value>The value.</value>
            public int Value { get; set; }

            /// <summary>
            /// Gets or sets the description.
            /// </summary>
            /// <value>The description.</value>
            public string Description { get; set; }
        }
    }
}
