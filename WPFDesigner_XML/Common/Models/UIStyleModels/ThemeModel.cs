using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace WPFDesigner_XML.Common.Models.UIStyleModels
{
    public class ThemeModel : MobiseModel, ICloneable
    {

        public TrulyObservableCollection<ClassStyleModel> Classes { get; private set; }

        public ThemeModel(MobiseModel parent)
            : base(parent)
        {
            this.Classes = new TrulyObservableCollection<ClassStyleModel>();
        }

        #region Mobise Model
        public override System.Xml.Linq.XElement ToXml()
        {
            XElement newElement = new XElement("theme");
            return this.ToXml(newElement, true);
        }

        public override System.Xml.Linq.XElement ToXml(System.Xml.Linq.XElement baseElement, bool AddCommonAttributes)
        {
            baseElement.SetAttributeValue("name", this.Name);
            AddToXMLChildElementsByName(baseElement, "classes", this.Classes);
            return baseElement;
        }

        public override void FromXml(System.Xml.Linq.XElement xml)
        {
            this.Name = xml.Attribute("name") != null ? xml.Attribute("name").Value : string.Empty;

            if (xml.HasElements)
            {
                // targets

                // classes
                ReadFromXMLChildItemsByName<ClassStyleModel>(xml, "classes", this.Classes, this);
            }
        }
        #endregion

        public Object Clone()
        {
            ThemeModel newTheme = new ThemeModel(this.Parent);
            newTheme.FromXml(this.ToXml());
            return newTheme;
        }
    }
}
