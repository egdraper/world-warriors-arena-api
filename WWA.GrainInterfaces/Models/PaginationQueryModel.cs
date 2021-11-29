using System;

namespace WWA.GrainInterfaces.Models
{
    [Serializable]
    public class PaginationQueryModel
    {
        public int Skip { get; set; } = 0;
        public int Take { get; set; } = 100;
        public string SortField { get; set; }
        public SortDirectionType SortDirection { get; set; }
        public string Search { get; set; }
    }

    public enum SortDirectionType { Ascending, Descending }
}
