namespace mini_project_csharp.Models
{
    public class PagedResult<T>
    {
        public required List<T> Items { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalPages { get; set; }
        public int TotalCount { get; set; }
    }
}
