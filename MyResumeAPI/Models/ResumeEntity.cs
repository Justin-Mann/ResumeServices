using ResumeCore.Entity.Base;
using ResumeCore.Entity.Models;

namespace MyResumeAPI.Models {
    public class ResumeEntity: BaseEntity {
        public Resume Resume { get; set; }
    }
}
