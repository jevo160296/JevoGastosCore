using JevoGastosCore.Esqueleto;
using JevoGastosCore.ModelView;
using JevoGastosCore.ModelView.EtiquetaMV;

namespace JevoGastosCore
{
    public class GastosContainer : Container<GastosContext>
    {
        private EtiquetaDAO etiquetaDao;
        private IngresoDAO ingresoDao;
        private CuentaDAO cuentaDao;
        private GastoDAO gastoDao;
        private TransaccionDAO transaccionDao;

        public EtiquetaDAO EtiquetaDAO
        {
            get
            {
                if (etiquetaDao is null)
                {
                    etiquetaDao = new EtiquetaDAO(this);
                }
                return etiquetaDao;
            }
        }
        public IngresoDAO IngresoDAO
        {
            get
            {
                if (ingresoDao is null)
                {
                    ingresoDao = new IngresoDAO(this);
                }
                return ingresoDao;
            }
        }
        public CuentaDAO CuentaDAO
        {
            get
            {
                if (cuentaDao is null)
                {
                    cuentaDao = new CuentaDAO(this);
                }
                return cuentaDao;
            }
        }
        public GastoDAO GastoDAO
        {
            get
            {
                if (gastoDao is null)
                {
                    gastoDao = new GastoDAO(this);
                }
                return gastoDao;
            }
        }
        public TransaccionDAO TransaccionDAO
        {
            get
            {
                if (transaccionDao is null)
                {
                    transaccionDao = new TransaccionDAO(this);
                }
                return transaccionDao;
            }
        }

        public GastosContainer(string folderpath)
        {
            this.Context = new GastosContext(folderpath);
        }
        public void SaveChanges()
        {
            this.Context.SaveChanges();
        }
    }
}
