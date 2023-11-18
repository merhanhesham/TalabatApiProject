using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;
using Talabat.Core.Repositories;
using Talabat.Core.Specifications;
using Talabat.Repository.Data;

namespace Talabat.Repository
{
    public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
    {
        private readonly StoreContext _dbcontext;

        public GenericRepository(StoreContext dbcontext)
        {
            _dbcontext = dbcontext;
        }

        //violates solid principal (open for extension, closed for modification)
        //y3ni hena ana modtra afta7 elcode a3del feh w a add else if to handle if dbset was for employee
        //
        #region get funcs without spec
        
        public async Task<IReadOnlyList<T>> GetAllAsync()
        {
            //lma roht a test endpoint, l2et elbrand w type null l2nhom related data mby7slhash loading,
            //f h3ml ana eager loading
            /*if(typeof(T)== typeof(Product))  not needed anymore, i use with spec
            {
                return (IReadOnlyList<T>) await _dbcontext.Products.Include(p=>p.ProductBrand).Include(p=>p.ProductType).ToListAsync();
            }*/

             return await _dbcontext.Set<T>().ToListAsync(); 
        }

        public async Task<T> GetByIdAsync(int id)
        {
            return await _dbcontext.Set<T>().FindAsync(id);
            // return await _dbcontext.Set<T>().Where(p=>p.Id==id).Include(p=>p.ProductBrand).Include(p=>p.ProductType);
        }


        #endregion

        #region withSpec
        //create 2 new versions of getall, getbyid to use specifications
        async Task<IReadOnlyList<T>> IGenericRepository<T>.GetAllWithSpecAsync(ISpecifications<T> spec)
        {
            //return await SpecificationEvaluator<T>.GetQuery(_dbcontext.Set<T>(), spec).ToListAsync();
            return await ApplySpecification(spec).ToListAsync();
        }

        async Task<T> IGenericRepository<T>.GetByIdWithSpecAsync(ISpecifications<T> spec)
        {
            // return await SpecificationEvaluator<T>.GetQuery(_dbcontext.Set<T>(),spec).FirstOrDefaultAsync();
            return await ApplySpecification(spec).FirstOrDefaultAsync();
        }
        //to avoid repetition
        private IQueryable<T> ApplySpecification(ISpecifications<T>spec)
        {
            return SpecificationEvaluator<T>.GetQuery(_dbcontext.Set<T>(), spec);
        }

        public async Task<int> GetCountWithSpecAsync(ISpecifications<T> spec)
        {
           return await ApplySpecification(spec).CountAsync();
        }

        Task<T> IGenericRepository<T>.GetCountWithSpecAsync(ISpecifications<T> spec)
        {
            throw new NotImplementedException();
        }
        #endregion

    }
}
