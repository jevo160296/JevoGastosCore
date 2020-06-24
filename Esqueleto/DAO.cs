using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace JevoGastosCore.Esqueleto
{
    public abstract class DAO<T, DataContext, DataContainer>
        where T : class
        where DataContainer : Container<DataContext>
        where DataContext : DbContext
    {
        protected DataContainer Container;
        protected DataContext Context
        {
            get
            {
                return Container.Context;
            }
        }
        protected ObservableCollection<T> items;
        public abstract ObservableCollection<T> Items { get; }
        public bool ItemsLoaded => !(items is null);
    }
}