using System.Collections.Generic;
using WPFDesigner_XML.Common.Models.Entity;

namespace WPFDesigner_XML.Common.DynamicContext
{
    /// <summary>
    /// Interface to standardize access to external data repositories that can be used to auto generate entities in the model.
    /// </summary>
    public interface IExternalDataConnector
    {
        /// <summary>
        /// Creates the connection string.
        /// </summary>
        /// <param name="server">The server.</param>
        /// <param name="database">The database.</param>
        /// <param name="userName">Name of the user.</param>
        /// <param name="password">The password.</param>
        /// <param name="isWindowsAuthentication">The is windows authentication.</param>
        /// <param name="path">The path.</param>
        /// <returns></returns>
        string CreateConnectionString(string server, string database, string userName, string password, bool? isWindowsAuthentication, string path);

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>The name.</value>
        string Name { get; set; }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        /// <value>The description.</value>
        string Description { get; set; }

        /// <summary>
        /// connect to the external data store or init the connection object
        /// </summary>
        /// <param name="connectionString">parameter string required to start the connection</param>
        /// <param name="timeoutSeconds">time out for establish the connection if apply</param>
        /// <returns>return true if the connection is successful.</returns>
        bool Connect(string connectionString, double timeoutSeconds);

        /// <summary>
        /// Close any open connection, and release the connection object if any
        /// </summary>
        void Close();

        /// <summary>
        /// Tests the connection.
        /// </summary>
        /// <returns>
        /// Returns true if connects successfully.
        /// </returns>
        bool TestConnection();

        /// <summary>
        /// Get the list of the objects that can be extracted as entity definitions. Example in MS SQL Server is the table name
        /// </summary>
        /// <returns>return a list of the entity names available in the data store</returns>
        List<string> GetEntityNames();

        /// <summary>
        /// Get the list of the functions or procedures that can be extracted as entity definitions. Example in MS SQL Server is the store procedures
        /// </summary>
        /// <returns>return a list of the entity names available in the data store</returns>
        List<string> GetFuntionsNames();

        /// <summary>
        /// For the given entity name (table name in case of sql server) gets the list of the columns as entity attributes.
        /// </summary>
        /// <param name="entityName">Name of the entity from where generate the attributes list</param>
        /// <returns>return a list of the attributes contained by the entity (table in case of sql server)</returns>
        List<EntityAttributeModel> GetEntityAttributes(string entityName);

        /// <summary>
        /// Get a EntityModel definition base on the entity object in the data repository
        /// </summary>
        /// <param name="entityName">entity name in the entity repository.</param>
        /// <returns>return the generated entity definition</returns>
        EntityModel GetEntityDefinion(string entityName);

        /// <summary>
        /// Gets all entity definitions.
        /// </summary>
        /// <returns></returns>
        IList<EntityModel> GetAllEntityDefinitions();

        /// <summary>
        /// Creates the or update tables.
        /// </summary>
        /// <param name="entities">The entities.</param>
        /// <returns></returns>
        bool CreateOrUpdateTables(IList<EntityModel> entities);

        /// <summary>
        /// Creates the new database.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        bool CreateNewDatabase(string name, string server, string database, string userName, string password, bool? isWindowsAuthentication);
    }
}
