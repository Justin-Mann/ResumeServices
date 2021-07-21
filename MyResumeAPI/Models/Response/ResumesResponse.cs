using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace MyResumeAPI.Models.Response {
    public class ResumesResponse {
        public ResumesResponse() {
            Resumes = new Collection<ResumeResponse>();
        }
        public ICollection<ResumeResponse> Resumes {
            get; set;
        }
    }
}
