using Data.Contexts;
using DomainModel.Entities;
using DomainModel.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repositories
{
    public abstract class EntityFrameworkRepositoryBase<T> : IRepository<T> where T : EntityBase
    {
        private readonly SocialNetworkContext _context;

        public EntityFrameworkRepositoryBase(SocialNetworkContext socialNetworkContext)
        {
            _context = socialNetworkContext;
        }

        public virtual void Create(T entity)
        {
            _context.Set<T>().Add(entity);
            _context.SaveChanges();
        }

        public virtual void Delete(Guid id)
        {   
            T originalEntity = _context.Set<T>()
                .SingleOrDefault(entity => entity.Id == id);
            _context.Set<T>().Remove(originalEntity);
            _context.SaveChanges();
        }

        public virtual T Get(Guid? id)
        {
            return _context.Set<T>()
                .SingleOrDefault(entity => entity.Id == id);
        }

        public virtual IEnumerable<T> GetAll()
        {
            return _context.Set<T>().ToList();
        }

        public virtual void Update(T entity)
        {
            var originalEntity = _context.Set<T>().SingleOrDefault(e => e.Id == entity.Id);
            _context.Set<T>().Remove(originalEntity);
            _context.Set<T>().Add(entity);
            _context.SaveChanges();
        }
    }
}
