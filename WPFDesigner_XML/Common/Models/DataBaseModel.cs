using System;
using System.Collections.Generic;
using System.Xml.Linq;
using WPF.Common.Models.Entity;
using WPFDesigner_XML.Common.DynamicContext;
using WPFDesigner_XML.Common.Models.Entity;

namespace WPFDesigner_XML.Common.Models
{

    public class DatabaseModel : FileSystemObjectModel
    {
        #region Constants

        /// <summary>
        /// A default value for the system version.
        /// </summary>
        private const string SYSVERSION = "11D50";

        /// <summary>
        /// A default value for the tools version.
        /// </summary>
        private const string TOOLSVERSION = "1171";

        #endregion

        #region fields
        /// <summary>
        /// The OS system version.
        /// </summary>
        private string systemVersion;

        /// <summary>
        /// The last saved tools version.
        /// </summary>
        private string lastSavedToolsVersion;

        /// <summary>
        /// The list of entities.
        /// </summary>
        private List<EntityModel> entities;

        /// <summary>
        /// The list of look up tables.
        /// </summary>
        private IList<LookupTableModel> lookupTables;

        /// <summary>
        /// The list of associated databases.
        /// </summary>
        private IList<DatabaseConnectionModel> databases;

        /// <summary>
        /// Version of the database
        /// </summary>
        private string databaseVersion;
        #endregion

        public DatabaseModel(ProjectModel project) : base(project)
        {
            this.SystemVersion = SYSVERSION;
            this.LastSavedToolsVersion = TOOLSVERSION;
            this.Entities = new List<EntityModel>();
            this.LookupTables = new List<LookupTableModel>();
            this.Databases = new List<DatabaseConnectionModel>();
            this.EntitiesRootCounter = 0;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Model"/> class.
        /// </summary>
        /// <param name="xmlModel">The XML model.</param>
          public DatabaseModel(XElement xmlModel, ProjectModel project)
            : this(project)
        {
            if (xmlModel != null)
            {
                this.FromXml(xmlModel);
            }
        }

        #region Properties

        public EntityModel SelectedEntity { get; set; }

        /// <summary>
        /// Gets or sets the entities root counter that controls the quantity of entities that are checked as root.
        /// </summary>
        /// <value>
        /// The entities root counter.
        /// </value>
        public int EntitiesRootCounter { get; set; }

        /// <summary>
        /// Gets or sets the system version.
        /// </summary>
        /// <value>The system version.</value>
        public string SystemVersion
        {
            get
            {
                return this.systemVersion;
            }

            set
            {
                if (this.systemVersion != value)
                {
                    this.systemVersion = value;
                    this.NotifyPropertyChanged("SystemVersion");
                }
            }
        }

        /// <summary>
        /// Gets or sets the last saved tools version.
        /// </summary>
        /// <value>The last saved tools version.</value>
        public string LastSavedToolsVersion
        {
            get
            {
                return this.lastSavedToolsVersion;
            }

            set
            {
                if (this.lastSavedToolsVersion != value)
                {
                    this.lastSavedToolsVersion = value;
                    this.NotifyPropertyChanged("LastSavedToolsVersion");
                }
            }
        }

        /// <summary>
        /// Gets or sets the entities.
        /// </summary>
        /// <value>The entities.</value>
        public List<EntityModel> Entities
        {
            get
            {
                return this.entities;
            }

            internal set
            {
                if (this.entities != value)
                {
                    this.entities = value;
                    this.NotifyPropertyChanged("Entities");
                }
            }
        }

        /// <summary>
        /// Gets or sets the lookup tables.
        /// </summary>
        /// <value>The lookup tables.</value>
        public IList<LookupTableModel> LookupTables
        {
            get
            {
                return this.lookupTables;
            }

            internal set
            {
                if (this.lookupTables != value)
                {
                    this.lookupTables = value;
                    this.NotifyPropertyChanged("LookupTables");
                }
            }
        }

        /// <summary>
        /// Gets or sets the databases.
        /// </summary>
        /// <value>
        /// The databases.
        /// </value>
        public IList<DatabaseConnectionModel> Databases
        {
            get
            {
                return this.databases;
            }

            internal set
            {
                if (this.databases != value)
                {
                    this.databases = value;
                    this.NotifyPropertyChanged("Databases");
                }
            }
        }

        /// <summary>
        /// Gets or sets the database version.
        /// </summary>
        /// <value>
        /// The database version.
        /// </value>
        public string DatabaseVersion
        {
            get
            {
                return this.databaseVersion;
            }
            set
            {
                if (this.databaseVersion != value)
                {
                    this.databaseVersion = value;
                    this.NotifyPropertyChanged("DatabaseVersion");
                }
            }
        }

        #endregion

        #region Helpers
        private List<EntityModel> CreateDefaultEntities()
        {
            List<EntityModel> defaultEntList = new List<EntityModel>();

            //Continue in attribute24 and relationship7

            #region BlobFile_mb

            EntityModel blobFileEntity = new EntityModel(this);
            blobFileEntity.Name = "BlobFile_mb";
            blobFileEntity.Attributes.Clear();

            EntityAttributeModel attribute1 = new EntityAttributeModel(blobFileEntity);
            attribute1.Name = "blob_mb";
            attribute1.AttributeType = AttributeType.BLOB;
            attribute1.AttributeInfo = new AttributeInfo();
            blobFileEntity.Attributes.Add(attribute1);

            EntityRelationshipModel relationship1 = new EntityRelationshipModel();
            relationship1.Name = "blobInfo_mb";
            relationship1.InverseRelationshipName = "blobFile_mb";
            relationship1.TargetTableName = "BlobInfo_mb";
            relationship1.SupportMultipleRelationships = false;
            blobFileEntity.Relationships.Add(relationship1);

            EntityRelationshipModel relationship6 = new EntityRelationshipModel();
            relationship6.Name = "blobInfoSmall_mb";
            relationship6.InverseRelationshipName = "smallBlobFile_mb";
            relationship6.TargetTableName = "BlobInfo_mb";
            relationship6.SupportMultipleRelationships = false;
            blobFileEntity.Relationships.Add(relationship6);

            defaultEntList.Add(blobFileEntity);

            #endregion

            #region BlobInfo_mb

            EntityModel blobInfoEntity = new EntityModel(this);
            blobInfoEntity.Name = "BlobInfo_mb";
            blobInfoEntity.Attributes.Clear();

            EntityAttributeModel attribute2 = new EntityAttributeModel(blobInfoEntity);
            attribute2.Name = "blobStatus_mb";
            attribute2.AttributeType = AttributeType.Integer16;
            attribute2.AttributeInfo = new AttributeInfoInteger();
            blobInfoEntity.Attributes.Add(attribute2);

            EntityAttributeModel attribute3 = new EntityAttributeModel(blobInfoEntity);
            attribute3.Name = "errorMsg_mb";
            attribute3.AttributeType = AttributeType.String;
            attribute3.AttributeInfo = new AttributeInfoString();
            blobInfoEntity.Attributes.Add(attribute3);

            EntityAttributeModel attribute4 = new EntityAttributeModel(blobInfoEntity);
            attribute4.Name = "guid_mb";
            attribute4.AttributeType = AttributeType.String;
            attribute4.AttributeInfo = new AttributeInfoString();
            attribute4.AttributeInfo.IsIndexed = true;
            blobInfoEntity.Attributes.Add(attribute4);

            EntityAttributeModel attribute20 = new EntityAttributeModel(blobInfoEntity);
            attribute20.Name = "location_mb";
            attribute20.AttributeType = AttributeType.String;
            attribute20.AttributeInfo = new AttributeInfoString();
            blobInfoEntity.Attributes.Add(attribute20);

            EntityRelationshipModel relationship2 = new EntityRelationshipModel();
            relationship2.Name = "blobFile_mb";
            relationship2.InverseRelationshipName = "blobInfo_mb";
            relationship2.TargetTableName = "BlobFile_mb";
            relationship2.SupportMultipleRelationships = false;
            relationship2.DeletionRule = "Cascade";
            blobInfoEntity.Relationships.Add(relationship2);

            EntityRelationshipModel relationship5 = new EntityRelationshipModel();
            relationship5.Name = "smallBlobFile_mb";
            relationship5.InverseRelationshipName = "blobInfoSmall_mb";
            relationship5.TargetTableName = "BlobFile_mb";
            relationship5.SupportMultipleRelationships = false;
            relationship5.DeletionRule = "Cascade";
            blobInfoEntity.Relationships.Add(relationship5);

            defaultEntList.Add(blobInfoEntity);

            #endregion

            #region ConflictError_mb

            EntityModel conflictErrorEntity = new EntityModel(this);
            conflictErrorEntity.Name = "ConflictError_mb";
            conflictErrorEntity.Attributes.Clear();

            EntityAttributeModel attribute5 = new EntityAttributeModel(conflictErrorEntity);
            attribute5.Name = "entityGuid_mb";
            attribute5.AttributeType = AttributeType.String;
            attribute5.AttributeInfo = new AttributeInfoString();
            attribute5.AttributeInfo.IsIndexed = true;
            conflictErrorEntity.Attributes.Add(attribute5);

            EntityAttributeModel attribute6 = new EntityAttributeModel(conflictErrorEntity);
            attribute6.Name = "entityKind_mb";
            attribute6.AttributeType = AttributeType.String;
            attribute6.AttributeInfo = new AttributeInfoString();
            attribute6.AttributeInfo.IsIndexed = true;
            conflictErrorEntity.Attributes.Add(attribute6);

            EntityAttributeModel attribute7 = new EntityAttributeModel(conflictErrorEntity);
            attribute7.Name = "modifiedFields_mb";
            attribute7.AttributeType = AttributeType.String;
            attribute7.AttributeInfo = new AttributeInfoString();
            conflictErrorEntity.Attributes.Add(attribute7);

            defaultEntList.Add(conflictErrorEntity);

            #endregion

            #region DeviceNotification_mb

            EntityModel deviceNotificationEntity = new EntityModel(this);
            deviceNotificationEntity.Name = "DeviceNotification_mb";
            deviceNotificationEntity.Attributes.Clear();

            EntityAttributeModel attribute8 = new EntityAttributeModel(deviceNotificationEntity);
            attribute8.Name = "deviceId_mb";
            attribute8.AttributeType = AttributeType.String;
            attribute8.AttributeInfo = new AttributeInfoString();
            deviceNotificationEntity.Attributes.Add(attribute8);

            EntityAttributeModel attribute9 = new EntityAttributeModel(deviceNotificationEntity);
            attribute9.Name = "isNewToken_mb";
            attribute9.AttributeType = AttributeType.Boolean;
            attribute9.AttributeInfo = new AttributeInfo();
            deviceNotificationEntity.Attributes.Add(attribute9);

            EntityAttributeModel attribute10 = new EntityAttributeModel(deviceNotificationEntity);
            attribute10.Name = "token_mb";
            attribute10.AttributeType = AttributeType.String;
            attribute10.AttributeInfo = new AttributeInfoString();
            deviceNotificationEntity.Attributes.Add(attribute10);

            defaultEntList.Add(deviceNotificationEntity);

            #endregion

            #region DownloadSyncChunk_mb

            EntityModel downloadSyncChunkEntity = new EntityModel(this);
            downloadSyncChunkEntity.Name = "DownloadSyncChunk_mb";
            downloadSyncChunkEntity.Attributes.Clear();

            EntityAttributeModel attribute11 = new EntityAttributeModel(downloadSyncChunkEntity);
            attribute11.Name = "downloadId_mb";
            attribute11.AttributeType = AttributeType.Integer64;
            attribute11.AttributeInfo = new AttributeInfoInteger();
            attribute11.AttributeInfo.IsIndexed = true;
            downloadSyncChunkEntity.Attributes.Add(attribute11);

            EntityAttributeModel attribute12 = new EntityAttributeModel(downloadSyncChunkEntity);
            attribute12.Name = "guid_mb";
            attribute12.AttributeType = AttributeType.String;
            attribute12.AttributeInfo = new AttributeInfoString();
            attribute12.AttributeInfo.IsIndexed = true;
            downloadSyncChunkEntity.Attributes.Add(attribute12);

            EntityAttributeModel attribute13 = new EntityAttributeModel(downloadSyncChunkEntity);
            attribute13.Name = "jsonMessage_mb";
            attribute13.AttributeType = AttributeType.BLOB;
            attribute13.AttributeInfo = new AttributeInfoString();
            downloadSyncChunkEntity.Attributes.Add(attribute13);

            EntityAttributeModel attribute23 = new EntityAttributeModel(downloadSyncChunkEntity);
            attribute23.Name = "chunkOrder";
            attribute23.AttributeType = AttributeType.Integer32;
            attribute23.AttributeInfo = new AttributeInfoInteger();
            downloadSyncChunkEntity.Attributes.Add(attribute23);

            EntityRelationshipModel relationship3 = new EntityRelationshipModel();
            relationship3.Name = "downloadSyncHeader_mb";
            relationship3.InverseRelationshipName = "downloadSyncChunk_mb";
            relationship3.TargetTableName = "DownloadSyncHeader_mb";
            relationship3.SupportMultipleRelationships = false;
            downloadSyncChunkEntity.Relationships.Add(relationship3);

            defaultEntList.Add(downloadSyncChunkEntity);

            #endregion

            #region DownloadSyncHeader_mb

            EntityModel downloadSyncHeaderEntity = new EntityModel(this);
            downloadSyncHeaderEntity.Name = "DownloadSyncHeader_mb";
            downloadSyncHeaderEntity.Attributes.Clear();

            EntityAttributeModel attribute14 = new EntityAttributeModel(downloadSyncHeaderEntity);
            attribute14.Name = "downloadedEntities_mb";
            attribute14.AttributeType = AttributeType.Integer32;
            attribute14.AttributeInfo = new AttributeInfoInteger();
            downloadSyncHeaderEntity.Attributes.Add(attribute14);

            EntityAttributeModel attribute15 = new EntityAttributeModel(downloadSyncHeaderEntity);
            attribute15.Name = "downloadId_mb";
            attribute15.AttributeType = AttributeType.Integer64;
            attribute15.AttributeInfo = new AttributeInfoInteger();
            downloadSyncHeaderEntity.Attributes.Add(attribute15);

            EntityAttributeModel attribute16 = new EntityAttributeModel(downloadSyncHeaderEntity);
            attribute16.Name = "numRecs_mb";
            attribute16.AttributeType = AttributeType.Integer32;
            attribute16.AttributeInfo = new AttributeInfoInteger();
            downloadSyncHeaderEntity.Attributes.Add(attribute16);

            EntityAttributeModel attribute17 = new EntityAttributeModel(downloadSyncHeaderEntity);
            attribute17.Name = "sourceTable_mb";
            attribute17.AttributeType = AttributeType.String;
            attribute17.AttributeInfo = new AttributeInfoString();
            downloadSyncHeaderEntity.Attributes.Add(attribute17);

            EntityAttributeModel attribute18 = new EntityAttributeModel(downloadSyncHeaderEntity);
            attribute18.Name = "syncDate_mb";
            attribute18.AttributeType = AttributeType.Double;
            attribute18.AttributeInfo = new AttributeInfoDouble();
            downloadSyncHeaderEntity.Attributes.Add(attribute18);

            EntityAttributeModel attribute19 = new EntityAttributeModel(downloadSyncHeaderEntity);
            attribute19.Name = "syncOrder_mb";
            attribute19.AttributeType = AttributeType.Integer32;
            attribute19.AttributeInfo = new AttributeInfoInteger();
            downloadSyncHeaderEntity.Attributes.Add(attribute19);

            EntityAttributeModel attribute22 = new EntityAttributeModel(downloadSyncHeaderEntity);
            attribute22.Name = "guid_mb";
            attribute22.AttributeType = AttributeType.String;
            attribute22.AttributeInfo = new AttributeInfoString();
            attribute22.AttributeInfo.IsIndexed = true;
            downloadSyncHeaderEntity.Attributes.Add(attribute22);

            attribute22 = new EntityAttributeModel(downloadSyncHeaderEntity);
            attribute22.Name = "syncType_mb";
            attribute22.AttributeType = AttributeType.Integer32;
            attribute22.AttributeInfo = new AttributeInfoInteger();
            attribute22.AttributeInfo.IsIndexed = true;
            downloadSyncHeaderEntity.Attributes.Add(attribute22);

            EntityRelationshipModel relationship4 = new EntityRelationshipModel();
            relationship4.Name = "downloadSyncChunk_mb";
            relationship4.InverseRelationshipName = "downloadSyncHeader_mb";
            relationship4.TargetTableName = "DownloadSyncChunk_mb";
            relationship4.SupportMultipleRelationships = true;
            downloadSyncHeaderEntity.Relationships.Add(relationship4);

            defaultEntList.Add(downloadSyncHeaderEntity);

            #endregion

            #region LastSync_mb

            EntityModel lastSyncEntity = new EntityModel(this);
            lastSyncEntity.Name = "LastSync_mb";
            lastSyncEntity.Attributes.Clear();

            EntityAttributeModel attribute21 = new EntityAttributeModel(lastSyncEntity);
            attribute21.Name = "serverDate_mb";
            attribute21.AttributeType = AttributeType.Double;
            attribute21.AttributeInfo = new AttributeInfoDouble();
            lastSyncEntity.Attributes.Add(attribute21);

            defaultEntList.Add(lastSyncEntity);

            #endregion

            return defaultEntList;
        }

        /// <summary>
        /// Converts the current object to its XCode XML representation.
        /// </summary>
        /// <returns>
        /// An XML representation of the current object.
        /// </returns>
        public XElement ToXCD()
        {
            XElement model = new XElement("model");
            model.SetAttributeValue("name", this.Name);
            model.SetAttributeValue("databaseVersion", this.DatabaseVersion);
            model.SetAttributeValue("lastSavedToolsVersion", this.LastSavedToolsVersion);
            model.SetAttributeValue("systemVersion", this.SystemVersion);
            model.SetAttributeValue("userDefinedModelVersionIdentifier", string.Empty);
            model.SetAttributeValue("type", "com.apple.IDECoreDataModeler.DataModel");
            model.SetAttributeValue("documentVersion", "1.0");
            model.SetAttributeValue("minimumToolsVersion", "Automatic");
            model.SetAttributeValue("macOSVersion", "Automatic");
            model.SetAttributeValue("iOSVersion", "Automatic");

            foreach (var entity in this.Entities)
            {
                model.Add(entity.ToXCD());
            }

            List<EntityModel> defaultEntList = this.CreateDefaultEntities();

            foreach (var entity in defaultEntList)
            {
                model.Add(entity.ToXCD());
            }

            return model;
        }

         
        /// <summary>
        /// Returns the xml representation of the current object.
        /// </summary>
        /// <returns>
        /// An XElement object.
        /// </returns>
        public XElement ToRPXCD()
        {
            XElement model = new XElement("model");
            model.SetAttributeValue("name", this.Name);
            model.SetAttributeValue("databaseVersion", this.DatabaseVersion);

            foreach (var entity in this.Entities)
            {
                model.Add(entity.ToRPXCD());
            }

            foreach (var lookupTable in this.LookupTables)
            {
                model.Add(lookupTable.ToRPXCD());
            }

            foreach (var database in this.Databases)
            {
                model.Add(database.ToRPXCD());
            }

            return model;
        }

        public DynamicMBContext ToMBContext()
        {
            return new DynamicMBContext(this, null);
        }

        public DynamicMBContext ToMBContext(string connectionString)
        {
            return new DynamicMBContext(this, connectionString);
        }



        #endregion

        #region MobiseModel Base
          /// <summary>
        /// Returns the xml representation of the current object.
        /// </summary>
        /// <returns>
        /// An XElement object.
        /// </returns>
        public override XElement ToXml()
        { 
            XElement model = new XElement("model");
            model.SetAttributeValue("mobiseID", this.MobiseObjectID);
            model.SetAttributeValue("name", this.Name);

            foreach (var entity in this.Entities)
            {
                model.Add(entity.ToRPXCD());
            }

            foreach (var lookupTable in this.LookupTables)
            {
                model.Add(lookupTable.ToRPXCD());
            }

            foreach (var database in this.Databases)
            {
                model.Add(database.ToRPXCD());
            }

            return model;
         
        }

        public override XElement ToXml(XElement baseElement, bool AddCommonAttributes)
        {
            throw new NotImplementedException();
        }

        public override void FromXml(XElement xmlModel)
        {
            this.FromXml(xmlModel, true);
        }

        public void FromXml(XElement xmlModel, bool readBaseAttributes)
        {
            this.EntitiesRootCounter = 0;
            if (xmlModel.HasAttributes && readBaseAttributes)
            {
                this.Name = xmlModel.Attribute("name") != null ? xmlModel.Attribute("name").Value : string.Empty;
                this.DatabaseVersion = xmlModel.Attribute("databaseVersion") != null ? xmlModel.Attribute("databaseVersion").Value : "1";
                this.SystemVersion = xmlModel.Attribute("systemVersion") != null ? xmlModel.Attribute("systemVersion").Value : SYSVERSION;
                this.LastSavedToolsVersion = xmlModel.Attribute("lastSavedToolsVersion") != null ? xmlModel.Attribute("lastSavedToolsVersion").Value : TOOLSVERSION;
            }

            if (xmlModel.HasElements)
            {
                foreach (var entity in xmlModel.Elements("entity"))
                {
                    this.Entities.Add(new EntityModel(entity, this));
                }
                this.Entities.Sort((l, r) => l.Name.CompareTo(r.Name));

                foreach (var lookupTable in xmlModel.Elements("lookupTable"))
                {
                    this.LookupTables.Add(new LookupTableModel(lookupTable));
                }

                foreach (var database in xmlModel.Elements("database"))
                {
                    this.Databases.Add(new DatabaseConnectionModel(database));
                }
            }
        }


        #endregion
    }
}
