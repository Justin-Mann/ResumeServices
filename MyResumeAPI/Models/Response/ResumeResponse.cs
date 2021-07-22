using System;

namespace MyResumeAPI.Models.Response {
    public class ResumeResponse: Resume {
        public Guid Id { get; set; }
    }
}
