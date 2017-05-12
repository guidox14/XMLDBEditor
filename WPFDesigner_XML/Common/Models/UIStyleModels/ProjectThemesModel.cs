using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace WPFDesigner_XML.Common.Models.UIStyleModels
{
    public class ProjectThemesModel : MobiseModel
    {
        public TrulyObservableCollection<ThemeModel> Themes { get; private set; }
        public TrulyObservableCollection<StyleAttributeDefaultValueModel> StyleAttributeDefaultValues { get; private set; }

        public ProjectThemesModel()
            : base()
        {
            this.Themes = new TrulyObservableCollection<ThemeModel>();
            this.StyleAttributeDefaultValues = new TrulyObservableCollection<StyleAttributeDefaultValueModel>();
        }

        #region Mobise Model
        /// <summary>
        /// To the XML.
        /// </summary>
        /// <returns></returns>
        public override XElement ToXml()
        {
            XElement baseElement = new XElement("ProjectThemes");
            return this.ToXml(baseElement, true);
        }

        /// <summary>
        /// To the XML.
        /// </summary>
        /// <param name="baseElement">The base element.</param>
        /// <param name="AddCommonAttributes">if set to <c>true</c> [add common attributes].</param>
        /// <returns></returns>
        public override XElement ToXml(XElement baseElement, bool AddCommonAttributes)
        {
            baseElement.SetAttributeValue("name", this.Name);

            AddToXMLChildElementsByName(baseElement, "themes", this.Themes);
            AddToXMLChildElementsByName(baseElement, "StyleAttributeDefaultValues", this.StyleAttributeDefaultValues);

            return baseElement;
        }

        /// <summary>
        /// Froms the XML.
        /// </summary>
        /// <param name="xml">The XML.</param>
        public override void FromXml(XElement xml)
        {
            if (xml.HasElements)
            {
                Task loadThemes = Task.Factory.StartNew(() =>
                {
                    ReadFromXMLChildItemsByName<ThemeModel>(xml, "themes", this.Themes, this);
                });

                Task loadStyleAttributeDefaultValues = Task.Factory.StartNew(() =>
                {
                    ReadFromXMLChildItemsByName<StyleAttributeDefaultValueModel>(xml, "StyleAttributeDefaultValues", this.StyleAttributeDefaultValues, this);
                });

                Task.WaitAll(loadThemes, loadStyleAttributeDefaultValues);
            }
        }

        #endregion

    }
}
