using System;
using System.Text.Json.Serialization;

namespace Products.Model
{
    public interface IAuditableEntity
    {
        DateTimeOffset? CreatedOn { get; set; }
        DateTimeOffset? UpdatedOn { get; set; }
    }

    public class AuditableEntity : IAuditableEntity
    {
        [JsonIgnore]
        public DateTimeOffset? UpdatedOn { get; set; }
        [JsonIgnore]
        public DateTimeOffset? CreatedOn { get; set; }
    }
}
