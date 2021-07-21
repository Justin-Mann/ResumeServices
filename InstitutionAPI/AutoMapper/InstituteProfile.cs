using AutoMapper;
using InstitutionAPI.Models;
using InstitutionAPI.Models.Response;
using ResumeCore.Entity.Models;

namespace InstitutionAPI.AutoMapper {
    public class InstituteProfile: Profile {
        public InstituteProfile() {
            CreateMap<InstitutionEntity, InstitutionResponse>().IncludeMembers(s => s.Institution);
            CreateMap<Institution, InstitutionResponse>();
        }
    }
}
