using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using WPFDesigner_XML.Common.Models;
using WPFDesigner_XML.Common.Models.UIModels;

namespace WPFDesigner_XML.Common
{
    /// <summary>
    /// Base class for all models and objects
    /// </summary>
    public class MobiseObject : INotifyPropertyChanged, INotifyDataErrorInfo
    {

        public MobiseObject()             
        {
            this.mMobiseObjectID = Guid.NewGuid().ToString();
            this.PropertyErrors = new Dictionary<string, List<string>>();
            
        }

        public string mMobiseObjectID;
        public string MobiseObjectID {
            get
            {
                return this.mMobiseObjectID;
            }

            set
            {
                if (this is ScreenModel || this is UIControlInstanceModel)
                {
                    this.mMobiseObjectID = value;
                }
                else
                {
                    throw new InvalidOperationException("The mobiseID is inmutable and should not be changed by external code.");
                }
            }
        }

        protected MobiseObjectValidState mValidState;
        public MobiseObjectValidState ValidState {
            get
            {
                return mValidState;
            }
            set
            {
                if (mValidState != value)
                {
                    mValidState = value;
                    this.NotifyPropertyChanged("ValidState");
                }
            }
        }

        protected MobiseObjectDirtyState mDirtyState;
        public MobiseObjectDirtyState DirtyState
        {
            get
            {
                return mDirtyState;
            }
            set
            {
                if (mDirtyState != value)
                {
                    mDirtyState = value;
                    this.NotifyPropertyChanged("DirtyState");
                }
            }
        }


        protected string mName;
        public string Name
        {
            get
            {
                return this.mName;
            }

            set
            {
                
                if (this.mName != value)
                {
                    this.mName = value;
                    if (!string.IsNullOrEmpty(this.mName))
                    {
                        Regex filter = new Regex(@"\W");
                        this.mName = filter.Replace(this.mName, "");
                    }

                    this.NotifyPropertyChanged("Name");
                }
            }
        }
         
        #region INotifyPropertyChanged
         
        /// <summary>
        /// Event fire when a property value was changed
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Notify that a property has changed.
        /// </summary>
        /// <param name="property">Name of the property that was changed</param>
        public virtual void NotifyPropertyChanged(string property)
        { 
            if (this.PropertyChanged != null)
            {
                ValidationContext validationContext = new ValidationContext(this, null, null);
                List<ValidationResult> validationResults = new List<ValidationResult>();
                this.PropertyErrors.Clear();

                if (Validator.TryValidateObject(this, validationContext, validationResults))
                {
                    foreach (var result in validationResults)
                    {
                        foreach (var member in result.MemberNames)
                        {
                            this.AddError(member, result.ErrorMessage);
                        }
                    }
                }
                if (this.PropertyErrors.Count() > 0)
                {
                    this.ValidState = MobiseObjectValidState.isInvalid;
                }

                // something changed.
                this.DirtyState = MobiseObjectDirtyState.isDirty;

                this.PropertyChanged(this, new PropertyChangedEventArgs(property));
            }
        }
         
        #endregion

        #region IDataErrorInfo

        public string Error
        {
            get
            {
                StringBuilder errorBuilder = new StringBuilder();
                foreach (var property in this.PropertyErrors.Keys)
                {
                    this.PropertyErrors[property].ForEach( message => errorBuilder.AppendLine(string.Format("{0}:{1}", property, message)) );
                }

                return errorBuilder.ToString();
            }
        }

        //public string this[string columnName]
        //{
        //    get
        //    {
        //        if (this.PropertyErrors.ContainsKey(columnName))
        //        {
        //            return this.PropertyErrors[columnName];
        //        }
        //        else
        //        {
        //            return String.Empty;
        //        }
        //    }
        //}

        /// <summary>
        /// Gets the dictionary use to keep track of the validation errors in the properties.
        /// </summary>
        protected Dictionary<string, List<string>> PropertyErrors
        {
            get;
            set;
        }

        #endregion
        public override int GetHashCode()
        {
            return this.MobiseObjectID.GetHashCode();
        }
        public override bool Equals(object obj)
        {
            if (obj.GetType().Equals(this.GetType()))
            {
                return ((MobiseObject)obj).mMobiseObjectID.Equals(this.mMobiseObjectID);
            }

            return false;
        }

        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

        public virtual void NotifyErrorChange(string propertyName)
        {
            if (this.ErrorsChanged != null)
            {
                this.ErrorsChanged(this, new DataErrorsChangedEventArgs(propertyName));
            }
        }

        public System.Collections.IEnumerable GetErrors(string propertyName)
        {
            if (string.IsNullOrEmpty(propertyName))
            {
                return null;
            }
            else if (this.PropertyErrors.ContainsKey(propertyName))
            {
                return this.PropertyErrors[propertyName];
            }
            else
            {
                return null;
            }
        }

        public bool HasErrors
        {
            get 
            { 
                if (this.PropertyErrors.Count > 0)
                {
                    foreach (string key in this.PropertyErrors.Keys)
                    {
                        if (this.PropertyErrors[key] != null && this.PropertyErrors[key].Count > 0)
                        {
                            return true;
                        }
                    }
                }
                return false;
            }
        }

        public virtual void ClearErrors(string propertyName)
        {
            if (this.PropertyErrors.ContainsKey(propertyName) && this.PropertyErrors[propertyName]  != null)
            {
                this.PropertyErrors[propertyName].Clear();
            }
        }

        public virtual void AddError(string propertyName, string errorMessage)
        {
            if (!this.PropertyErrors.ContainsKey(propertyName))
            {
                this.PropertyErrors.Add(propertyName, new List<string>());
            }
            else if (this.PropertyErrors[propertyName] == null)
            {
                this.PropertyErrors[propertyName] = new List<string>();
            }
            this.PropertyErrors[propertyName].Add(errorMessage);
            this.NotifyErrorChange(propertyName);
        }

    }

    public enum MobiseObjectValidState
    { 
        isInvalid,
        isValid
    }

    public enum MobiseObjectDirtyState
    {
        isNew,
        isDirty,
        isDeleted,
        isUnchanged 
    }
}
