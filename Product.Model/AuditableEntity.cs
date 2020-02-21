using System;
using System.Text.Json.Serialization;

namespace Products.Model
{
    public interface IAuditableEntity
    {
        DateTimeOffset? CreatedDate { get; set; }
        DateTimeOffset? UpdatedDate { get; set; }
    }

    public class AuditableEntity : IAuditableEntity
    {
        [JsonIgnore]
        public DateTimeOffset? UpdatedDate { get; set; }
        [JsonIgnore]
        public DateTimeOffset? CreatedDate { get; set; }
    }
}
