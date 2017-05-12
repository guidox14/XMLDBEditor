using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace WPFDesigner_XML.Common.Models
{
    public class ReferencesModel : MobiseModel
    {
        private TrulyObservableCollection<PluginModel> mPlugins;
        public TrulyObservableCollection<PluginModel> Plugins
        {
            get
            {
                return this.mPlugins;
            }
            private set
            {
                this.mPlugins = value;
                this.NotifyPropertyChanged("Plugins");
            }
        }

         public ReferencesModel() : base()
        {
            this.mPlugins = new TrulyObservableCollection<PluginModel>();
        }

          /// <summary>
        /// Initializes a new instance of the <see cref="File"/> class.
        /// </summary>
        /// <param name="xmlModel">The XML model.</param>
         public ReferencesModel(XElement xmlModel)
            : this()
        { 
            if (xmlModel != null)
            {
                this.FromXml(xmlModel);
            }
        }

        #region MobiseModel Base

        public override XElement ToXml()
        {
            XElement references = new XElement("references");
            references.SetAttributeValue("mobiseID", this.MobiseObjectID);

            XElement pluginsElement = new XElement("Plugins");
            references.Add(pluginsElement);

            foreach (PluginModel plugin in this.Plugins)
            {
                pluginsElement.Add(plugin.ToXml());
            }

            return references;
        }

        public override XElement ToXml(XElement baseElement, bool AddCommonAttributes)
        {
            throw new NotImplementedException();
        }

        public override void FromXml(XElement xml)
        {
            foreach(var plugin in xml.Elements("Plugins").Elements("Plugin"))
            {
                this.Plugins.Add(new PluginModel(plugin, null));
            }
        }


        #endregion
    }
}
