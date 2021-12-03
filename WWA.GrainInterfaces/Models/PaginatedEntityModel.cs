using System.Collections.Generic;

namespace WWA.GrainInterfaces.Models
{
    public class PaginatedEntityModel<TEntity>
    {
        public int TotalCount { get; set; }
        public IList<TEntity> Page { get; set; }
    }
}
