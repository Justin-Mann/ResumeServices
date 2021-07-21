using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace InstitutionAPI.Models.Response {
    public class InstitutionsResponse {
        public InstitutionsResponse() {
            Institutions = new Collection<InstitutionResponse>();
        }
        public ICollection<InstitutionResponse> Institutions { get; set; }
    }
}
