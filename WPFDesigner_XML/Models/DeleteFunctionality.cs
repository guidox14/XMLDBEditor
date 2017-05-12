using DBXTemplateDesigner;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBXTemplateDesigner.Models
{
    public class DeleteFunctionality
    {
        modelEntity parent;

        /// <summary>
        /// The list of the entity current deleted attributes.
        /// </summary>
        private IList<modelEntityAttribute> deletedAttributes;

        /// <summary>
        /// Gets or sets the attributes.
        /// </summary>
        /// <value>The attributes.</value>
        public IList<modelEntityAttribute> DeletedAttributes
        {
            get
            {
                if (deletedAttributes == null)
                {
                    deletedAttributes = new List<modelEntityAttribute>();
                }
                return deletedAttributes;
            }

            set
            {
                if (this.deletedAttributes != value)
                {
                    this.deletedAttributes = value;
                    //this.NotifyPropertyChanged("DeletedAttributes");
                }
            }
        }

        public DeleteFunctionality()
        {
            deletedAttributes = new List<modelEntityAttribute>();
        }

        public DeleteFunctionality(modelEntity m_parent) : this()
        {
            parent = m_parent;
        }

        /// <summary>
        /// Deletes the attribute.
        /// </summary>
        /// <param name="attribute">The attribute.</param>
        public void DeleteAttribute(modelEntityAttribute attribute)
        {
            parent.attribute.Remove(attribute);
            this.DeletedAttributes.Add(attribute);
        }
    }
}
