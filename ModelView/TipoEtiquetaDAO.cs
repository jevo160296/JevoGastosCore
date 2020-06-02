using JevoGastosCore.Context;
using JevoGastosCore.Model;
using System.Linq;

namespace JevoGastosCore.ModelView
{
    public class TipoEtiquetaDAO:DAO<TipoEtiqueta, GastosContext, GastosContainer>
    {
        public TipoEtiquetaDAO(GastosContainer gastosContainer)
        {
            this.Container = gastosContainer;
        }

        public DAOList GetTiposEtiquetas()
        {
            return new DAOList(Context.TiposEtiquetas.ToList());
        }
    }
}
