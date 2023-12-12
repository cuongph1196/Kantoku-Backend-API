using System;

namespace QLKTX.Class.Dtos
{
    public class PagedReponBase
    {
        public int PageIndex { get; set; }

        public int PageSize { get; set; }

        public int TotalRow { get; set; }

        public int PageCount
        {
            get
            {
                var pageCount = ((double)TotalRow / PageSize) - 1;
                return (int)Math.Ceiling(pageCount);
            }
        }
        public string SortItem { get; set; }
        public string SortDirection { get; set; }
    }
}
