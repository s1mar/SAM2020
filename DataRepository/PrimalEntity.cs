using System;


namespace SAM2020.DataRepository
{
    public abstract class PrimalEntity
    {
        //The primal enitity represents the basic elementary unit that can be stored in a database and every such unit, especially in a relation database has an Id
        public Int64 Id { get; protected set; }
    }
}
