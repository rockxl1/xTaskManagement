using System;
using System.Collections.Generic;

namespace xTask.SharedEntities.DTOs
{
    public class PaginationDTO
    {
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public int Skip => (Page - 1) * PageSize;
    }

    public class PaginatedResultDTO<T>
    {
        public List<T> Data { get; set; }
        public int TotalCount { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }
        public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);
        public bool HasNextPage => Page < TotalPages;
        public bool HasPreviousPage => Page > 1;
    }
}