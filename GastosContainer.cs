using JevoGastosCore.Context;
using JevoGastosCore.ModelView;

namespace JevoGastosCore
{
    public class GastosContainer:Container<GastosContext>
    {
        private EtiquetaDAO etiquetadao;
        private TransaccionDAO transaccionDao;
        private TipoEtiquetaDAO tipoEtiquetaDao;
        private TipoTransaccionDAO tipoTransaccionDao;

        public EtiquetaDAO EtiquetaDAO
        {
            get
            {
                if(etiquetadao is null)
                {
                    etiquetadao = new EtiquetaDAO(this);
                }
                return etiquetadao;
            }
        }
        public TransaccionDAO TransaccionDAO
        {
            get
            {
                if(transaccionDao is null)
                {
                    transaccionDao = new TransaccionDAO(this);
                }
                return transaccionDao;
            }
        }
        public TipoEtiquetaDAO TipoEtiquetaDAO
        {
            get
            {
                if(tipoEtiquetaDao is null)
                {
                    tipoEtiquetaDao = new TipoEtiquetaDAO(this);
                }
                return tipoEtiquetaDao;
            }
        }
        public TipoTransaccionDAO TipoTransaccionDAO
        {
            get
            {
                if(tipoTransaccionDao is null)
                {
                    tipoTransaccionDao = new TipoTransaccionDAO(this);
                }
                return tipoTransaccionDao;
            }
        }

        public GastosContainer(string folderpath)
        {
            this.Context =new GastosContext(folderpath);
        }
        public void SaveChanges()
        {
            this.Context.SaveChanges();
        }
    }
}
