using DatingApp.Core.Interfaces;
using DatingApp.Repository.DataBase.UserContext;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Talapat.Repository;

namespace DatingApp.Repository.Repository
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly DataContext _context;

        public GenericRepository(DataContext context)
        {
           _context = context;
        }

        public async Task<T> GetByIdAsync(int id)
        {

            //_dbContext.Set<T>().Where(P=>P.Id== id).Include().FirstOrDefaultAsync();
            return await _context.Set<T>().FindAsync(id);
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {

            return await _context.Set<T>().ToListAsync();
        }

        public async Task<T> GetByIdWithSpecAsync(ISpecification<T> Spec) 
        {

            return await GetSpec(Spec).FirstOrDefaultAsync();

        }

        public async Task<IEnumerable<T>> GetAllWithSpecAsync(ISpecification<T> Spec)
        {

            return await GetSpec(Spec).ToListAsync();

        }
       
        public async Task<int> GetConutWithSpecAsync(ISpecification<T> Spec)
        {
            return await GetSpec(Spec).CountAsync();
        }

        public IQueryable<T> GetSpec(ISpecification<T> Spec)
        {
            return BuildQueryWithSpecification<T>.GetQuery(_context.Set<T>(), Spec);
        }


        public async Task<bool> CheckExistAsync(Expression<Func<T,bool>> fun)
        {
            return await _context.Set<T>().AnyAsync(fun);
        }
        public async Task AddAsync(T item)
        {
            await _context.Set<T>().AddAsync(item);
        }

        public void Delete(T item)
        {
             _context.Set<T>().Remove(item);
        }

        public void Update(T item)
        {
             _context.Set<T>().Update(item);
        }

        public async Task<int> Complete()
        {
             return await _context.SaveChangesAsync();
        }
    }

}
