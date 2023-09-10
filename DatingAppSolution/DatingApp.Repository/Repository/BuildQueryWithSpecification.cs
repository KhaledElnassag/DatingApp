using DatingApp.Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Talapat.Repository
{
	public static class BuildQueryWithSpecification<T>where T : class
	{
		public static IQueryable<T> GetQuery(IQueryable<T> Entity, ISpecification<T> Spec)
		{
			var Query = Entity;//DbContext.Set<T>

			if (Spec.Crietria is not null) /*DbContext.Set<T>.where(Cietria)*/
			Query =Query.Where(Spec.Crietria);

			if (Spec.OrderByCrietria is not null) 
				Query = Query.OrderBy(Spec.OrderByCrietria);

			if (Spec.OrderByDescCrietria is not null)
				Query = Query.OrderByDescending(Spec.OrderByDescCrietria);

			if (Spec.IsPaginationEnapled)
				Query = Query.Skip(Spec.Skip).Take(Spec.Take);

			Query = Spec.IncludeCrietria.Aggregate(Query, (CurrentQuery, IncludeSpec) => CurrentQuery.Include(IncludeSpec));/*DbContext.Set<T>.where(Cietria).include(IncludeSpec)*/
			

			return Query;
		}
	}
}
