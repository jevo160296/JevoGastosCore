using Microsoft.EntityFrameworkCore;

namespace JevoGastosCore.Esqueleto
{
    public abstract class Container<T>
        where T : DbContext
    {
        public T Context
        {
            get;
            protected set;
        }
    }
}
