using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;
using WPFDesigner_XML.Common.Models.ControllerModels;
using WPFDesigner_XML.Common.Models.ScriptableControlMetadata;
using WPFDesigner_XML.Common.Models.UIModels;
using WPFDesigner_XML.Common.Models.UIStyleModels;

namespace WPFDesigner_XML.Common.Models
{
    public class MobiseConfiguration : MobiseModel
    {
        public TrulyObservableCollection<UIControlDefinitionModel> Controls { get; private set; }
        public TrulyObservableCollection<ThemeModel> Themes { get; private set; }
        public TrulyObservableCollection<StyleSchemaMappingModel> StyleSchemaMappings { get; private set; }
        public TrulyObservableCollection<StyleAttributeDefaultValueModel> StyleAttributeDefaultValues { get; private set; }
        public TrulyObservableCollection<AttributeCategoryModel> baseControlAttributes { get; private set; }
        public TrulyObservableCollection<ControllerDefinitionModel> ControllerDefinitons { get; private set; }
        public TrulyObservableCollection<MBObjectModel> MBObjects { get; private set; }

        public MobiseConfiguration()
            : base()
        {
            this.Controls = new TrulyObservableCollection<UIControlDefinitionModel>();
            this.Themes = new TrulyObservableCollection<ThemeModel>();
            this.StyleSchemaMappings = new TrulyObservableCollection<StyleSchemaMappingModel>();
            this.StyleAttributeDefaultValues = new TrulyObservableCollection<StyleAttributeDefaultValueModel>();
            this.baseControlAttributes = new TrulyObservableCollection<AttributeCategoryModel>();
            this.ControllerDefinitons = new TrulyObservableCollection<ControllerDefinitionModel>();
            this.MBObjects = new TrulyObservableCollection<MBObjectModel>();
        }

        #region Mobise Model
        /// <summary>
        /// To the XML.
        /// </summary>
        /// <returns></returns>
        public override XElement ToXml()
        {
            XElement baseElement = new XElement("MobiseConfigurations");
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

            AddToXMLChildElementsByName(baseElement, "controllerDefinitions", this.ControllerDefinitons);
            AddToXMLChildElementsByName(baseElement, "controls", this.Controls);
            AddToXMLChildElementsByName(baseElement, "themes", this.Themes);
            AddToXMLChildElementsByName(baseElement, "StyleSchemaMappings", this.StyleSchemaMappings);
            AddToXMLChildElementsByName(baseElement, "StyleAttributeDefaultValues", this.StyleAttributeDefaultValues);
            AddToXMLChildElementsByName(baseElement, "MBObjects", this.MBObjects);

            XElement baseControl = baseElement.Element("baseControl");
            if (baseControl == null)
            {
                baseControl = new XElement("baseControl");
                baseElement.Add(baseControl);
            }
            AddToXMLChildElementsByName(baseControl, "AttributeCategories", this.baseControlAttributes);

            


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
                Task loadControllerDefinitions = Task.Factory.StartNew(() =>
                {
                    ReadFromXMLChildItemsByName<ControllerDefinitionModel>(xml, "controllerDefinitions", this.ControllerDefinitons, this);
                });
                
                Task loadControls = Task.Factory.StartNew(() =>
                    {
                        XElement baseControl = xml.Element("baseControl");
                        if (baseControl != null && baseControl.HasElements)
                        {
                            ReadFromXMLChildItemsByName<AttributeCategoryModel>(baseControl, "AttributeCategories", this.baseControlAttributes, this);
                        }
                        ReadFromXMLChildItemsByName<UIControlDefinitionModel>(xml, "controls", this.Controls, this);
                    });

                Task loadThemes = Task.Factory.StartNew(() =>
                    {
                        ReadFromXMLChildItemsByName<ThemeModel>(xml, "themes", this.Themes, this);
                    });

                Task loadStyleSchemaMappings = Task.Factory.StartNew(() =>
                {
                    ReadFromXMLChildItemsByName<StyleSchemaMappingModel>(xml, "StyleSchemaMappings", this.StyleSchemaMappings, this);
                });

                Task loadStyleAttributeDefaultValues = Task.Factory.StartNew(() =>
                {
                    ReadFromXMLChildItemsByName<StyleAttributeDefaultValueModel>(xml, "StyleAttributeDefaultValues", this.StyleAttributeDefaultValues, this);
                });

                Task loadMBObjects = Task.Factory.StartNew(() =>
                {
                    ReadFromXMLChildItemsByName<MBObjectModel>(xml, "MBObjects", this.MBObjects, this);
                });

                Task.WaitAll(loadControllerDefinitions, loadControls, loadThemes, loadStyleSchemaMappings, loadStyleAttributeDefaultValues, loadMBObjects);

                // set default styles for controls
                foreach (UIControlDefinitionModel control in this.Controls)
                {
                    var className = this.Themes.FirstOrDefault().Classes.Where(c => c.Name == "Default" + control.Name).FirstOrDefault();
                    if (className != null)
                    {
                        control.DefaultStyleClass = className.Name;
                    }
                }
            }
        }

        #endregion

    }
}
