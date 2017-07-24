using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlowUtilities
{
    public class TimeZone
    {
        private int pos = 0;
        private ReadOnlyCollection<TimeZoneInfo> list = TimeZoneInfo.GetSystemTimeZones();

        public int RecordCount()
        {
            return list.Count;
        }

        public int RecordPosition()
        {
            return pos;
        }

        public void First()
        {
            pos = 0;
        }

        public void Next()
        {
            ++pos;
        }

        public void Last()
        {
            pos = list.Count - 1;
        }

        public bool BOF()
        {
            return pos == 0;
        }

        public bool EOF()
        {
            return pos >= list.Count;
        }

        public bool Find(string field, string value)
        {
            switch (field.ToLower())
            {
                case "id":
                    for (int i = 0; i < list.Count; ++i)
                        if (list[i].Id == value)
                        {
                            pos = i;
                            return true;
                        }
                    return false;
                case "daylightname":
                    for (int i = 0; i < list.Count; ++i)
                        if (list[i].DaylightName == value)
                        {
                            pos = i;
                            return true;
                        }
                    return false;
                case "displayname":
                    for (int i = 0; i < list.Count; ++i)
                        if (list[i].DisplayName == value)
                        {
                            pos = i;
                            return true;
                        }
                    return false;
                case "standardname":
                    for (int i = 0; i < list.Count; ++i)
                        if (list[i].StandardName == value)
                        {
                            pos = i;
                            return true;
                        }
                    return false;
                default:
                    return false;
            }
        }

        public string Id()
        {
            if (pos < list.Count)
                return list[pos].Id;
            else
                return null;
        }

        public bool SupportsDaylightSavingTime()
        {
            if (pos < list.Count)
                return list[pos].SupportsDaylightSavingTime;
            else
                return false;
        }

        public double BaseUtcOffset()
        {
            if (pos < list.Count)
                return list[pos].BaseUtcOffset.TotalDays;
            else
                return 0;
        }

        public string DisplayName()
        {
            if (pos < list.Count)
                return list[pos].DisplayName;
            else
                return null;
        }

        public string StandardName()
        {
            if (pos < list.Count)
                return list[pos].StandardName;
            else
                return null;
        }

        public string DaylightName()
        {
            if (pos < list.Count)
                return list[pos].DaylightName;
            else
                return null;
        }
    }
}
