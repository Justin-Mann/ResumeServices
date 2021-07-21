using AutoMapper;
using PersonAPI.Models;
using PersonAPI.Models.Response;
using ResumeCore.Entity.Models;

namespace PersonAPI.AutoMapper {
    public class PersonProfile: Profile {
        public PersonProfile() {
            CreateMap<PersonEntity, PersonResponse>().IncludeMembers(s => s.Person);
            CreateMap<Person, PersonResponse>();
        }
    }
}
