﻿using System;
using System.Collections.Generic;
using System.Globalization;
using Newtonsoft.Json;
using Plugins.CountlySDK.Persistance;

namespace Plugins.CountlySDK.Models
{
    [Serializable]
    public class CountlyEventModel : IModel
    {
        /// <summary>
        ///     Initializes a new instance of event model.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="segmentation"></param>
        /// <param name="count"></param>
        /// <param name="sum"></param>
        /// <param name="duration"></param>
        public CountlyEventModel(string key, IDictionary<string, object> segmentation = null, int? count = 1,
            double? sum = null,
            double? duration = null)
        {
            Key = key;
            Count = count ?? 1;
            if (segmentation != null)
            {
                Segmentation = new SegmentModel(segmentation);
            }
            Duration = duration;
            Sum = sum;

            //Records the time the time the event was recorded
//            TimeRecorded = DateTime.Now;

            var now = DateTime.Now;
            Hour = now.TimeOfDay.Hours;
            DayOfWeek = (int)now.DayOfWeek;
            Timezone = TimeZone.CurrentTimeZone.GetUtcOffset(new DateTime()).TotalMinutes;
            Timestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

        }

        public CountlyEventModel()
        {
        }

        [JsonIgnore]
        public long Id { get; set; }
        
        [JsonProperty("key")] public string Key { get; set; }

        [JsonProperty("count")] public int? Count { get; set; }

        [JsonProperty("sum")] public double? Sum { get; set; }

        [JsonProperty("dur")] public double? Duration { get; set; }

        [JsonProperty("segmentation")] public SegmentModel Segmentation { get; set; }

        [JsonProperty("timestamp")] public long Timestamp { get; set; }

        [JsonProperty("hour")] public int Hour { get; set; }

        [JsonProperty("dow")] public int DayOfWeek { get; set; }

        [JsonProperty("tz")] public double Timezone { get; set; }

//        [JsonIgnore] public DateTime TimeRecorded { get; set; }

        #region Reserved Event Names

        [JsonIgnore] internal const string ViewEvent = "[CLY]_view";

        [JsonIgnore] internal const string ViewActionEvent = "[CLY]_action";

        [JsonIgnore] internal const string StarRatingEvent = "[CLY]_star_rating";

        [JsonIgnore] internal const string PushActionEvent = "[CLY]_push_action";

        #endregion

        protected bool Equals(CountlyEventModel other)
        {
            return Id == other.Id && string.Equals(Key, other.Key) && Count == other.Count && Sum.Equals(other.Sum) && Duration.Equals(other.Duration) && Equals(Segmentation, other.Segmentation) && Timestamp == other.Timestamp && Hour == other.Hour && DayOfWeek == other.DayOfWeek && Timezone.Equals(other.Timezone);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((CountlyEventModel) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = Id.GetHashCode();
                hashCode = (hashCode * 397) ^ (Key != null ? Key.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ Count.GetHashCode();
                hashCode = (hashCode * 397) ^ Sum.GetHashCode();
                hashCode = (hashCode * 397) ^ Duration.GetHashCode();
                hashCode = (hashCode * 397) ^ (Segmentation != null ? Segmentation.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ Timestamp.GetHashCode();
                hashCode = (hashCode * 397) ^ Hour;
                hashCode = (hashCode * 397) ^ DayOfWeek;
                hashCode = (hashCode * 397) ^ Timezone.GetHashCode();
                return hashCode;
            }
        }

        public override string ToString()
        {
            return $"{nameof(Id)}: {Id}, {nameof(Key)}: {Key}, {nameof(Count)}: {Count}, {nameof(Sum)}: {Sum}, {nameof(Duration)}: {Duration}, {nameof(Segmentation)}: {Segmentation}, {nameof(Timestamp)}: {Timestamp}, {nameof(Hour)}: {Hour}, {nameof(DayOfWeek)}: {DayOfWeek}, {nameof(Timezone)}: {Timezone}";
        }
    }
}