using ResumeCore.Entity.Base;
using ResumeCore.Entity.Models;

namespace PersonAPI.Models {
    public class PersonEntity : BaseEntity {
        public Person Person { get; set; }
    }
}
