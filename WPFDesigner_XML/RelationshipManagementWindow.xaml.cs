using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Collections.ObjectModel;
using DBXTemplateDesigner.CCModels;
using WPFDesigner_XML.Common.Models.Entity;
using WPFDesigner_XML;

namespace Microsoft.XmlTemplateDesigner
{
    /// <summary>
    /// Interaction logic for RelationshipManagementWindow.xaml
    /// </summary>
    public partial class RelationshipManagementWindow : Window
    {
        VsDesignerControl parentWindow;

        /// <summary>
        /// Gets the relationship.
        /// </summary>
        public modelEntityRelationship EntityRelationshipModel { get; private set; }

        /// <summary>
        /// Observable collection of client keys
        /// </summary>
        private ObservableCollection<modelEntityRelationshipClientKey> clientKeys = new ObservableCollection<modelEntityRelationshipClientKey>();

        /// <summary>
        /// Name of destination entity
        /// </summary>
        private string destination = string.Empty;

        /// <summary>
        /// Initializes a new instance of the <see cref="RelationshipManagementWindow"/> class.
        /// </summary>
        /// <param name="relationship">The relationship.</param>
        public RelationshipManagementWindow(modelEntityRelationship relationship, VsDesignerControl parent)
        {
            InitializeComponent();

            parentWindow = parent;

            this.EntityRelationshipModel = relationship;

            if (this.EntityRelationshipModel != null) 
            {
                this.RelationshipNameTextBox.Text = this.EntityRelationshipModel.name;
                this.MultipleRelationshipsCheckBox.IsChecked = Helper.ValidateBoolFromString( this.EntityRelationshipModel.toMany == null ? "False" : EntityRelationshipModel.toMany);
                this.LoadComboBoxes();
                this.RelationshipNameTextBox.SelectAll();
                this.RelationshipNameTextBox.Focus();
                this.ClientKeyRelationsDataGrid.ItemsSource = clientKeys;
            }
        }

        /// <summary>
        /// Loads the combo boxes.
        /// </summary>
        private void LoadComboBoxes() 
        {
            List<string> otherEntities = new List<string>();
            otherEntities.Add(WPFDesigner_XML.Resources.NoDestination);
            string selected = string.Empty;

            if (parentWindow.SelectedEntity != null && this.EntityRelationshipModel != null)
            {
                if ( ((ViewModel)parentWindow.DataContext).XmlTemplateModel.entity != null)
                {
                    ((ViewModel)parentWindow.DataContext).XmlTemplateModel.entity.Where(r => r.name != parentWindow.SelectedEntity.name).ToList().ForEach(a => otherEntities.Add(a.name));
                }

                selected = otherEntities.FirstOrDefault(a => a == this.EntityRelationshipModel.destinationEntity);
            }

            this.DestinationComboBox.ItemsSource = new ObservableCollection<string>(otherEntities);
            this.DestinationComboBox.UpdateLayout();

            if (!string.IsNullOrEmpty(selected))
            {
                this.DestinationComboBox.SelectedItem = selected;
            }
            else 
            {
                this.DestinationComboBox.SelectedIndex = 0;
            }

            if (parentWindow.SelectedEntity != null)
            {
                List<string> attrsName = new List<string>();
                parentWindow.SelectedEntity.attribute.Where(a => a != null && Helper.ValidateBoolFromString( a.isClientKey) ).ToList().ForEach(a => attrsName.Add(a.name));
                this.SourceKeyComboBox.ItemsSource = new ObservableCollection<string>(attrsName);
            }
        }

        /// <summary>
        /// Handles the Click event of the CancelButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        /// <summary>
        /// Handles the Click event of the OKButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            if (this.ValidateValues())
            {
                this.EntityRelationshipModel.name = this.RelationshipNameTextBox.Text;
                this.DialogResult = true;
                this.EntityRelationshipModel.clientKey = clientKeys;
                this.EntityRelationshipModel.destinationEntity = destination;
                this.EntityRelationshipModel.toMany = Helper.ConvertBoolToString(MultipleRelationshipsCheckBox.IsChecked);
                var inverseItem = InverseComboBox.SelectedItem.ToString();
                if (inverseItem.Equals("No Inverse"))
                {
                    var messageResult = MessageBox.Show("Do you wanto to auto-generate it?", "Missing inverse relationship",
                        MessageBoxButton.YesNo, MessageBoxImage.Question);
                    if (messageResult == MessageBoxResult.Yes)
                    {
                        var inverseRelation = new modelEntityRelationship();
                        inverseRelation.name = "r_" + this.RelationshipNameTextBox.Text;
                        inverseRelation.destinationEntity = parentWindow.SelectedEntity.name;
                        inverseRelation.inverseName = this.EntityRelationshipModel.name;
                        inverseRelation.inverseEntity = destination;
                        inverseRelation.toMany = Helper.ConvertBoolToString( !MultipleRelationshipsCheckBox.IsChecked );
                        this.EntityRelationshipModel.inverseEntity = inverseRelation.destinationEntity;
                        this.EntityRelationshipModel.inverseName = inverseRelation.name;
                        var entities = ((ViewModel)parentWindow.DataContext).XmlTemplateModel.entity;
                        modelEntity selectedEntity = null;
                        foreach (var currentEntity in entities)
                        {
                            if (currentEntity.name.Equals(destination))
                            {
                                selectedEntity = currentEntity;
                            }
                        }
                        if (selectedEntity != null)
                        {
                            selectedEntity.relationship.Add(inverseRelation);
                        }
                    }
                    else if (messageResult == MessageBoxResult.No)
                    {
                        this.EntityRelationshipModel.inverseEntity = this.InverseComboBox.SelectedItem.ToString();
                    }
                }
            }
            else 
            {
                this.RelationshipNameTextBox.Text = this.EntityRelationshipModel.name;
            }
        }

        /// <summary>
        /// Validates the values.
        /// </summary>
        /// <returns></returns>
        private bool ValidateValues() 
        {
            bool isValid = false;
            if (!string.IsNullOrEmpty(this.RelationshipNameTextBox.Text))
            {
                isValid = char.IsLetter(this.RelationshipNameTextBox.Text.FirstOrDefault()) && this.RelationshipNameTextBox.Text.FirstOrDefault() == char.ToLower(this.RelationshipNameTextBox.Text.FirstOrDefault());
                if (isValid)
                {
                    modelEntityRelationship rel = parentWindow.SelectedEntity.relationship.FirstOrDefault(r => r.name == RelationshipNameTextBox.Text);
                    if (rel != null)
                    {
                        if (rel != this.EntityRelationshipModel)
                        {
                            isValid = false;
                            MessageBox.Show(MessageResources.NameBeenUsedRelationship, MessageResources.Error, MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                }
                else
                {
                    MessageBox.Show(MessageResources.InvalidRelationshipName, MessageResources.Error, MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else 
            {
                MessageBox.Show(MessageResources.EmptyRelationshipName, MessageResources.Error, MessageBoxButton.OK, MessageBoxImage.Error);
            }

            return isValid;
        }

        /// <summary>
        /// Handles the SelectionChanged event of the DestinationComboBox control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.Controls.SelectionChangedEventArgs"/> instance containing the event data.</param>
        private void DestinationComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string selected = this.DestinationComboBox.SelectedItem.ToString();
            destination = selected;
            clientKeys.Clear();
            if (destination == this.EntityRelationshipModel.destinationEntity && EntityRelationshipModel.clientKey != null)
            {
                this.EntityRelationshipModel.clientKey.ToList().ForEach(r => clientKeys.Add(r));
            }

            if (selected != WPFDesigner_XML.Resources.NoDestination)
            {
                modelEntity otherEntity = ((ViewModel)parentWindow.DataContext).XmlTemplateModel.entity.FirstOrDefault(ent => ent.name == selected);

                if (otherEntity != null) 
                {
                    List<string> attrsName = new List<string>();
                    otherEntity.attribute.ToList().ForEach(a => attrsName.Add(a.name));
                    this.DestinationKeyComboBox.ItemsSource = new ObservableCollection<string>(attrsName);
                }


                List<string> relationships = new List<string>();
                if (otherEntity != null && otherEntity.relationship != null && parentWindow.SelectedEntity != null)
                {
                    otherEntity.relationship.Where(r => r.destinationEntity == parentWindow.SelectedEntity.name).ToList().ForEach(a => relationships.Add(a.name));
                }

                if (relationships.Count > 0)
                {
                    this.LoadInverseCombobox(relationships);
                }
                else 
                {
                    this.LoadInverseCombobox(null);
                } 
            }
            else 
            {
                this.LoadInverseCombobox(null);
            }
        }

        /// <summary>
        /// Loads the inverse combo box.
        /// </summary>
        /// <param name="values">The values.</param>
        private void LoadInverseCombobox(List<string> values) 
        {
            if (values == null)
            {
                this.InverseComboBox.ItemsSource = new ObservableCollection<string>(new List<string>() { WPFDesigner_XML.Resources.NoInverse });
                this.InverseComboBox.UpdateLayout();
                this.InverseComboBox.SelectedIndex = 0;
            }
            else 
            {
                values.Insert(0, WPFDesigner_XML.Resources.NoInverse);
                this.InverseComboBox.ItemsSource = new ObservableCollection<string>(values);
                this.InverseComboBox.UpdateLayout();

                string selected = values.FirstOrDefault(a => a == this.EntityRelationshipModel.inverseEntity);
                this.InverseComboBox.SelectedItem = selected;
            }
        }

        /// <summary>
        /// Handles the SelectionChanged event of the InverseComboBox control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.Controls.SelectionChangedEventArgs"/> instance containing the event data.</param>
        private void InverseComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string inverse = this.InverseComboBox.SelectedItem.ToString();

            // Logic to force the M:N relationships only possible via intermediary tables.
            string selected = this.DestinationComboBox.SelectedItem.ToString();
            modelEntity otherEntity = ((ViewModel)parentWindow.DataContext).XmlTemplateModel.entity.FirstOrDefault(ent => ent.name == selected);
            if(otherEntity != null)
            {
                modelEntityRelationship inverseRel = otherEntity.relationship.FirstOrDefault(r => r.name == inverse);
                if (inverseRel != null && Helper.ValidateBoolFromString( inverseRel.toMany) )
                {
                    this.MultipleRelationshipsCheckBox.IsChecked = false;
                    this.MultipleRelationshipsCheckBox.IsEnabled = false;
                }
            }
        }

        /// <summary>
        /// Handles the Checked event of the MultipleRelationshipsCheckBox control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        private void MultipleRelationshipsCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            this.EntityRelationshipModel.toMany = "True";
        }

        /// <summary>
        /// Handles the Unchecked event of the MultipleRelationshipsCheckBox control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        private void MultipleRelationshipsCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            this.EntityRelationshipModel.toMany = "False";
        }

        /// <summary>
        /// Handles the Click event of the AddButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs" /> instance containing the event data.</param>
        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            if (SourceKeyComboBox.SelectedIndex >= 0 && DestinationKeyComboBox.SelectedIndex >= 0)
            {
                modelEntityRelationshipClientKey clientKey = new modelEntityRelationshipClientKey(SourceKeyComboBox.SelectedValue.ToString(), DestinationKeyComboBox.SelectedValue.ToString());
                if (!clientKeys.Contains(clientKey))
                {
                    clientKeys.Add(clientKey);
                }
            }
        }
    }
}
