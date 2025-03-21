using System.ComponentModel;

namespace EventManagement.Model
{
    public class EventScheduleDTO
    {
        [DisplayName("Event name")]
        public string? EventTitle { get; set; }

        [DisplayName("Schedule date")]
        public string? ScheduleDate { get; set; }

        [DisplayName("Start time")]
        public string? StartTime { get; set; }
        [DisplayName("End time")]
        public string? EndTime { get; set; }
    }
}
