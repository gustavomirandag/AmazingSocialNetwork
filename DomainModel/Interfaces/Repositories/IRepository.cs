using DomainModel.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainModel.Interfaces.Repositories
{
    public interface IRepository<T> where T : EntityBase
    {
        void Create(T entity);
        T Get(Guid? id);
        IEnumerable<T> GetAll();
        void Update(T entity);
        void Delete(Guid id);
    }
}
