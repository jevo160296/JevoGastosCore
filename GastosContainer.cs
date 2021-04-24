using EntityCoreBasics;
using JevoCrypt;
using JevoCrypt.Classes;
using JevoGastosCore.ModelView;
using JevoGastosCore.ModelView.EtiquetaMV;
using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace JevoGastosCore
{
    public class GastosContainer : Container<GastosContext>,INotifyPropertyChanged
    {
        #region Exceptions
        public class FileNotUserException : Exception
        {
            public FileNotUserException() : base() { }
            public FileNotUserException(string message) : base(message) { }
        }
        public class UserExistsException : Exception
        {
            public UserExistsException() : base() { }
            public UserExistsException(string message) : base(message) { }
        }
        public class PasswordException : Exception
        {
            public PasswordException() : base() { }
            public PasswordException(string message) : base(message) { }
        }
        #endregion
        private EtiquetaDAO etiquetaDao;
        private IngresoDAO ingresoDao;
        private CuentaDAO cuentaDao;
        private GastoDAO gastoDao;
        private CreditoDAO creditoDao;
        private TransaccionDAO transaccionDao;
        private PlanDAO planDao;
        private PayDaysDAO payDaysDao;
        private Medidas medidas;
        private readonly User user;
        private readonly UsersContainer usersContainer;

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
        public User User => user;
        public UsersContainer UsersContainer => usersContainer;
        public GastosContainer(string folderPath):this(folderPath,new User("db",""),new UsersContainer(folderPath))
        {
           
        }
        public GastosContainer(string folderpath, User user,UsersContainer usersContainer,bool stayInSyncWithDisc = true)
        {
            this.Context = new GastosContext(folderpath,user.UserName+".db");
            this.stayInSyncWithDisc = stayInSyncWithDisc;
            this.user = user;
            this.usersContainer = usersContainer;
        }

        public void SaveChanges()
        {
            this.Context.SaveChanges();
            this.Context.Database.ExecuteSqlRaw("VACUUM");
        }
        public bool ChangeUserName(string name,string password,string newName)
        {
            string oldName = this.User.UserName;
            bool correctPassword = this.User.IsCorrectPassword(password);
            bool userNameChanged,fileExists;
            if (correctPassword)
            {
                userNameChanged = this.UsersContainer.UserDAO.ChangeUserName(this.User, password, newName);
                if (userNameChanged)
                {
                    fileExists=!ChangeDBName(newName);
                    if (fileExists)
                    {
                        this.UsersContainer.UserDAO.ChangeUserName(this.User, password, name);
                        throw new FileNotUserException($"Existe un archivo para el usuario {newName} posiblemente el usuario fue eliminado pero se guardó su información.");
                    }
                }
                else
                {
                    throw new UserExistsException($"Ya existe un usuario llamado {newName}");
                }
            }
            else
            {
                userNameChanged = false;
                throw new PasswordException($"Contraseña incorrecta.");
            }
            return userNameChanged;
        }
        public void DeleteDB() => Context.Database.EnsureDeleted();
        private bool ChangeDBName(string newName)
        {
            try
            {
                System.IO.File.Move( this.Context.DbPath, System.IO.Path.Combine(this.Context.FolderPath,newName + ".db"));
            }
            catch(Exception)
            {
                return false;
            }
            this.Context.DbName = newName + ".db";
            return true;
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