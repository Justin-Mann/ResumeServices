using AutoMapper;
using MyResumeAPI.Models;
using MyResumeAPI.Models.Response;

namespace MyResumeAPI.AutoMapper {
    public class ResumeProfile: Profile {
        public ResumeProfile() {
            CreateMap<ResumeEntity, ResumeResponse>().IncludeMembers(s => s.Resume);
            CreateMap<Resume, ResumeResponse>();
        }
    }
}
