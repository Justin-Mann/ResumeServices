using ResumeCore.Entity.Models;
using System;

namespace InstitutionAPI.Models.Response {
    public class InstitutionResponse: Institution {
        public string Id { get; set; }
    }
}
