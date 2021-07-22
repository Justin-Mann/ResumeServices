using AutoMapper;
using InstitutionAPI.Models;
using InstitutionAPI.Models.Response;

namespace InstitutionAPI.AutoMapper {
    public class InstituteProfile: Profile {
        public InstituteProfile() {
            CreateMap<InstitutionEntity, InstitutionResponse>().IncludeMembers(s => s.Institution);
            CreateMap<Institution, InstitutionResponse>();
        }
    }
}
