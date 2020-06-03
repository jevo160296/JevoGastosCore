using JevoGastosCore.Esqueleto;

namespace JevoGastosCore.ModelView
{
    public abstract class JevoGastosDAO<T> : DAO<T, GastosContext, GastosContainer>
        where T : class
    {
        public JevoGastosDAO(GastosContainer gastosContainer)
        {
            this.Container = gastosContainer;
        }
    }
}
