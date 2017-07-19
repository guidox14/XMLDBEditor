using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPFDesigner_XML.Common.Models.Entity;

namespace WPFDesigner_XML
{
    public static class Helper
    {
        public static bool ValidateBoolFromString(string currentBool)
        {
            if (currentBool == null)
                return false;
            if (currentBool.ToLower().Equals("false"))
                return false;
            else
                return true;
        }

        public static SyncType ConvertIntToSyncType(int syncType)
        {
            SyncType newSyncType = (SyncType)syncType;
            return newSyncType;
        }

        public static int ConvertSyncTypeToInt(SyncType syncType)
        {
            int newSyncType = (Int32)syncType;
            return newSyncType;
        }

        public static string ConvertSyncTypeToString(SyncType syncType)
        {
            string newSyncType = syncType.ToString();
            return newSyncType;
        }

        public static SyncType ConvertStringToSyncType(string syncType)
        {
            switch (syncType)
            {
                case "syncBothDirections":
                    return SyncType.syncBothDirections;
                case "syncToDevice":
                    return SyncType.syncToDevice;
                case "syncToMiddleTier":
                    return SyncType.syncToMiddleTier;
                default:
                    return SyncType.syncBothDirections;
            }
        }

            public static string ConvertAttTypeToString(AttributeType attType)
        {
            string newAttType = attType.ToString();
            switch (newAttType)
            {
                case "Integer16":
                    newAttType = "Integer 16";
                    break;
                case "Integer32":
                    newAttType = "Integer 32";
                    break;
                case "Integer64":
                    newAttType = "Integer 64";
                    break;
            }
            return newAttType;
        }

        public static string ConvertConflictResRuleToString(ConflictResolutionRule conflictRule)
        {
            string newConflictRule = conflictRule.ToString();
            return newConflictRule;
        }

        public static string ConvertBoolToString(bool? value)
        {
            if (value == null)
                return "False";
            if ( (bool) value)
                return "True";
            else
                return "False";
        }
    }
}
