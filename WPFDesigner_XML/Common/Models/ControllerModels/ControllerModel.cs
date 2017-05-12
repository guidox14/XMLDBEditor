using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Xml.Linq;
using WPFDesigner_XML.Common.Models.UIModels;

namespace WPFDesigner_XML.Common.Models.ControllerModels
{
    public class ControllerModel : ScreenModel
    {


        /// <summary>
        /// Initializes a new instance of the <see cref="ControllerModel"/> class.
        /// </summary>
        /// <param name="definition">The definition.</param>
        /// <param name="projectOwner">The project owner.</param>
        /// <exception cref="System.ArgumentException">Required container property is not present in the screen definition</exception>
        public ControllerModel(ControllerDefinitionModel definition, ProjectModel projectOwner)
            : base(definition, projectOwner)
        {
        }

        #region MobiseModel Base

        /// <summary>
        /// To the XML.
        /// </summary>
        /// <returns></returns>
        public override XElement ToXml()
        {
            XElement element = new XElement("controller");
            return base.ToXml(element, true);
        }

        #endregion

    }
}
