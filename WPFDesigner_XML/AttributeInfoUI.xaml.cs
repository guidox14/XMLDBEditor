using System.Windows;
using System.Windows.Controls;
using DBXTemplateDesigner;
using WPFDesigner_XML;

namespace Microsoft.XmlTemplateDesigner
{
    /// <summary>
    /// Interaction logic for AttribbuteInfoUI.xaml
    /// </summary>
    public partial class AttributeInfoUI : UserControl
    {
        /// <summary>
        /// Gets or sets the attribute.
        /// </summary>
        /// <value>
        /// The attribute.
        /// </value>
        public modelEntityAttribute Attribute { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="AttributeInfoUI"/> class.
        /// </summary>
        public AttributeInfoUI()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Sets the attribute.
        /// </summary>
        /// <param name="attribute">The attribute.</param>
        public void SetAttribute(modelEntityAttribute attribute)
        {
            this.Attribute = attribute;
            
            if (this.Attribute != null && this.Attribute != null) 
            {
                //this.TxtMapAlias.Text = this.Attribute.MapAlias;
                //this.chkAllowForMap.IsChecked = this.Attribute.AllowForMap;
                //this.EditableCheckBox.IsChecked = this.Attribute.AttributeInfo.IsEditable;
                //this.LevelComboBox.SelectedItem = this.Attribute.AttributeInfo.AttributeLevel;
                this.IndexedCheckBox.IsChecked = Helper.ValidateBoolFromString( this.Attribute.indexed );
                this.ChkPrimaryKey.IsChecked = Helper.ValidateBoolFromString( this.Attribute.isPrimaryKey );
                //this.ChkSQLiteDb.IsChecked = this.Attribute.AttributeInfo.PresentInSQLiteDB;
                //this.ChkClientKey.IsChecked = this.Attribute.IsClientKey;
                //this.TxtDefaultValue.Text = this.Attribute.AttributeInfo.DefaultValue;
                
                this.TxtFieldDescription.Text = this.Attribute.description;
                this.TxtFieldLookupScript.Text = this.Attribute.lookupScript;
            }

            /*this.TypeComboBox.SelectedIndex = -1;
            this.TypeComboBox.SelectedItem = this.Attribute != null ? this.Attribute.attributeType : AttributeType.Undefined;*/
        }

        /// <summary>
        /// Handles the SelectionChanged event of the TypeComboBox control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.Controls.SelectionChangedEventArgs"/> instance containing the event data.</param>
        private void TypeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this.TypeCanvas != null)
            {
                /*if (this.Attribute != null && this.TypeComboBox.SelectedItem != null)
                {*/
                    /*AttributeType type = (AttributeType)this.TypeComboBox.SelectedItem;
                    if (!this.Attribute.IsDefault || (this.Attribute.IsDefault && this.Attribute.AttributeType == type))
                    {
                        AttributeInfo info = this.Attribute != null && this.Attribute.AttributeInfo != null ? this.Attribute.AttributeInfo : null;
                        if (info == null || info.GetType() != DatabaseManagementHelper.GetAttributeInfoFromType(type).GetType())
                        {
                            this.Attribute.AttributeInfo = DatabaseManagementHelper.GetAttributeInfoFromType(type);
                            info = this.Attribute.AttributeInfo;
                        }

                        this.TypeCanvas.Children.Clear();

                        switch (type)
                        {
                            case AttributeType.String:
                                this.TypeCanvas.Children.Add(new StringAttributeUI(info));
                                break;
                            case AttributeType.Date:
                                this.TypeCanvas.Children.Add(new DateAttributeUI(info));
                                break;
                            case AttributeType.Double:
                                this.TypeCanvas.Children.Add(new DoubleAttributeUI(info));
                                break;
                            case AttributeType.Integer16:
                            case AttributeType.Integer32:
                            case AttributeType.Integer64:
                                this.TypeCanvas.Children.Add(new NumericAttributeUI(info));
                                break;
                            case AttributeType.BLOB:
                            case AttributeType.Boolean:
                            default:
                                break;
                        }

                        if (this.Attribute.AttributeType != type)
                        {
                            this.Attribute.AttributeType = type;
                        }
                    }
                    else
                    {
                        this.TypeComboBox.SelectedItem = this.Attribute.AttributeType;
                        MessageBox.Show(MessageResources.DefaultAttributeChanges, MessageResources.Information, MessageBoxButton.OK, MessageBoxImage.Information);
                    }*/
                /*}
                else 
                {
                    this.TypeCanvas.Children.Clear();
                }*/
            }
        }

        /// <summary>
        /// Handles the Checked event of the EditableCheckBox control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        //private void EditableCheckBox_Checked(object sender, RoutedEventArgs e)
        //{
        //    if (this.Attribute != null && this.Attribute.AttributeInfo != null) 
        //    {
        //        this.Attribute.AttributeInfo.IsEditable = true;
        //    }
        //}

        /// <summary>
        /// Handles the Unchecked event of the EditableCheckBox control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        //private void EditableCheckBox_Unchecked(object sender, RoutedEventArgs e)
        //{
        //    if (this.Attribute != null && this.Attribute.AttributeInfo != null) 
        //    {
        //        this.Attribute.AttributeInfo.IsEditable = false;
        //        this.LevelComboBox.SelectedIndex = 0;
        //    }
        //}

        /// <summary>
        /// Handles the SelectionChanged event of the LevelComboBox control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.Controls.SelectionChangedEventArgs"/> instance containing the event data.</param>
        //private void LevelComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        //{
        //    if (this.Attribute != null && this.Attribute.AttributeInfo != null)
        //    {
        //        this.Attribute.AttributeInfo.AttributeLevel = (AttributeLevel)this.LevelComboBox.SelectedItem;
        //    }
        //}

        /// <summary>
        /// Handles the Checked event of the IndexedCheckBox control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        private void IndexedCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            /*if (this.Attribute != null && this.Attribute.AttributeInfo != null)
            {
                this.Attribute.AttributeInfo.IsIndexed = true;
            }*/
        }

        /// <summary>
        /// Handles the Unchecked event of the IndexedCheckBox control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        private void IndexedCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            /*if (this.Attribute != null && this.Attribute.AttributeInfo != null)
            {
                this.Attribute.AttributeInfo.IsIndexed = false;
            }*/
        }

        /// <summary>
        /// Handles the Checked event of the ChkPrimaryKey control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs" /> instance containing the event data.</param>
        private void ChkPrimaryKey_Checked(object sender, RoutedEventArgs e)
        {
            /*if (this.Attribute != null && this.Attribute.AttributeInfo != null)
            {
                this.Attribute.AttributeInfo.IsPrimaryKey = true;
            }*/
        }

        /// <summary>
        /// Handles the Unchecked event of the ChkPrimaryKey control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs" /> instance containing the event data.</param>
        private void ChkPrimaryKey_Unchecked(object sender, RoutedEventArgs e)
        {
            /*if (this.Attribute != null && this.Attribute.AttributeInfo != null)
            {
                this.Attribute.AttributeInfo.IsPrimaryKey = false;
            }*/
        }

        /// <summary>
        /// Handles the Checked event of the ChkSQLiteDb control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs" /> instance containing the event data.</param>
        //private void ChkSQLiteDb_Checked(object sender, RoutedEventArgs e)
        //{
        //    if (this.Attribute != null && this.Attribute.AttributeInfo != null)
        //    {
        //        this.Attribute.AttributeInfo.PresentInSQLiteDB = true;
        //    }
        //}

        /// <summary>
        /// Handles the Unchecked event of the ChkSQLiteDb control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs" /> instance containing the event data.</param>
        //private void ChkSQLiteDb_Unchecked(object sender, RoutedEventArgs e)
        //{
        //    if (this.Attribute != null && this.Attribute.AttributeInfo != null)
        //    {
        //        this.Attribute.AttributeInfo.PresentInSQLiteDB = false;
        //    }
        //}

        /// <summary>
        /// Handles the Checked event of the ChkClientKey control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs" /> instance containing the event data.</param>
        private void ChkClientKey_Checked(object sender, RoutedEventArgs e)
        {
            /*if (this.Attribute != null && this.Attribute.AttributeInfo != null)
            {
                this.Attribute.AttributeInfo.IsClientKey = true;
                IndexedCheckBox_Checked(sender, e);
            }*/
        }

        /// <summary>
        /// Handles the Unchecked event of the ChkClientKey control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs" /> instance containing the event data.</param>
        private void ChkClientKey_Unchecked(object sender, RoutedEventArgs e)
        {
            /*if (this.Attribute != null && this.Attribute.AttributeInfo != null)
            {
                this.Attribute.AttributeInfo.IsClientKey = false;
            }*/
        }

        /// <summary>
        /// Handles the TextChanged event of the TxtDefaultValue control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="TextChangedEventArgs" /> instance containing the event data.</param>
        //private void TxtDefaultValue_TextChanged(object sender, TextChangedEventArgs e)
        //{
        //    if (this.Attribute != null && this.Attribute.AttributeInfo != null)
        //    {
        //        this.Attribute.AttributeInfo.DefaultValue = ((TextBox)sender).Text;
        //    }
        //}

        /// <summary>
        /// Handles the TextChanged event of the TxtFieldDescription control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="TextChangedEventArgs" /> instance containing the event data.</param>
        private void TxtFieldDescription_TextChanged(object sender, TextChangedEventArgs e)
        {
            /*if (this.Attribute != null && this.Attribute.AttributeInfo != null)
            {
                this.Attribute.AttributeInfo.Description = ((TextBox)sender).Text;
            }*/
        }

        //private void chkAllowForMap_Checked(object sender, RoutedEventArgs e)
        //{
        //    if (this.Attribute != null)
        //    {
        //        this.Attribute.AllowForMap = chkAllowForMap.IsChecked;
        //    }
        //}

        //private void TxtMapAlias_TextChanged(object sender, TextChangedEventArgs e)
        //{
        //    if (this.Attribute != null)
        //    {
        //        this.Attribute.MapAlias = TxtMapAlias.Text;
        //    }
        //}

        /// <summary>
        /// Handles the TextChanged event of the TxtFieldLookupScript control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="TextChangedEventArgs" /> instance containing the event data.</param>
        private void TxtFieldLookupScript_TextChanged(object sender, TextChangedEventArgs e)
        {
            /*if (this.Attribute != null && this.Attribute.AttributeInfo != null)
            {
                this.Attribute.AttributeInfo.LookupScript = ((TextBox)sender).Text;
            }*/
        }
    }
}
