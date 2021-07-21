using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace PersonAPI.Models.Response {
    public class PeopleResponse {
        public PeopleResponse() {
            People = new Collection<PersonResponse>();
        }
        public ICollection<PersonResponse> People { get; set; }
    }
}
