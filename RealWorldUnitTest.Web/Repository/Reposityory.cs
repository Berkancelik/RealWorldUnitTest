using Microsoft.EntityFrameworkCore;
using RealWorldUnitTest.Web.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RealWorldUnitTest.Web.Repository
{
    public class Reposityory<TEntity> : IRepository<TEntity> where TEntity : class
    {

        private readonly UnitTestDbContext _context;
        private readonly DbSet<TEntity> _dbSet;

        public Reposityory(UnitTestDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<TEntity>();
        }

        public async Task Create(TEntity entity)
        {
            await _dbSet.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public void Delete(TEntity entity)
        {
            _dbSet.Remove(entity);
            _context.SaveChanges();
        }

        public async Task<IEnumerable<TEntity>> GetAll()
        {
            return await _dbSet.ToListAsync();
        }

        public Task<TEntity> GetById(int id)
        {
            throw new System.NotImplementedException();
        }

        public void Update(TEntity entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
            _dbSet.Update(entity);
            _context.SaveChanges();
        }
    }
}
