using System;
using Famoser.LectureSync.Business.Models.Base;
using Newtonsoft.Json;

namespace Famoser.LectureSync.Business.Models
{
    public class Lecture : BaseEventModel
    {
        private DayOfWeek _dayOfWeek;
        public DayOfWeek DayOfWeek
        {
            get { return _dayOfWeek; }
            set { Set(ref _dayOfWeek, value); }
        }

        private TimeSpan _startTime;
        public TimeSpan StartTime
        {
            get { return _startTime; }
            set { Set(ref _startTime, value); }
        }

        private TimeSpan _endTime;
        public TimeSpan EndTime
        {
            get { return _endTime; }
            set { Set(ref _endTime, value); }
        }

        [JsonIgnore]
        public Course Course { get; set; }

    }
}
