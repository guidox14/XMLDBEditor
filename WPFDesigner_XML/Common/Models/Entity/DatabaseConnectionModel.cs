

namespace WPFDesigner_XML.Common.Models.Entity
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Xml.Linq;

    /// <summary>
    /// This class will represented a database associated to the model.
    /// </summary>
    public class DatabaseConnectionModel : MobiseModel
    {
        /// <summary>
        /// The value of the  server name.
        /// </summary>
        private string server;

        /// <summary>
        /// Flag for windows authentication security for MS SQL.
        /// </summary>
        private bool windowsAuthentication;

        /// <summary>
        /// The value of the database name.
        /// </summary>
        private string databaseName;

        /// <summary>
        /// The value of the user name.
        /// </summary>
        private string userName;

        /// <summary>
        /// The name of the connector needed to connect to the database.
        /// </summary>
        private string connector;

        /// <summary>
        /// The value to keep while the program is running.
        /// </summary>
        private string password;

        /// <summary>
        /// Gets or sets the server.
        /// </summary>
        /// <value>
        /// The server.
        /// </value>
        public string Server
        {
            get
            {
                return this.server;
            }

            set
            {
                if (this.server != value)
                {
                    this.server = value;
                    this.NotifyPropertyChanged("Server");
                }
            }
        }

        /// <summary>
        /// Gets or sets the name of the database.
        /// </summary>
        /// <value>
        /// The name of the database.
        /// </value>
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
        /// Gets or sets the name of the user.
        /// </summary>
        /// <value>
        /// The name of the user.
        /// </value>
        public string UserName
        {
            get
            {
                return this.userName;
            }

            set
            {
                if (this.userName != value)
                {
                    this.userName = value;
                    this.NotifyPropertyChanged("UserName");
                }
            }
        }

        /// <summary>
        /// Gets or sets the password.
        /// </summary>
        /// <value>
        /// The password.
        /// </value>
        public string Password
        {
            get
            {
                return this.password;
            }

            set
            {
                if (this.password != value)
                {
                    this.password = value;
                    this.NotifyPropertyChanged("Password");
                }
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the database will connect using windows authentication.
        /// </summary>
        /// <value>
        /// <c>true</c> if will connect using windows authentication; otherwise, <c>false</c>.
        /// </value>
        public bool WindowsAuthentication
        {
            get
            {
                return this.windowsAuthentication;
            }

            set
            {
                if (this.windowsAuthentication != value)
                {
                    this.windowsAuthentication = value;
                    this.NotifyPropertyChanged("WindowsAuthentication");
                }
            }
        }

        /// <summary>
        /// Gets or sets the platform.
        /// </summary>
        /// <value>
        /// The platform.
        /// </value>
        public string Connector
        {
            get
            {
                return this.connector;
            }

            set
            {
                if (this.connector != value)
                {
                    this.connector = value;
                    this.NotifyPropertyChanged("Platform");
                }
            }

        }
        /// <summary>
        /// Initializes a new instance of the <see cref="Database" /> class.
        /// </summary>
        public DatabaseConnectionModel() : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Database" /> class.
        /// </summary>
        /// <param name="xmlDatabase">The XML database.</param>
        public DatabaseConnectionModel(XElement xmlDatabase)
            : this()
        {
            if (xmlDatabase != null)
            {
                this.ProcessXml(xmlDatabase);
            }
        }

        /// <summary>
        /// Processes the XML.
        /// </summary>
        /// <param name="xmlDatabase">The XML database.</param>
        /// <exception cref="System.NotImplementedException"></exception>
        private void ProcessXml(XElement xmlDatabase)
        {
            if(xmlDatabase.HasAttributes)
            {
                this.Server = xmlDatabase.Attribute("server") != null ? xmlDatabase.Attribute("server").Value : string.Empty;
                this.Connector = xmlDatabase.Attribute("connector") != null ? xmlDatabase.Attribute("connector").Value : string.Empty;
                this.DatabaseName = xmlDatabase.Attribute("databaseName") != null ? xmlDatabase.Attribute("databaseName").Value : string.Empty;
                this.WindowsAuthentication = xmlDatabase.Attribute("windowsAuthentication") != null ? Convert.ToBoolean(xmlDatabase.Attribute("windowsAuthentication").Value) : false;
                this.UserName = xmlDatabase.Attribute("user") != null ? xmlDatabase.Attribute("user").Value : string.Empty;
                this.Password = xmlDatabase.Attribute("password") != null ? xmlDatabase.Attribute("password").Value : string.Empty;
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
            XElement database = new XElement("database");
            database.SetAttributeValue("server", this.Server);
            database.SetAttributeValue("connector", this.Connector);
            database.SetAttributeValue("databaseName", this.DatabaseName);
            if (this.WindowsAuthentication)
            {
                database.SetAttributeValue("windowsAuthentication", this.WindowsAuthentication);
            }
            else 
            {
                database.SetAttributeValue("user", this.UserName);
                database.SetAttributeValue("password", this.Password);
            }
            
            return database;
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
    }
}
