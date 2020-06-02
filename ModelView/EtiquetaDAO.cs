using JevoGastosCore.Context;
using JevoGastosCore.Model;
using System.Linq;

namespace JevoGastosCore.ModelView
{
    public class EtiquetaDAO:DAO<Etiqueta, GastosContext, GastosContainer>
    {
        public EtiquetaDAO(GastosContainer gastosContainer)
        {
            this.Container = gastosContainer;
        }

        public DAOList GetEtiquetas()
        {
            return new DAOList(Context.Etiquetas.ToList());
        }
        public Etiqueta Add(string name,TipoEtiqueta tipoEtiqueta)
        {
            Etiqueta newEtiqueta = LazyAdd(name, tipoEtiqueta);
            Context.SaveChanges();
            return newEtiqueta;
        }
        public Etiqueta LazyAdd(string name, TipoEtiqueta tipoEtiqueta)
        {
            Etiqueta newEtiqueta = new Etiqueta()
            {
                Name = name,
                TipoEtiqueta = tipoEtiqueta,
            };
            Context.Etiquetas.Add(newEtiqueta);
            return newEtiqueta;
        }
    }
}
