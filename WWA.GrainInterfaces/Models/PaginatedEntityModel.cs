using System.Collections.Generic;

namespace WWA.GrainInterfaces.Models
{
    public class PaginatedEntityModel<TEntity>
    {
        public int TotalCount { get; set; }
        public IEnumerable<TEntity> Page { get; set; }
    }
}
