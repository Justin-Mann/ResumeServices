using System;
using System.Collections.Generic;

namespace ResumeCore.Entity.Models {
    public class WorkHistoryItem {
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public Institution Employer { get; set; }
        public Institution Client { get; set; }
        public string JobTitle { get; set; }
        public string JobType { get; set; }
        public string DescriptionOfDuties { get; set; }
        public string ReasonForLeaving { get; set; }
        public IEnumerable<Person> References { get; set; }
        public IEnumerable<string> CompletedProjects { get; set; }
    }
}
