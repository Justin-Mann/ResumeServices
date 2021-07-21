using Newtonsoft.Json;
using System;

namespace ResumeCore.Entity.Base {
    public class BaseEntity {
        [JsonProperty(PropertyName = "id")]
        public virtual string Id { get; set; }
        public virtual string PartitionKey { get; set; }
    }
}
