using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;

namespace WPFDesigner_XML.Common.DynamicContext
{
    public abstract class DynamicEntityBase
    {
         /// <summary>
        /// Hash table containing the values for each property definition
        /// </summary>
        private Dictionary<string, object> propertyBag = new Dictionary<string, object>();
        
        /// <summary>
        /// Gets the kind.
        /// </summary>
        /// <value>
        /// The kind.
        /// </value>
        [NotMapped]
        public string Kind
        {
            get;
            private set;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DynamicEntityBase" /> class.
        /// </summary>
        /// <param name="kind">The kind.</param>
        public DynamicEntityBase(string kind)
        {
            this.Kind = kind;
        }

        /// <summary>
        /// Gets or sets the <see cref="System.Object" /> with the specified name.
        /// </summary>
        /// <value>
        /// The <see cref="System.Object" />.
        /// </value>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        [NotMapped]
        public object this[string name]
        {
            get
            {
                return propertyBag[name];
            }
            set
            {
                propertyBag[name] = value;
            }
        }
    }
}
