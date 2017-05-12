﻿/***************************************************************************

Copyright (c) Microsoft Corporation. All rights reserved.
THIS CODE IS PROVIDED *AS IS* WITHOUT WARRANTY OF
ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING ANY
IMPLIED WARRANTIES OF FITNESS FOR A PARTICULAR
PURPOSE, MERCHANTABILITY, OR NON-INFRINGEMENT.

***************************************************************************/

using DBXTemplateDesigner;
using MobiseStudio.Resources;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WPFDesigner_XML;

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
            /*if (ApplicationController.ApplicationMainController.CurrentModel.SelectedEntity != null)
            {
                FrameworkElement element1 = AttributeDataGrid.Columns[0].GetCellContent(e.Row);
                TextBox entityNameTextBox = GetChildControl(element1, "EntityNameTextBox") as TextBox;
                if (entityNameTextBox != null)
                {
                    if (!string.IsNullOrEmpty(entityNameTextBox.Text))
                    {
                        bool isValid = char.IsLetter(entityNameTextBox.Text.FirstOrDefault()) && entityNameTextBox.Text.FirstOrDefault() == char.ToUpper(entityNameTextBox.Text.FirstOrDefault());
                        if (isValid)
                        {
                            EntityModel ent = ApplicationController.ApplicationMainController.CurrentModel.Entities.FirstOrDefault(entity => entity.Name == entityNameTextBox.Text);
                            if (ApplicationController.ApplicationMainController.CurrentModel.Entities != null && ent == null)
                            {
                                ApplicationController.ApplicationMainController.CurrentModel.SelectedEntity.Name = entityNameTextBox.Text;
                            }
                            else
                            {
                                if (ent != ApplicationController.ApplicationMainController.CurrentModel.SelectedEntity)
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
            }*/
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
                this.CkbIsRoot.IsChecked = Helper.ValidateBoolFromString( SelectedEntity.isRoot);
                this.CkbIsRootRelated.IsChecked = Helper.ValidateBoolFromString(SelectedEntity.isRootRelated);
                this.ChbEnableTracing.IsChecked = Helper.ValidateBoolFromString( SelectedEntity.enableTracing);
                this.TxtSyncOrder.Text = SelectedEntity.syncOrder;
                this.TxtDescription.Text = SelectedEntity.description;
                this.TxtFriendlyName.Text = SelectedEntity.friendlyName;
                this.CkbIsMediaEntity.IsChecked = Helper.ValidateBoolFromString(SelectedEntity.isMediaEntity);
                this.txtBackendQuery.Text = SelectedEntity.backendQuery;
                /*this.RuleComboBox.SelectedItem = SelectedEntity.conflictResolutionRule;
                this.SyncComboBox.SelectedItem = SelectedEntity.syncType;*/
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
                /*this.RuleComboBox.SelectedItem = ConflictResolutionRule.mbSyncWin;
                this.SyncComboBox.SelectedItem = SyncType.syncBothDirections;*/
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
            /*if (textBox != null && ApplicationController.ApplicationMainController.CurrentModel.SelectedEntity != null)
            {
                textBox.Text = ApplicationController.ApplicationMainController.CurrentModel.SelectedEntity.Name;
                textBox.SelectAll();
                textBox.Focus();
            }*/
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
            /*EntityModel newEntity = new EntityModel(ApplicationController.ApplicationMainController.CurrentModel);
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
                if (ApplicationController.ApplicationMainController.CurrentModel.Entities.FirstOrDefault(ent => ent.Name == tempName) == null)
                {
                    notListed = true;
                    newEntity.Name = tempName;
                }
            }

            ApplicationController.ApplicationMainController.CurrentModel.Entities.Add(newEntity);
            this.EntitiesDataGrid.ItemsSource = new ObservableCollection<EntityModel>(ApplicationController.ApplicationMainController.CurrentModel.Entities.Where(e => !e.Name.EndsWith("_mb")).ToList());
            this.EntitiesDataGrid.UpdateLayout();

            this.EntitiesDataGrid.SelectedItem = newEntity;*/
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
            if (this.EntitiesDataGrid.SelectedItem != null)
            {
                /*EntityModel entity = this.EntitiesDataGrid.SelectedItem as EntityModel;
                if (entity != null)
                {
                    if (MessageBox.Show(MessageResources.DeleteEntityWarning, MessageResources.Confirmation, MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                    {
                        foreach (EntityModel ent in ApplicationController.ApplicationMainController.CurrentModel.Entities)
                        {
                            if (ent != entity)
                            {
                                List<EntityRelationshipModel> relationships = ent.Relationships.ToList();
                                foreach (EntityRelationshipModel rel in relationships)
                                {
                                    if (rel.TargetTableName == entity.Name)
                                    {
                                        rel.TargetTableName = Resource.NoDestination;
                                        if (rel.InverseRelationshipName != Resource.NoInverse)
                                        {
                                            rel.InverseRelationshipName = Resource.NoInverse;
                                        }
                                    }
                                }
                            }
                        }

                        ApplicationController.ApplicationMainController.CurrentModel.Entities.Remove(entity);
                        this.EntitiesDataGrid.ItemsSource = new ObservableCollection<EntityModel>(ApplicationController.ApplicationMainController.CurrentModel.Entities.Where(e => !e.Name.EndsWith("_mb")).ToList());
                        this.EntitiesDataGrid.UpdateLayout();
                        this.EntitiesDataGrid.SelectedIndex = 0;
                    }
                }*/
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
            /*if (ApplicationController.ApplicationMainController.CurrentModel.SelectedEntity != null)
            {
                EntityAttributeModel attr = new EntityAttributeModel(ApplicationController.ApplicationMainController.CurrentModel.SelectedEntity);
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
                    if (ApplicationController.ApplicationMainController.CurrentModel.SelectedEntity.Attributes.FirstOrDefault(a => a.Name == tempName) == null)
                    {
                        notListed = true;
                        attr.Name = tempName;
                    }
                }

                attr.AttributeType = AttributeType.Undefined;
                ApplicationController.ApplicationMainController.CurrentModel.SelectedEntity.Attributes.Add(attr);
                this.Attributes.Add(attr);
                this.AttributeDataGrid.ItemsSource = this.Attributes.Where(a => !a.Name.EndsWith("_mb")).ToList();
                this.AttributeDataGrid.UpdateLayout();

                this.AttributeDataGrid.SelectedItem = attr;
            }*/
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
            /*if (ApplicationController.ApplicationMainController.CurrentModel.SelectedEntity != null)
            {
                EntityAttributeModel attr = this.AttributeDataGrid.SelectedItem as EntityAttributeModel;
                if (attr != null)
                {
                    if (!attr.IsDefault)
                    {
                        if (MessageBox.Show(MessageResources.DeleteAttributeWarning, MessageResources.Confirmation, MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                        {
                            ApplicationController.ApplicationMainController.CurrentModel.SelectedEntity.DeleteAttribute(attr);
                            this.Attributes.Remove(attr);
                            this.AttributeDataGrid.ItemsSource = this.Attributes.Where(a => !a.Name.EndsWith("_mb")).ToList();
                            this.AttributeDataGrid.UpdateLayout();
                            this.AttributeDataGrid.SelectedIndex = 0;
                        }
                    }
                    else
                    {
                        MessageBox.Show(MessageResources.DefaultAttributeChanges, MessageResources.Information, MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
            }*/
        }

        /// <summary>
        /// Handles the SelectionChanged event of the RuleComboBox control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="SelectionChangedEventArgs" /> instance containing the event data.</param>
        private void RuleComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            /*if (ApplicationController.ApplicationMainController.CurrentModel.SelectedEntity != null)
            {
                ApplicationController.ApplicationMainController.CurrentModel.SelectedEntity.ConflictResolutionRule = (ConflictResolutionRule)this.RuleComboBox.SelectedItem;
            }*/
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
                //ApplicationController.ApplicationMainController.CurrentModel.SelectedEntity.Description = tb.Text;
            }
        }

        private void TxtFriendlyName_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox tb = sender as TextBox;
            if (tb != null)
            {
                //ApplicationController.ApplicationMainController.CurrentModel.SelectedEntity.FriendlyName = tb.Text;
            }
        }

        private void ChbEnableTracing_Checked(object sender, RoutedEventArgs e)
        {
            /*if (ApplicationController.ApplicationMainController.CurrentModel.SelectedEntity != null)
            {
                ApplicationController.ApplicationMainController.CurrentModel.SelectedEntity.EnableTracing = true;
            }*/
        }

        private void ChbEnableTracing_Unchecked(object sender, RoutedEventArgs e)
        {
            /*if (ApplicationController.ApplicationMainController.CurrentModel.SelectedEntity != null)
            {
                ApplicationController.ApplicationMainController.CurrentModel.SelectedEntity.EnableTracing = false;
            }*/
        }

        private void CkbIsRootRelated_Checked(object sender, RoutedEventArgs e)
        {
            /*if (ApplicationController.ApplicationMainController.CurrentModel.SelectedEntity != null)
            {
                ApplicationController.ApplicationMainController.CurrentModel.SelectedEntity.IsRootRelated = true;
                AddAttibute("parcelNbr_mb", "string");
                this.AttributeDataGrid.ItemsSource = null;
                this.AttributeDataGrid.ItemsSource = this.Attributes.Where(a => !a.Name.EndsWith("_mb")).ToList();
            }*/
        }

        private void CkbIsRootRelated_Unchecked(object sender, RoutedEventArgs e)
        {
            /*if (ApplicationController.ApplicationMainController.CurrentModel.SelectedEntity != null)
            {
                ApplicationController.ApplicationMainController.CurrentModel.SelectedEntity.IsRootRelated = false;
                RemoveAttribute("parcelNbr_mb");
                this.AttributeDataGrid.ItemsSource = null;
                this.AttributeDataGrid.ItemsSource = this.Attributes.Where(a => !a.Name.EndsWith("_mb")).ToList();
            }*/
        }

        private void txtBackendQuery_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox tb = sender as TextBox;
            if (tb != null)
            {
                //ApplicationController.ApplicationMainController.CurrentModel.SelectedEntity.BackendQuery = tb.Text;
            }
        }

        private void SyncComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            /*if (ApplicationController.ApplicationMainController.CurrentModel.SelectedEntity != null)
            {
                ApplicationController.ApplicationMainController.CurrentModel.SelectedEntity.SyncType = (SyncType)this.SyncComboBox.SelectedItem;
            }*/
        }

        /// <summary>
        /// Handles the Checked event of the CkbIsRoot control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs" /> instance containing the event data.</param>
        private void CkbIsRoot_Checked(object sender, RoutedEventArgs e)
        {
            /*if (ApplicationController.ApplicationMainController.CurrentModel.SelectedEntity != null)
            {
                ApplicationController.ApplicationMainController.CurrentModel.SelectedEntity.IsRoot = true;
                AddAttibute("editState_mb", "int");
                this.AttributeDataGrid.ItemsSource = null;
                this.AttributeDataGrid.ItemsSource = this.Attributes.Where(a => !a.Name.EndsWith("_mb")).ToList();
                //ApplicationController.ApplicationMainController.CurrentModel.SelectedEntity.AllowForMaps = true;
                if (!ApplicationController.ApplicationMainController.CurrentModel.SelectedEntity.IsRoot)
                {
                    this.CkbIsRoot.IsChecked = false;
                }
            }*/
        }

        /// <summary>
        /// Handles the Unchecked event of the CkbIsRoot control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs" /> instance containing the event data.</param>
        private void CkbIsRoot_Unchecked(object sender, RoutedEventArgs e)
        {
            /*if (ApplicationController.ApplicationMainController.CurrentModel.SelectedEntity != null)
            {
                ApplicationController.ApplicationMainController.CurrentModel.SelectedEntity.IsRoot = false;
                RemoveAttribute("editState_mb");
                this.AttributeDataGrid.ItemsSource = null;
                this.AttributeDataGrid.ItemsSource = this.Attributes.Where(a => !a.Name.EndsWith("_mb")).ToList();
                //ApplicationController.ApplicationMainController.CurrentModel.SelectedEntity.AllowForMaps = false;
            }*/
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
            if (Int32.TryParse(tb.Text, out value))
            {
                //ApplicationController.ApplicationMainController.CurrentModel.SelectedEntity.SyncOrder = value;
            }
            else
            {
                //tb.Text = ApplicationController.ApplicationMainController.CurrentModel.SelectedEntity.SyncOrder.ToString();
            }
        }

        /// <summary>
        /// Handles the Checked event of the CkbIsMediaEntity control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs" /> instance containing the event data.</param>
        private void CkbIsMediaEntity_Checked(object sender, RoutedEventArgs e)
        {
            /*if (ApplicationController.ApplicationMainController.CurrentModel.SelectedEntity != null)
            {
                ApplicationController.ApplicationMainController.CurrentModel.SelectedEntity.IsMediaEntity = true;
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
                this.AttributeDataGrid.ItemsSource = this.Attributes.Where(a => !a.Name.EndsWith("_mb")).ToList();

            }*/
        }



        /// <summary>
        /// Handles the Unchecked event of the CkbIsMediaEntity control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs" /> instance containing the event data.</param>
        private void CkbIsMediaEntity_Unchecked(object sender, RoutedEventArgs e)
        {
            /*if (ApplicationController.ApplicationMainController.CurrentModel.SelectedEntity != null)
            {
                ApplicationController.ApplicationMainController.CurrentModel.SelectedEntity.IsMediaEntity = false;
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
                this.AttributeDataGrid.ItemsSource = this.Attributes.Where(a => !a.Name.EndsWith("_mb")).ToList();
            }*/
        }

        private void AddAttibute(string attrName, string attributeType)
        {
            /*EntityAttributeModel attr = null;
            switch (attributeType)
            {
                case "string":
                    if (!ApplicationController.ApplicationMainController.CurrentModel.SelectedEntity.Attributes.Any(a => a.Name == attrName))
                        attr = new EntityAttributeModel(ApplicationController.ApplicationMainController.CurrentModel.SelectedEntity) { AttributeType = AttributeType.String, Name = attrName, AttributeInfo = new AttributeInfoString() { IsIndexed = false, MaxChars = 4000 }, IsDefault = true };
                    break;
                case "text":
                    if (!ApplicationController.ApplicationMainController.CurrentModel.SelectedEntity.Attributes.Any(a => a.Name == attrName))
                        attr = new EntityAttributeModel(ApplicationController.ApplicationMainController.CurrentModel.SelectedEntity) { AttributeType = AttributeType.Text, Name = attrName, AttributeInfo = new AttributeInfoString() { IsIndexed = false, MaxChars = 4000 }, IsDefault = true };
                    break;

                case "double":
                    if (!ApplicationController.ApplicationMainController.CurrentModel.SelectedEntity.Attributes.Any(a => a.Name == attrName))
                        attr = new EntityAttributeModel(ApplicationController.ApplicationMainController.CurrentModel.SelectedEntity) { AttributeType = AttributeType.Double, Name = attrName, AttributeInfo = new AttributeInfoDouble() { IsIndexed = false }, IsDefault = true };
                    break;
                case "boolean":
                    if (!ApplicationController.ApplicationMainController.CurrentModel.SelectedEntity.Attributes.Any(a => a.Name == attrName))
                        attr = new EntityAttributeModel(ApplicationController.ApplicationMainController.CurrentModel.SelectedEntity) { AttributeType = AttributeType.Boolean, Name = attrName, AttributeInfo = new AttributeInfo() { IsIndexed = false }, IsDefault = true };
                    break;
                case "int":
                    if (!ApplicationController.ApplicationMainController.CurrentModel.SelectedEntity.Attributes.Any(a => a.Name == attrName))
                        attr = new EntityAttributeModel(ApplicationController.ApplicationMainController.CurrentModel.SelectedEntity) { AttributeType = AttributeType.Integer32, Name = attrName, AttributeInfo = new AttributeInfoInteger() { IsIndexed = false }, IsDefault = true };
                    break;

                case "date":
                    if (!ApplicationController.ApplicationMainController.CurrentModel.SelectedEntity.Attributes.Any(a => a.Name == attrName))
                        attr = new EntityAttributeModel(ApplicationController.ApplicationMainController.CurrentModel.SelectedEntity) { AttributeType = AttributeType.Date, Name = attrName, AttributeInfo = new AttributeInfoDate() { IsIndexed = false }, IsDefault = true };
                    break;

                default:
                    break;
            }

            if (attr != null)
            {
                ApplicationController.ApplicationMainController.CurrentModel.SelectedEntity.Attributes.Add(attr);
                this.Attributes.Add(attr);
            }*/
        }

        private void RemoveAttribute(string attrName)
        {
            /*EntityAttributeModel attr;
            attr = ApplicationController.ApplicationMainController.CurrentModel.SelectedEntity.Attributes.FirstOrDefault(a => a.Name == attrName);
            if (attr != null)
            {
                ApplicationController.ApplicationMainController.CurrentModel.SelectedEntity.Attributes.Remove(attr);
                this.Attributes.Remove(attr);
            }*/
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
                if (! Helper.ValidateBoolFromString( this.SelectedAttribute.isDefault) )
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
                            if (!attr.attributeType.Equals( attributeTypeComboBox.SelectedItem.ToString()))
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
            this.SelectedAttribute = (modelEntityAttribute) this.AttributeDataGrid.SelectedItem;

            if (this.AttributeDataGrid.SelectedItem != null)
            {
                this.SelectedAttribute = (modelEntityAttribute) this.AttributeDataGrid.SelectedItem;
                if (this.SelectedAttribute != null)
                {
                    this.SelectedAttribute.AttributeTypeChanged += new EventHandler(SelectedAttribute_AttributeTypeChanged);
                    this.AttributeInformation.SetAttribute(this.SelectedAttribute);
                    this.AttributeInformation.IsEnabled = ! Helper.ValidateBoolFromString( this.SelectedAttribute.isDefault );
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
            modelEntityAttribute attribute = (modelEntityAttribute) e.Row.DataContext;
            if (attribute != null)
            {
                if (Helper.ValidateBoolFromString(attribute.isDefault))
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
            //this.SelectedRelationship = this.RelationshipDataGrid.SelectedItem as EntityRelationshipModel;
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
                    /*EntityRelationshipModel selectedRelationship = grid.SelectedItem as EntityRelationshipModel;
                    RelationshipManagementWindow relWindow = new RelationshipManagementWindow(selectedRelationship);
                    relWindow.Owner = App.Current.MainWindow;
                    bool? result = relWindow.ShowDialog();

                    if (result == true)
                    {
                        this.RelationshipDataGrid.ItemsSource = this.Relationships;
                        this.RelationshipDataGrid.UpdateLayout();
                    }*/
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
            /*if (ApplicationController.ApplicationMainController.CurrentModel.SelectedEntity != null)
            {
                EntityRelationshipModel rel = new EntityRelationshipModel();
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
                    if (ApplicationController.ApplicationMainController.CurrentModel.SelectedEntity.Relationships.FirstOrDefault(a => a.Name == tempName) == null)
                    {
                        notListed = true;
                        rel.Name = tempName;
                    }
                }

                RelationshipManagementWindow relWindow = new RelationshipManagementWindow(rel);
                relWindow.Owner = App.Current.MainWindow;
                bool? result = relWindow.ShowDialog();

                if (result == true)
                {
                    ApplicationController.ApplicationMainController.CurrentModel.SelectedEntity.Relationships.Add(relWindow.EntityRelationshipModel);
                    this.Relationships.Add(relWindow.EntityRelationshipModel);
                    this.RelationshipDataGrid.ItemsSource = this.Relationships;
                    this.RelationshipDataGrid.UpdateLayout();
                }
            }*/
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
                /*EntityRelationshipModel selectedRelationship = this.RelationshipDataGrid.SelectedItem as EntityRelationshipModel;
                if (selectedRelationship != null)
                {
                    if (MessageBox.Show(MessageResources.DeleteRelationshipWarning, MessageResources.Confirmation, MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                    {
                        foreach (EntityModel ent in ApplicationController.ApplicationMainController.CurrentModel.Entities)
                        {
                            if (ent != ApplicationController.ApplicationMainController.CurrentModel.SelectedEntity)
                            {
                                List<EntityRelationshipModel> relationships = ent.Relationships.ToList();
                                foreach (EntityRelationshipModel rel in relationships)
                                {
                                    if (rel.TargetTableName == ApplicationController.ApplicationMainController.CurrentModel.SelectedEntity.Name && rel.InverseRelationshipName == selectedRelationship.Name)
                                    {
                                        rel.InverseRelationshipName = Resource.NoInverse;
                                    }
                                }
                            }
                        }

                        ApplicationController.ApplicationMainController.CurrentModel.SelectedEntity.DeleteRelationship(selectedRelationship);
                        this.Relationships.Remove(selectedRelationship);
                        this.RelationshipDataGrid.ItemsSource = this.Relationships;
                        this.RelationshipDataGrid.UpdateLayout();
                        this.RelationshipDataGrid.SelectedIndex = 0;
                    }
                }*/
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
    }
}
