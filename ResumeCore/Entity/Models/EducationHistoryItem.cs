using System;
using System.Collections.Generic;

namespace ResumeCore.Entity.Models {
    public class EducationHistoryItem {
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public Institution Institution { get; set; }
        public string AreasOfStudy { get; set; }
        public IEnumerable<Degree> CompletedDegrees { get; set; }
        public IEnumerable<Degree> Partials { get; set; }
    }
}
