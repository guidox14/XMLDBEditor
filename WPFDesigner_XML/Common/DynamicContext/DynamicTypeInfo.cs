using System;
using WPFDesigner_XML.Common.Models.Entity;

namespace WPFDesigner_XML.Common.DynamicContext
{
    /// <summary>
    /// Represent the related static information about a dynamic entity type in the DynamicEntityTypeManager
    /// </summary>
    public class DynamicTypeInfo
    {
        /// <summary>
        /// Gets a value indicating the type of the Business object
        /// </summary>
        public Type EntityType
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets a value indicating the friendly name for this type on the server
        /// </summary>
        public string Name
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets a value indicating the last time that the type was modified
        /// </summary>
        public DateTime Timestamp
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets a value indicating the entity definition for this type
        /// </summary>
        public EntityModel EntityDefinition
        {
            get;
            private set;
        }

        /// <summary>
        /// Create new DynamicTypeInfo with the associated info
        /// </summary>
        /// <param name="name">friendly name for server side</param>
        /// <param name="timestamp">date of the type definition was build</param>
        /// <param name="runtimeType">type of the business object on the client</param>
        /// <param name="entityDefinition">metadata info of the entityDefinition</param>
        public DynamicTypeInfo(string name, DateTime timestamp, Type runtimeType, EntityModel entityDefinition)
        {
            this.Name = name;
            this.Timestamp = timestamp;
            this.EntityType = runtimeType;
            this.EntityDefinition = entityDefinition;
        }

        /// <summary>
        /// Create new DynamicTypeInfo with the associated info
        /// </summary>
        /// <param name="name">friendly name for server side</param>
        /// <param name="timestamp">date of the type definition was build</param>
        /// <param name="runtimeType">type of the business object on the client</param>
        public DynamicTypeInfo(string name, DateTime timestamp, Type runtimeType)
        {
            this.Name = name;
            this.Timestamp = timestamp;
            this.EntityType = runtimeType;
            //BLFieldMapList fieldMapList = new BLFieldMapList();
            //PropertyInfo[] properties = businessObjectType.GetProperties(BindingFlags.Instance | BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.SetProperty);
            //int order = 0;
            //foreach (PropertyInfo property in properties)
            //{
            //    BLFieldMap propertyMetadata = new BLFieldMap(BLFieldMap.FieldMapTypeEnum.DataField, property.Name, false, true, true, order, property.Name, property.PropertyType);
            //    // here we can check for validation attributes of the property to add that info the the BLFieldMap
            //    fieldMapList.Add(propertyMetadata);
            //    order++;
            //}
            //this.FieldMapList = fieldMapList;
        }
    }
}
