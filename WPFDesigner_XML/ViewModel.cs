/***************************************************************************

Copyright (c) Microsoft Corporation. All rights reserved.
THIS CODE IS PROVIDED *AS IS* WITHOUT WARRANTY OF
ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING ANY
IMPLIED WARRANTIES OF FITNESS FOR A PARTICULAR
PURPOSE, MERCHANTABILITY, OR NON-INFRINGEMENT.

***************************************************************************/

using System;
using System.Collections;
using System.ComponentModel;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Package;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.TextManager.Interop;
using Microsoft.VisualStudio.XmlEditor;
using IOleServiceProvider = Microsoft.VisualStudio.OLE.Interop.IServiceProvider;
using DBXTemplateDesigner.CCModels;
using System.Collections.Generic;
using WPFDesigner_XML;
using System.Linq;

namespace Microsoft.XmlTemplateDesigner
{
    public class ResourceInfo
    {
        public static string ErrorMessageBoxTitle = "VsTemplateDesigner";
        public static string FieldNameDescription = "Description";
        public static string FieldNameId = "ID";
        public static string InvalidVsTemplateData = "The vstemplate file you are attempting to load is missing TemplateData";
        public static string SynchronizeBuffer = "Synchronize XML file with view";
        public static string ReformatBuffer = "Reformat";
        public static string ValidationFieldMaxLength = "{0} must be {1} characters or less.";
        public static string ValidationRequiredField = "{0} is a required value.";
    }

    /// <summary>
    /// ViewModel is where the interesting portion of the VsTemplate Designer lives. The View binds to an instance of this class.
    /// 
    /// The View binds the various designer controls to the methods derived from IViewModel that get and set values in the XmlModel.
    /// The ViewModel and an underlying XmlModel manage how an IVsTextBuffer is shared between the designer and the XML editor (if opened).
    /// </summary>
    public class ViewModel : INotifyPropertyChanged
    {
        const int MaxIdLength = 100;
        const int MaxProductNameLength = 60;
        const int MaxDescriptionLength = 1024;

        XmlModel _defaultTables;
        XmlModel _xmlModel;
        XmlStore _xmlStore;

        public model XmlTemplateModel { get; set; }
        public model DefaultTablesModel { get; set; }

        public IServiceProvider ServiceProvider { get; }

        IVsTextLines _buffer;

        bool _synchronizing;
        long _dirtyTime;
        EventHandler<XmlEditingScopeEventArgs> _editingScopeCompletedHandler;
        EventHandler<XmlEditingScopeEventArgs> _undoRedoCompletedHandler;
        EventHandler _bufferReloadedHandler;

        LanguageService _xmlLanguageService;

        string m_FileName;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="xmlStore">THe XML Store</param>
        /// <param name="xmlModel">The XML Model</param>
        /// <param name="provider">The Service Provider</param>
        /// <param name="buffer">The buffer</param>
        public ViewModel(XmlStore xmlStore, XmlModel xmlModel, IServiceProvider provider, IVsTextLines buffer, string fileName, XmlModel defaultTables)
        {
            if (defaultTables == null)
                throw new ArgumentNullException("defaultTables");
            if (xmlModel == null)
                throw new ArgumentNullException("xmlModel");
            if (xmlStore == null)
                throw new ArgumentNullException("xmlStore");
            if (provider == null)
                throw new ArgumentNullException("provider");
            if (buffer == null)
                throw new ArgumentNullException("buffer");

            BufferDirty = false;
            DesignerDirty = false;

            ServiceProvider = provider;
            _buffer = buffer;

            _xmlStore = xmlStore;
            // OnUnderlyingEditCompleted
            _editingScopeCompletedHandler = new EventHandler<XmlEditingScopeEventArgs>(OnUnderlyingEditCompleted);
            _xmlStore.EditingScopeCompleted += _editingScopeCompletedHandler;
            // OnUndoRedoCompleted
            _undoRedoCompletedHandler = new EventHandler<XmlEditingScopeEventArgs>(OnUndoRedoCompleted);
            _xmlStore.UndoRedoCompleted += _undoRedoCompletedHandler;

            _defaultTables = defaultTables;
            _xmlModel = xmlModel;

            // BufferReloaded
            _bufferReloadedHandler += new EventHandler(BufferReloaded);
            _xmlModel.BufferReloaded += _bufferReloadedHandler;

            m_FileName = fileName;

            LoadModelFromXmlModel();
        }

        public void Close()
        {
            //Unhook the events from the underlying XmlStore/XmlModel
            if (_xmlStore != null)
            {
                _xmlStore.EditingScopeCompleted -= _editingScopeCompletedHandler;
                _xmlStore.UndoRedoCompleted -= _undoRedoCompletedHandler;
            }
            if (_xmlModel != null)
            {
                _xmlModel.BufferReloaded -= _bufferReloadedHandler;
            }
        }

        /// <summary>
        /// Property indicating if there is a pending change in the underlying text buffer
        /// that needs to be reflected in the ViewModel.
        /// 
        /// Used by DoIdle to determine if we need to sync.
        /// </summary>
        bool BufferDirty
        {
            get;
            set;
        }

        /// <summary>
        /// Property indicating if there is a pending change in the ViewModel that needs to 
        /// be committed to the underlying text buffer.
        /// 
        /// Used by DoIdle to determine if we need to sync.
        /// </summary>
        public bool DesignerDirty
        {
            get;
            set;
        }

        /// <summary>
        /// We must not try and update the XDocument while the XML Editor is parsing as this may cause
        /// a deadlock in the XML Editor!
        /// </summary>
        /// <returns></returns>
        bool IsXmlEditorParsing
        {
            get
            {
                LanguageService langsvc = GetXmlLanguageService();
                return langsvc != null ? langsvc.IsParsing : false;
            }
        }

        /// <summary>
        /// Called on idle time. This is when we check if the designer is out of sync with the underlying text buffer.
        /// </summary>
        /*public void DoIdle()
        {
            if (BufferDirty || DesignerDirty)
            {
                int delay = 100;

                if ((Environment.TickCount - _dirtyTime) > delay)
                {
                    // Must not try and sync while XML editor is parsing otherwise we just confuse matters.
                    if (IsXmlEditorParsing)
                    {
                        _dirtyTime = Environment.TickCount;
                        return;
                    }

                    //If there is contention, give the preference to the designer.
                    if (DesignerDirty)
                    {
                        SaveModelToXmlModel(ResourceInfo.SynchronizeBuffer);
                        //We don't do any merging, so just overwrite whatever was in the buffer.
                        BufferDirty = false;
                    }
                    else if (BufferDirty)
                    {
                        LoadModelFromXmlModel();
                    }
                }
            }
        }*/

        /// <summary>
        /// Load the model from the underlying text buffer.
        /// </summary>
        private void LoadModelFromXmlModel()
        {
            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(model));

                using (XmlReader reader = GetParseTree(_defaultTables).CreateReader())
                {
                    DefaultTablesModel = (model)serializer.Deserialize(reader);
                }

                using (XmlReader reader = GetParseTree(_xmlModel).CreateReader())
                {
                    XmlTemplateModel = (model)serializer.Deserialize(reader);
                }
                
                if (XmlTemplateModel == null )
                {
                    throw new Exception(ResourceInfo.InvalidVsTemplateData);
                }
            }
            catch (Exception e)
            {
                //Display error message
                ErrorHandler.ThrowOnFailure(VsShellUtilities.ShowMessageBox(ServiceProvider,
                    e.Message,
                    ResourceInfo.ErrorMessageBoxTitle,
                    OLEMSGICON.OLEMSGICON_CRITICAL,
                    OLEMSGBUTTON.OLEMSGBUTTON_OK,
                    OLEMSGDEFBUTTON.OLEMSGDEFBUTTON_FIRST));
            }

            BufferDirty = false;

            if (ViewModelChanged != null)
            {
                // Update the Designer View
                ViewModelChanged(this, new EventArgs());
            }
        }


        /// <summary>
        /// Get an up to date XML parse tree from the XML Editor.
        /// </summary>
        XDocument GetParseTree(XmlModel model)
        {
            LanguageService langsvc = GetXmlLanguageService();

            // don't crash if the language service is not available
            if (langsvc != null)
            {
                Source src = langsvc.GetSource(_buffer);

                // We need to access this method to get the most up to date parse tree.
                // public virtual XmlDocument GetParseTree(Source source, IVsTextView view, int line, int col, ParseReason reason) {
                MethodInfo mi = langsvc.GetType().GetMethod("GetParseTree");
                int line = 0, col = 0;
                mi.Invoke(langsvc, new object[] { src, null, line, col, ParseReason.Check });
            }

            // Now the XmlDocument should be up to date also.
            return model.Document;
        }

        /// <summary>
        /// Get the XML Editor language service
        /// </summary>
        /// <returns></returns>
        LanguageService GetXmlLanguageService()
        {
            if (_xmlLanguageService == null)
            {
                IOleServiceProvider vssp = ServiceProvider.GetService(typeof(IOleServiceProvider)) as IOleServiceProvider;
                Guid xmlEditorGuid = new Guid("f6819a78-a205-47b5-be1c-675b3c7f0b8e");
                Guid iunknown = new Guid("00000000-0000-0000-C000-000000000046");
                IntPtr ptr;
                if (ErrorHandler.Succeeded(vssp.QueryService(ref xmlEditorGuid, ref iunknown, out ptr)))
                {
                    try
                    {
                        _xmlLanguageService = Marshal.GetObjectForIUnknown(ptr) as LanguageService;
                    }
                    finally
                    {
                        Marshal.Release(ptr);
                    }
                }
            }
            return _xmlLanguageService;
        }

        /// <summary>
        /// Reformat the text buffer
        /// </summary>
        void FormatBuffer(Source src)
        {
            using (EditArray edits = new EditArray(src, null, false, ResourceInfo.ReformatBuffer))
            {
                TextSpan span = src.GetDocumentSpan();
                src.ReformatSpan(edits, span);
            }
        }

        /// <summary>
        /// Get the XML Editor Source object for this document.
        /// </summary>
        /// <returns></returns>
        Source GetSource()
        {
            LanguageService langsvc = GetXmlLanguageService();
            if (langsvc == null)
            {
                return null;
            }
            Source src = langsvc.GetSource(_buffer);
            return src;
        }

        bool SearchDefaultTablesInModel()
        {
            foreach (var currentEntity in XmlTemplateModel.entity)
            {
                if (currentEntity.name.ToLower().Equals("changelog_mb"))
                {
                    return true;
                }
            }
            return false;
        }

        void InsertDefaultTablesInModel()
        {
            var defaultTables = DefaultTablesModel.entity;
            foreach (var currentEntity in defaultTables)
            {
                XmlTemplateModel.entity.Add(currentEntity);
            }
        }

        /// <summary>
        /// This method is called when it is time to save the designer values to the
        /// underlying buffer.
        /// </summary>
        /// <param name="undoEntry"></param>
        public void SaveModelToXmlModel(string undoEntry)
        {
            LanguageService langsvc = GetXmlLanguageService();

            try
            {
                //We can't edit this file (perhaps the user cancelled a SCC prompt, etc...)
                if (!CanEditFile())
                {
                    DesignerDirty = false;
                    BufferDirty = true;
                    throw new Exception();
                }

                if (!SearchDefaultTablesInModel())
                {
                    InsertDefaultTablesInModel();
                }

                UpdateAttTypes();
                UpdateSyncOrder();

                XmlSerializer serializer = new XmlSerializer(typeof(model));
                XDocument documentFromDesignerState = new XDocument();
                using (XmlWriter w = documentFromDesignerState.CreateWriter())
                {
                    serializer.Serialize(w, XmlTemplateModel);
                }

                XDocument document = GetParseTree(_xmlModel);
                /*_synchronizing = true;
                Source src = GetSource();
                if (src == null || langsvc == null)
                {
                    return;
                }

                langsvc.IsParsing = true; // lock out the background parse thread.*/

                // Wrap the buffer sync and the formatting in one undo unit.
                /*using (CompoundAction ca = new CompoundAction(src, ResourceInfo.SynchronizeBuffer))
                {*/
                using (XmlEditingScope scope = _xmlStore.BeginEditingScope(ResourceInfo.SynchronizeBuffer, this))
                    {
                        //Replace the existing XDocument with the new one we just generated.
                        document.Root.ReplaceWith(documentFromDesignerState.Root);
                        document.Save(m_FileName);
                        scope.Complete();
                    }
                    /*ca.FlushEditActions();
                    FormatBuffer(src);
                }
                DesignerDirty = false;*/
            }
            catch (Exception ex)
            {
                // if the synchronization fails then we'll just try again in a second.
                _dirtyTime = Environment.TickCount;
            }
            /*finally
            {
                langsvc.IsParsing = false;
                _synchronizing = false;
            }*/
        }

        void UpdateAttTypes()
        {
            foreach (var currentEntity in XmlTemplateModel.entity)
            {
                CheckIsRootRelated(currentEntity);

                foreach (var currentAttribute in currentEntity.attribute)
                {
                    if (currentAttribute.isClientKey == null)
                        currentAttribute.isClientKey = "False";

                    string attType = currentAttribute.attributeType;
                    switch (attType)
                    {
                        case "Integer16":
                            currentAttribute.attributeType = "Integer 16";
                            break;
                        case "Integer32":
                            currentAttribute.attributeType = "Integer 32";
                            break;
                        case "Integer64":
                            currentAttribute.attributeType = "Integer 64";
                            break;
                    }
                }
            }
        }

        void CheckIsRootRelated(modelEntity selectedEntity)
        {
            if (selectedEntity != null && selectedEntity.isRootRelated.Equals("True"))
            {
                List<modelEntityAttribute> parcelNumberAtts = selectedEntity.attribute.Where(a => a.name.Contains("parcelNbr_mb")).ToList();
                List<modelEntityAttribute> rootIdAtt = selectedEntity.attribute.Where(a => a.name.Contains("rootId_mb")).ToList();
                foreach (var currentAtt in parcelNumberAtts)
                {
                    selectedEntity.attribute.Remove(currentAtt);
                    if (rootIdAtt.Count == 0)
                    {
                        currentAtt.name = "rootId_mb";
                        selectedEntity.attribute.Add(currentAtt);
                    }
                }
            }
        }

        void UpdateSyncOrder()
        {
            foreach (var currentEntity in XmlTemplateModel.entity)
            {
                if (currentEntity.relationship.Count == 0)
                {
                    currentEntity.syncOrder = "0";
                }
                else
                {
                    currentEntity.syncOrder = RelationshipHeight(currentEntity).ToString();
                }
            }
        }

        Dictionary<string, int> entityHeightProccessed = new Dictionary<string, int>();
        int RelationshipHeight(modelEntity currentEntity)
        {
            try
            {
                int value;
                var canGetValue = entityHeightProccessed.TryGetValue(currentEntity.name, out value);
                if (canGetValue)
                    return value;
                else if (currentEntity.relationship.Count == 0)
                {
                    entityHeightProccessed[currentEntity.name] = 0;
                    return 0;
                }
                else
                {
                    var height = 0;
                    foreach (var relation in currentEntity.relationship)
                    {
                        if (relation != null)
                        {
                            if (relation.toMany == null)
                            {
                                relation.toMany = "False";
                            }
                            if (relation.toMany.ToLower().Equals("no") || relation.toMany.ToLower().Equals("false"))
                            {
                                int entityIndex = -1;
                                entityIndex = FindEntityIndex(relation.destinationEntity);
                                if (entityIndex > -1)
                                {
                                    var relationEntity = XmlTemplateModel.entity[entityIndex];
                                    entityHeightProccessed[currentEntity.name] = 1;
                                    var relatedHeight = RelationshipHeight(relationEntity) + 1;
                                    entityHeightProccessed[currentEntity.name] = relatedHeight;
                                    if (relatedHeight > height)
                                        height = relatedHeight;
                                }
                            }
                        } 
                    }
                    return height;
                }
            }
            catch (Exception ex)
            {
                var error = ex.Message;
                return 0;
            }
        }

        int FindEntityIndex(string entityName)
        {
            for (int i = 0; i < XmlTemplateModel.entity.Count; i++)
            {
                var currentEntity = XmlTemplateModel.entity[i];
                if (currentEntity.name.ToLower().Equals(entityName.ToLower()))
                {
                    return i;
                }
            }
            return -1;
        }

        /// <summary>
        /// Fired when all controls should be re-bound.
        /// </summary>
        public event EventHandler ViewModelChanged;

        private void BufferReloaded(object sender, EventArgs e)
        {
            if (!_synchronizing)
            {
                BufferDirty = true;
                _dirtyTime = Environment.TickCount;
            }
        }

        /// <summary>
        /// Handle undo/redo completion event.  This happens when the user invokes Undo/Redo on a buffer edit operation.
        /// We need to resync when this happens.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void OnUndoRedoCompleted(object sender, XmlEditingScopeEventArgs e)
        {
            if (!_synchronizing)
            {
                BufferDirty = true;
                _dirtyTime = Environment.TickCount;
            }
        }

        /// <summary>
        /// Handle edit scope completion event.  This happens when the XML editor buffer decides to update
        /// it's XDocument parse tree.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void OnUnderlyingEditCompleted(object sender, XmlEditingScopeEventArgs e)
        {
            if (e.EditingScope.UserState != this && !_synchronizing)
            {
                BufferDirty = true;
                _dirtyTime = Environment.TickCount;
            }
        }

        #region Source Control

        bool? _canEditFile;
        bool _gettingCheckoutStatus;

        /// <summary>
        /// This function asks the QueryEditQuerySave service if it is possible to edit the file.
        /// This can result in an automatic checkout of the file and may even prompt the user for
        /// permission to checkout the file.  If the user says no or the file cannot be edited 
        /// this returns false.
        /// </summary>
        private bool CanEditFile()
        {
            // Cache the value so we don't keep asking the user over and over.
            if (_canEditFile.HasValue)
            {
                return (bool)_canEditFile;
            }

            // Check the status of the recursion guard
            if (_gettingCheckoutStatus)
                return false;

            _canEditFile = false; // assume the worst
            try
            {
                // Set the recursion guard
                _gettingCheckoutStatus = true;

                // Get the QueryEditQuerySave service
                IVsQueryEditQuerySave2 queryEditQuerySave = ServiceProvider.GetService(typeof(SVsQueryEditQuerySave)) as IVsQueryEditQuerySave2;

                string filename = _xmlModel.Name;

                // Now call the QueryEdit method to find the edit status of this file
                string[] documents = { filename };
                uint result;
                uint outFlags;

                // Note that this function can popup a dialog to ask the user to checkout the file.
                // When this dialog is visible, it is possible to receive other request to change
                // the file and this is the reason for the recursion guard
                int hr = queryEditQuerySave.QueryEditFiles(
                    0,              // Flags
                    1,              // Number of elements in the array
                    documents,      // Files to edit
                    null,           // Input flags
                    null,           // Input array of VSQEQS_FILE_ATTRIBUTE_DATA
                    out result,     // result of the checkout
                    out outFlags    // Additional flags
                );
                if (ErrorHandler.Succeeded(hr) && (result == (uint)tagVSQueryEditResult.QER_EditOK))
                {
                    // In this case (and only in this case) we can return true from this function
                    _canEditFile = true;
                }
            }
            finally
            {
                _gettingCheckoutStatus = false;
            }
            return (bool)_canEditFile;
        }

        #endregion

        #region IDataErrorInfo
        public string Error
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        /*public string this[string columnName]
        {
            get
            {
                string error = null;
                switch (columnName)
                {
                    case "ID":
                        error = ValidateId();
                        break;
                    case "Description":
                        error = ValidateDescription();
                        break;
                }
                return error;
            }
        }

        private string ValidateId()
        {
            if (string.IsNullOrEmpty(TemplateID))
            {
                return string.Format(ResourceInfo.ValidationRequiredField, ResourceInfo.FieldNameId);
            }

            if (TemplateID.Length > MaxIdLength)
            {
                return string.Format(ResourceInfo.ValidationFieldMaxLength, ResourceInfo.FieldNameId, MaxIdLength);
            }
            return null;
        }

        private string ValidateDescription()
        {
            if (string.IsNullOrEmpty(Description))
            {
                return string.Format(ResourceInfo.ValidationRequiredField, ResourceInfo.FieldNameDescription);
            }

            if (Description.Length > MaxDescriptionLength)
            {
                return string.Format(ResourceInfo.ValidationFieldMaxLength, ResourceInfo.FieldNameDescription, MaxDescriptionLength);
            }
            return null;
        }*/


        #endregion

        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        protected void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion

        #region TreeView SelectionChanged

        private ITrackSelection trackSel;
        private ITrackSelection TrackSelection
        {
            get
            {
                if (trackSel == null)
                    trackSel = ServiceProvider.GetService(typeof(STrackSelection)) as ITrackSelection;
                return trackSel;
            }
        }

        private Microsoft.VisualStudio.Shell.SelectionContainer selContainer;
        public void OnSelectChanged(object p)
        {
            selContainer = new VisualStudio.Shell.SelectionContainer(true, false);
            ArrayList items = new ArrayList();
            items.Add(p);
            selContainer.SelectableObjects = items;
            selContainer.SelectedObjects = items;

            ITrackSelection track = TrackSelection;
            if (track != null)
                track.OnSelectChange(selContainer);
        }

        #endregion
    }    
}
