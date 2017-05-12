using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace WPFDesigner_XML.Common.Models.UIStyleModels
{
    public class TargetModel : MobiseModel
    {
        private TrulyObservableCollection<DeviceModel> mDevices;
        public TrulyObservableCollection<DeviceModel> Devices
        {
            get
            {
                return this.mDevices;
            }
            private set
            {
                this.mDevices = value;
                this.NotifyPropertyChanged("Devices");
            }
        }

        public TargetModel() : base()
        {
            this.mDevices = new TrulyObservableCollection<DeviceModel>();
        }

          /// <summary>
        /// Initializes a new instance of the <see cref="File"/> class.
        /// </summary>
        /// <param name="xmlModel">The XML model.</param>
        public TargetModel(XElement xmlModel)
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
            XElement targets = new XElement("targets");
            targets.SetAttributeValue("mobiseID", this.MobiseObjectID);

            XElement devicesElement = new XElement("devices");
            targets.Add(devicesElement);

            foreach (DeviceModel device in this.Devices)
            {
                devicesElement.Add(device.ToXml());
            }

            return targets;
        }

        public override XElement ToXml(XElement baseElement, bool AddCommonAttributes)
        {
            throw new NotImplementedException();
        }

        public override void FromXml(XElement xml)
        {
            foreach (var device in xml.Elements("devices").Elements("device"))
            {
                this.Devices.Add(new DeviceModel(device));
            }
        }


        #endregion
    }
}
