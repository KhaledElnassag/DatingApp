using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;


namespace DatingApp.Core.Interfaces
{
	public class BaseSpecification<T> : ISpecification<T> where T : class
	{
		public Expression<Func<T, bool>> Crietria { get; set; }
		public List<Expression<Func<T, object>>> IncludeCrietria { get ; set; }=new List<Expression<Func<T, object>>>();
		public Expression<Func<T, object>> OrderByCrietria { get; set ; }
		public Expression<Func<T, object>> OrderByDescCrietria { get; set ; }
		public int Take { get ; set ; }
		public int Skip { get ; set ; }
		public bool IsPaginationEnapled { get ; set ; }

		public BaseSpecification()
		{

			
		}

		public BaseSpecification(Expression<Func<T, bool>> crietia )
		{
			Crietria= crietia;
		}

		public void AddOrderBy(Expression<Func<T, object>> orderby)
		{
			OrderByCrietria=orderby;
		}
		public void AddOrderByDesc(Expression<Func<T, object>> orderbydesc)
		{
			OrderByDescCrietria=orderbydesc;
		}

		public void ApplyPagination(int take,int skip)
		{
			IsPaginationEnapled = true;
			Take=take;
			Skip=skip;
		}
	}
}
