using System.Collections.Generic;
using System.Threading.Tasks;

namespace RealWorldUnitTest.Web.Repository
{
    public interface IRepository<TEntity> where TEntity : class
    {

        Task<TEntity> GetById(int id);
        Task<IEnumerable<TEntity>> GetAll();
        Task Create(TEntity entity);
        void Update(TEntity entity);
        void Delete(TEntity entity);
    }
}
