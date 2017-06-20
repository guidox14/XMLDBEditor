

namespace WPFDesigner_XML.Common.Models.Entity
{
    using Microsoft.XmlTemplateDesigner;
    using System;
    using System.Xml.Linq;
    using WPF.Common.Models.Entity;
    using WPFDesigner_XML.Common.Converters;


    /// <summary>
    /// The list of possible values types for the Attribute.
    /// </summary>
    public enum AttributeType
    {
        /// <summary>
        /// The type is undefined.
        /// </summary>
        [LocalizableDescription(@"Undefined", typeof(Resources))]
        Undefined = 0,

        /// <summary>
        /// The type is an Integer 16.
        /// </summary>
        [LocalizableDescription(@"Integer 16", typeof(Resources))]
        Integer16 = 1,

        /// <summary>
        /// The type is an Integer32.
        /// </summary>
        [LocalizableDescription(@"Integer 32", typeof(Resources))]
        Integer32 = 2,

        /// <summary>
        /// The type is an Integer64.
        /// </summary>
        [LocalizableDescription(@"Integer 64", typeof(Resources))]
        Integer64 = 3,

        /// <summary>
        /// The type is an Boolean.
        /// </summary>
        [LocalizableDescription(@"Boolean", typeof(Resources))]
        Boolean = 4,

        /// <summary>
        /// The type is an Double.
        /// </summary>
        [LocalizableDescription(@"Double", typeof(Resources))]
        Double = 5,

        /////// <summary>
        /////// The type is an Float.
        /////// </summary>
        ////[LocalizableDescription(@"Float", typeof(Resources))]
        ////Float = 6,

        /// <summary>
        /// The type is an String.
        /// </summary>
        [LocalizableDescription(@"String", typeof(Resources))]
        String = 7,

        /// <summary>
        /// The type is an Date.
        /// </summary>
        [LocalizableDescription(@"Date", typeof(Resources))]
        Date = 8,

        /// <summary>
        /// The type is an Blob.
        /// </summary>
        [LocalizableDescription(@"BLOB", typeof(Resources))]
        BLOB = 9,

        /// <summary>
        /// The type is an Blob.
        /// </summary>
        [LocalizableDescription(@"Guid", typeof(Resources))]
        GUID = 10,

        /// <summary>
        /// The type is an TEXT.
        /// </summary>
        [LocalizableDescription(@"Text", typeof(Resources))]
        Text = 11
    }

    /// <summary>
    /// The list of possible attribute levels.
    /// </summary>
    public enum AttributeLevel
    {
        /// <summary>
        /// The attribute level 0.
        /// </summary>
        [LocalizableDescription(@"Level0", typeof(Resources))]
        Level0 = 0,

        /// <summary>
        /// The attribute level 1.
        /// </summary>
        [LocalizableDescription(@"Level1", typeof(Resources))]
        Level1 = 1,

        /// <summary>
        /// The attribute level 2.
        /// </summary>
        [LocalizableDescription(@"Level2", typeof(Resources))]
        Level2 = 2,
    }

    /// <summary>
    /// The class that will define the structure for the attributes.
    /// </summary>
    public class EntityAttributeModel : MobiseModel
    {
        #region Fields

        /// <summary>
        /// Occurs when [attribute type changed].
        /// </summary>
        public event EventHandler AttributeTypeChanged;

        /// <summary>
        /// The attribute type.
        /// </summary>
        private AttributeType attributeType;

        /// <summary>
        /// The attribute name.
        /// </summary>
        private string name;

        /// <summary>
        /// The attribute value.
        /// </summary>
        private object value;

        /// <summary>
        /// The attribute is default.
        /// </summary>
        private bool isDefault;

        /// <summary>
        /// Extra information about the attribute.
        /// </summary>
        private AttributeInfo attributeInfo;

        private bool? allowForMap;
        private string mapAlias;

        #endregion

        #region Properties
        public bool? AllowForMap 
        {
            get
            {
                return this.allowForMap;
            }
            set
            {
                this.allowForMap = value;
                this.NotifyPropertyChanged("AllowForMap");
            }
        }

        public string MapAlias 
        {
            get
            {
                return this.mapAlias;
            }
            set
            {
                this.mapAlias = value;
                this.NotifyPropertyChanged("MapAlias");
            }
        }

        /// <summary>
        /// Parent entity of the attribute
        /// </summary>
        private EntityModel parentEntity;

        /// <summary>
        /// Gets or sets the type of the attribute.
        /// </summary>
        /// <value>The type of the attribute.</value>
        public AttributeType AttributeType
        {
            get
            {
                return this.attributeType;
            }

            set
            {
                if (this.attributeType != value)
                {
                    this.attributeType = value;
                    this.NotifyPropertyChanged("AttributeType");
                    if (this.AttributeTypeChanged != null)
                    {
                        this.AttributeTypeChanged(this, new EventArgs());
                    }
                }
            }
        }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>The name.</value>
        public string Name
        {
            get
            {
                return this.name;
            }

            set
            {
                if (this.name != value)
                {
                    this.name = value;
                    this.NotifyPropertyChanged("Name");
                }
            }
        }

        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        /// <value>The value.</value>
        public object Value
        {
            get
            {
                return this.value;
            }

            set
            {
                if (this.value != value)
                {
                    this.value = value;
                    this.NotifyPropertyChanged("Value");
                }
            }
        }

        /// <summary>
        /// Gets or sets the is default.
        /// </summary>
        /// <value>The is default.</value>
        public bool IsDefault
        {
            get
            {
                return this.isDefault;
            }

            set
            {
                this.isDefault = value;
                this.NotifyPropertyChanged("IsDefault");
            }
        }

        /// <summary>
        /// Gets or sets the attribute info.
        /// </summary>
        /// <value>The attribute info.</value>
        public AttributeInfo AttributeInfo
        {
            get
            {
                return this.attributeInfo;
            }

            set
            {
                if (this.attributeInfo != value)
                {
                    this.attributeInfo = value;
                    this.NotifyPropertyChanged("AttributeInfo");
                }
            }
        }

        #endregion

        /// <summary>
        /// Gets or sets the Parent Entity.
        /// </summary>
        /// <value>The Parent Entity.</value>
        public EntityModel ParentEntity
        {
            get
            {
                return this.parentEntity;
            }

            set
            {
                if (this.parentEntity != value)
                {
                    this.parentEntity = value;
                    this.NotifyPropertyChanged("ParentEntity");
                }
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Attribute"/> class.
        /// </summary>
        public EntityAttributeModel(EntityModel parent) : base()
        {
            this.parentEntity = parent;
            this.attributeInfo = new AttributeInfo();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Attribute"/> class.
        /// </summary>
        /// <param name="xmlAttribute">The XML attribute.</param>
        public EntityAttributeModel(XElement xmlAttribute, EntityModel parent)
            : this(parent)
        {
            if (xmlAttribute != null)
            {
                this.FromXml(xmlAttribute);
            }
        }

        /// <summary>
        /// Gets the valid attribute type from string.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>
        /// A valid enum for the string attribute type.
        /// </returns>
        public static AttributeType GetValidAttributeTypeFromString(string type)
        {
            AttributeType attrType = AttributeType.Undefined;

            try
            {
                // Try to cast
                attrType = (AttributeType)Enum.Parse(typeof(AttributeType), type);
            }
            catch (Exception)
            {
                // if failed, try the switch
                switch (type.ToUpperInvariant())
                {
                    case "SINGLE":
                    case "DECIMAL":
                    case "FLOAT":
                        attrType = AttributeType.Double;
                        break;

                    case "BYTE":
                        attrType = AttributeType.Integer16;
                        break;

                    case "BINARY":
                        attrType = AttributeType.BLOB;
                        break;
                    default:
                        attrType = AttributeType.Undefined;
                        break;
                }
            }

            return attrType;
        }


        /// <summary>
        /// Converts the current object to its XCode XML representation.
        /// </summary>
        /// <returns>
        /// An XML representation of the current object.
        /// </returns>
        public XElement ToXCD()
        {
            XElement attribute = this.ToXML();
            attribute.Add(this.AttributeInfo.ToXCD());
            return attribute;
        }

        /// <summary>
        /// Converts the current object to its Mobise Studio XCode XML representation.
        /// </summary>
        /// <returns>
        /// An XML representation of the current object.
        /// </returns>
        public XElement ToRPXCD()
        {
            XElement attribute = this.ToXML(true);
            attribute.SetAttributeValue("isDefault", this.IsDefault.ToString());
            if (this.allowForMap.HasValue && this.allowForMap.Value)
            {
                attribute.SetAttributeValue("allowForMap", this.allowForMap.Value.ToString());
                attribute.SetAttributeValue("mapAlias", this.MapAlias.ToString());
            }
            foreach (var attr in this.AttributeInfo.ToRPXCD())
            {
                attribute.Add(attr);
            }

            return attribute;
        }

        /// <summary>
        /// Converts the current object to its XML representation.
        /// </summary>
        /// <returns>
        /// An XML representation of the current object.
        /// </returns>
        private XElement ToXML(bool isToSQL = false)
        {
            XElement attribute = new XElement("attribute");
            attribute.SetAttributeValue("optional", "YES");
            attribute.SetAttributeValue("syncable", "YES");
            attribute.SetAttributeValue("name", this.Name);
            switch (this.AttributeType)
            {
                case AttributeType.BLOB:
                    {
                        attribute.SetAttributeValue("attributeType", "Binary");
                    }
                    break;

                case AttributeType.GUID:
                    {
                        if (!isToSQL)
                            attribute.SetAttributeValue("attributeType", "String");
                        else
                            attribute.SetAttributeValue("attributeType", EnumToFriendlyNameConverter.ConvertEnum(this.AttributeType));
                    }
                    break;

                default:
                    {
                        attribute.SetAttributeValue("attributeType", EnumToFriendlyNameConverter.ConvertEnum(this.AttributeType));
                    }
                    break;
            }

            return attribute;
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
            return this.ToRPXCD();
        }

        public override XElement ToXml(XElement baseElement, bool AddCommonAttributes)
        {
            throw new NotImplementedException();
        }

        public override void FromXml(XElement xmlAttribute)
        {
            if (xmlAttribute.HasAttributes)
            {
                this.Name = xmlAttribute.Attribute("name") != null ? xmlAttribute.Attribute("name").Value : string.Empty;
                this.IsDefault = xmlAttribute.Attribute("isDefault") != null ? bool.Parse(xmlAttribute.Attribute("isDefault").Value.ToString()) : false;
                this.AttributeType = EntityAttributeModel.GetValidAttributeTypeFromString(xmlAttribute.Attribute("attributeType").Value.Replace(" ", ""));
                this.AllowForMap = xmlAttribute.Attribute("allowForMap") != null ? bool.Parse(xmlAttribute.Attribute("allowForMap").Value.ToString()) : false;
                if (this.AllowForMap.Value)
                {
                    this.MapAlias = xmlAttribute.Attribute("mapAlias") != null ? xmlAttribute.Attribute("mapAlias").Value : string.Empty;
                }

                switch (this.AttributeType)
                {
                    case AttributeType.Integer16:
                    case AttributeType.Integer32:
                    case AttributeType.Integer64:
                        this.AttributeInfo = new AttributeInfoInteger(xmlAttribute);
                        break;

                    case AttributeType.String:
                        this.AttributeInfo = new AttributeInfoString(xmlAttribute);
                        break;

                    case AttributeType.Double:
                        this.AttributeInfo = new AttributeInfoDouble(xmlAttribute);
                        break;

                    case AttributeType.Date:
                        this.AttributeInfo = new AttributeInfoDate(xmlAttribute);
                        break;

                    default:
                        this.AttributeInfo = new AttributeInfo(xmlAttribute);
                        break;
                }
            }
        }


        #endregion

       
    }



}
