using System;

namespace PersonAPI.Models.Request {
    public class PersonRequest {
        public Guid? Id { get; set; }
        public string Name { get; set; }
    }
}