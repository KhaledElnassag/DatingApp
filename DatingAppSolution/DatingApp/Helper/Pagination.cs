using Microsoft.EntityFrameworkCore;

namespace DatingApp.Helper
{
    public class Pagination<T>
    {
        public Pagination(int currentPage, int pageSize, int totalCount, IEnumerable<T> data)
        {
            CurrentPage = currentPage;
            TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize);
            PageSize = pageSize;
            TotalCount = totalCount;
            Data = data;
        }

        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }
        public IEnumerable<T> Data { get; set; }
       
    }
}
