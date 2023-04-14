using EntityCoreBasics;
using JevoGastosCore.Clases;
using System.ComponentModel;

namespace JevoGastosCore.ModelView
{
    public abstract class JevoGastosDAO<T> : DAO<T, GastosContext, GastosContainer>
        where T : class,INotifyPropertyChanged,IHaveId
    {
        public JevoGastosDAO(GastosContainer gastosContainer)
        {
            this.Container = gastosContainer;
        }
    }
}
