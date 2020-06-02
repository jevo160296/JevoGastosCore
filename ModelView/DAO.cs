using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace JevoGastosCore.ModelView
{
    public class DAO<T,DataContext,DataContainer>
        where T : class
        where DataContainer:Container<DataContext>
        where DataContext:DbContext
    {
        protected DataContainer Container;
        protected DataContext Context
        {
            get
            {
                return Container.Context;
            }
        }
        public class DAOList : List<T>
        {
            public DAOList(IEnumerable<T> items) : base(items) { }
        }
    }
}