using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Web;

namespace System.Linq
{
    [AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
    public static class IQueryablePagedListExtensions
    {
        #region FirstOrDefault
        public static object FirstOrDefault(this IQueryable source)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }

            foreach (object obj in source)
                return obj;

            return null;
        }
        #endregion

        #region PagedList

        public static PagedList<T> GetPagedList<T>(this IQueryable<T> query, int? page, int? pagesize)
where T : new()
        {
            int total = query.Count();

            if (page.HasValue == false)
            {
                page = 1;
            }

            if (pagesize.HasValue == false)
            {
                pagesize = 10;
            }

            int skip = (page.Value - 1) * pagesize.Value;

            var data = query.Skip(skip).Take(pagesize.Value);

            return new PagedList<T>(data.ToList(), page.Value, pagesize.Value, total);
        }

        public static PagedList<T> GetPagedList<T>(this IQueryable<T> query, int start, int limit, string sort, string dir)
where T : new()
        {
            Check.Assert(limit != 0);

            int page = 1;

            if (start != 0)
            {
                page = start / limit;

                if (start % limit != 0)
                {
                    page++;
                }
            }

            int total = query.Count();

            IQueryable<T> orderedQuery = null;

            if (sort.IsNullOrEmpty() == false)
            {
                if (dir.ToLower() == "desc")
                {
                    orderedQuery = query.ApplyOrderByDescendingClause<T>(sort);
                }
                else
                {
                    orderedQuery = query.ApplyOrderByClause<T>(sort);
                }
            }
            else
            {
                orderedQuery = query.AsQueryable();
            }

            int skip = start; // -1;

            if (skip < 0)
                skip = 0;

            var data = orderedQuery.Skip(skip).Take(limit);

            return new PagedList<T>(data.ToList(), page, limit, total);
        }
        #endregion
    }
}
