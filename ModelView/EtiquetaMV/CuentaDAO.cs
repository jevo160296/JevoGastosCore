using JevoGastosCore.Model;
using System.Linq;

namespace JevoGastosCore.ModelView.EtiquetaMV
{
    public class CuentaDAO : JevoGastosDAO<Cuenta>
    {
        public CuentaDAO(GastosContainer gastosContainer) : base(gastosContainer) { }

        public DAOList Get()
        {
            return new DAOList(Context.Cuentas.ToList());
        }

        public Cuenta Add(string name)
        {
            Cuenta added = new Cuenta() { Name = name };
            Container.Context.Cuentas.Add(added);
            Context.SaveChanges();
            return added;
        }
    }
}
