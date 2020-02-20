using System;

namespace Products.Model
{
    public interface IAuditableEntity
    {
        DateTimeOffset? CreatedDate { get; set; }
        DateTimeOffset? UpdatedDate { get; set; }
    }

    public class AuditableEntity : IAuditableEntity
    {
        public DateTimeOffset? UpdatedDate { get; set; }
        public DateTimeOffset? CreatedDate { get; set; }
    }
}
