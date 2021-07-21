using ResumeCore.Entity.Models;
using System;

namespace PersonAPI.Models.Response {
    public class PersonResponse : Person {
        public Guid Id { get; set; }
    }
}
