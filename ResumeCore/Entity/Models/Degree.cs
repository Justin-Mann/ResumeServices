using System;

namespace ResumeCore.Entity.Models {
    public class Degree {
        public string Name { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string Detail { get; set; }
    }
}
