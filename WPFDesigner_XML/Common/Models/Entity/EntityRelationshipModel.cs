using System;
using System.Xml.Linq;
using System.Collections.ObjectModel;
using Microsoft.XmlTemplateDesigner;

namespace WPFDesigner_XML.Common.Models.Entity
{

    /// <summary>
    /// the class that will define the structure for the relationships.
    /// </summary>
    public class EntityRelationshipModel : MobiseModel
    { 
        /// <summary>
        /// The name of the target table.
        /// </summary>
        private string targetTableName;

        /// <summary>
        /// The name of the inverse relationship, if exist.
        /// </summary>
        private string inverseRelationshipName;

        /// <summary>
        /// Mark wherever the relationship support 1:N.
        /// </summary>
        private bool supportMultipleRelationships;

        /// <summary>
        /// The name of constrain.
        /// </summary>
        private string constrainName;

        /// <summary>
        /// The name of the current column.
        /// </summary>
        private string columnName;

        /// <summary>
        /// List of client keys
        /// </summary>
        private ObservableCollection<ClientKey> clientKeys;

        /// <summary>
        /// Deletion Rule value
        /// </summary>
        private string deletionRule;

        /// <summary>
        /// Gets or sets the name of the target table.
        /// </summary>
        /// <value>The name of the target table.</value>
        public string TargetTableName
        {
            get
            {
                return this.targetTableName;
            }

            set
            {
                if (this.targetTableName != value)
                {
                    this.targetTableName = value;
                    this.NotifyPropertyChanged("TargetTableName");
                }
            }
        }

        /// <summary>
        /// Gets or sets the name of the inverse relationship.
        /// </summary>
        /// <value>The name of the inverse relationship.</value>
        public string InverseRelationshipName
        {
            get
            {
                return this.inverseRelationshipName;
            }

            set
            {
                if (this.inverseRelationshipName != value)
                {
                    this.inverseRelationshipName = value;
                    this.NotifyPropertyChanged("InverseRelationshipName");
                }
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the relationship supports multiple relationships.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if relationship support multiple relationships; otherwise, <c>false</c>.
        /// </value>
        public bool SupportMultipleRelationships
        {
            get
            {
                return this.supportMultipleRelationships;
            }

            set
            {
                if (this.supportMultipleRelationships != value)
                {
                    this.supportMultipleRelationships = value;
                    this.NotifyPropertyChanged("SupportMultipleRelationships");
                }
            }
        }

        /// <summary>
        /// Gets or sets the name of the constrain.
        /// </summary>
        /// <value>
        /// The name of the constrain.
        /// </value>
        public string ConstrainName
        {
            get
            {
                return this.constrainName;
            }

            set
            {
                if (this.constrainName != value)
                {
                    this.constrainName = value;
                    this.NotifyPropertyChanged("ConstrainName");
                }
            }
        }

        /// <summary>
        /// Gets or sets the name of the column.
        /// </summary>
        /// <value>
        /// The name of the column.
        /// </value>
        public string ColumnName
        {
            get
            {
                return this.columnName;
            }

            set
            {
                if (this.columnName != value)
                {
                    this.columnName = value;
                    this.NotifyPropertyChanged("ColumnName");
                }
            }
        }

        /// <summary>
        /// Gets or sets the client keys.
        /// </summary>
        /// <value>
        /// The client keys.
        /// </value>
        public ObservableCollection<ClientKey> ClientKeys
        {
            get
            {
                return this.clientKeys;
            }

            set
            {
                if (this.clientKeys != value)
                {
                    this.clientKeys = value;
                    this.NotifyPropertyChanged("ClientKeys");
                }
            }
        }

        /// <summary>
        /// Gets or sets the deletion rule.
        /// </summary>
        /// <value>
        /// The deletion rule.
        /// </value>
        public string DeletionRule
        {
            get
            {
                return this.deletionRule;
            }

            set
            {
                if (this.deletionRule != value)
                {
                    this.deletionRule = value;
                    this.NotifyPropertyChanged("DeletionRule");
                }
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Relationship"/> class.
        /// </summary>
        public EntityRelationshipModel() : base()
        {
            this.TargetTableName = "No Destination";//Resources.NoDestination;
            this.InverseRelationshipName = "No Inverse"; //Resources.NoInverse;
            this.ClientKeys = new ObservableCollection<ClientKey>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Relationship" /> class.
        /// </summary>
        /// <param name="copyRelationship">The copy relationship.</param>
        public EntityRelationshipModel(EntityRelationshipModel copyRelationship)
            : this()
        {
            this.Name = copyRelationship.Name;
            this.ColumnName = copyRelationship.ColumnName;
            this.ConstrainName = copyRelationship.ConstrainName;
            this.InverseRelationshipName = copyRelationship.InverseRelationshipName;
            this.SupportMultipleRelationships = copyRelationship.SupportMultipleRelationships;
            this.TargetTableName = copyRelationship.TargetTableName;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Relationship"/> class.
        /// </summary>
        /// <param name="xmlRelationship">The XML relationship.</param>
        public EntityRelationshipModel(XElement xmlRelationship)
            : this()
        {
            if (xmlRelationship != null)
            {
                this.FromXml(xmlRelationship);
            }
        } 

        /// <summary>
        /// Converts the current object to its XCode XML representation.
        /// </summary>
        /// <returns>
        /// An XML representation of the current object.
        /// </returns>
        public XElement ToXCD()
        {
            XElement relationship = new XElement("relationship");
            relationship.SetAttributeValue("optional", "YES");
            relationship.SetAttributeValue("syncable", "YES");
            relationship.SetAttributeValue("name", this.Name);
            relationship.SetAttributeValue("destinationEntity", this.TargetTableName);

            if (this.SupportMultipleRelationships)
            {
                relationship.SetAttributeValue("toMany", "YES");
                if (string.IsNullOrEmpty(this.DeletionRule))
                {
                    relationship.SetAttributeValue("deletionRule", "Cascade");
                }
            }
            else
            {
                relationship.SetAttributeValue("minCount", "1");
                relationship.SetAttributeValue("maxCount", "1");
                if (string.IsNullOrEmpty(this.DeletionRule))
                {
                    relationship.SetAttributeValue("deletionRule", "Nullify");
                }
            }

            if (!string.IsNullOrEmpty(this.DeletionRule))
            {
                relationship.SetAttributeValue("deletionRule", this.DeletionRule);
            }

            if (!string.IsNullOrEmpty(this.InverseRelationshipName) && !this.InverseRelationshipName.Equals("No Inverse"))
            {
                relationship.SetAttributeValue("inverseName", this.InverseRelationshipName);
                relationship.SetAttributeValue("inverseEntity", this.TargetTableName);
            }

            return relationship;
        }

        /// <summary>
        /// Converts the current object to its Mobise Studio XCode XML representation.
        /// </summary>
        /// <returns>
        /// An XML representation of the current object.
        /// </returns>
        public XElement ToRPXCD()
        {
            XElement relationship = this.ToXCD();
            foreach(var clientKey in this.ClientKeys)
            {
                relationship.Add(clientKey.ToRPXCD());
            }

            return relationship;
        }


        #region MobiseModel Base
        /// <summary>
        /// Returns the xml representation of the current object.
        /// </summary>
        /// <returns>
        /// An XElement object.
        /// </returns>
        public override XElement ToXml()
        {
            throw new NotImplementedException(); 
        }

        public override XElement ToXml(XElement baseElement, bool AddCommonAttributes)
        {
            throw new NotImplementedException();
        }

        public override void FromXml(XElement xmlRelationship)
        {
            if (xmlRelationship.HasAttributes)
            {
                this.Name = xmlRelationship.Attribute("name") != null ? xmlRelationship.Attribute("name").Value : string.Empty;
                this.TargetTableName = xmlRelationship.Attribute("destinationEntity") != null ? xmlRelationship.Attribute("destinationEntity").Value : string.Empty;
                this.SupportMultipleRelationships = xmlRelationship.Attribute("toMany") != null && xmlRelationship.Attribute("toMany").Value.ToUpperInvariant().Equals("YES") ? true : false;
                this.InverseRelationshipName = xmlRelationship.Attribute("inverseName") != null ? xmlRelationship.Attribute("inverseName").Value : string.Empty;
            }

            if (xmlRelationship.HasElements)
            {
                foreach (var key in xmlRelationship.Elements("clientKey"))
                {
                    this.ClientKeys.Add(new ClientKey(key));
                }
            }
        }


        #endregion
    }

    /// <summary>
    /// the class that will define the structure for the client key mappings in the relationship
    /// </summary>
    public class ClientKey : MobiseModel
    {
        /// <summary>
        /// The field name of the client key in the source table.
        /// </summary>
        string sourceName;

        /// <summary>
        /// The field name of the client key in the destination table.
        /// </summary>
        string destinationName;

        /// <summary>
        /// Gets or sets the source name.
        /// </summary>
        /// <value>The source name.</value>
        public string SourceName
        {
            get
            {
                return this.sourceName;
            }

            set
            {
                if (this.sourceName != value)
                {
                    this.sourceName = value;
                    this.NotifyPropertyChanged("SourceName");
                }
            }
        }

        /// <summary>
        /// Gets or sets the destination name.
        /// </summary>
        /// <value>The destination name.</value>
        public string DestinationName
        {
            get
            {
                return this.destinationName;
            }

            set
            {
                if (this.destinationName != value)
                {
                    this.destinationName = value;
                    this.NotifyPropertyChanged("DestinationName");
                }
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ClientKey"/> class.
        /// </summary>
        public ClientKey()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ClientKey"/> class.
        /// </summary>
        /// <param name="source">The name of the field in the source table.</param>
        /// <param name="destination">The name of the field in the destination table.</param>
        public ClientKey(string source, string destination)
        {
            this.SourceName = source;
            this.DestinationName = destination;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ClientKey" /> class.
        /// </summary>
        /// <param name="xmlClientKey">The XML clientKey.</param>
        public ClientKey(XElement xmlClientKey)
        {
            if (xmlClientKey != null)
            {
                this.ProcessXML(xmlClientKey);
            }
        }

        /// <summary>
        /// Processes the XML.
        /// </summary>
        /// <param name="xmlClientKey">The XML clientKey.</param>
        private void ProcessXML(XElement xmlClientKey)
        {
            if (xmlClientKey.HasAttributes)
            {
                this.SourceName = xmlClientKey.Attribute("sourceName") != null ? xmlClientKey.Attribute("sourceName").Value : string.Empty;
                this.DestinationName = xmlClientKey.Attribute("destinationName") != null ? xmlClientKey.Attribute("destinationName").Value : string.Empty;
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
            XElement clientKey = new XElement("clientKey");
            clientKey.SetAttributeValue("sourceName", this.SourceName);
            clientKey.SetAttributeValue("destinationName", this.DestinationName);
            return clientKey;
        }

        /// <summary>
        /// Determines whether the specified <see cref="System.Object" /> is equal to this instance.
        /// </summary>
        /// <param name="obj">The <see cref="System.Object" /> to compare with this instance.</param>
        /// <returns>
        ///   <c>true</c> if the specified <see cref="System.Object" /> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        public override bool Equals(object obj)
        {
            return this.SourceName == ((ClientKey)obj).SourceName && this.DestinationName == ((ClientKey)obj).DestinationName;
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
        /// </returns>
        public override int GetHashCode()
        {
            return this.SourceName.GetHashCode() + this.DestinationName.GetHashCode();
        }

        #region MobiseModel Base

        public override XElement ToXml()
        {
            XElement entity = new XElement("entity");
            entity.SetAttributeValue("name", this.Name);
            entity.SetAttributeValue("representedClassName", this.Name);
            entity.SetAttributeValue("syncable", "YES");
            return entity;
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