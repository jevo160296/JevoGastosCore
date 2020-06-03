using JevoGastosCore.Model;
using System.Linq;

namespace JevoGastosCore.ModelView.EtiquetaMV
{
    public class IngresoDAO : JevoGastosDAO<Ingreso>
    {
        public IngresoDAO(GastosContainer gastosContainer) : base(gastosContainer) { }

        public DAOList Get()
        {
            return new DAOList(Context.Ingresos.ToList());
        }

        public Ingreso Add(string name)
        {
            Ingreso added = new Ingreso() { Name = name };
            Context.Ingresos.Add(added);
            Context.SaveChanges();
            return added;
        }
    }
}
