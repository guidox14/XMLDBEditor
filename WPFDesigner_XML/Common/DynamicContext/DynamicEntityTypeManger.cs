using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text.RegularExpressions;
using WPFDesigner_XML.Common.Models;
using WPFDesigner_XML.Common.Models.Entity;

namespace WPFDesigner_XML.Common.DynamicContext
{
    /// <summary>
    /// Allow create the type of the Entity defined in the model in memory so can be used for validation or to generate the DynamicMBContext    
    /// </summary>
    public class DynamicEntityTypeManager
    {
        /// <summary>
        /// builder of the assembly will be generated
        /// </summary>
        private static AssemblyBuilder assemblyBuilder;

        /// <summary>
        /// The module builder for generate the new types
        /// </summary>
        private static ModuleBuilder moduleBuilder;

        /// <summary>
        /// Holds a regular expression that represents the a valid property name
        /// </summary>
        private static readonly Regex propertNameRegex = new Regex(@"^[A-Za-z]+[A-Za-z0-9_]*$", RegexOptions.Singleline);

        /// <summary>
        /// Holds all the types created by this class so far
        /// </summary>
        private static Dictionary<string, DynamicTypeInfo> currentTypes = new Dictionary<string, DynamicTypeInfo>();

        /// <summary>
        /// Gets the type corresponding to the name given that had been constructed before. 
        ///     It the type hasn't been constructed, it returns null
        /// </summary>
        /// <param name="name">The name of the type to search for</param>
        /// <returns>The type corresponding to that name. If the type hasn't been constructed before it returns null</returns>
        public static Type GetType(string name)
        {
            if (name != null)
            {
                if (currentTypes.ContainsKey(name.Replace(".", "_")))
                {
                    return currentTypes[name.Replace(".", "_")].EntityType;
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Get the type info of the Business Object using his friendly name
        /// </summary>
        /// <param name="name">Friendly type name</param>
        /// <returns>return a DynamicTypeInfo with the info of the type</returns>
        public static DynamicTypeInfo GetTypeInfo(string name)
        {
            return currentTypes[name.Replace(".", "_")];
        }

        /// <summary>
        /// Register a custom DynamicTypeInfo on the current types table
        /// </summary>
        /// <param name="typeInfo">info of the type to be register</param>
        public static void RegisterTypeInfo(DynamicTypeInfo typeInfo)
        {
            if (currentTypes.ContainsKey(typeInfo.Name.Replace(".", "_")))
            {
                currentTypes[typeInfo.Name.Replace(".", "_")] = typeInfo;
            }
            else
            {
                currentTypes.Add(typeInfo.Name.Replace(".", "_"), typeInfo);
            }
        }

        /// <summary>
        /// Remove a register type info.
        /// </summary>
        /// <param name="name">Friendly name of the type to unregister</param>
        public static void UnregisterTypeInfo(string name)
        {
            currentTypes.Remove(name.Replace(".", "_"));
        }

        /// <summary>
        /// Initialize the common data for the static class
        /// </summary>
        private static void InitData(DatabaseModel model)
        {
            AssemblyName assemblyName = new AssemblyName("TempAssembly" + System.Guid.NewGuid().ToString().Replace("-", "_"));
            assemblyBuilder =
                AppDomain.CurrentDomain.DefineDynamicAssembly(
                    assemblyName, AssemblyBuilderAccess.RunAndSave);
            //LocalBuilder lb = assemblyBuilder.DeclareLocal //  "BLTypeManagerTest.dll"
            moduleBuilder = assemblyBuilder.DefineDynamicModule(model.Name, true);
        }

        /// <summary>
        /// Creates a new type with the given name
        /// </summary>
        /// <param name="resourceFriendlyName">The name for the new type</param>
        /// <returns>The created type</returns>
        private static TypeBuilder GetTypeBuilder(string resourceFriendlyName)
        {
            TypeBuilder typeBuilder = moduleBuilder.DefineType(resourceFriendlyName.Replace(".", "_"), TypeAttributes.Public | TypeAttributes.Class | TypeAttributes.AutoClass | TypeAttributes.AnsiClass | TypeAttributes.BeforeFieldInit | TypeAttributes.AutoLayout, typeof(DynamicEntityBase));
            return typeBuilder;
        }

        /// <summary>
        /// Removes all type from the type cache
        /// </summary>
        public static void InvalidateTypeCache(DatabaseModel model)
        {
            lock (currentTypes)
            {
                currentTypes.Clear();
                moduleBuilder = null;
                InitData(model);
            }
        }

        /// <summary>
        /// Creates a new property with the given name and type, create get and set methods for it, 
        ///    and attach it to the given type builder
        /// </summary>
        /// <param name="typeBuilder">The type builder to attach the new property to</param>
        /// <param name="propertyName">The name for the property</param>
        /// <param name="propertyType">The type of the property</param>
        private static void CreateProperty(TypeBuilder typeBuilder, string propertyName, Type propertyType, bool isKey, bool isRequired, List<CustomAttributeBuilder> additionalAttributes)
        {
            if (propertyType != null)
            {
                if (propertyType.IsValueType)
                {
                    Type[] args = new Type[] { propertyType };
                    propertyType = typeof(Nullable<>).MakeGenericType(args);
                }

                // If the type of the property is recognized then add to dynamic class
                //FieldBuilder fieldBuilder = typeBuilder.DefineField("_" + propertyName, propertyType, FieldAttributes.Private);

                MethodInfo targetGetMethod = typeof(DynamicEntityBase).GetMethod("get_Item", BindingFlags.Public | BindingFlags.Instance);
                MethodInfo targetSetMethod = typeof(DynamicEntityBase).GetMethod("set_Item", BindingFlags.Public | BindingFlags.Instance);

                // avoid add properties that already exist on the base type.
                if (typeBuilder.BaseType.GetProperty(propertyName) == null)
                {
                    MethodBuilder getPropMthdBldr = typeBuilder.DefineMethod("get_" + propertyName, MethodAttributes.Public | MethodAttributes.SpecialName | MethodAttributes.HideBySig, propertyType, Type.EmptyTypes);
                  
                    ILGenerator getIL = getPropMthdBldr.GetILGenerator();
                    getIL.DeclareLocal(propertyType);
                    getIL.Emit(OpCodes.Ldarg_0);
                    getIL.Emit(OpCodes.Ldstr, propertyName);
                    getIL.Emit(OpCodes.Call, targetGetMethod);
                    if (propertyType.IsValueType)
                    {
                        getIL.Emit(OpCodes.Unbox_Any, propertyType);
                        //Unbox if necessary
                    }
                    else
                    {
                        getIL.Emit(OpCodes.Castclass, propertyType);
                    }
                    getIL.Emit(OpCodes.Stloc_0);
                    getIL.Emit(OpCodes.Ldloc_0);
                    //getIL.Emit(OpCodes.Ldfld, fieldBuilder);
                    getIL.Emit(OpCodes.Ret);

                    MethodBuilder setPropMthdBldr =
                        typeBuilder.DefineMethod("set_" + propertyName, MethodAttributes.Public | MethodAttributes.SpecialName | MethodAttributes.HideBySig, null, new Type[] { propertyType });

                    ILGenerator setIL = setPropMthdBldr.GetILGenerator();
                    setIL.Emit(OpCodes.Nop);
                    setIL.Emit(OpCodes.Ldarg_0);
                    setIL.Emit(OpCodes.Ldstr, propertyName);
                    setIL.Emit(OpCodes.Ldarg_1);
                    //setIL.Emit(OpCodes.Stfld, fieldBuilder);
                    if (propertyType.IsValueType)
                    {
                        setIL.Emit(OpCodes.Box, propertyType);
                        //Box if necessary
                    }
                    setIL.Emit(OpCodes.Call, targetSetMethod);
                    setIL.Emit(OpCodes.Nop);
                    setIL.Emit(OpCodes.Ret);

                    PropertyBuilder propertyBuilder =
                        typeBuilder.DefineProperty(
                            propertyName, PropertyAttributes.HasDefault, propertyType, Type.EmptyTypes);

                    if (isKey)
                    {
                        ConstructorInfo KeyAttributeConstructor = typeof(System.ComponentModel.DataAnnotations.KeyAttribute).GetConstructor(new Type[] { });
                        CustomAttributeBuilder attributeBuilder = new CustomAttributeBuilder(KeyAttributeConstructor, new object[] { }, new FieldInfo[] { }, new object[] { });
                        //getPropMthdBldr.SetCustomAttribute(new CustomAttributeBuilder(KeyAttributeConstructor, new object[] {}));
                        propertyBuilder.SetCustomAttribute(attributeBuilder);
                    }

                    if (isRequired)
                    {
                        ConstructorInfo requiredAttributeConstructor = typeof(System.ComponentModel.DataAnnotations.RequiredAttribute).GetConstructor(new Type[] { });
                        CustomAttributeBuilder attributeBuilder = new CustomAttributeBuilder(requiredAttributeConstructor, new object[] { }, new FieldInfo[] { }, new object[] { });
                        //getPropMthdBldr.SetCustomAttribute(new CustomAttributeBuilder(KeyAttributeConstructor, new object[] {}));
                        propertyBuilder.SetCustomAttribute(attributeBuilder);
                    }

                    if (additionalAttributes != null && additionalAttributes.Count > 0)
                    {
                        additionalAttributes.ForEach(attribute => propertyBuilder.SetCustomAttribute(attribute));
                    }

                    propertyBuilder.SetGetMethod(getPropMthdBldr);
                    propertyBuilder.SetSetMethod(setPropMthdBldr);
                }
            }
        }

        /// <summary>
        /// Create a new dynamic entity strongly typed class base on a xsd document
        /// </summary>
        /// <param name="entityDefinition">Entity Definition to be parsed</param>
        /// <param name="timestamp">timestamp of the last version of the XSD definition</param>
        /// <returns>return the dynamic entity generate type base on the xsd.</returns>
        public static Type CreateTypeFromEntityDefinition(DatabaseModel ParentModel, EntityModel entityDefinition, DateTime timestamp)
        {
            lock (currentTypes)
            {
                string entityName = entityDefinition.Name;
                //System.Xml.Schema.XmlSchemaSet x;
                //x = new System.Xml.Schema.XmlSchemaSet();
                entityName = entityName.Replace(".", "_");
                Type currentType = DynamicEntityTypeManager.GetType(entityName);
                if (currentType == null)
                {
                    if (moduleBuilder == null)
                    {
                        InitData(ParentModel);
                    }

                    TypeBuilder typeBuilder = GetTypeBuilder(entityName);

                    Type[] constructorArgTypes = new Type[] { };
                    ConstructorBuilder constructorBuilder = typeBuilder.DefineConstructor(MethodAttributes.Public | MethodAttributes.SpecialName | MethodAttributes.RTSpecialName, CallingConventions.Standard, constructorArgTypes);
                    Type[] baseConstructorArgTypes = new Type[] { typeof(string) };
                    ILGenerator ilGenerator = constructorBuilder.GetILGenerator();
                    ConstructorInfo baseConstructor = typeof(DynamicEntityBase).GetConstructor(baseConstructorArgTypes);
                    ilGenerator.Emit(OpCodes.Ldarg_0);
                    ilGenerator.Emit(OpCodes.Ldstr, entityDefinition.Name);
                    ilGenerator.Emit(OpCodes.Call, baseConstructor);
                    ilGenerator.Emit(OpCodes.Ret);

                    // add copy constructor
                   /* constructorArgTypes = new Type[] { typeof(DynamicEntityBase) };
                    ConstructorBuilder constructorBuilder2 = typeBuilder.DefineConstructor(MethodAttributes.Public | MethodAttributes.SpecialName | MethodAttributes.RTSpecialName, CallingConventions.Standard, constructorArgTypes);
                    Type[] baseConstructorArgTypes2 = new Type[] { typeof(DynamicEntityBase) };
                    ConstructorInfo baseConstructor2 = typeof(DynamicEntityBase).GetConstructor(baseConstructorArgTypes2);
                    ilGenerator = constructorBuilder2.GetILGenerator();
                    ilGenerator.Emit(OpCodes.Ldarg_0);
                    ilGenerator.Emit(OpCodes.Ldarg_1);
                    ilGenerator.Emit(OpCodes.Call, baseConstructor2);
                    ilGenerator.Emit(OpCodes.Nop);
                    ilGenerator.Emit(OpCodes.Nop);
                    ilGenerator.Emit(OpCodes.Nop);
                    ilGenerator.Emit(OpCodes.Ret);*/

                    foreach (EntityAttributeModel element in entityDefinition.Attributes)
                    {
                        string propName = element.Name;
                        if (propertNameRegex.IsMatch(propName, 0))
                        {
                            // gets the type code of the property, and ensure that they don't have the namespace to basic schema definition 
                            Type propType = DynamicEntityTypeManager.GetClrTypeFromPropertyType(element.AttributeType);

                            CreateProperty(typeBuilder, propName, propType, element.AttributeInfo.IsPrimaryKey, element.AttributeInfo.IsPrimaryKey, null);
                        }
                        else
                        {
                            throw new ArgumentException(
                                        @"Each property name must be 
                            alphanumeric and start with character.");
                        }
                    }

                    // temporary type info for forward reference
                    currentTypes[entityName] = new DynamicTypeInfo(entityName, timestamp, typeBuilder);

                    foreach (EntityRelationshipModel relationship in entityDefinition.Relationships)
                    {
                        string propName = relationship.Name;
                        if (propertNameRegex.IsMatch(propName, 0))
                        {
                            // gets the type code of the property, and ensure that they don't have the namespace to basic schema definition 
                            Type relationshipType = DynamicEntityTypeManager.GetType(relationship.TargetTableName);
                            EntityModel targetEntityDefinition = ParentModel.Entities.FirstOrDefault(entity => entity.Name == relationship.TargetTableName);
                            bool isBuildChild = false;
                            if (relationshipType == null)
                            {
                                if (relationship.TargetTableName == entityDefinition.Name)
                                {
                                    // reference this same type being build
                                    relationshipType = typeBuilder;
                                    isBuildChild = false;
                                }
                                else
                                {
                                    // create the type look for the model entity definition
                                    relationshipType = CreateTypeFromEntityDefinition(ParentModel, targetEntityDefinition, DateTime.UtcNow);
                                    isBuildChild = true;
                                }
                            }

                            Type propType = null;
                            List<CustomAttributeBuilder> additionalAttributes = new List<CustomAttributeBuilder>();
                            if (relationship.SupportMultipleRelationships)
                            {
                                propType = typeof(ICollection<>).MakeGenericType(relationshipType);
                                if (!string.IsNullOrEmpty(relationship.InverseRelationshipName))
                                {
                                    EntityRelationshipModel inverseRelation = targetEntityDefinition.Relationships.FirstOrDefault(rel => rel.Name == relationship.InverseRelationshipName);
                                    ConstructorInfo attributeConstructor = null;
                                    CustomAttributeBuilder attributeBuilder = null;
                                    if (inverseRelation != null)
                                    {
                                        attributeConstructor = typeof(System.ComponentModel.DataAnnotations.Schema.InversePropertyAttribute).GetConstructor(new Type[] { typeof(string) });
                                        attributeBuilder = new CustomAttributeBuilder(attributeConstructor, new object[] { relationship.InverseRelationshipName }, new FieldInfo[] { }, new object[] { });
                                        additionalAttributes.Add(attributeBuilder);
                                    }

                                }
                            }
                            else
                            {
                                propType = relationshipType;
                                if (!string.IsNullOrEmpty(relationship.InverseRelationshipName))
                                {
                                    EntityRelationshipModel inverseRelation = targetEntityDefinition.Relationships.FirstOrDefault(rel => rel.Name == relationship.InverseRelationshipName);
                                    ConstructorInfo attributeConstructor = null;
                                    CustomAttributeBuilder attributeBuilder = null;
                                    if (inverseRelation != null)
                                    {
                                        attributeConstructor = typeof(System.ComponentModel.DataAnnotations.Schema.InversePropertyAttribute).GetConstructor(new Type[] { typeof(string) });
                                        attributeBuilder = new CustomAttributeBuilder(attributeConstructor, new object[] { relationship.InverseRelationshipName }, new FieldInfo[] { }, new object[] { });
                                        additionalAttributes.Add(attributeBuilder);
                                    }

                                    //create foreign key field
                                     EntityAttributeModel targetKey = targetEntityDefinition.Attributes.FirstOrDefault(a => a.AttributeInfo.IsPrimaryKey);
                                    if (targetKey != null)
                                    {
                                        string fkFieldName = relationship.Name + "_FK";
                                        attributeConstructor = typeof(System.ComponentModel.DataAnnotations.Schema.ForeignKeyAttribute).GetConstructor(new Type[] { typeof(string) });
                                        if (inverseRelation.SupportMultipleRelationships)
                                        {
                                            CreateProperty(typeBuilder, fkFieldName, DynamicEntityTypeManager.GetClrTypeFromPropertyType(targetKey.AttributeType), false, true, null);
                                            attributeBuilder = new CustomAttributeBuilder(attributeConstructor, new object[] { fkFieldName }, new FieldInfo[] { }, new object[] { });
                                        }
                                        else
                                        {
                                            attributeBuilder = new CustomAttributeBuilder(attributeConstructor, new object[] { targetKey.Name }, new FieldInfo[] { }, new object[] { });
                                        }
                                        additionalAttributes.Add(attributeBuilder);
                                        
                                    }
                                }
                            }

                            if (propType == null)
                            {
                                throw new NullReferenceException(string.Format("The relationship {0} type can't be determinate", relationship.Name));
                            }

                            CreateProperty(typeBuilder, propName, propType, false, !isBuildChild, additionalAttributes);
                        }
                        else
                        {
                            throw new ArgumentException(
                                        @"Each property name must be 
                            alphanumeric and start with character.");
                        }
                    }

                    Type newType = typeBuilder.CreateType();

                    //currentTypes[resourceFriendlyName] = newType;
                    currentTypes[entityName] = new DynamicTypeInfo(entityName, timestamp, newType);

                    //assemblyBuilder.Save("BLTypeManagerTest.dll");

                    return newType;

                }
                return currentType;
            }
        }

        /// <summary>
        /// Gets the type of the CLR type from property.
        /// </summary>
        /// <param name="attributeType">Type of the attribute.</param>
        /// <returns>Return the respective system type</returns>
        private static Type GetClrTypeFromPropertyType(AttributeType attributeType)
        {
            switch (attributeType)
            {
                case AttributeType.Undefined:
                    return typeof(object);
                    
                case AttributeType.Integer16:
                    return typeof(Int16);
                    
                case AttributeType.Integer32:
                    return typeof(int);
                    
                case AttributeType.Integer64:
                    return typeof(Int64);
                    
                case AttributeType.Boolean:
                    return typeof(bool);

                case AttributeType.Double:
                    return typeof(double);

                case AttributeType.String:
                    return typeof(string);

                case AttributeType.Date:
                    return typeof(DateTime);

                case AttributeType.BLOB:
                    return typeof(byte[]);

                case AttributeType.GUID:
                    return typeof(Guid);

                default:
                    return typeof(object);
            }
        }

        /// <summary>
        /// Saves the DLL to disk for analyze or future reference.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        public void SaveDll(string fileName)
        {
            assemblyBuilder.Save(fileName);
        }
    }
}
