using System;

namespace Order.Application.Common
{
    public abstract class EntityBase
    {
        public int Id { get; protected set; }

        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
    }
}