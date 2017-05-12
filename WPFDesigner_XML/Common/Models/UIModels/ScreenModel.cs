using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Xml.Linq;

namespace WPFDesigner_XML.Common.Models.UIModels
{
    public class ScreenModel : UIControlInstanceModel
    {
        /// <summary>
        /// The container attribute
        /// </summary>
        private AttributeModel containerAttribute = null;

        public ProjectModel ParentProject { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ScreenModel"/> class.
        /// </summary>
        /// <param name="definition">The definition.</param>
        /// <exception cref="System.ArgumentException">Required container property is not present in the screen definition</exception>
        public ScreenModel(UIControlDefinitionModel definition, ProjectModel projectOwner)
            : base(projectOwner, null, definition)
        {
            System.Diagnostics.Debug.Assert(definition != null, "Screen definition can't be null");
            //System.Diagnostics.Debug.Assert(!string.IsNullOrEmpty(definition.Name) && (definition.Name.Equals("screen", StringComparison.OrdinalIgnoreCase) || definition.Name.Equals("view", StringComparison.OrdinalIgnoreCase)), "definition must be for screen or view");
            System.Diagnostics.Debug.Assert(projectOwner != null, "Project owner can't be null");
            
            this.ParentProject = projectOwner;
            string containerPropertyName = "items";
            if (!string.IsNullOrEmpty(definition.ContainerProperty))
            {
                containerPropertyName = definition.ContainerProperty;
            }
            containerAttribute = this.ControlDefinition.AllAttributes.FirstOrDefault(a => a.Name == containerPropertyName);
            //if (containerAttribute == null)
            //{
            //    throw new ArgumentException("Required container property is not present in the screen definition");
            //}
        }

        #region MobiseModel Base

        /// <summary>
        /// To the XML.
        /// </summary>
        /// <returns></returns>
        public override XElement ToXml()
        {
            XElement element = new XElement("screen");
            return base.ToXml(element, true);
        }

        #endregion

        /// <summary>
        /// Gets the children.
        /// </summary>
        /// <value>
        /// The children.
        /// </value>
        public TrulyObservableCollection<UIControlInstanceModel> Children
        {
            get
            {
                if (containerAttribute != null)
                {
                    var children = this.Attributes[containerAttribute] as TrulyObservableCollection<UIControlInstanceModel>;
                    if (children == null)
                    {
                        children = new TrulyObservableCollection<UIControlInstanceModel>();
                        this.Attributes[containerAttribute] = children;
                    }

                    return children;
                }

                return null;
            }
        }
        
        /// <summary>
        /// Creates the control instance.
        /// </summary>
        /// <param name="definition">The definition.</param>
        /// <returns></returns>
        public UIControlInstanceModel CreateAndAddControlInstance(UIControlDefinitionModel definition)
        {
            if (this.Children != null)
            {
                UIControlInstanceModel newInstance = new UIControlInstanceModel(this, definition);
                this.Children.Add(newInstance);
                return newInstance;
            }
            return null;
        }

        /// <summary>
        /// Adds the specified instance.
        /// </summary>
        /// <param name="instace">The instance.</param>
        public void Add(UIControlInstanceModel instance)
        {
            if (this.Children != null)
            {
                instance.ParentScreen = this;
                if (!this.Children.Contains(instance))
                {
                    this.Children.Add(instance);
                }
            }
        }

        /// <summary>
        /// Removes the specified instance.
        /// </summary>
        /// <param name="instance">The instance.</param>
        public void Remove(UIControlInstanceModel instance)
        {
            if (this.Children != null)
            {
                if (this.Children.Contains(instance))
                {
                    this.Children.Remove(instance);
                }
            }
        }
    }
}
