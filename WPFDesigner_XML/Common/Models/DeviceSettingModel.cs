using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace WPFDesigner_XML.Common.Models
{
    public class DeviceSettingModel : MobiseModel
    {
        private string mVersion;
        public string Version
        {
            get
            {
                return mVersion;
            }
            set
            {
                if (mVersion != value)
                {
                    mVersion = value;
                    this.NotifyPropertyChanged("Version");
                }
            }
        }

        private string mStartupObject;
        public string StartupObject
        {
            get
            {
                return mStartupObject;
            }
            set
            {
                if (mStartupObject != value)
                {
                    mStartupObject = value;
                    this.NotifyPropertyChanged("StartupObject");
                }
            }
        }
        #region MobiseModel Base

        public override XElement ToXml()
        {
            throw new NotImplementedException();
        }

        public override XElement ToXml(XElement baseElement, bool AddCommonAttributes)
        {
            throw new NotImplementedException();
        }

        public override void FromXml(XElement xml)
        {
            throw new NotImplementedException();
        }


        #endregion
    }
}
