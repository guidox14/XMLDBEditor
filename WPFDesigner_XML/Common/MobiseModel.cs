using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace WPFDesigner_XML.Common
{
    public abstract class MobiseModel : MobiseObject
    {
        public MobiseModel Parent { get; set; }

        public DateTime? LoadDate { get; set; }
        
        public MobiseModel()
            : base()
        {
             this.Parent = null;
        }

        public MobiseModel(MobiseModel parent)
            : this()
        {
            this.Parent = parent;
        }

        #region xml manangement
      
        public abstract XElement ToXml();
        public abstract XElement ToXml(XElement baseElement, bool AddCommonAttributes);
        public abstract void FromXml(XElement xml);

        public static void AddToXMLChildElementsByName(XElement parent, string collectionTagName, IEnumerable<MobiseModel> childItems)
        {
            XElement childCollectionTag = parent.Element(collectionTagName);
            if (childCollectionTag == null)
            {
                childCollectionTag = new XElement(collectionTagName);
                parent.Add(childCollectionTag);
            }
            foreach (MobiseModel childItem in childItems)
            {
                XElement childElement = childCollectionTag.Elements().FirstOrDefault(item => (item.Attribute("name") != null ? item.Attribute("name").Value : string.Empty) == childItem.Name.ToLowerInvariant());
                if (childElement == null)
                {
                    childCollectionTag.Add(childItem.ToXml());
                }
                else
                {
                    childItem.ToXml(childElement, true);
                }

            }
        }

        public static void ReadFromXMLChildItemsByName<T>(XElement xml, string childColletionTagName, ICollection<T> childCollection, MobiseModel parent) where T : MobiseModel
        {
            XElement childColletionTag = xml.Element(childColletionTagName);
            if (childColletionTag != null && childColletionTag.HasElements)
            {

                // Parallel.ForEach(controls.Elements("control"), (element) =>
                foreach (XElement element in childColletionTag.Elements())
                {
                    string childItemName = element.Attribute("name") != null ? element.Attribute("name").Value : string.Empty;
                    T newControl = childCollection.FirstOrDefault(ctrl => ctrl.Name == childItemName);
                    if (newControl == null)
                    {
                        newControl = System.Activator.CreateInstance(typeof(T), parent) as T;
                        childCollection.Add(newControl);
                    }
                    newControl.FromXml(element);

                };

            }
        }

        public static void AddToXMLChildElementsByID(XElement parent, string collectionTagName, IEnumerable<MobiseModel> childItems)
        {
            XElement childCollectionTag = parent.Element(collectionTagName);
            if (childCollectionTag == null)
            {
                childCollectionTag = new XElement(collectionTagName);
                parent.Add(childCollectionTag);
            }
            foreach (MobiseModel childItem in childItems)
            {
                XElement childElement = childCollectionTag.Elements().FirstOrDefault(item => (item.Attribute("mobiseID") != null?item.Attribute("mobiseID").Value: Guid.NewGuid().ToString()) == childItem.MobiseObjectID);
                if (childElement == null)
                {
                    childCollectionTag.Add(childItem.ToXml());
                }
                else
                {
                    childItem.ToXml(childElement, true);
                }

            }
        }
        public static void ReadFromXMLChildItemsByID<T>(XElement xml, string childColletionTagName, ICollection<T> childCollection, MobiseModel parent) where T : MobiseModel
        {
            XElement childColletionTag = xml.Element(childColletionTagName);
            if (childColletionTag != null && childColletionTag.HasElements)
            {

                // Parallel.ForEach(controls.Elements("control"), (element) =>
                foreach (XElement element in childColletionTag.Elements())
                {
                    string childItemID = element.Attribute("mobiseID") != null ? element.Attribute("mobiseID").Value : string.Empty;
                    T newControl = childCollection.FirstOrDefault(ctrl => ctrl.MobiseObjectID == childItemID);
                    if (newControl == null)
                    {
                        newControl = System.Activator.CreateInstance(typeof(T), parent) as T;
                        childCollection.Add(newControl);
                    }
                    newControl.FromXml(element);

                };

            }
        }
        #endregion

    }
}
