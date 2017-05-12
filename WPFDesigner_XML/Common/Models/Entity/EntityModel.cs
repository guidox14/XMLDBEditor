

namespace WPFDesigner_XML.Common.Models.Entity
{
    using System.Collections.Generic;
    using System.Xml.Linq;
    using System.Collections;
    using System.Windows.Data;
    using System;
    using System.Linq;
    using System.ComponentModel.DataAnnotations;
    using WPFDesigner_XML.Common.Models;
    using WPF.Common.Models.Entity;
    using Microsoft.XmlTemplateDesigner;

    /// <summary>
    /// The base class to handle the entities.
    /// </summary>
    public class EntityModel : MobiseModel
    {
        #region Fields

        /// <summary>
        /// The list of the entity current attributes.
        /// </summary>
        private List<EntityAttributeModel> attributes;

        /// <summary>
        /// The list of the entity current deleted attributes.
        /// </summary>
        private IList<EntityAttributeModel> deletedAttributes;

        /// <summary>
        /// The list of the entity current relationships.
        /// </summary>
        private List<EntityRelationshipModel> relationships;

        /// <summary>
        /// The list of the entity current deleted relationships.
        /// </summary>
        private IList<EntityRelationshipModel> deletedRelationships;

        /// <summary>
        /// Flag to mark this entity as a Root entity.
        /// </summary>
        private bool isRoot;

        /// <summary>
        /// Flag to mark this entity as a Root related.
        /// </summary>
        private bool isRootRelated;

        /// <summary>
        /// Flag to mark this entity to enable the tracing.
        /// </summary>
        private bool enableTracing;

        /// <summary>
        /// The name of the entity.
        /// </summary>
        private string name;

        /// <summary>
        /// The synchronization order of the entity
        /// </summary>
        private int syncOrder;

        private bool defaultProcess;

        /// <summary>
        /// The description of the entity
        /// </summary>
        private string description;

        /// <summary>
        /// The external references fileds
        /// </summary>
        private string externalReferenceFields;

        /// <summary>
        /// The is default this entity. So it can be modify by the user.
        /// </summary>
        private bool isDefault;

        /// <summary>
        /// Flag to mark this entity as a media entity.
        /// </summary>
        private bool isMediaEntity;

        /// <summary>
        /// The allow for thematic maps (Slicers)
        /// </summary>
        private bool allowForMaps;

        /// <summary>
        /// The friendly name
        /// </summary>
        private string friendlyName;

        /// <summary>
        /// The parent model
        /// </summary>
        protected DatabaseModel parentModel;

        /// <summary>
        /// Occurs when [conflic resolution rule changed].
        /// </summary>
        public event EventHandler ConflictResolutionRuleChanged;

        /// <summary>
        /// The conflict resolution rule.
        /// </summary>
        private ConflictResolutionRule conflictResolutionRule;

        /// <summary>
        /// Occurs when [sync type changed].
        /// </summary>
        public event EventHandler SyncTypeChanged;

        /// <summary>
        /// The synchronization direction type.
        /// </summary>
        private SyncType syncType;

        /// <summary>
        /// Query to obtain the data from the backend database
        /// </summary>
        private string backendQuery;

        #endregion

        #region Properties

        public IList Children
        {
            get
            {
                return new CompositeCollection()
            {
                new CollectionContainer() { Collection = Attributes },
                new CollectionContainer() { Collection = Relationships }
            };
            }
        }

        /// <summary>
        /// Gets or sets the attributes.
        /// </summary>
        /// <value>The attributes.</value>
        public List<EntityAttributeModel> Attributes
        {
            get
            {
                return this.attributes;
            }

            set
            {
                if (this.attributes != value)
                {
                    this.attributes = value;

                    // Make sure to update the parent entity for each attribute.
                    foreach (var attr in this.attributes)
                    {
                        attr.ParentEntity = this;
                    }

                    this.NotifyPropertyChanged("Attributes");
                }
            }
        }

        /// <summary>
        /// Gets or sets the attributes.
        /// </summary>
        /// <value>The attributes.</value>
        public IList<EntityAttributeModel> DeletedAttributes
        {
            get
            {
                return this.deletedAttributes;
            }

             set
            {
                if (this.deletedAttributes != value)
                {
                    this.deletedAttributes = value;
                    this.NotifyPropertyChanged("DeletedAttributes");
                }
            }
        }

        /// <summary>
        /// Gets or sets the relationships.
        /// </summary>
        /// <value>The relationships.</value>
        public List<EntityRelationshipModel> Relationships
        {
            get
            {
                return this.relationships;
            }

             set
            {
                if (this.relationships != value)
                {
                    this.relationships = value;
                    this.NotifyPropertyChanged("Relationships");
                }
            }
        }

        /// <summary>
        /// Gets or sets the deleted relationships.
        /// </summary>
        /// <value>
        /// The deleted relationships.
        /// </value>
        public IList<EntityRelationshipModel> DeletedRelationships
        {
            get
            {
                return this.deletedRelationships;
            }

             set
            {
                if (this.deletedRelationships != value)
                {
                    this.deletedRelationships = value;
                    this.NotifyPropertyChanged("DeletedRelationships");
                }
            }
        }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>The name.</value>
        [Required]
        public string Name
        {
            get
            {
                return this.name;
            }

            set
            {
                if (this.name != value && !this.IsDefault)
                {
                    this.name = value;
                    this.ClearErrors("Name");
                    string firstLetter = value.Substring(0, 1);
                    if (string.IsNullOrEmpty(value) || firstLetter == firstLetter.ToLowerInvariant())
                    {
                        this.AddError("Name", "Invalid entity name. It can't be empty, or start with lower case");
                    }
                }
                this.NotifyPropertyChanged("Name");
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is root.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is root; otherwise, <c>false</c>.
        /// </value>
        public bool IsRoot
        {
            get
            {
                return this.isRoot;
            }

            set
            {
                if (this.isRoot != value)
                {
                    EntityModel entityRoot = parentModel.Entities.Where(e => e.IsRoot == true && e.Name != this.Name).FirstOrDefault();
                    if (entityRoot != null)
                    {
                        //throw new Exception();
                        this.AddError("IsRoot", "There is already a root entity");
                    }
                    else
                    {
                       // if (ApplicationController.ApplicationMainController.CurrentModel != null)
                        {
                            if (this.IsRoot)
                            {
                                parentModel.EntitiesRootCounter -= 1;
                            }
                            else
                            {
                                parentModel.EntitiesRootCounter += 1;
                            }
                        }

                        this.isRoot = value;
                    }

                    this.NotifyPropertyChanged("IsRoot");
                }
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this entity needs enabled the tracing.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this entity has enabled the tracing; otherwise, <c>false</c>.
        /// </value>
        public bool EnableTracing
        {
            get
            {
                return this.enableTracing;
            }
            set
            {
                if (this.enableTracing != value)
                {
                    this.enableTracing = value;
                    this.NotifyPropertyChanged("EnableTracing");
                }
            }
        }

        /// <summary>
        /// Gets or sets the synchronization order number.
        /// </summary>
        /// <value>
        /// The synchronizatio order.
        /// </value>
        public int SyncOrder
        {
            get
            {
                return this.syncOrder;
            }

            set
            {
                if (this.syncOrder != value)
                {
                    this.syncOrder = value;
                    this.NotifyPropertyChanged("SyncOrder");
                }
            }
        }

        public bool DefaultProcess
        {
            get
            {
                return this.defaultProcess;
            }

            set
            {
                if (this.defaultProcess != value)
                {
                    this.defaultProcess = value;
                    this.NotifyPropertyChanged("DefaultProcess");
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
        /// Gets or sets the external reference fields.
        /// </summary>
        /// <value>
        /// The external reference fields.
        /// </value>
        public string ExternalReferenceFields
        {
            get 
            {
                return externalReferenceFields;
            }

            set 
            {
                if (!this.IsDefault)
                {
                    this.externalReferenceFields = value;
                    this.PropertyErrors["ExternalReferenceFields"] = null;
                    if (!string.IsNullOrEmpty(value))
                    {
                        string[] fields = value.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                        foreach (string field in fields)
                        {
                            if (this.attributes.Count(a => a.Name.Equals(field, StringComparison.OrdinalIgnoreCase)) == 0)
                            {
                                this.AddError("ExternalReferenceFields", string.Format("The field {0} don't exist in the entity. \r\n", field));
                            }
                        }
                    }
                }

                this.NotifyPropertyChanged("ExternalReferenceFields");
            }
        }

        /// <summary>
        /// Gets a value indicating whether this instance is default.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is default; otherwise, <c>false</c>.
        /// </value>
        public bool IsDefault
        {
            get
            {
                return this.isDefault;
            }
            private set
            {
                this.isDefault = value;
                this.NotifyPropertyChanged("IsDefault");
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is media entity.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is media entity; otherwise, <c>false</c>.
        /// </value>
        public bool IsMediaEntity
        {
            get
            {
                return this.isMediaEntity;
            }

            set
            {
                if (!this.IsDefault)
                {
                    if (this.isMediaEntity != value)
                    {
                        this.isMediaEntity = value;
                    }
                }
                this.NotifyPropertyChanged("IsMediaEntity");
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is related with the root entity.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is related with the root entity; otherwise, <c>false</c>.
        /// </value>
        public bool IsRootRelated
        {
            get
            {
                return this.isRootRelated;
            }

            set
            {
                if (this.isRootRelated != value)
                {
                    this.isRootRelated = value;
                    this.NotifyPropertyChanged("IsRootRelated");
                }
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether [allow for maps] (Slicers).
        /// </summary>
        /// <value>
        ///   <c>true</c> if [allow for maps]; otherwise, <c>false</c>.
        /// </value>
        public bool AllowForMaps
        {
            get
            {
                return this.allowForMaps;
            }
            set
            {
                this.allowForMaps = value;
                this.NotifyPropertyChanged("AllowForMaps");
            }
        }

        /// <summary>
        /// Gets or sets the name of the friendly.
        /// </summary>
        /// <value>
        /// The name of the friendly.
        /// </value>
        public string FriendlyName
        {
            get
            {
                return this.friendlyName;
            }
            set
            {
                this.friendlyName = value;
                this.NotifyPropertyChanged("FriendlyName");
            }
        }

        /// <summary>
        /// Gets or sets the conflict resolution rule.
        /// </summary>
        /// <value>
        /// The conflict resolution rule.
        /// </value>
        public ConflictResolutionRule ConflictResolutionRule
        {
            get
            {
                return this.conflictResolutionRule;
            }

            set
            {
                if (this.conflictResolutionRule != value)
                {
                    this.conflictResolutionRule = value;
                    this.NotifyPropertyChanged("ConflictResolutionRule");
                    if (this.ConflictResolutionRuleChanged != null)
                    {
                        this.ConflictResolutionRuleChanged(this, new EventArgs());
                    }
                }
            }
        }

        /// <summary>
        /// Gets or sets the type of the sync.
        /// </summary>
        /// <value>
        /// The type of the sync.
        /// </value>
        public SyncType SyncType
        {
            get
            {
                return this.syncType;
            }

            set
            {
                if (this.syncType != value)
                {
                    this.syncType = value;
                    this.NotifyPropertyChanged("SyncType");
                    if (this.SyncTypeChanged != null)
                    {
                        this.SyncTypeChanged(this, new EventArgs());
                    }
                }
            }
        }

        public bool isMapped { get; set; }

        /// <summary>
        /// Gets or sets the backend query.
        /// </summary>
        /// <value>
        /// The backend query.
        /// </value>
        public string BackendQuery
        {
            get
            {
                return this.backendQuery;
            }
            set
            {
                this.backendQuery = value;
                this.NotifyPropertyChanged("BackendQuery");
            }
        }

        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseEntity"/> class.
        /// </summary>
        public EntityModel(DatabaseModel parent)
        {
            this.Attributes = new List<EntityAttributeModel>();
            this.DeletedAttributes = new List<EntityAttributeModel>();
            this.Relationships = new List<EntityRelationshipModel>();
            this.DeletedRelationships = new List<EntityRelationshipModel>();
            this.IsRoot = false;
            this.IsRootRelated = false;
            this.EnableTracing = false;
            this.SyncOrder = 0;
            this.DefaultProcess = true;
            this.Description = string.Empty;
            this.IsMediaEntity = false;
            this.ConflictResolutionRule = ConflictResolutionRule.mbSyncWin;
            this.SyncType = SyncType.syncBothDirections;
            this.parentModel = parent;
            this.BackendQuery = string.Empty;
            this.CreateDefaultAttributes();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseEntity"/> class.
        /// </summary>
        /// <param name="xmlEntity">The XML entity.</param>
        public EntityModel(XElement xmlEntity, DatabaseModel parent)
            : this(parent)
        {
            this.Attributes = new List<EntityAttributeModel>();
            this.DeletedAttributes = new List<EntityAttributeModel>();
            this.Relationships = new List<EntityRelationshipModel>();
            this.DeletedRelationships = new List<EntityRelationshipModel>();
            this.IsRoot = false;
            this.IsRootRelated = false;
            this.EnableTracing = false;
            this.SyncOrder = 0;
            this.DefaultProcess = true;
            this.Description = string.Empty;
            this.IsMediaEntity = false;
            this.ConflictResolutionRule = ConflictResolutionRule.mbSyncWin;
            this.SyncType = SyncType.syncBothDirections;
            this.BackendQuery = string.Empty;
            if (xmlEntity != null)
            {
                this.ProcessXml(xmlEntity);
            }

            this.CreateDefaultAttributes();
        }

        /// <summary>
        /// Creates the default attributes for a new entity.
        /// </summary>
        public void CreateDefaultAttributes()
        {
            if (!this.Attributes.Any(a => a.Name == "guid_mb"))
            {
                this.Attributes.Add(new EntityAttributeModel(this) { AttributeType = AttributeType.GUID, Name = "guid_mb", AttributeInfo = new AttributeInfo() { IsIndexed = true, IsPrimaryKey = true }, IsDefault = true });
            }

            if (!this.Attributes.Any(a => a.Name == "area_mb"))
            {
                this.Attributes.Add(new EntityAttributeModel(this) { AttributeType = AttributeType.Integer32, Name = "area_mb", AttributeInfo = new AttributeInfoInteger() { IsIndexed = true }, IsDefault = true });
            }

            if (!this.Attributes.Any(a => a.Name == "rowStatus_mb"))
            {
                this.Attributes.Add(new EntityAttributeModel(this) { AttributeType = AttributeType.String, Name = "rowStatus_mb", AttributeInfo = new AttributeInfoString() { IsIndexed = true }, IsDefault = true });
            }

            if (!this.Attributes.Any(a => a.Name == "errMsg_mb"))
            {
                this.Attributes.Add(new EntityAttributeModel(this) { AttributeType = AttributeType.String, Name = "errMsg_mb", AttributeInfo = new AttributeInfoString() { IsIndexed = false, MaxChars = 4000 }, IsDefault = true });
            }
        }

        /// <summary>
        /// Deletes the attribute.
        /// </summary>
        /// <param name="attribute">The attribute.</param>
        public void DeleteAttribute(EntityAttributeModel attribute)
        {
            this.Attributes.Remove(attribute);
            this.DeletedAttributes.Add(attribute);
        }

        /// <summary>
        /// Deletes the relationship.
        /// </summary>
        /// <param name="relationship">The relationship.</param>
        public void DeleteRelationship(EntityRelationshipModel relationship)
        {
            this.Relationships.Remove(relationship);
            this.DeletedRelationships.Add(new EntityRelationshipModel(relationship));
        }

        /// <summary>
        /// Converts the current object to its XCode XML representation.
        /// </summary>
        /// <returns>
        /// An XML representation of the current object.
        /// </returns>
        public XElement ToXCD()
        {
            XElement entity = this.ToXml();

            foreach (var attr in this.Attributes)
            {
                if (attr != null && attr.AttributeInfo != null && attr.AttributeInfo.PresentInSQLiteDB)
                {
                    entity.Add(attr.ToXCD());
                }
            }

            foreach (var rel in this.Relationships)
            {
                if (!rel.SupportMultipleRelationships)
                {
                    entity.Add(CreateRelationAttribute(rel.Name));
                }

                entity.Add(rel.ToXCD());
            }

            return entity;
        }

        /// <summary>
        /// Creates the attribute for the relation.
        /// </summary>
        /// <param name="relationName">Name of the relation.</param>
        /// <returns>The XElement with the attribute generated</returns>
        private XElement CreateRelationAttribute(string relationName)
        {
            XElement relationAttr = new XElement("attribute");
            relationAttr.SetAttributeValue("optional", "YES");
            relationAttr.SetAttributeValue("syncable", "YES");
            relationAttr.SetAttributeValue("name", "fk_" + relationName);
            relationAttr.SetAttributeValue("attributeType", "String");
            relationAttr.SetAttributeValue("indexed", "YES");
            return relationAttr;
        }

        /// <summary>
        /// Converts the current object to its Mobise Studio XCode XML representation.
        /// </summary>
        /// <returns>
        /// An XML representation of the current object.
        /// </returns>
        public XElement ToRPXCD()
        {
            XElement entity = this.ToXml();
            entity.SetAttributeValue("isRoot", this.IsRoot);
            entity.SetAttributeValue("syncOrder", this.SyncOrder);
            entity.SetAttributeValue("defaultProcess", this.DefaultProcess);
            entity.SetAttributeValue("isDefault", this.IsDefault);
            entity.SetAttributeValue("externalReferences", this.ExternalReferenceFields);
            entity.SetAttributeValue("isMediaEntity", this.IsMediaEntity);
            entity.SetAttributeValue("conflictResolutionRule", this.ConflictResolutionRule);
            entity.SetAttributeValue("syncType", this.SyncType);
            entity.SetAttributeValue("description", this.Description);
            entity.SetAttributeValue("friendlyName", this.FriendlyName);
            entity.SetAttributeValue("allowForMaps", this.AllowForMaps);
            entity.SetAttributeValue("isRootRelated", this.IsRootRelated);
            entity.SetAttributeValue("enableTracing", this.EnableTracing);
            entity.SetAttributeValue("backendQuery", this.BackendQuery);
            foreach (var attr in this.Attributes)
            {
                entity.Add(attr.ToRPXCD());
            }

            foreach (var rel in this.Relationships)
            {
                entity.Add(rel.ToRPXCD());
            }

            return entity;
        }
         

        /// <summary>
        /// Gets the valid conflict resolution rule from string.
        /// </summary>
        /// <param name="rule">The rule.</param>
        /// <returns></returns>
        public static ConflictResolutionRule GetValidConflictResolutionRuleFromString(string rule)
        {
            ConflictResolutionRule conflictResolutionRule; 

            try
            {
                conflictResolutionRule = (ConflictResolutionRule)Enum.Parse(typeof(ConflictResolutionRule), rule);
            }
            catch
            {
                conflictResolutionRule = ConflictResolutionRule.mbSyncWin;
            }

            return conflictResolutionRule;
        }

        /// <summary>
        /// Gets the valid sync type from string.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        public static SyncType GetValidSyncTypeFromString(string type)
        {
            SyncType syncType;

            try
            {
                syncType = (SyncType)Enum.Parse(typeof(SyncType), type);
            }
            catch
            {
                syncType = SyncType.syncBothDirections;
            }

            return syncType;
        }

        /// <summary>
        /// Processes the XML.
        /// </summary>
        /// <param name="xmlEntity">The XML entity.</param>
        private void ProcessXml(XElement xmlEntity)
        {
            if (xmlEntity.HasAttributes)
            {
                this.Name = xmlEntity.Attribute("name") != null ? xmlEntity.Attribute("name").Value : string.Empty;
                this.IsRoot = xmlEntity.Attribute("isRoot") != null ? Convert.ToBoolean(xmlEntity.Attribute("isRoot").Value) : false;
                this.IsDefault = xmlEntity.Attribute("isDefault") != null ? Convert.ToBoolean(xmlEntity.Attribute("isDefault").Value) : false;
                this.SyncOrder = xmlEntity.Attribute("syncOrder") != null ? Convert.ToInt32(xmlEntity.Attribute("syncOrder").Value) : 0;
                this.DefaultProcess = xmlEntity.Attribute("defaultProcess") != null ? Convert.ToBoolean(xmlEntity.Attribute("defaultProcess").Value) : true;
                this.Description = xmlEntity.Attribute("description") != null ? xmlEntity.Attribute("description").Value : string.Empty;
                this.ExternalReferenceFields = xmlEntity.Attribute("externalReferences") != null ? xmlEntity.Attribute("externalReferences").Value : string.Empty;
                this.IsMediaEntity = xmlEntity.Attribute("isMediaEntity") != null ? Convert.ToBoolean(xmlEntity.Attribute("isMediaEntity").Value) : false;
                this.ConflictResolutionRule = xmlEntity.Attribute("conflictResolutionRule") != null ? EntityModel.GetValidConflictResolutionRuleFromString(xmlEntity.Attribute("conflictResolutionRule").Value) : ConflictResolutionRule.mbSyncWin;
                this.SyncType = xmlEntity.Attribute("syncType") != null ? EntityModel.GetValidSyncTypeFromString(xmlEntity.Attribute("syncType").Value) : SyncType.syncBothDirections;
                this.AllowForMaps = xmlEntity.Attribute("allowForMaps") != null ? Convert.ToBoolean(xmlEntity.Attribute("allowForMaps").Value) : false;
                this.FriendlyName = xmlEntity.Attribute("friendlyName") != null ? xmlEntity.Attribute("friendlyName").Value : string.Empty;
                this.IsRootRelated = xmlEntity.Attribute("isRootRelated") != null ? Convert.ToBoolean(xmlEntity.Attribute("isRootRelated").Value) : false;
                this.EnableTracing = xmlEntity.Attribute("enableTracing") != null ? Convert.ToBoolean(xmlEntity.Attribute("enableTracing").Value) : false;
                this.BackendQuery = xmlEntity.Attribute("backendQuery") != null ? xmlEntity.Attribute("backendQuery").Value : string.Empty;
            }

            if (xmlEntity.HasElements)
            {
                foreach (var ele in xmlEntity.Elements("attribute"))
                {
                    this.Attributes.Add(new EntityAttributeModel(ele, this));
                    
                }
                
                foreach (var ele in xmlEntity.Elements("relationship"))
                {
                    this.Relationships.Add(new EntityRelationshipModel(ele));
                }
            }
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
            this.ProcessXml(xml);
        }


        #endregion
    }


    /// <summary>
    /// Conflict Resolution Rule options
    /// </summary>
    public enum ConflictResolutionRule
    {

        /// <summary>
        /// The conflict resolution MBSync Win Rule
        /// </summary>
        [LocalizableDescription(@"MBSyncWin", typeof(Resources))]
        mbSyncWin = 0,

        /// <summary>
        /// The conflict resolution Production Win Rule
        /// </summary>
        [LocalizableDescription(@"ProductionWin", typeof(Resources))]
        productionWin = 1
    }

    /// <summary>
    /// Sync Direction Type options
    /// </summary>
    public enum SyncType
    {

        /// <summary>
        /// Sync to both directions type
        /// </summary>
        [LocalizableDescription(@"SyncBothDirections", typeof(Resources))]
        syncBothDirections = 0,

        /// <summary>
        /// Sync to device only type
        /// </summary>
        [LocalizableDescription(@"SyncToDevice", typeof(Resources))]
        syncToDevice = 1,

        /// <summary>
        /// Sync to middler tier only type
        /// </summary>
        [LocalizableDescription(@"SyncToMiddleTier", typeof(Resources))]
        syncToMiddleTier = 2
    }
}
