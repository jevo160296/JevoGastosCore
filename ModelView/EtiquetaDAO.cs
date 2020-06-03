using JevoGastosCore.Model;
using System.Linq;

namespace JevoGastosCore.ModelView
{
    public class EtiquetaDAO : JevoGastosDAO<Etiqueta>
    {
        public EtiquetaDAO(GastosContainer gastosContainer) : base(gastosContainer) { }

        public DAOList Get<T>()
            where T : Etiqueta
        {
            return new DAOList(Context.Etiquetas.OfType<T>().ToList());
        }
        public static void UpdateTotal(Etiqueta etiqueta, GastosContext context)
        {
            etiqueta.Total =
                context.Transacciones.Where(p => p.DestinoId == etiqueta.Id).Sum(p => p.Valor) -
                context.Transacciones.Where(p => p.OrigenId == etiqueta.Id).Sum(p => p.Valor);
            EtiquetaDAO.Update(etiqueta, context);
        }
        public static Etiqueta Update(Etiqueta etiqueta, GastosContext context)
        {
            context.Etiquetas.Update(etiqueta);
            context.SaveChanges();
            return etiqueta;
        }
        public static Etiqueta Delete(Etiqueta etiqueta, GastosContext context)
        {
            context.Etiquetas.Remove(etiqueta);
            context.SaveChanges();
            return etiqueta;
        }
    }
}
