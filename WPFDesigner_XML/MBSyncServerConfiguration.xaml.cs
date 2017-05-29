using Microsoft.XmlTemplateDesigner;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace WPFDesigner_XML
{
    enum MBSyncSettingsSections
    {
        none,
        authorization,
        connectionStrings,
        security,
        blob,
        log,
        connector,
        all
    }

    /// <summary>
    /// Interaction logic for MBSyncServerConfiguration.xaml
    /// </summary>
    public partial class MBSyncServerConfiguration : Window
    {
        #region Variables
        VsDesignerControl parentWindow;
        string token;
        /*WebClient webClient;
        WebRequest webRequest;*/
        HttpRequestMessage request;
        bool isConnected;
        bool processingRequest;
        bool isConnecting;
        Uri syncServiceUri;
        string webBody;
        MBSyncSettingsSections isUpdatingSettings = MBSyncSettingsSections.none;
        bool _isDirty;
        
        /*
         *    var token: String = ""
    var webClient: NSURLConnection?
    var webBody: NSMutableData?
    var processingRequest = false
    var syncServiceUri: URL?
    var isConnecting = false
    var isConnected = false
    var isUpdatingSettings = MBSyncSettingsSections.none
    var connectionStringsList: [String:DBConnetionStringModel] = [:]
    var cnnEditorWindow: MBSyncConnetionStringEditor?
    var userEditorWindow: MBSyncUserEditor?
    
    // local variables
    
    var basicUserNames : [String] = []
    var basicUserData : [String:SyncServiceBasicUser] = [:]
    
    weak var currentProject: MBProjectModel?
    

         */
        #endregion

        #region Constructors
        public MBSyncServerConfiguration()
        {
            InitializeComponent();
            processingRequest = false;
            isConnecting = false;
            isConnected = false;
            new Task(async () => {
                try
                {
                    await onConnect();
                }
                catch(Exception ex)
                {
                    var error = ex.Message;
                }
            }).Start();
        }

        public MBSyncServerConfiguration(VsDesignerControl parent) : this()
        {
            parentWindow = parent;
        }
        #endregion

        private async Task onConnect()
        {
            changeButtonsState(false);
            if (isConnected && request != null)
            {
                request = null;
                isConnected = false;
                btConnect.IsEnabled = true;
            }
            else if (!processingRequest && SyncServerURL.Text.Length > 6 && SyncServerUserName.Text.Length > 0 && SyncServerPassword.Text.Length > 0)
            {
                //request authentication
                processingRequest = true;
                isConnecting = true;
                btConnect.IsEnabled = false;

                // disable connect button, or change value to cancel
                syncServiceUri = new Uri(SyncServerURL.Text);
                if (syncServiceUri != null)
                {
                    Uri tempUri = new Uri("api/Authentication");
                    Uri authUrl = new Uri(tempUri, syncServiceUri);
                    if (authUrl != null)
                    {
                        var credentials = SyncServerUserName.Text + ":" + SyncServerPassword.Text;
                        var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(credentials);
                        var base64credentials = System.Convert.ToBase64String(plainTextBytes);

                        using (HttpClient httpClient = new HttpClient())
                        {
                            httpClient.Timeout = new TimeSpan(0, 1, 0);
                            using (request = new HttpRequestMessage(HttpMethod.Post, authUrl))
                            {
                                request.Content = new StringContent(credentials, System.Text.Encoding.UTF8);
                                isUpdatingSettings = MBSyncSettingsSections.authorization;
                                request.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
                                request.Headers.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                                request.Content.Headers.ContentLength = plainTextBytes.Length;

                                using (HttpResponseMessage response = await httpClient.SendAsync(request))
                                {
                                    var _lastResponseCode = response.StatusCode;
                                    response.EnsureSuccessStatusCode();
                                    string responseContent = await response.Content.ReadAsStringAsync();
                                    List<Dictionary<string, object>> respDictList = JsonConvert.DeserializeObject<List<Dictionary<string, object>>>(responseContent);
                                }
                            }
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Incorrect service URL!", "Can't connect to the Sync Server",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show("The Url, the user name and password are required to connect!", "Can't connect to the Sync Server",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }

        async Task getSyncServerStatus()
        {
            if (token.Length <= 0)
            {
                MessageBox.Show("Can't get the Sync Server Status", "You are not connected!", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            var serverUri = new Uri(SyncServerURL.Text);
            if (serverUri != null)
            {
                syncServiceUri = serverUri;
                var authUrl = new Uri( new Uri("api/Configuration/GetStatus"), syncServiceUri);
                if(authUrl != null)
                {
                    using (HttpClient httpClient = new HttpClient())
                    {
                        httpClient.Timeout = new TimeSpan(0, 1, 0);
                        using (request = new HttpRequestMessage(HttpMethod.Get, authUrl))
                        {
                            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
                            request.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
                            request.Headers.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                            using (HttpResponseMessage response = await httpClient.SendAsync(request))
                            {
                                var _lastResponseCode = response.StatusCode;
                                response.EnsureSuccessStatusCode();
                                string responseContent = await response.Content.ReadAsStringAsync();
                                Dictionary<string, object> respDict = JsonConvert.DeserializeObject<Dictionary<string, object>>(responseContent);
                            }
                        }
                    }
                }
            }
        }

        void connection(string connection, string error)
        {
            processingRequest = false;
            syncServiceUri = null;
            webBody = null;
            isUpdatingSettings = MBSyncSettingsSections.none;
            changeButtonsState(true);
            // if the connect button is disable we can re-enable it here
            if( isConnecting)
            {
                isConnecting = false;
                btConnect.IsEnabled = true;
                UpdateStatusToDisconnected();
            }
            MessageBox.Show("Can't connect to the Sync Server", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        void connection()
        {
            processingRequest = false;
            syncServiceUri = null;
            webBody = null;
            isUpdatingSettings = MBSyncSettingsSections.none;
            changeButtonsState(true);
            if (isConnecting)
            {
                isConnecting = false;
                btConnect.IsEnabled = true;
                UpdateStatusToDisconnected();
            }
            else
            {
                MessageBox.Show("Can't connect to the Sync Server", "Access is denied!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        void connectionDidFinishLoading(string connection)
        {
            processingRequest = false;
            if (webBody != null)
            {
                var jsonStatus = JsonConvert.DeserializeObject<Dictionary<string, object>>(webBody);
                if( isConnecting)
                {
                    if (isUpdatingSettings == MBSyncSettingsSections.authorization)
                    {
                        isUpdatingSettings = MBSyncSettingsSections.none;
                        token = jsonStatus["token"] is String? jsonStatus["token"].ToString() : string.Empty;
                        if (token.Length == 0)
                        {
                            isConnecting = false;
                            btConnect.IsEnabled = true;
                            UpdateStatusToDisconnected();
                            MessageBox.Show("Can't connect to the Sync Server", "Access is denied!", MessageBoxButton.OK, MessageBoxImage.Error);
                            return;
                        }
                        getSyncServerStatus();
                        return;
                    }
                    isConnected = true;
                    btConnect.Content = "Disconnect";
                    //ConnectProgress.stopAnimation(self)
                    // process response
                    UpdateStatus(jsonStatus);
                    //print(jsonStatus)
                    isConnecting = false; // if was connecting we now finish and is connected or not
                    btConnect.IsEnabled = true;
                }
                else
                {
                    switch (isUpdatingSettings)
                    {
                        case MBSyncSettingsSections.blob:
                            //updateBlobSettings(jsonStatus! as AnyObject)
                            break;
                    case MBSyncSettingsSections.log:
                            //updateLogSettings(jsonStatus! as AnyObject)
                            break;
                    case MBSyncSettingsSections.security:
                            //self.updateSecuritySettings(jsonStatus! as AnyObject)
                            break;
                    case MBSyncSettingsSections.connectionStrings:
                            //updateConnetionStrings(jsonStatus! as AnyObject)
                            break;
                    case MBSyncSettingsSections.connector:
                            //updateConnectorSettings(jsonStatus! as AnyObject)
                            break;
                    case MBSyncSettingsSections.all:
                            //UpdateStatus(jsonStatus as AnyObject)
                            //window!.sheetParent!.endSheet(self.window!, returnCode: NSModalResponseOK)
                            break;
                    default:
                            break;
                    }
                }
            }

            isUpdatingSettings = MBSyncSettingsSections.none;
            webBody = null;// at the end of process the request we clean up the response

            calculateGeneralStatus();
            changeButtonsState(true);
            _isDirty = false;
        }

        void connection(string connection, HttpResponseMessage response)
        {
            var statusCode = response.StatusCode;
            if (statusCode == HttpStatusCode.OK)
            {
                //webBody = NSMutableData();
            }
            else if (statusCode == HttpStatusCode.Unauthorized)
            {
                if (isConnecting)
                {
                    isConnected = false;
                    processingRequest = true;
                    UpdateStatusToDisconnected();

                    MessageBox.Show("Invalid user or Password", "Alert", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                }
            }
            else
            {
                isConnected = false;
                processingRequest = false;
                UpdateStatusToDisconnected();
            }
        }

        void UpdateStatus(Dictionary<string, object> status)
        {
            /*Color greenColor = new Color();
            greenColor.R = 0;
            greenColor.G = 128;
            greenColor.B = 0;
            greenColor.A = 255;
            int? middletierDBStatus = status["middletierDBStatus"] as int?;
            int? statusCode = status["status"] as int?;
            if (middletierDBStatus != null)
            {
                switch (middletierDBStatus)
                {
                    case 1:
                        lblMTDatabaseStatus.Text = "Invalid Database";
                        lblMTDatabaseStatus.ToolTip = "The DB version doesn't match the current uploaded DB Model. You must publish the model!";
                        lblMTDatabaseStatus.Foreground = new SolidColorBrush(greenColor);
                        break;
                    case 2:
                        string dbVersion = status["dbVersion"] as string;
                        lblMTDatabaseStatus.Text = "Published (v " + dbVersion + ")";
                        lblMTDatabaseStatus.ToolTip = "Database ready for syncronize!";
                        lblMTDatabaseStatus.Foreground = new SolidColorBrush(greenColor);
                        break;
                    default:
                        lblMTDatabaseStatus.Text = "Unpublished";
                        lblMTDatabaseStatus.ToolTip = "Can't connect to the Sync DB or database doesn't exist";
                        lblMTDatabaseStatus.Foreground = Brushes.Red;
                        break;
                }
            }
            if (statusCode != null)
            {
                switch (statusCode)
                {
                    case 1:
                        lblSyncServerStatus.Text = "Online - Unconfigured";
                        lblSyncServerStatus.Foreground = Brushes.Red;
                        break;
                    case 2:
                        lblSyncServerStatus.Text = "Online - Configured";
                        lblSyncServerStatus.Foreground = new SolidColorBrush(greenColor);
                        break;
                    default:
                        lblSyncServerStatus.Text = "Ofline - Unconfigured";
                        lblSyncServerStatus.Foreground = Brushes.Red;
                        break;
                }
            }

            var logSettings = status["logSettings"];
            if (logSettings != null)
            {
                UpdateLogSettings(logSettings as Dictionary<string, object> );
            }
            var blobSettings = status["blobSettings"];
            if (blobSettings != null)
            {
                UpdateBlobSettings(blobSettings as Dictionary<string, object>);
            }

            var connectionStringSettings = status["connectionStringsSettings"];
            if (connectionStringSettings != null)
            {
                UpdateConnectionSettings(connectionStringSettings as Dictionary<string, object>);
            }

            var securitySettings = status["securitySettings"];
            if (securitySettings != null)
            {
                UpdateSecuritySettings(securitySettings as Dictionary<string, object>);
            }

            var connectorSettings = status["connectorSettings"];
            if (connectorSettings != null)
            {
                UpdateConnectorSettings(connectorSettings as Dictionary<string, object>);
            }*/
        }

        void UpdateLogSettings(Dictionary<string, object> logSettings)
        {
            /*var errorMessage = logSettings["errorMessage"];
            if (errorMessage != null)
            {
                SyncLog.Text = errorMessage as string;
            }

            var folderPathVal = logSettings["folderPath"] as string;
            if (folderPathVal != null)
            {
                LogPath.Text = folderPathVal;
            }

            var logLevelVal = logSettings["logLevel"] as int?;
            if (logLevelVal != null)
            {
                LogLevel.SelectedIndex = (int)logLevelVal;
            }

            var statusVal = logSettings["status"] as int?;
            if (statusVal != null)
            {
                setStatusImage(LogPathStatus, statusVal);
            }*/

            //LogPathStatusProgress.stopAnimation(this);
        }

        void UpdateBlobSettings(Dictionary<string, object> blobSettings)
        {
            /*var errorMessage = blobSettings["errorMessage"];
            if (errorMessage != null)
            {
                BlobLog.Text = errorMessage.ToString();
            }

            var folderPathVal = blobSettings["folderPath"] as string;
            if (folderPathVal != null)
            {
                BlobServiceFolderPath.Text = folderPathVal;
            }

            var chunkSizeVal = blobSettings["chunkSize"] as int?;
            if (chunkSizeVal != null)
            {
                BlobChunckSize.Text = chunkSizeVal.ToString();
            }

            var statusVal = blobSettings["status"] as int?;
            if (statusVal != null)
            {
                setStatusImage(BlobFolderStatus, statusVal);
            }*/
            //BlobFolderStatusProgress.stopAnimation(this);
        }

        void UpdateSecuritySettings(Dictionary<string, object> securitySettings)
        {
            /*var audience = securitySettings["audience"] as string;
            if (audience != null)
            {
                SecurityAudience.Text = audience;
            }

            var tokenLife = securitySettings["tokenLife"] as int?;
            if (tokenLife != null)
            {
                TokenLifeSpan.Text = tokenLife.ToString();
            }

            var authType = securitySettings["authenticationType"] as int?;
            if (authType != null)
            {
                SecurityType.SelectedIndex = (int)authType;
            }

            var statusVal = securitySettings["status"] as int?;
            if (statusVal != null)
            {
                setStatusImage(SecurityStatus, statusVal);
            }

            //basicUserData.removeAll();
            //basicUserNames.removeAll();

            var basicUsers = securitySettings["users"] as Array;
            if (basicUsers != null)
            {
                foreach(var usr in basicUsers)
                {
                    var currentUsr = (Dictionary<string, object>)usr;
                    var userName = currentUsr["userName"] != null ? currentUsr["userName"].ToString() : string.Empty;
                    var password = currentUsr["password"] != null ? currentUsr["password"].ToString() : string.Empty;
                    var userCode = currentUsr["userCode"] != null ? currentUsr["userCode"].ToString() : string.Empty;
                    var userLevel = currentUsr["userLevel"] != null ? (int)currentUsr["userLevel"] : 0;
                    var userData = SyncServerBasicUser(userName, password, userLevel, userCode);
                    basicUserData[userName] = userData;
                    basicUserNames.append(userName);
                }
                BasicUserTable.reloadData();
            }

            SecurityUpdateProgress.stopAnimation(this);*/
        }

        void UpdateConnectionStrings(Dictionary<string, object> connectionStringsData)
        {
            //connectionStringsSettings
            /*CSLog.Text = string.Empty;
            connectionStringsList.removeAll();
            var cnnList = connectionStringsData["connectionStrings"] as Array;
            if (cnnList != null)
            {
                foreach (var cnn in cnnList)
                {
                    var currentCnn = cnn as Dictionary<string, object>;
                    var cnnName = currentCnn["name"] as string;
                    if (cnnName.Equals("MBSyncDB"))
                    {
                        UpdateDBSettings(cnnName, cnn as Dictionary<string, object>, SyncDBCS, SyncDBStatus, SyncDBStatusProgress);
                    }
                    else if (cnnName.Equals("BackendDB"))
                    {
                        UpdateDBSettings(cnnName, cnn as Dictionary<string, object>, BackendDBCS, BackendDBStatus, BackendDBStatusProgress);
                    }
                    else if (cnnName.Equals("tilesDB"))
                    {
                        UpdateDBSettings(cnnName, cnn as Dictionary<string, object>, GISDBCS, GISDBStatus, GISDBStatusProgress);
                    }
                }
            }*/
        }

        void UpdateDBSettings(string cnnName, Dictionary<string, object> cnnData, System.Windows.Controls.TextBox dbConnectionString,
            MessageBoxButton dbStatusIndicator, string progressIndicator)
        {
            string dataSource = cnnData["dataSource"] as string != null ? cnnData["dataSource"].ToString() : string.Empty;
            string dataBase = cnnData["database"] as string != null ? cnnData["database"].ToString() : string.Empty;
            string user = cnnData["user"] as string != null ? cnnData["user"].ToString() : string.Empty;
            string errorMessage = cnnData["errorMessage"] as string != null ? cnnData["errorMessage"].ToString() : string.Empty;
            int integratedSecurity = cnnData["integratedSecurity"] as int? != null ?
                (int)cnnData["integratedSecurity"] : 0;
            string status = cnnData["status"] as string != null ? cnnData["status"].ToString() : string.Empty;
            int connectionTimeout = cnnData["connectionTimeout"] as int? != null ? (int)cnnData["status"] : 0;

            //var cnn = DBConnectionStringModel();


        }

        void UpdateConnectorSettings(Dictionary<string, object> connectorSettings)
        {
        }

        /// @description Update the status and progress indicators to the default disconnected state
        void UpdateStatusToDisconnected()
        {
            // general status
            request = null;
            changeButtonsState(false);
            btConnect.IsEnabled = true;

            lblSyncServerStatus.Text = "Offline - Unconfigured";
            BrushConverter brushConvert = new BrushConverter();
            var converter = brushConvert.ConvertFromString("FFFF0000");

            lblSyncServerStatus.Foreground = (Brush)converter;


            lblMTDatabaseStatus.Text = "Unkown";
            lblMTDatabaseStatus.Foreground = (Brush)converter;

            // Blob Settings
            BlobFolderStatus.Content = "None";
            BlobServiceFolderPath.Text = string.Empty;
            BlobLog.Text = string.Empty;
            BlobChunckSize.Text = "0";

            // Log Settings
            LogPathStatus.Content = "None";
            LogLevel.SelectedIndex = 0;
            SyncLog.Text = string.Empty;
            LogPath.Text = string.Empty;

            // connection strings
            CSLog.Text = string.Empty;
            SyncDBCS.Text = string.Empty;
            SyncDBStatus.Content = "None";
            //SyncDBStatusProgress.stopAnimation(self)
            BackendDBCS.Text = string.Empty;
            BackendDBStatus.Content = "None";
            //BackendDBStatusProgress.stopAnimation(self)
            GISDBCS.Text = string.Empty;
            GISDBStatus.Content = "None";
            //GISDBStatusProgress.stopAnimation(self)

            //security
            SecurityAudience.Text = string.Empty;
            SecurityStatus.Content = "None";
            //SecurityUpdateProgress.stopAnimation(self)
            SecurityType.SelectedIndex = 0;

            //connector
            ConnectorUrl.Text = string.Empty;
            ConnectorStatus.Content = "None";
            //ConnectorUpdateProgress.stopAnimation(self)

            //ConnectProgress.stopAnimation(self)*/
        }

        /// check the current status of all the individual settings, to check if all is ok, so the general status need to be changed to online - ready
        void calculateGeneralStatus()
        {
            lblSyncServerStatus.Text = "Offline - Unconfigured";
            lblSyncServerStatus.Foreground = new SolidColorBrush(Colors.Red);
            if(isConnected)
            {
                bool isOk = SyncDBStatus.Content.Equals("NSStatusAvailable")
                && BackendDBStatus.Content.Equals("NSStatusAvailable")
                && GISDBStatus.Content.Equals("NSStatusAvailable")
                && BlobFolderStatus.Content.Equals("NSStatusAvailable")
                && SecurityStatus.Content.Equals("NSStatusAvailable")
                && LogPathStatus.Content.Equals("NSStatusAvailable")
                && ConnectorStatus.Content.Equals("NSStatusAvailable");
            
                if (isOk)
                {
                    lblSyncServerStatus.Text = "Online - Ready";
                    Color newColor = new Color();
                    newColor.R = 0;
                    newColor.G = 128;
                    newColor.B = 0;
                    newColor.A = 255;
                    lblSyncServerStatus.Foreground = new SolidColorBrush(newColor);
                }
                else
                {
                    lblSyncServerStatus.Text = "Online - Unconfigured";
                }
            }
        }

        // change all buttons to enable or disable. This is a utility to disable buttons while an update is in progress
        void changeButtonsState( bool enable)
        {
            btConnect.IsEnabled = enable;
            /*btBackendCSUpdate.isEnabled = enable;
            btBlobUpdate.isEnabled = enable;
            btGisCSUpdate.isEnabled = enable;
            btLogUpdate.isEnabled = enable;
            btSecurityUpdate.isEnabled = enable;
            btSyncCSUpdate.isEnabled = enable;
            btConnectorUrlUpdate.isEnabled = enable;*/
        }

    }
}
