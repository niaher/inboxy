namespace Inboxy.Core
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Linq.Expressions;
	using System.Threading.Tasks;
	using Inboxy.Infrastructure;
	using Inboxy.Infrastructure.EntityFramework;
	using Microsoft.EntityFrameworkCore;
	using UiMetadataFramework.Basic.Input;
	using UiMetadataFramework.Basic.Output;

	internal static class Extensions
	{
		public static T FindOrException<T>(this DbSet<T> dbSet, object key) where T : class
		{
			var entity = dbSet.Find(key);

			if (entity == null)
			{
				throw new BusinessException("Item not found");
			}

			return entity;
		}

		public static async Task<T> FindOrExceptionAsync<T>(this DbSet<T> dbSet, object key) where T : class
		{
			var entity = await dbSet.FindAsync(key);

			if (entity == null)
			{
				throw new BusinessException("Item not found");
			}

			return entity;
		}

		public static async Task<PaginatedData<TSource>> PaginateAsync<TSource, TResult>(
			this IQueryable<TResult> query,
			Func<TResult, TSource> transform,
			int pageNum,
			int pageSize,
			string orderBy = null,
			bool ascending = true)
		{
			if (pageSize <= 0)
			{
				pageSize = 10;
			}

			//Total result count
			var rowsCount = query.Count();

			//If page number should be > 0 else set to first page
			if (rowsCount <= pageSize || pageNum <= 0)
			{
				pageNum = 1;
			}

			//Calculate number of rows to skip on page size
			var excludedRows = (pageNum - 1) * pageSize;

			var data = await query
				.OrderBy(orderBy, ascending)
				.Skip(excludedRows)
				.Take(pageSize)
				.ToListAsync();

			return new PaginatedData<TSource>
			{
				Results = data.Select(transform).ToArray(),
				TotalCount = rowsCount
			};
		}

		public static async Task<PaginatedData<TSource>> PaginateAsync<TSource>(
			this IQueryable<TSource> query,
			Paginator paginationParams)
		{
			return await query.PaginateAsync(
				t => t,
				paginationParams?.PageIndex,
				paginationParams?.PageSize,
				paginationParams?.OrderBy,
				paginationParams?.Ascending);
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

		public static void RemoveRange<T>(this ICollection<T> list, Func<T, bool> filter)
		{
			var toRemove = list.Where(filter).ToList();
			foreach (var item in toRemove)
			{
				list.Remove(item);
			}
		}

		public static async Task<T> SingleOrExceptionAsync<T>(this IQueryable<T> queryable, Expression<Func<T, bool>> where)
		{
			var item = await queryable.SingleOrDefaultAsync(where);
			if (item == null)
			{
				throw new BusinessException("Item not found.");
			}

			return item;
		}
	}
}