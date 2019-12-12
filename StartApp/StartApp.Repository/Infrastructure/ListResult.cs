using System.Collections.Generic;

namespace StartApp.Repository.Infrastructure
{
    public class ListResult<T> where T : class
    {
        public IEnumerable<T> List { get; set; }
        public int TotalCount { get; set; }
    }
}