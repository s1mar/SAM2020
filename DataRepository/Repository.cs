using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SAM2020.DataRepository
{
    public abstract class Repository<T> : IRepository<T> where T : PrimalEntity
    {

        public abstract bool create(T entity);
        public abstract bool delete(T entity);
        public abstract T get(long id);
        public abstract bool update(T entity);
    }
}
