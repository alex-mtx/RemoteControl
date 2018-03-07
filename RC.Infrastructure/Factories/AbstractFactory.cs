using RC.Interfaces.Factories;
using System;
using System.Collections.Generic;

namespace RC.Infrastructure.Factories
{
    public abstract class AbstractFactory<TCreateParam, TReturn> : IFactory<TReturn, TCreateParam>
    {
        protected IDictionary<TCreateParam, Func<TReturn>> _map;

        public AbstractFactory()
        {
            _map = BuildMap();
        }
        protected abstract IDictionary<TCreateParam, Func<TReturn>> BuildMap();

        public TReturn Create(TCreateParam what)
        {
            return _map[what]();
        }
    }

    
}