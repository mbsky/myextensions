using System.Collections.Generic;
using System.Linq;

namespace System.Data
{
    public interface IPagedList
    {
        int Count { get; }
        int TotalCount { get; }
        int PageNumber { get; set; }
        int PageSize { get; }
        bool HasPreviousPage { get; }
        bool HasNextPage { get; }
        int PageCount { get; }
        int StartIndex { get; }
        int EndIndex { get; }
    }

    public class PagedList<T> : List<T>, IPagedList
    {

        public PagedList(IQueryable<T> source, int page, int pageSize)
        {
            this.TotalCount = source.Count();
            this.PageNumber = page;
            this.PageSize = pageSize;

            if (this.TotalCount != 0)
            {
                if (this.PageCount < page || page <= 0)
                    throw new ArgumentOutOfRangeException("page");
            }
            else
            {
                this.PageNumber = 0;
            }

            if (this.TotalCount != 0)
                this.AddRange(source.Skip((page - 1) * pageSize).Take(pageSize).ToList());
        }

        public PagedList(List<T> source, int page, int pageSize)
        {
            this.TotalCount = source.Count();
            this.PageNumber = page;
            this.PageSize = pageSize;

            if (this.TotalCount != 0)
            {
                if (this.PageCount < page || page <= 0)
                    throw new ArgumentOutOfRangeException("page");
            }
            else
            {
                this.PageNumber = 0;
            }

            if (this.TotalCount != 0)
                this.AddRange(source.Skip((page - 1) * pageSize).Take(pageSize).ToList());
        }

        public int TotalCount { get; private set; }
        public int PageSize { get; private set; }

        public bool HasPreviousPage
        {
            get
            {
                return (PageNumber - 1) > 0;
            }
        }

        public bool HasNextPage
        {
            get
            {
                return (PageNumber * PageSize) < TotalCount;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public int PageNumber { get; set; }

        public int StartIndex
        {
            get
            {
                if (TotalCount == 0)
                    return 0;

                return (this.PageNumber - 1) * this.PageSize + 1;
            }
        }

        public int EndIndex
        {
            get
            {
                if (TotalCount == 0)
                    return 0;

                return StartIndex + this.Count() - 1;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public int PageCount
        {
            get
            {
                if (PageSize == 0)
                    throw new ArgumentOutOfRangeException("page");

                if (TotalCount == 0)
                {
                    return 0;
                }

                int remainder = TotalCount % PageSize;

                if (remainder == 0)
                    return TotalCount / PageSize;
                else
                    return (TotalCount / PageSize) + 1;
            }
        }
    }

}
