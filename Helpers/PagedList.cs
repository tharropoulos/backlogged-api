using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using backlogged_api.Models;


namespace backlogged_api.Helpers
{
    public static class PageListBuilder
    {
        public static async Task<PagedList<TEntity>> CreatePagedListAsync<TEntity, TOrder>(IQueryable<TEntity> source,
                                                                                           Expression<Func<TEntity, TOrder>> orderBy,
                                                                                           int pageNumber,
                                                                                           int pageSize)
        {
            var count = await source.CountAsync();
            var items = await source.OrderBy(orderBy).Skip((pageNumber - 1) * pageSize).Take(pageSize).Distinct().ToListAsync();
            return new PagedList<TEntity>(items, count, pageNumber, pageSize);
        }
    }
    public class PagedList<T> : List<T>
    {
        public int CurrentPage { get; private set; }
        public int TotalPages { get; private set; }
        public int PageSize { get; private set; }
        public int TotalCount { get; private set; }
        public bool HasPrevious => CurrentPage > 1;
        public bool HasNext => CurrentPage < TotalPages;

        public PagedList(List<T> items, int count, int pageNumber, int pageSize)
        {
            TotalCount = count;
            PageSize = pageSize;
            CurrentPage = pageNumber;
            TotalPages = (int)System.Math.Ceiling(count / (double)pageSize);
            AddRange(items);
        }


    }
}