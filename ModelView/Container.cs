using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace JevoGastosCore.ModelView
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
