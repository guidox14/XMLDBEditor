using DBXTemplateDesigner;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBXTemplateDesigner.CCModels
{
    public class DeleteFunctionality
    {
        modelEntity parent;

        /// <summary>
        /// The list of the entity current deleted attributes.
        /// </summary>
        private IList<modelEntityAttribute> deletedAttributes;

        /// <summary>
        /// The list of the entity current deleted relationships.
        /// </summary>
        private IList<modelEntityRelationship> deletedRelationships;

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

        /// <summary>
        /// Gets or sets the deleted relationships.
        /// </summary>
        /// <value>
        /// The deleted relationships.
        /// </value>
        public IList<modelEntityRelationship> DeletedRelationships
        {
            get
            {
                if (deletedRelationships == null)
                    deletedRelationships = new List<modelEntityRelationship>();
                return this.deletedRelationships;
            }

            set
            {
                if (this.deletedRelationships != value)
                {
                    this.deletedRelationships = value;
                    //this.NotifyPropertyChanged("DeletedRelationships");
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

        /// <summary>
        /// Deletes the relationship.
        /// </summary>
        /// <param name="relationship">The relationship.</param>
        public void DeleteRelationship(modelEntityRelationship relationship)
        {
            parent.relationship.Remove(relationship);
            this.DeletedRelationships.Add(new modelEntityRelationship(relationship));
        }

    }
}
