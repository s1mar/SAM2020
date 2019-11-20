using System;


namespace SAM2020.DataRepository
{
    public interface IRepository<S> where S : PrimalEntity
    {
        public S get(Int64 id);
        public bool create(S entity);

        public bool update(S entity);

        public bool delete(S entity);

    }
}
