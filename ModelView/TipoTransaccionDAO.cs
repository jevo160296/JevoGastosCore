using JevoGastosCore.Context;
using JevoGastosCore.Model;
using System.Linq;

namespace JevoGastosCore.ModelView
{
    public class TipoTransaccionDAO:DAO<TipoTransaccion, GastosContext, GastosContainer>
    {
        public TipoTransaccionDAO(GastosContainer gastosContainer)
        {
            this.Container = gastosContainer;
        }
        public DAOList GetTiposTransacciones()
        {
            return new DAOList(Context.TiposTransacciones.ToList());
        }
    }
}
