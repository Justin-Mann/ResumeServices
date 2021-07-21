using System.Collections.Generic;

namespace ResumeCore.Entity.Models {
    public class Resume {
        public DocumentVersion DocumentVersion { get; set; }
        public Person Person { get; set; }
        public string JobTitle { get; set; }
        public string ObjectiveStatement { get; set; }
        public IEnumerable<SkillGroup> SkillGroups { get; set; }
        public IEnumerable<EducationHistoryItem> EducationHistory { get; set; }
        public IEnumerable<WorkHistoryItem> WorkHistory { get; set; }
    }
}
