using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DatingApp.Core.Interfaces
{
    public interface IGenericRepository<T>where T : class
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<T> GetByIdAsync(int id);

        Task<IEnumerable<T>> GetAllWithSpecAsync(ISpecification<T> Spec);
        Task<T> GetByIdWithSpecAsync(ISpecification<T> Spec);
        Task<int> GetConutWithSpecAsync(ISpecification<T> Spec);
        public Task<bool> CheckExistAsync(Expression<Func<T, bool>> fun);

        public Task AddAsync(T item);
        public void Update(T item);
        public void Delete(T item);
        public Task<int> Complete();
    }
}
