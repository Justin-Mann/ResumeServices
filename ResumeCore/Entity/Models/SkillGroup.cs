using System.Collections.Generic;

namespace ResumeCore.Entity.Models {
    public class SkillGroup {
        public string Name { get; set; }
        public IEnumerable<Skill> Skills { get; set; }
    }
}
