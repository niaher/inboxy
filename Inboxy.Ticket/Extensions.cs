namespace Inboxy.Ticket
{
    using System;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Threading.Tasks;
    using Inboxy.Infrastructure;
    using Microsoft.EntityFrameworkCore;
    using UiMetadataFramework.Basic.Input;
    using UiMetadataFramework.Basic.Output;

    public static class Extensions
    {

        public static T FindOrException<T>(this DbSet<T> dbSet, object key, string exceptionMessage = null) where T : class
        {
            var entity = dbSet.Find(key);

            if (entity == null)
            {
                throw new BusinessException(exceptionMessage ?? "Item not found");
            }

            return entity;
        }

        public static async Task<T> SingleOrExceptionAsync<T>(this IQueryable<T> queryable, Expression<Func<T, bool>> where, string exceptionMessage = null)
        {
            var item = await queryable.SingleOrDefaultAsync(where);
            if (item == null)
            {
                throw new BusinessException(exceptionMessage ?? "Item not found.");
            }

            return item;
        }

        public static async Task<T> FindOrExceptionAsync<T>(this DbSet<T> dbSet, object key, string exceptionMessage = null) where T : class
        {
            var entity = await dbSet.FindAsync(key);

            if (entity == null)
            {
                throw new BusinessException(exceptionMessage ?? "Item not found");
            }

            return entity;
        }



        public static async Task<PaginatedData<TSource>> PaginateAsync<TSource, TResult>(
            this IQueryable<TResult> query,
            Func<TResult, TSource> transform,
            int? pageNum,
            int? pageSize,
            string orderBy = null,
            bool? ascending = true)
        {
            return await query.PaginateAsync(
                transform,
                pageNum ?? 1,
                pageSize ?? 10,
                orderBy,
                ascending ?? true);
        }

        public static async Task<PaginatedData<TSource>> PaginateAsync<TSource, TResult>(
            this IQueryable<TResult> query,
            Func<TResult, TSource> transform,
            Paginator paginationParams)
        {
            return await query.PaginateAsync(
                transform,
                paginationParams?.PageIndex,
                paginationParams?.PageSize,
                paginationParams?.OrderBy,
                paginationParams?.Ascending);
        }
    }
}