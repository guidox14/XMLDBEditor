/***************************************************************************

Copyright (c) Microsoft Corporation. All rights reserved.
THIS CODE IS PROVIDED *AS IS* WITHOUT WARRANTY OF
ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING ANY
IMPLIED WARRANTIES OF FITNESS FOR A PARTICULAR
PURPOSE, MERCHANTABILITY, OR NON-INFRINGEMENT.

***************************************************************************/

using DBXTemplateDesigner.CCModels;
using EDBFramework;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using WPFDesigner_XML;
using WPFDesigner_XML.Common.Models;
using WPFDesigner_XML.Common.Models.Entity;

namespace Microsoft.XmlTemplateDesigner
{
    /// <summary>
    /// Interaction logic for VsDesignerControl.xaml
    /// </summary>
    public partial class VsDesignerControl : UserControl, INotifyPropertyChanged
    {
        private modelEntityAttribute selectedAttribute = null;
        private modelEntityRelationship selectedRelationship = null;
        private List<string> reservedWords = new List<string>();

        public modelEntity SelectedEntity { get; set; }

        /// <summary>
        /// Event fire when a property value was changed
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Gets the attributes.
        /// </summary>
        public ObservableCollection<modelEntityAttribute> Attributes { get; private set; }

        /// <summary>
        /// Gets the relationships.
        /// </summary>
        public ObservableCollection<modelEntityRelationship> Relationships { get; private set; }

        /// <summary>
        /// Gets or sets the selected attribute.
        /// </summary>
        /// <value>
        /// The selected attribute.
        /// </value>
        public modelEntityAttribute SelectedAttribute
        {
            get
            {
                return this.selectedAttribute;
            }
            set
            {
                this.selectedAttribute = value;
                this.NotifyPropertyChanged("SelectedAttribute");
            }
        }

        /// <summary>
        /// Gets or sets the selected relationship.
        /// </summary>
        /// <value>
        /// The selected relationship.
        /// </value>
        public modelEntityRelationship SelectedRelationship
        {
            get
            {
                return this.selectedRelationship;
            }
            set
            {
                this.selectedRelationship = value;
                this.NotifyPropertyChanged("SelectedRelationship");
            }
        }


        /// <summary>
        /// Constructor
        /// </summary>
        public VsDesignerControl()
        {
            InitializeComponent();
        }

        public VsDesignerControl(ViewModel viewModel)
        {
            DataContext = viewModel;
            InitializeComponent();
            // wait until we're initialized to handle events
            viewModel.ViewModelChanged += new EventHandler(ViewModelChanged);
        }

        internal void DoIdle()
        {
            // only call the view model DoIdle if this control has focus
            // otherwise, we should skip and this will be called again
            // once focus is regained
            ViewModel viewModel = DataContext as ViewModel;
            if (viewModel != null && this.IsKeyboardFocusWithin)
            {
                viewModel.DoIdle();
            }
        }

        /// <summary>
        /// Notify that a property has changed.
        /// </summary>
        /// <param name="property">Name of the property that was changed</param>
        public virtual void NotifyPropertyChanged(string property)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(property));
            }
        }

        private void ViewModelChanged(object sender, EventArgs e)
        {
            // this gets called when the view model is updated because the Xml Document was updated
            // since we don't get individual PropertyChanged events, just re-set the DataContext
            ViewModel viewModel = DataContext as ViewModel;
            DataContext = null; // first, set to null so that we see the change and rebind
            DataContext = viewModel;
        }

        private void treeContent_Loaded(object sender, RoutedEventArgs e)
        {
            var treeView = sender as TreeView;
            if (treeView != null)
            {
                // make sure that any top-level items that contain other items are expanded
                foreach (object item in treeView.Items)
                {
                    TreeViewItem treeItem = treeView.ItemContainerGenerator.ContainerFromItem(item) as TreeViewItem;
                    treeItem.IsExpanded = true;
                }
            }
        }

        private void treeContent_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            var viewModel = DataContext as ViewModel;
            var treeView = sender as TreeView;
            if ((viewModel != null) && (treeView != null))
            {
                // pass Selection events along to the view model so that the Properties window is updated
                viewModel.OnSelectChanged(treeView.SelectedItem);
            }
        }

        private void cbLocation_Loaded(object sender, RoutedEventArgs e)
        {
            var viewModel = DataContext as ViewModel;
            var comboBox = sender as ComboBox;
            /*if (!viewModel.IsLocationFieldSpecified)
            {*/
            // don't show selection in combobox if there was no data in file
            comboBox.SelectedIndex = -1;
            //}
        }

        /// <summary>
        /// Handles the Loaded event of the EntitiesDataGrid control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        private void EntitiesDataGrid_Loaded(object sender, RoutedEventArgs e)
        {
            model newDataContext = ((ViewModel)DataContext).XmlTemplateModel;
            this.EntitiesDataGrid.ItemsSource = newDataContext.entity.Where(ent => !ent.name.EndsWith("_mb")).ToList();
            //this.EntitiesDataGrid.ItemsSource = ApplicationController.ApplicationMainController.CurrentModel.Entities.Where(ent => !ent.Name.EndsWith("_mb")).ToList();
            this.EntitiesDataGrid.SelectedIndex = 0;
        }

        /// <summary>
        /// Handles the CellEditEnding event of the EntitiesDataGrid control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.Controls.DataGridCellEditEndingEventArgs"/> instance containing the event data.</param>
        private void EntitiesDataGrid_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            if (SelectedEntity != null)
            {
                List<modelEntity> entities = ((ViewModel)DataContext).XmlTemplateModel.entity;

                FrameworkElement element1 = AttributeDataGrid.Columns[0].GetCellContent(e.Row);
                TextBox entityNameTextBox = GetChildControl(element1, "EntityNameTextBox") as TextBox;
                if (entityNameTextBox != null)
                {
                    if (!string.IsNullOrEmpty(entityNameTextBox.Text))
                    {
                        bool isValid = char.IsLetter(entityNameTextBox.Text.FirstOrDefault()) && entityNameTextBox.Text.FirstOrDefault() == char.ToUpper(entityNameTextBox.Text.FirstOrDefault());
                        if (isValid)
                        {
                            modelEntity ent = entities.FirstOrDefault(entity => entity.name == entityNameTextBox.Text);
                            if (entities != null && ent == null)
                            {
                                SelectedEntity.name = entityNameTextBox.Text;
                            }
                            else
                            {
                                if (ent != SelectedEntity)
                                {
                                    MessageBox.Show(MessageResources.NameBeenUsedEntity, MessageResources.Error, MessageBoxButton.OK, MessageBoxImage.Error);
                                }
                            }
                        }
                        else
                        {
                            MessageBox.Show(MessageResources.InvalidEntityName, MessageResources.Error, MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                    else
                    {
                        MessageBox.Show(MessageResources.EmptyEntityName, MessageResources.Error, MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
        }

        /// <summary>
        /// Handles the SelectionChanged event of the EntitiesDataGrid control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.Controls.SelectionChangedEventArgs"/> instance containing the event data.</param>
        private void EntitiesDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            this.SelectedEntity = (modelEntity)EntitiesDataGrid.SelectedItem;
            //ApplicationController.ApplicationMainController.CurrentModel.SelectedEntity = this.SelectedEntity;
            if (this.SelectedEntity != null)
            {
                if (this.SelectedEntity.attribute == null)
                {
                    this.SelectedEntity.attribute = new List<modelEntityAttribute>();
                }

                if (this.SelectedEntity.relationship == null)
                {
                    this.SelectedEntity.relationship = new List<modelEntityRelationship>();
                }

                this.Attributes = new ObservableCollection<modelEntityAttribute>(this.SelectedEntity.attribute.ToList());
                this.Relationships = new ObservableCollection<modelEntityRelationship>(this.SelectedEntity.relationship.ToList());
                this.CkbIsRoot.IsChecked = Helper.ValidateBoolFromString(SelectedEntity.isRoot == null ? "False" : SelectedEntity.isRoot);
                this.CkbIsRootRelated.IsChecked = Helper.ValidateBoolFromString(SelectedEntity.isRootRelated == null ? "False" : SelectedEntity.isRootRelated);
                this.ChbEnableTracing.IsChecked = Helper.ValidateBoolFromString(SelectedEntity.enableTracing == null ? "False" : SelectedEntity.enableTracing);
                this.TxtSyncOrder.Text = SelectedEntity.syncOrder;
                this.TxtDescription.Text = SelectedEntity.description;
                this.TxtFriendlyName.Text = SelectedEntity.friendlyName;
                this.CkbIsMediaEntity.IsChecked = Helper.ValidateBoolFromString(SelectedEntity.isMediaEntity == null ? "False" : SelectedEntity.isMediaEntity);
                this.txtBackendQuery.Text = SelectedEntity.backendQuery;
                this.RuleComboBox.SelectedItem = SelectedEntity.conflictResolutionRule;
                this.SyncComboBox.SelectedItem = SelectedEntity.syncType;
                this.AttributeDataGrid.ItemsSource = SelectedEntity.attribute.Where(a => !a.name.EndsWith("_mb")).ToList();
                this.AttributeDataGrid.SelectedIndex = 0;
                this.RelationshipDataGrid.ItemsSource = this.Relationships;
                this.RelationshipDataGrid.SelectedIndex = 0;
            }
            else
            {
                this.AttributeDataGrid.ItemsSource = new ObservableCollection<modelEntityAttribute>();
                this.AttributeDataGrid.SelectedIndex = -1;
                this.RelationshipDataGrid.ItemsSource = new ObservableCollection<modelEntityRelationship>();
                this.RuleComboBox.SelectedItem = ConflictResolutionRule.mbSyncWin;
                this.SyncComboBox.SelectedItem = SyncType.syncBothDirections;
                this.RelationshipDataGrid.SelectedIndex = -1;
                this.CkbIsRoot.IsChecked = false;
                this.CkbIsRootRelated.IsChecked = false;
                this.ChbEnableTracing.IsChecked = false;
                this.TxtSyncOrder.Text = "0";
                this.TxtDescription.Text = string.Empty;
                this.CkbIsMediaEntity.IsChecked = false;
                this.txtBackendQuery.Text = string.Empty;
            }
        }

        /// <summary>
        /// Handles the Loaded event of the EntityNameTextBox control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        private void EntityNameTextBox_Loaded(object sender, RoutedEventArgs e)
        {
            TextBox textBox = sender as TextBox;
            if (textBox != null && SelectedEntity != null)
            {
                textBox.Text = SelectedEntity.name;
                textBox.SelectAll();
                textBox.Focus();
            }
        }

        /// <summary>
        /// Handles the Click event of the AddEntityButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        private void AddEntityButton_Click(object sender, RoutedEventArgs e)
        {
            this.AddNewEntity();
        }

        /// <summary>
        /// Adds the new entity.
        /// </summary>
        public void AddNewEntity()
        {
            //EntityModel newEntity = new EntityModel(ApplicationController.ApplicationMainController.CurrentModel);
            modelEntity newEntity = new modelEntity();
            bool notListed = false;
            int i = 0;
            while (!notListed)
            {
                string tempName = string.Empty;
                if (i == 0)
                {
                    tempName = "Entity";
                }
                else
                {
                    tempName = string.Format("Entity{0}", i.ToString());
                }

                ++i;
                if (((ViewModel)DataContext).XmlTemplateModel.entity.FirstOrDefault(ent => ent.name == tempName) == null)
                {
                    notListed = true;
                    newEntity.name = tempName;
                    newEntity.friendlyName = tempName;
                }
            }
            List<modelEntity> entities = ((ViewModel)DataContext).XmlTemplateModel.entity;
            entities.Add(newEntity);
            this.EntitiesDataGrid.ItemsSource = new ObservableCollection<modelEntity>(entities.Where(e => !e.name.EndsWith("_mb")).ToList());
            this.EntitiesDataGrid.UpdateLayout();

            this.EntitiesDataGrid.SelectedItem = newEntity;
        }

        /// <summary>
        /// Handles the Click event of the DeleteEntityButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        private void DeleteEntityButton_Click(object sender, RoutedEventArgs e)
        {
            this.DeleteSelectedEntity();
        }

        /// <summary>
        /// Deletes the selected entity.
        /// </summary>
        public void DeleteSelectedEntity()
        {
            List<modelEntity> entities = ((ViewModel)DataContext).XmlTemplateModel.entity;
            if (this.EntitiesDataGrid.SelectedItem != null)
            {
                modelEntity entity = (modelEntity)this.EntitiesDataGrid.SelectedItem;
                if (entity != null)
                {
                    if (MessageBox.Show(MessageResources.DeleteEntityWarning, MessageResources.Confirmation, MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                    {
                        foreach (modelEntity ent in entities)
                        {
                            if (ent != entity)
                            {
                                List<modelEntityRelationship> relationships = ent.relationship;
                                foreach (modelEntityRelationship rel in relationships)
                                {
                                    if (rel.destinationEntity == entity.name)
                                    {
                                        rel.destinationEntity = WPFDesigner_XML.Resources.NoDestination;
                                        if (rel.inverseEntity != WPFDesigner_XML.Resources.NoInverse)
                                        {
                                            rel.inverseEntity = WPFDesigner_XML.Resources.NoInverse;
                                        }
                                    }
                                }
                            }
                        }

                        entities.Remove(entity);
                        this.EntitiesDataGrid.ItemsSource = new ObservableCollection<modelEntity>(entities.Where(e => !e.name.EndsWith("_mb")).ToList());
                        this.EntitiesDataGrid.UpdateLayout();
                        this.EntitiesDataGrid.SelectedIndex = 0;
                    }
                }
            }
        }

        /// <summary>
        /// Handles the Click event of the AddAttributeButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        private void AddAttributeButton_Click(object sender, RoutedEventArgs e)
        {
            this.AddNewAttribute();
        }

        /// <summary>
        /// Adds the new attribute.
        /// </summary>
        public void AddNewAttribute()
        {
            if (SelectedEntity != null)
            {
                modelEntityAttribute attr = new modelEntityAttribute(SelectedEntity);
                //EntityAttributeModel attr = new EntityAttributeModel(SelectedEntity);
                bool notListed = false;
                int i = 0;
                while (!notListed)
                {
                    string tempName = string.Empty;
                    if (i == 0)
                    {
                        tempName = "attribute";
                    }
                    else
                    {
                        tempName = string.Format("attribute{0}", i.ToString());
                    }

                    ++i;
                    if (SelectedEntity.attribute.FirstOrDefault(a => a.name == tempName) == null)
                    {
                        notListed = true;
                        attr.name = tempName;
                    }
                }

                attr.attributeType = Helper.ConvertAttTypeToString(AttributeType.Undefined);
                SelectedEntity.attribute.Add(attr);
                this.Attributes.Add(attr);
                this.AttributeDataGrid.ItemsSource = this.Attributes.Where(a => !a.name.EndsWith("_mb")).ToList();
                this.AttributeDataGrid.UpdateLayout();

                this.AttributeDataGrid.SelectedItem = attr;
            }
        }

        /// <summary>
        /// Handles the Click event of the DeleteAttributeButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        private void DeleteAttributeButton_Click(object sender, RoutedEventArgs e)
        {
            this.DeleteSelectedAttribute();
        }

        /// <summary>
        /// Deletes the selected attribute.
        /// </summary>
        public void DeleteSelectedAttribute()
        {
            if (SelectedEntity != null)
            {
                modelEntityAttribute attr = (modelEntityAttribute)this.AttributeDataGrid.SelectedItem;
                if (attr != null)
                {
                    if (!attr.isDefault.Equals("True") ? true : false)
                    {
                        if (MessageBox.Show(MessageResources.DeleteAttributeWarning, MessageResources.Confirmation, MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                        {
                            DeleteFunctionality newDelete = new DeleteFunctionality(SelectedEntity);
                            newDelete.DeleteAttribute(attr);
                            this.Attributes.Remove(attr);
                            this.AttributeDataGrid.ItemsSource = this.Attributes.Where(a => !a.name.EndsWith("_mb")).ToList();
                            this.AttributeDataGrid.UpdateLayout();
                            this.AttributeDataGrid.SelectedIndex = 0;
                        }
                    }
                    else
                    {
                        MessageBox.Show(MessageResources.DefaultAttributeChanges, MessageResources.Information, MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
            }
        }

        /// <summary>
        /// Handles the SelectionChanged event of the RuleComboBox control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="SelectionChangedEventArgs" /> instance containing the event data.</param>
        private void RuleComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (SelectedEntity != null)
            {
                SelectedEntity.conflictResolutionRule = Helper.ConvertConflictResRuleToString((ConflictResolutionRule)this.RuleComboBox.SelectedItem);
            }
        }

        /// <summary>
        /// Handles the TextChanged event of the TxtDescription control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="TextChangedEventArgs" /> instance containing the event data.</param>
        private void TxtDescription_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox tb = sender as TextBox;
            if (tb != null)
            {
                SelectedEntity.description = tb.Text;
            }
        }

        private void TxtFriendlyName_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox tb = sender as TextBox;
            if (tb != null)
            {
                SelectedEntity.friendlyName = tb.Text;
            }
        }

        private void ChbEnableTracing_Checked(object sender, RoutedEventArgs e)
        {
            if (SelectedEntity != null)
            {
                SelectedEntity.enableTracing = "True";
            }
        }

        private void ChbEnableTracing_Unchecked(object sender, RoutedEventArgs e)
        {
            if (SelectedEntity != null)
            {
                SelectedEntity.enableTracing = "False";
            }
        }

        private void CkbIsRootRelated_Checked(object sender, RoutedEventArgs e)
        {
            if (SelectedEntity != null)
            {
                SelectedEntity.isRootRelated = "True";
                AddAttibute("parcelNbr_mb", "string");
                this.AttributeDataGrid.ItemsSource = null;
                this.AttributeDataGrid.ItemsSource = this.Attributes.Where(a => !a.name.EndsWith("_mb")).ToList();
            }
        }

        private void CkbIsRootRelated_Unchecked(object sender, RoutedEventArgs e)
        {
            if (SelectedEntity != null)
            {
                SelectedEntity.isRootRelated = "False";
                RemoveAttribute("parcelNbr_mb");
                this.AttributeDataGrid.ItemsSource = null;
                this.AttributeDataGrid.ItemsSource = this.Attributes.Where(a => !a.name.EndsWith("_mb")).ToList();
            }
        }

        private void txtBackendQuery_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox tb = sender as TextBox;
            if (tb != null)
            {
                SelectedEntity.backendQuery = tb.Text;
            }
        }

        private void SyncComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this.SelectedEntity != null && SyncComboBox.SelectedItem != null)
            {
                this.SelectedEntity.syncType = Helper.ConvertSyncTypeToString((SyncType)this.SyncComboBox.SelectedItem);
            }
        }

        /// <summary>
        /// Handles the Checked event of the CkbIsRoot control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs" /> instance containing the event data.</param>
        private void CkbIsRoot_Checked(object sender, RoutedEventArgs e)
        {
            if (SelectedEntity != null)
            {
                SelectedEntity.isRoot = "True";
                AddAttibute("editState_mb", "int");
                this.AttributeDataGrid.ItemsSource = null;
                this.AttributeDataGrid.ItemsSource = this.Attributes.Where(a => !a.name.EndsWith("_mb")).ToList();
                //ApplicationController.ApplicationMainController.CurrentModel.SelectedEntity.AllowForMaps = true;
                if (!SelectedEntity.isRoot.Equals("True"))
                {
                    this.CkbIsRoot.IsChecked = false;
                }
            }
        }

        /// <summary>
        /// Handles the Unchecked event of the CkbIsRoot control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs" /> instance containing the event data.</param>
        private void CkbIsRoot_Unchecked(object sender, RoutedEventArgs e)
        {
            if (SelectedEntity != null)
            {
                SelectedEntity.isRoot = "False";
                RemoveAttribute("editState_mb");
                this.AttributeDataGrid.ItemsSource = null;
                this.AttributeDataGrid.ItemsSource = this.Attributes.Where(a => !a.name.EndsWith("_mb")).ToList();
                //ApplicationController.ApplicationMainController.CurrentModel.SelectedEntity.AllowForMaps = false;
            }
        }

        /// <summary>
        /// Handles the TextChanged event of the TxtSyncOrder control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="TextChangedEventArgs" /> instance containing the event data.</param>
        private void TxtSyncOrder_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox tb = sender as TextBox;
            int value;
            if (SelectedEntity != null)
            {
                if (Int32.TryParse(tb.Text, out value))
                {
                    SelectedEntity.syncOrder = value.ToString();
                }
                else
                {
                    tb.Text = SelectedEntity.syncOrder;
                }
            }
        }

        /// <summary>
        /// Handles the Checked event of the CkbIsMediaEntity control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs" /> instance containing the event data.</param>
        private void CkbIsMediaEntity_Checked(object sender, RoutedEventArgs e)
        {
            if (SelectedEntity != null)
            {
                SelectedEntity.isMediaEntity = "True";
                AddAttibute("imageSourceURI_mb", "string");
                AddAttibute("imageType_mb", "string");
                AddAttibute("imageFilter_mb", "int");
                AddAttibute("latitude_mb", "double");
                AddAttibute("longitude_mb", "double");
                AddAttibute("legend_mb", "string");
                AddAttibute("user_mb", "string");
                AddAttibute("isPrincipal_mb", "boolean");
                AddAttibute("isPrivate_mb", "boolean");
                AddAttibute("comments_mb", "string");
                AddAttibute("orderedBy_mb", "int");
                AddAttibute("key_mb", "string");
                AddAttibute("createdDate_mb", "date");
                AddAttibute("updatedDate_mb", "date");
                this.AttributeDataGrid.ItemsSource = null;
                this.AttributeDataGrid.ItemsSource = this.Attributes.Where(a => !a.name.EndsWith("_mb")).ToList();
            }
        }



        /// <summary>
        /// Handles the Unchecked event of the CkbIsMediaEntity control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs" /> instance containing the event data.</param>
        private void CkbIsMediaEntity_Unchecked(object sender, RoutedEventArgs e)
        {
            if (SelectedEntity != null)
            {
                SelectedEntity.isMediaEntity = "False";
                RemoveAttribute("imageSourceURI_mb");
                RemoveAttribute("imageType_mb");
                RemoveAttribute("imageFilter_mb");
                RemoveAttribute("latitude_mb");
                RemoveAttribute("longitude_mb");
                RemoveAttribute("legend_mb");
                RemoveAttribute("user_mb");
                RemoveAttribute("isPrincipal_mb");
                RemoveAttribute("isPrivate_mb");
                RemoveAttribute("comments_mb");
                RemoveAttribute("orderedBy_mb");
                RemoveAttribute("key_mb");
                RemoveAttribute("createdDate_mb");
                RemoveAttribute("updatedDate_mb");
                this.AttributeDataGrid.ItemsSource = null;
                this.AttributeDataGrid.ItemsSource = this.Attributes.Where(a => !a.name.EndsWith("_mb")).ToList();
            }
        }

        private void AddAttibute(string attrName, string attributeType)
        {
            modelEntityAttribute attr = null;
            switch (attributeType)
            {
                case "string":
                    if (!SelectedEntity.attribute.Any(a => a.name == attrName))
                        attr = new modelEntityAttribute(SelectedEntity)
                        {
                            attributeType = Helper.ConvertAttTypeToString(AttributeType.String),
                            name = attrName,
                            //attributeInfo = new AttributeInfoString() { IsIndexed = false, MaxChars = 4000 },
                            isDefault = "True"
                        };
                    break;
                case "text":
                    if (!SelectedEntity.attribute.Any(a => a.name == attrName))
                        attr = new modelEntityAttribute(SelectedEntity)
                        {
                            attributeType = Helper.ConvertAttTypeToString(AttributeType.Text),
                            name = attrName,
                            //AttributeInfo = new AttributeInfoString() { IsIndexed = false, MaxChars = 4000 },
                            isDefault = "True"
                        };
                    break;

                case "double":
                    if (!SelectedEntity.attribute.Any(a => a.name == attrName))
                        attr = new modelEntityAttribute(SelectedEntity)
                        {
                            attributeType = Helper.ConvertAttTypeToString(AttributeType.Double),
                            name = attrName,
                            //AttributeInfo = new AttributeInfoDouble() { IsIndexed = false },
                            isDefault = "True"
                        };
                    break;
                case "boolean":
                    if (!SelectedEntity.attribute.Any(a => a.name == attrName))
                        attr = new modelEntityAttribute(SelectedEntity)
                        {
                            attributeType = Helper.ConvertAttTypeToString(AttributeType.Boolean),
                            name = attrName,
                            //AttributeInfo = new AttributeInfo() { IsIndexed = false },
                            isDefault = "True"
                        };
                    break;
                case "int":
                    if (!SelectedEntity.attribute.Any(a => a.name == attrName))
                        attr = new modelEntityAttribute(SelectedEntity)
                        {
                            attributeType = Helper.ConvertAttTypeToString(AttributeType.Integer32),
                            name = attrName,
                            //AttributeInfo = new AttributeInfoInteger() { IsIndexed = false },
                            isDefault = "True"
                        };
                    break;

                case "date":
                    if (!SelectedEntity.attribute.Any(a => a.name == attrName))
                        attr = new modelEntityAttribute(SelectedEntity)
                        {
                            attributeType = Helper.ConvertAttTypeToString(AttributeType.Date),
                            name = attrName,
                            //AttributeInfo = new AttributeInfoDate() { IsIndexed = false },
                            isDefault = "True"
                        };
                    break;

                default:
                    break;
            }

            if (attr != null)
            {
                SelectedEntity.attribute.Add(attr);
                this.Attributes.Add(attr);
            }
        }

        private void RemoveAttribute(string attrName)
        {
            modelEntityAttribute attr;
            attr = SelectedEntity.attribute.FirstOrDefault(a => a.name == attrName);
            if (attr != null)
            {
                SelectedEntity.attribute.Remove(attr);
                this.Attributes.Remove(attr);
            }
        }

        /// <summary> 
        /// Gets a child control. 
        /// </summary>
        /// <param name="parent">      The parent. </param>
        /// <param name="controlName"> Name of the control. </param>
        /// <returns> The child control. </returns>
        private object GetChildControl(DependencyObject parent, string controlName)
        {
            Object tempObj = null;
            int count = VisualTreeHelper.GetChildrenCount(parent);
            for (int counter = 0; counter < count; counter++)
            {
                tempObj = VisualTreeHelper.GetChild(parent, counter);

                if ((tempObj as DependencyObject).GetValue(NameProperty).ToString() == controlName)
                {
                    return tempObj;
                }
                else
                {
                    tempObj = GetChildControl(tempObj as DependencyObject, controlName);
                    if (tempObj != null)
                    {
                        return tempObj;
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// Handles the CellEditEnding event of the AttributeDataGrid control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.Controls.DataGridCellEditEndingEventArgs"/> Instance containing the event data.</param>
        private void AttributeDataGrid_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            if (this.SelectedAttribute != null)
            {
                if (!Helper.ValidateBoolFromString(this.SelectedAttribute.isDefault == null ? "False" : this.SelectedAttribute.isDefault))
                {
                    FrameworkElement element1 = AttributeDataGrid.Columns[0].GetCellContent(e.Row);
                    modelEntityAttribute attr = null;
                    TextBox attributeNameTextBox = GetChildControl(element1, "AttributeNameTextBox") as TextBox;
                    if (attributeNameTextBox != null)
                    {
                        if (this.SelectedEntity != null)
                        {
                            attr = this.SelectedEntity.attribute.FirstOrDefault(a => a.name == attributeNameTextBox.Text);
                        }

                        if (attr == null && this.SelectedAttribute != null)
                        {
                            if (!string.IsNullOrEmpty(attributeNameTextBox.Text))
                            {
                                bool isValid = char.IsLetter(attributeNameTextBox.Text.FirstOrDefault()) && attributeNameTextBox.Text.FirstOrDefault() == char.ToLower(attributeNameTextBox.Text.FirstOrDefault());
                                if (isValid)
                                {
                                    isValid = !reservedWords.Contains(attributeNameTextBox.Text);

                                    if (isValid)
                                    {
                                        this.SelectedAttribute.name = attributeNameTextBox.Text;
                                    }
                                    else
                                    {
                                        MessageBox.Show(MessageResources.ReservedWordUsed, MessageResources.Error, MessageBoxButton.OK, MessageBoxImage.Error);
                                    }
                                }
                                else
                                {
                                    MessageBox.Show(MessageResources.InvalidAttributeName, MessageResources.Error, MessageBoxButton.OK, MessageBoxImage.Error);
                                }
                            }
                            else
                            {
                                MessageBox.Show(MessageResources.EmptyAttributeName, MessageResources.Error, MessageBoxButton.OK, MessageBoxImage.Error);
                            }
                        }
                        else
                        {
                            if (attr != this.SelectedAttribute)
                            {
                                MessageBox.Show(MessageResources.NameBeenUsedAttribute, MessageResources.Error, MessageBoxButton.OK, MessageBoxImage.Error);
                            }
                        }
                    }

                    FrameworkElement element2 = AttributeDataGrid.Columns[1].GetCellContent(e.Row);
                    ComboBox attributeTypeComboBox = GetChildControl(element2, "TypeComboBox") as ComboBox;
                    if (attributeTypeComboBox != null)
                    {
                        if (attr == null && attributeNameTextBox == null)
                        {
                            attr = this.SelectedEntity.attribute.FirstOrDefault(a => a.name == this.SelectedAttribute.name);
                        }

                        if (attr != null)
                        {
                            if (!attr.attributeType.Equals(attributeTypeComboBox.SelectedItem.ToString()))
                            {
                                attr.attributeType = attributeTypeComboBox.SelectedItem.ToString();
                                //attr.AttributeInfo = DatabaseManagementHelper.GetAttributeInfoFromType(attr.AttributeType);
                                this.AttributeInformation.SetAttribute(attr);
                            }
                        }

                        this.SelectedAttribute.attributeType = attributeTypeComboBox.SelectedItem.ToString();
                    }
                }
                else
                {
                    MessageBox.Show(MessageResources.DefaultAttributeChanges, MessageResources.Information, MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
        }

        /// <summary>
        /// Handles the SelectionChanged event of the AttributeDataGrid control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.Controls.SelectionChangedEventArgs"/> Instance containing the event data.</param>
        private void AttributeDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            this.SelectedAttribute = (modelEntityAttribute)this.AttributeDataGrid.SelectedItem;

            if (this.AttributeDataGrid.SelectedItem != null)
            {
                this.SelectedAttribute = (modelEntityAttribute)this.AttributeDataGrid.SelectedItem;
                if (this.SelectedAttribute != null)
                {
                    //this.SelectedAttribute.AttributeTypeChanged += new EventHandler(SelectedAttribute_AttributeTypeChanged);
                    this.AttributeInformation.SetAttribute(this.SelectedAttribute);
                    this.AttributeInformation.IsEnabled = !Helper.ValidateBoolFromString(this.SelectedAttribute.isDefault == null ? "False" : this.SelectedAttribute.isDefault);
                }
            }
            else
            {
                this.AttributeInformation.SetAttribute(null);
            }
        }

        /// <summary>
        /// Handles the AttributeTypeChanged event of the SelectedAttribute control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void SelectedAttribute_AttributeTypeChanged(object sender, EventArgs e)
        {
            this.EntitiesDataGrid.UpdateLayout();
        }

        /// <summary>
        /// Handles the LoadingRow event of the AttributeDataGrid control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.Controls.DataGridRowEventArgs"/> instance containing the event data.</param>
        private void AttributeDataGrid_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            modelEntityAttribute attribute = (modelEntityAttribute)e.Row.DataContext;
            if (attribute != null)
            {
                if (Helper.ValidateBoolFromString(attribute.isDefault == null ? "False" : attribute.isDefault))
                    e.Row.Background = new SolidColorBrush(Colors.AliceBlue);
            }
        }

        /// <summary>
        /// Handles the SelectionChanged event of the RelationshipDataGrid control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.Controls.SelectionChangedEventArgs"/> instance containing the event data.</param>
        private void RelationshipDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            this.SelectedRelationship = (modelEntityRelationship) this.RelationshipDataGrid.SelectedItem;
        }

        /// <summary>
        /// Handles the Loaded event of the AttributeNameTextBox control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        private void AttributeNameTextBox_Loaded(object sender, RoutedEventArgs e)
        {
            TextBox textBox = sender as TextBox;
            if (textBox != null && this.SelectedAttribute != null)
            {
                textBox.Text = this.SelectedAttribute.name;
                textBox.SelectAll();
                textBox.Focus();
            }
        }

        /// <summary>
        /// Handles the MouseDoubleClick event of the RelationshipDataGrid control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.Input.MouseButtonEventArgs"/> instance containing the event data.</param>
        private void RelationshipDataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (sender != null)
            {
                DataGrid grid = sender as DataGrid;
                if (grid != null && grid.SelectedItems != null && grid.SelectedItems.Count == 1)
                {
                    modelEntityRelationship selectedRelationship = grid.SelectedItem as modelEntityRelationship;
                    RelationshipManagementWindow relWindow = new RelationshipManagementWindow(selectedRelationship, this);
                    //relWindow.Owner = App.Current.MainWindow;
                    bool? result = relWindow.ShowDialog();

                    if (result == true)
                    {
                        this.RelationshipDataGrid.ItemsSource = this.Relationships;
                        this.RelationshipDataGrid.UpdateLayout();
                    }
                }
            }
        }

        /// <summary>
        /// Handles the Click event of the AddRelationshipButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        private void AddRelationshipButton_Click(object sender, RoutedEventArgs e)
        {
            this.AddNewRelationship();
        }

        /// <summary>
        /// Adds the new relationship.
        /// </summary>
        public void AddNewRelationship()
        {
            if (SelectedEntity != null)
            {
                modelEntityRelationship rel = new modelEntityRelationship();
                bool notListed = false;
                int i = 0;
                while (!notListed)
                {
                    string tempName = string.Empty;
                    if (i == 0)
                    {
                        tempName = "relationship";
                    }
                    else
                    {
                        tempName = string.Format("relationship{0}", i.ToString());
                    }

                    ++i;
                    if (SelectedEntity.relationship.FirstOrDefault(a => a.name == tempName) == null)
                    {
                        notListed = true;
                        rel.name = tempName;
                    }
                }

                RelationshipManagementWindow relWindow = new RelationshipManagementWindow(rel, this);
                //relWindow.Owner = App.Current.MainWindow;
                bool? result = relWindow.ShowDialog();

                if (result == true)
                {
                    SelectedEntity.relationship.Add(relWindow.EntityRelationshipModel);
                    this.Relationships.Add(relWindow.EntityRelationshipModel);
                    this.RelationshipDataGrid.ItemsSource = this.Relationships;
                    this.RelationshipDataGrid.UpdateLayout();
                }
            }
        }

        /// <summary>
        /// Handles the Click event of the DeleteRelationshipButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        private void DeleteRelationshipButton_Click(object sender, RoutedEventArgs e)
        {
            this.DeleteSelectedRelationship();
        }

        /// <summary>
        /// Deletes the selected relationship.
        /// </summary>
        public void DeleteSelectedRelationship()
        {
            if (this.RelationshipDataGrid.SelectedItem != null)
            {
                List<modelEntity> entities = ((ViewModel)DataContext).XmlTemplateModel.entity;

                modelEntityRelationship selectedRelationship = this.RelationshipDataGrid.SelectedItem as modelEntityRelationship;
                if (selectedRelationship != null)
                {
                    if (MessageBox.Show(MessageResources.DeleteRelationshipWarning, MessageResources.Confirmation, MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                    {
                        foreach (modelEntity ent in entities)
                        {
                            if (ent != SelectedEntity)
                            {
                                List<modelEntityRelationship> relationships = ent.relationship.ToList();
                                foreach (modelEntityRelationship rel in relationships)
                                {
                                    if (rel.destinationEntity == SelectedEntity.name && rel.inverseEntity == selectedRelationship.name)
                                    {
                                        rel.inverseEntity = WPFDesigner_XML.Resources.NoInverse;
                                    }
                                }
                            }
                        }

                        DeleteFunctionality newDelete = new DeleteFunctionality(SelectedEntity);
                        newDelete.DeleteRelationship(selectedRelationship);
                        this.Relationships.Remove(selectedRelationship);
                        this.RelationshipDataGrid.ItemsSource = this.Relationships;
                        this.RelationshipDataGrid.UpdateLayout();
                        this.RelationshipDataGrid.SelectedIndex = 0;
                    }
                }
            }
        }

        private void SyncComboBox_Loaded(object sender, RoutedEventArgs e)
        {
            List<SyncType> newSyncType = new List<SyncType>();
            newSyncType.Add(SyncType.syncBothDirections);
            newSyncType.Add(SyncType.syncToDevice);
            newSyncType.Add(SyncType.syncToMiddleTier);

            SyncComboBox.ItemsSource = newSyncType;
        }

        private void AttributeTypeComboBox_Loaded(object sender, RoutedEventArgs e)
        {
            List<AttributeType> attTypeList = new List<AttributeType>();
            attTypeList.Add(AttributeType.Undefined);
            attTypeList.Add(AttributeType.Integer16);
            attTypeList.Add(AttributeType.Integer32);
            attTypeList.Add(AttributeType.Integer64);
            attTypeList.Add(AttributeType.Boolean);
            attTypeList.Add(AttributeType.Double);
            attTypeList.Add(AttributeType.String);
            attTypeList.Add(AttributeType.Date);
            attTypeList.Add(AttributeType.BLOB);
            attTypeList.Add(AttributeType.GUID);
            attTypeList.Add(AttributeType.Text);

            ComboBox attTypeComboBox = (ComboBox)sender;
            attTypeComboBox.ItemsSource = attTypeList;
        }

        private void TypeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox attTypeComboBox = (ComboBox)sender;
            if (this.SelectedAttribute != null)
            {
                this.SelectedAttribute.attributeType = Helper.ConvertAttTypeToString((AttributeType)attTypeComboBox.SelectedItem);
            }
        }

        private void AttributeDescriptionTextBox_Loaded(object sender, RoutedEventArgs e)
        {
            TextBox textBox = sender as TextBox;
            if (textBox != null && this.SelectedAttribute != null)
            {
                textBox.Text = this.SelectedAttribute.description;
                textBox.SelectAll();
                textBox.Focus();
            }
        }

        private void AttributeDescriptionTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            TextBox descriptionTB = (TextBox)sender;
            if (SelectedAttribute != null)
            {
                SelectedAttribute.description = descriptionTB.Text;
            }
        }

        private void AttributeNameTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            TextBox nameTB = (TextBox)sender;
            if (SelectedAttribute != null)
            {
                SelectedAttribute.name = nameTB.Text;
            }
        }

        private void TxtFriendlyName_LostFocus(object sender, RoutedEventArgs e)
        {
            TextBox friendlyName = (TextBox)sender;
            //this.SelectedEntity = (modelEntity)EntitiesDataGrid.SelectedItem;
            if (this.SelectedEntity != null)
            {
                SelectedEntity.friendlyName = friendlyName.Text;
            }
        }

        private void SaveToXML_Btn_Click(object sender, RoutedEventArgs e)
        {
            ((ViewModel)DataContext).SaveModelToXmlModel(string.Empty);
        }

        private void AttributeIndexedCheckBox_Loaded(object sender, RoutedEventArgs e)
        {
            CheckBox checkBox = sender as CheckBox;
            if (checkBox != null && this.SelectedAttribute != null)
            {
                checkBox.IsChecked = Helper.ValidateBoolFromString( this.SelectedAttribute.indexed);
                checkBox.Focus();
            }
        }

        private void AttributeIndexedCheckBox_LostFocus(object sender, RoutedEventArgs e)
        {
            CheckBox indexedCB = (CheckBox)sender;
            if (SelectedAttribute != null)
            {
                SelectedAttribute.indexed = Helper.ConvertBoolToString( indexedCB.IsChecked);
            }
        }

        private void AttributeIsClientKeyCheckBox_Loaded(object sender, RoutedEventArgs e)
        {
            CheckBox checkBox = sender as CheckBox;
            if (checkBox != null && this.SelectedAttribute != null)
            {
                checkBox.IsChecked = Helper.ValidateBoolFromString(this.SelectedAttribute.isClientKey);
                checkBox.Focus();
            }
        }

        private void AttributeIsClientKeyCheckBox_LostFocus(object sender, RoutedEventArgs e)
        {
            CheckBox indexedCB = (CheckBox)sender;
            if (SelectedAttribute != null)
            {
                SelectedAttribute.isClientKey = Helper.ConvertBoolToString(indexedCB.IsChecked);
            }
        }

        private void AttributeToManyCheckBox_Loaded(object sender, RoutedEventArgs e)
        {
            CheckBox checkBox = sender as CheckBox;
            if (checkBox != null && this.SelectedRelationship != null)
            {
                checkBox.IsChecked = Helper.ValidateBoolFromString(this.SelectedRelationship.toMany);
                checkBox.Focus();
            }
        }

        private void AttributeToManyCheckBox_LostFocus(object sender, RoutedEventArgs e)
        {
            CheckBox indexedCB = (CheckBox)sender;
            if (SelectedRelationship != null)
            {
                SelectedRelationship.toMany = Helper.ConvertBoolToString(indexedCB.IsChecked);
            }
        }

        private void CreateDB_Btn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                CreateDB();
            }
            catch (Exception ex)
            {
                var error = ex.Message;
            }
        }

        private void CreateDB()
        {
        }
    }
}
