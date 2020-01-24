using System;

namespace bgAPI.Entities.Base
{
    public abstract class EntityBase
    {
        public int Id { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }
    }
}