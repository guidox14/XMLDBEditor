﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// 
// This source code was auto-generated by xsd, Version=4.6.1055.0.
// 
namespace DBXTemplateDesigner.CCModels {
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Xml.Serialization;
    
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true)]
    [System.Xml.Serialization.XmlRootAttribute(Namespace="", IsNullable=false)]
    public partial class model {
        
        private List<modelEntity> entityField;
        
        private List<modelDatabase> databaseField;
        
        private string nameField;
        
        private string databaseVersionField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("entity", Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public List<modelEntity> entity {
            get {
                return this.entityField;
            }
            set {
                this.entityField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("database", Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public List<modelDatabase> database {
            get {
                return this.databaseField;
            }
            set {
                this.databaseField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string name {
            get {
                return this.nameField;
            }
            set {
                this.nameField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string databaseVersion {
            get {
                return this.databaseVersionField;
            }
            set {
                this.databaseVersionField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true)]
    public partial class modelEntity {
        
        private List<modelEntityAttribute> attributeField;
        
        private List<modelEntityRelationship> relationshipField;
        
        private string nameField;
        
        private string representedClassNameField;
        
        private string syncableField;
        
        private string isRootField;
        
        private string syncOrderField;
        
        private string defaultProcessField;
        
        private string isDefaultField;
        
        private string externalReferencesField;
        
        private string isMediaEntityField;
        
        private string conflictResolutionRuleField;
        
        private string syncTypeField;
        
        private string descriptionField;
        
        private string friendlyNameField;
        
        private string allowForMapsField;
        
        private string isRootRelatedField;
        
        private string enableTracingField;
        
        private string backendQueryField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("attribute", Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public List<modelEntityAttribute> attribute {
            get {
                return this.attributeField;
            }
            set {
                this.attributeField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("relationship", Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public List<modelEntityRelationship> relationship {
            get {
                return this.relationshipField;
            }
            set {
                this.relationshipField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string name {
            get {
                return this.nameField;
            }
            set {
                this.nameField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string representedClassName {
            get {
                return this.representedClassNameField;
            }
            set {
                this.representedClassNameField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string syncable {
            get {
                return this.syncableField;
            }
            set {
                this.syncableField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string isRoot {
            get {
                return this.isRootField;
            }
            set {
                this.isRootField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string syncOrder {
            get {
                return this.syncOrderField;
            }
            set {
                this.syncOrderField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string defaultProcess {
            get {
                return this.defaultProcessField;
            }
            set {
                this.defaultProcessField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string isDefault {
            get {
                return this.isDefaultField;
            }
            set {
                this.isDefaultField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string externalReferences {
            get {
                return this.externalReferencesField;
            }
            set {
                this.externalReferencesField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string isMediaEntity {
            get {
                return this.isMediaEntityField;
            }
            set {
                this.isMediaEntityField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string conflictResolutionRule {
            get {
                return this.conflictResolutionRuleField;
            }
            set {
                this.conflictResolutionRuleField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string syncType {
            get {
                return this.syncTypeField;
            }
            set {
                this.syncTypeField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string description {
            get {
                return this.descriptionField;
            }
            set {
                this.descriptionField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string friendlyName {
            get {
                return this.friendlyNameField;
            }
            set {
                this.friendlyNameField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string allowForMaps {
            get {
                return this.allowForMapsField;
            }
            set {
                this.allowForMapsField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string isRootRelated {
            get {
                return this.isRootRelatedField;
            }
            set {
                this.isRootRelatedField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string enableTracing {
            get {
                return this.enableTracingField;
            }
            set {
                this.enableTracingField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string backendQuery {
            get {
                return this.backendQueryField;
            }
            set {
                this.backendQueryField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true)]
    public partial class modelEntityAttribute {

        modelEntity parent;

        private string optionalField;
        
        private string syncableField;
        
        private string nameField;
        
        private string attributeTypeField;
        
        private string isDefaultField;
        
        private string indexedField;
        
        private string isEditableField;
        
        private string isPrimaryKeyField;
        
        private string attributeLevelField;
        
        private string inSQLiteDBField;
        
        private string infoTypeField;
        
        private string maxCharsField;
        
        private string canBeEmptyField;
        
        private string decimalsField;
        
        private string moneySymbolField;
        
        private string isTimeOnlyField;
        
        private string isDateOnlyField;
        
        private string isClientKeyField;
        
        private string descriptionField;
        
        private string lookupScriptField;
        
        private string allowForMapField;
        
        private string mapAliasField;
        
        private string defaultValueField;
        
        private string validateField;
        
        private string minValueField;
        
        private string maxValueField;
        
        private string stepField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string optional {
            get {
                return this.optionalField;
            }
            set {
                this.optionalField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string syncable {
            get {
                return this.syncableField;
            }
            set {
                this.syncableField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string name {
            get {
                return this.nameField;
            }
            set {
                this.nameField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string attributeType {
            get {
                return this.attributeTypeField;
            }
            set {
                this.attributeTypeField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string isDefault {
            get {
                return this.isDefaultField;
            }
            set {
                this.isDefaultField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string indexed {
            get {
                return this.indexedField;
            }
            set {
                this.indexedField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string isEditable {
            get {
                return this.isEditableField;
            }
            set {
                this.isEditableField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string isPrimaryKey {
            get {
                return this.isPrimaryKeyField;
            }
            set {
                this.isPrimaryKeyField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string attributeLevel {
            get {
                return this.attributeLevelField;
            }
            set {
                this.attributeLevelField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string inSQLiteDB {
            get {
                return this.inSQLiteDBField;
            }
            set {
                this.inSQLiteDBField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string infoType {
            get {
                return this.infoTypeField;
            }
            set {
                this.infoTypeField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string maxChars {
            get {
                return this.maxCharsField;
            }
            set {
                this.maxCharsField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string canBeEmpty {
            get {
                return this.canBeEmptyField;
            }
            set {
                this.canBeEmptyField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string decimals {
            get {
                return this.decimalsField;
            }
            set {
                this.decimalsField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string moneySymbol {
            get {
                return this.moneySymbolField;
            }
            set {
                this.moneySymbolField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string isTimeOnly {
            get {
                return this.isTimeOnlyField;
            }
            set {
                this.isTimeOnlyField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string isDateOnly {
            get {
                return this.isDateOnlyField;
            }
            set {
                this.isDateOnlyField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string isClientKey {
            get {
                if (isClientKeyField == null)
                    isClientKeyField = "False";
                return this.isClientKeyField;
            }
            set {
                this.isClientKeyField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string description {
            get {
                return this.descriptionField;
            }
            set {
                this.descriptionField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string lookupScript {
            get {
                return this.lookupScriptField;
            }
            set {
                this.lookupScriptField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string allowForMap {
            get {
                return this.allowForMapField;
            }
            set {
                this.allowForMapField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string mapAlias {
            get {
                return this.mapAliasField;
            }
            set {
                this.mapAliasField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string defaultValue {
            get {
                return this.defaultValueField;
            }
            set {
                this.defaultValueField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string validate {
            get {
                return this.validateField;
            }
            set {
                this.validateField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string minValue {
            get {
                return this.minValueField;
            }
            set {
                this.minValueField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string maxValue {
            get {
                return this.maxValueField;
            }
            set {
                this.maxValueField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string step {
            get {
                return this.stepField;
            }
            set {
                this.stepField = value;
            }
        }

        public modelEntityAttribute() { }

        public modelEntityAttribute(modelEntity m_parent)
        {
            parent = m_parent;
            isDateOnlyField = "False";
            isDefaultField = "False";
            isEditableField = "False";
            isPrimaryKeyField = "False";
            isTimeOnlyField = "False";
        }

    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true)]
    public partial class modelEntityRelationship {
        
        private ObservableCollection<modelEntityRelationshipClientKey> clientKeyField;
        
        private string optionalField;
        
        private string syncableField;
        
        private string nameField;
        
        private string destinationEntityField;
        
        private string minCountField;
        
        private string maxCountField;
        
        private string deletionRuleField;
        
        private string inverseNameField;
        
        private string inverseEntityField;
        
        private string toManyField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("clientKey", Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public ObservableCollection<modelEntityRelationshipClientKey> clientKey {
            get {
                return this.clientKeyField;
            }
            set {
                this.clientKeyField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string optional {
            get {
                return this.optionalField;
            }
            set {
                this.optionalField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string syncable {
            get {
                return this.syncableField;
            }
            set {
                this.syncableField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string name {
            get {
                return this.nameField;
            }
            set {
                this.nameField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string destinationEntity {
            get {
                return this.destinationEntityField;
            }
            set {
                this.destinationEntityField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string minCount {
            get {
                return this.minCountField;
            }
            set {
                this.minCountField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string maxCount {
            get {
                return this.maxCountField;
            }
            set {
                this.maxCountField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string deletionRule {
            get {
                return this.deletionRuleField;
            }
            set {
                this.deletionRuleField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string inverseName {
            get {
                return this.inverseNameField;
            }
            set {
                this.inverseNameField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string inverseEntity {
            get {
                return this.inverseEntityField;
            }
            set {
                this.inverseEntityField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string toMany {
            get {
                return this.toManyField;
            }
            set {
                this.toManyField = value;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Relationship"/> class.
        /// </summary>
        public modelEntityRelationship() : base()
        {
            this.destinationEntity = WPFDesigner_XML.Resources.NoDestination;
            this.inverseEntity = WPFDesigner_XML.Resources.NoInverse;
            //this.ClientKeys = new ObservableCollection<ClientKey>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Relationship" /> class.
        /// </summary>
        /// <param name="copyRelationship">The copy relationship.</param>
        public modelEntityRelationship(modelEntityRelationship copyRelationship)
            : this()
        {
            this.optional = copyRelationship.optional;
            this.syncable = copyRelationship.syncable;
            this.name = copyRelationship.name;
            this.destinationEntity = copyRelationship.destinationEntity;
            this.minCount = copyRelationship.minCount;
            this.maxCount = copyRelationship.maxCount;
            this.deletionRule = copyRelationship.deletionRule;
            this.inverseName = copyRelationship.inverseName;
            this.inverseEntity = copyRelationship.inverseEntity;
            this.toMany = copyRelationship.toMany;

        }

    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true)]
    public partial class modelEntityRelationshipClientKey {
        
        private string sourceNameField;
        
        private string destinationNameField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string sourceName {
            get {
                return this.sourceNameField;
            }
            set {
                this.sourceNameField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string destinationName {
            get {
                return this.destinationNameField;
            }
            set {
                this.destinationNameField = value;
            }
        }

        public modelEntityRelationshipClientKey() { }

        public modelEntityRelationshipClientKey(string source, string destination)
        {
            this.sourceName = source;
            this.destinationName = destination;
        }

    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true)]
    public partial class modelDatabase {
        
        private string serverField;
        
        private string connectorField;
        
        private string databaseNameField;
        
        private string userField;
        
        private string passwordField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string server {
            get {
                return this.serverField;
            }
            set {
                this.serverField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string connector {
            get {
                return this.connectorField;
            }
            set {
                this.connectorField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string databaseName {
            get {
                return this.databaseNameField;
            }
            set {
                this.databaseNameField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string user {
            get {
                return this.userField;
            }
            set {
                this.userField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string password {
            get {
                return this.passwordField;
            }
            set {
                this.passwordField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true)]
    [System.Xml.Serialization.XmlRootAttribute(Namespace="", IsNullable=false)]
    public partial class NewDataSet {
        
        private model[] itemsField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("model")]
        public model[] Items {
            get {
                return this.itemsField;
            }
            set {
                this.itemsField = value;
            }
        }
    }
}
