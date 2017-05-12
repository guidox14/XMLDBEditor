using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Xml;
using WPFDesigner_XML.Common.Models;
using WPFDesigner_XML.Common.Models.Entity;

namespace WPFDesigner_XML.Common.DynamicContext
{
    public class DynamicMBContext : DbContext
    {
        private DatabaseModel targetModel = null;
        private string cnnString = string.Empty;

        public DynamicMBContext(DatabaseModel model, string connectionString)
            : base()
        {
            this.targetModel = model;
            this.cnnString = connectionString;
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            if (targetModel != null)
            {
                foreach (EntityModel item in targetModel.Entities)
                {
                    Type dynamicEntityType = DynamicEntityTypeManager.CreateTypeFromEntityDefinition(targetModel, item, DateTime.UtcNow);

                    MethodInfo registerMethod = modelBuilder.GetType().GetMethod("Entity").MakeGenericMethod(dynamicEntityType);
                    dynamic configuration = registerMethod.Invoke(modelBuilder, null);
                    // configuration.MapInheritedProperties();
                    var action = new Action<dynamic>(m =>
                    {
                        m.MapInheritedProperties();
                        m.ToTable(item.Name);
                    });
                    configuration.Map(action);
                    //DataStructures.Attribute primaryKey = item.Attributes.First(a => a.AttributeInfo.IsPrimaryKey == true);
                    //configuration.HasKey(primaryKey
                    /*modelBuilder.Entity<BaseEntity>().Map(m =>
                    {
                        m.MapInheritedProperties();
                        m.ToTable(item.Name);
                    });*/
                    ///.HasKey<string>(ent => ent.Name);
                }
            }
            
            DbModel model = null;
            if (targetModel.Databases.Count > 0 && !string.IsNullOrEmpty(this.cnnString))
            {
                if (targetModel.Databases[0].Connector.Contains("MSSQL"))
                {
                    var providerFactory = System.Data.Common.DbProviderFactories.GetFactory("System.Data.SqlClient");
                    System.Data.Common.DbConnection connection = providerFactory.CreateConnection();
                    connection.ConnectionString = this.cnnString;
                    model = modelBuilder.Build(connection);
                }
            }
            else
            {
                var provider = new DbProviderInfo("System.Data.SqlClient", "2008");
                model = modelBuilder.Build(provider);
            }

            string edmxpath = System.IO.Path.Combine(targetModel.ParentProject.Path, System.IO.Path.ChangeExtension(targetModel.Path, "edmx"));
            if (System.IO.File.Exists(edmxpath))
            {
                System.IO.File.Delete(edmxpath);
            }
            var writer = new XmlTextWriter(edmxpath, Encoding.ASCII);
            EdmxWriter.WriteEdmx(model, writer);
            writer.Flush();
            writer.Close();
            //base.OnModelCreating(modelBuilder);
        }
    }
}
