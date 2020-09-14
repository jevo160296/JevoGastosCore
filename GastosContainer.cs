using JevoGastosCore.Clases;
using JevoGastosCore.Esqueleto;
using JevoGastosCore.ModelView;
using JevoGastosCore.ModelView.EtiquetaMV;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace JevoGastosCore
{
    public class GastosContainer : Container<GastosContext>,INotifyPropertyChanged
    {
        private EtiquetaDAO etiquetaDao;
        private IngresoDAO ingresoDao;
        private CuentaDAO cuentaDao;
        private GastoDAO gastoDao;
        private CreditoDAO creditoDao;
        private TransaccionDAO transaccionDao;
        private PlanDAO planDao;
        private PayDaysDAO payDaysDao;
        private Medidas medidas;
        private bool stayInSyncWithDisc;

        public EtiquetaDAO EtiquetaDAO
        {
            get
            {
                if (etiquetaDao is null)
                {
                    etiquetaDao = new EtiquetaDAO(this);
                    etiquetaDao.PropertyChanged += Dao_PropertyChanged;
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
                    ingresoDao.PropertyChanged += Dao_PropertyChanged;
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
                    cuentaDao.PropertyChanged += Dao_PropertyChanged;
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
                    gastoDao.PropertyChanged += Dao_PropertyChanged;
                }
                return gastoDao;
            }
        }
        public CreditoDAO CreditoDAO
        {
            get
            {
                if (creditoDao is null)
                {
                    creditoDao = new CreditoDAO(this);
                    creditoDao.PropertyChanged += Dao_PropertyChanged;
                }
                return creditoDao;
            }
        }
        public TransaccionDAO TransaccionDAO
        {
            get
            {
                if (transaccionDao is null)
                {
                    transaccionDao = new TransaccionDAO(this);
                    transaccionDao.PropertyChanged += Dao_PropertyChanged;
                }
                return transaccionDao;
            }
        }
        public PayDaysDAO PayDaysDAO
        {
            get
            {
                if (payDaysDao is null)
                {
                    payDaysDao = new PayDaysDAO(this);
                    payDaysDao.PropertyChanged += Dao_PropertyChanged;
                }
                return payDaysDao;
            }
        }
        public PlanDAO PlanDAO
        {
            get
            {
                if (planDao is null)
                {
                    planDao = new PlanDAO(this);
                    planDao.PropertyChanged += Dao_PropertyChanged;
                }
                return planDao;
            }
        }
        public Medidas Medidas
        {
            get
            {
                if (medidas is null)
                {
                    medidas = new Medidas(this);
                }
                return medidas;
            }
        }
        public bool StayInSyncWithDisc => stayInSyncWithDisc;

        public GastosContainer(string folderpath,bool stayInSyncWithDisc=true)
        {
            this.Context = new GastosContext(folderpath);
            this.stayInSyncWithDisc = stayInSyncWithDisc;
        }
        public void SaveChanges()
        {
            this.Context.SaveChanges();
            this.Context.Database.ExecuteSqlRaw("VACUUM");
        }

        private void Dao_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            OnPropertyChanged(e.PropertyName);
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName]string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
