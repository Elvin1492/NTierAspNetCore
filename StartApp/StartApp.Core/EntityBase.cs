using System;

namespace StartApp.Core
{
    public class EntityBase
    {
        public virtual int Id { get; set; }

        public virtual DateTime AddedDate { get; set; }

        public virtual DateTime ModifiedDate { get; set; }

        public virtual bool Deleted { get; set; }

        public virtual bool NewEntity => Id == 0;
    }
}
