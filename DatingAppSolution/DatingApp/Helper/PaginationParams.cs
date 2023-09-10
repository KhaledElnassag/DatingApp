namespace DatingApp.Helper
{
    public class PaginationParams
    {
        private const int MaxPageSize = 50;
        public int PageNumber { get; set; } = 1;
        private int _pageSize = 12;

        public int PageSize
        {
            get => _pageSize;
            set => _pageSize = (value > MaxPageSize) ? MaxPageSize : value;
        }
        public string? CurrentName { get; set; }
        public string? Gender { get; set; }
        public int MinAge { get; set; } = 18;
        public int MaxAge { get; set; } = 100;
        public DateTime MinAg => DateTime.Today.AddYears(-MaxAge-1);
        public DateTime MaxAg =>DateTime.Today.AddYears(-MinAge-1);
        public string OrderBy { get; set; } = "lastActivity";
    }
}
