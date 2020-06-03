using JevoGastosCore.Model;
using System.Linq;

namespace JevoGastosCore.ModelView.EtiquetaMV
{
    public class GastoDAO : JevoGastosDAO<Gasto>
    {
        public GastoDAO(GastosContainer gastosContainer) : base(gastosContainer) { }

        public DAOList Get()
        {
            return new DAOList(Context.Gastos.ToList());
        }

        public Gasto Add(string name)
        {
            Gasto added = new Gasto() { Name = name };
            Context.Gastos.Add(added);
            Context.SaveChanges();
            return added;
        }
    }
}
