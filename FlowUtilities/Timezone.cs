using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlowUtilities
{
    /// <summary>
    /// Methods to manipulate and convert date, time and timezones
    /// </summary>
    public class TimeZone
    {
        private int pos = 0;
        private ReadOnlyCollection<TimeZoneInfo> list = TimeZoneInfo.GetSystemTimeZones();

        /// <summary>
        /// Return the number of records in the entire collection of known SystemTimeZones
        /// </summary>
        /// <returns>Number of Timezones in the collection</returns>
        public int RecordCount()
        {
            return list.Count;
        }

        /// <summary>
        /// Return the index of the currently selected TimeZoneInfo object
        /// </summary>
        /// <returns>currently selected position</returns>
        public int RecordPosition()
        {
            return pos;
        }

        /// <summary>
        /// Set the TimeZoneInfo collection to the beginning
        /// </summary>
        public void First()
        {
            pos = 0;
        }

        /// <summary>
        /// void Next()
        /// </summary>
        public void Next()
        {
            ++pos;
        }

        /// <summary>
        /// Advance to the next TimeZoneInfo in the collection
        /// </summary>
        public void Last()
        {
            pos = list.Count - 1;
        }

        /// <summary>
        /// Test current position
        /// </summary>
        /// <returns>true if at start of collection, else false</returns>
        public bool BOF()
        {
            return pos == 0;
        }

        
        /// <summary>
        /// Test current position
        /// </summary>
        /// <returns>true if at end of collection, else false</returns>
        public bool EOF()
        {
            return pos >= list.Count;
        }

        /// <summary>
        /// Find a TimeZoneInfo object by checking for a specified field value.  Set the RecordPosition if found.
        /// </summary>
        /// <param name="field">The TimeZoneInfo property being searched</param>
        /// <param name="value">The value to search for</param>
        /// <returns>true if found, else false</returns>
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

        /// <summary>
        /// The Id property of the currently selected TimeZoneInfo
        /// </summary>
        /// <returns>TimeZoneInfo.Id</returns>
        public string Id()
        {
            if (pos < list.Count)
                return list[pos].Id;
            else
                return null;
        }

        /// <summary>
        /// The SupportsDaylightSavingTime property of the currently selected TimeZoneInfo
        /// </summary>
        /// <returns>TimeZoneInfo.SupportsDaylightSavingTime</returns>
        public bool SupportsDaylightSavingTime()
            {
            if (pos < list.Count)
                return list[pos].SupportsDaylightSavingTime;
            else
                return false;
        }

        /// <summary>
        /// The BaseUtcOffset property of the currently selected TimeZoneInfo
        /// </summary>
        /// <returns>TimeZoneInfo.BaseUtcOffset in minutes</returns>
        public double BaseUtcOffset()
        {
            if (pos < list.Count)
                return list[pos].BaseUtcOffset.Hours * 60 + list[pos].BaseUtcOffset.Minutes;
            else
                return 0;
        }

        /// <summary>
        /// The GetUtcOffset property of the currently selected TimeZoneInfo for the specified time
        /// </summary>
        /// <param name="dateTime">the specified time in the current timezone to get the offset for</param>
        /// <returns>total offset in minutes</returns>
        public float GetUtcOffset(DateTime dateTime)
        {
            TimeSpan offset;
            if (pos < list.Count)
            {
                offset = list[pos].GetUtcOffset(dateTime);
                return offset.Hours * 60 + offset.Minutes;
            }
            else
                return 0;
        }

        /// <summary>
        /// The DisplayName property of the currently selected TimeZoneInfo
        /// </summary>
        /// <returns>TimeZoneInfo.DisplayName</returns>
        public string DisplayName()
        {
            if (pos < list.Count)
                return list[pos].DisplayName;
            else
                return null;
        }

        /// <summary>
        /// The StandardName property of the currently selected TimeZoneInfo
        /// </summary>
        /// <returns>TimeZoneInfo.StandardName</returns>
        public string StandardName()
        {
            if (pos < list.Count)
                return list[pos].StandardName;
            else
                return null;
        }

        /// <summary>
        /// The DaylightName property of the currently selected TimeZoneInfo
        /// </summary>
        /// <returns>TimeZoneInfo.DaylightName</returns>
        public string DaylightName()
        {
            if (pos < list.Count)
                return list[pos].DaylightName;
            else
                return null;
        }

        /// <summary>
        /// The IsDaylightSavingTime property of the currently selected TimeZoneInfo
        /// </summary>
        /// <returns>true if Daylight Savings is in effect</returns>
        public bool IsDaylightSavingTime(DateTime dateTime)
        {
            if (pos < list.Count)
                return list[pos].IsDaylightSavingTime(dateTime);
            else
                return false;
        }

        /// <summary>
        /// Id of the Local TimeZone
        /// </summary>
        /// <returns>The TimeZoneInfo Id for the Local timezone</returns>
        public string LocalId()
        {
            return TimeZoneInfo.Local.Id;
        }

        /// <summary>
        /// The GetUtcOffset property of the Local TimeZoneInfo for the specified time
        /// </summary>
        /// <param name="dateTime">the specified time in the Local timezone to get the offset for</param>
        /// <returns>total offset in minutes</returns>
        public float GetLocalUtcOffset(DateTime dateTime)
        {
            TimeSpan offset = TimeZoneInfo.Local.GetUtcOffset(dateTime);
            return offset.Hours * 60 + offset.Minutes;
        }

        /// <summary>
        /// The IsDaylightSavingTime property of the Local TimeZoneInfo for the specified time
        /// </summary>
        /// <param name="dateTime">the specified time in the Local timezone to be checked</param>
        /// <returns>true if Daylight Savings is in effect</returns>
        public bool LocalIsDaylightSavingTime(DateTime dateTime)
        {
            return TimeZoneInfo.Local.IsDaylightSavingTime(dateTime);
        }

        /// <summary>
        /// Id of the UTC TimeZone
        /// </summary>
        /// <returns>The TimeZoneInfo Id for the UTC timezone</returns>
        public string UtcId()
        {
            return TimeZoneInfo.Utc.Id;
        }

        /// <summary>
        /// Convert a dateTime from one Timezone to another
        /// </summary>
        /// <param name="dateTime">The DateTime to be converted</param>
        /// <param name="sourceTzId">Source Timezone Id</param>
        /// <param name="destinationTzId">Target Timezone Id</param>
        /// <returns>The converted dateTime</returns>
        public DateTime ConvertTime(DateTime dateTime, string sourceTzId, string destinationTzId)
        {
            return TimeZoneInfo.ConvertTime(dateTime, TimeZoneInfo.FindSystemTimeZoneById(sourceTzId), TimeZoneInfo.FindSystemTimeZoneById(destinationTzId));
        }

        /// <summary>
        /// Convert a UTC dateTime to Local
        /// </summary>
        /// <param name="dateTime">The UTC dateTime to be converted</param>
        /// <returns>the converted Local dateTime</returns>
        public DateTime ConvertUtcToLocal(DateTime dateTime)
        {
            return TimeZoneInfo.ConvertTimeFromUtc(dateTime, TimeZoneInfo.Local);
        }

        /// <summary>
        /// Convert a Local dateTime to UTC
        /// </summary>
        /// <param name="dateTime">The local dateTime to be converted</param>
        /// <returns>the converted UTC dateTime</returns>
        public DateTime ConvertLocalToUtc(DateTime dateTime)
        {
            return TimeZoneInfo.ConvertTimeToUtc(dateTime, TimeZoneInfo.Local);
        }
    }
}
