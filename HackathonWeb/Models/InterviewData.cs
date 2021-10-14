using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HackathonWeb.Models
{
    public class InterviewData
    {
        public int SrNo { get; set; }
        public string Name { get; set; }
        public List<string> SkillSet { get; set; }
        public string Source { get; set; }
        public Show Show { get; set; }
        public string Contact { get; set; }
        public string Email { get; set; }
        public Decimal Experience { get; set; }
        public int NoticePeriod { get; set; }
        public DateTime? InterviewTime { get; set; }
        public string TechnicalPanel { get; set; }
        public string MeetingLink { get; set; }
        public Status Status { get; set; }
        public string Feedback { get; set; }
        public string Note { get; set; }
        public string ResumeLink { get; set; }
    }

    public enum Show
    {
        Yes = 0,
        No = 1
    }

    public enum Status
    {
        Selected = 0,
        Rejected = 1
    }
}
