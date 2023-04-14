using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using EntityCoreBasics;
using JevoGastosCore.Clases;

namespace JevoGastosCore.Model
{
    /// <summary>
    /// Representa una etiqueta
    /// </summary>
    public abstract class Etiqueta:INotifyPropertyChanged,IHaveId
    {
        protected double total;
        private string name;

        public int Id { get; set; }
        public string Name 
        { 
            get=>name;
            set 
            {
                if (!(name==value))
                {
                    name = value;
                    OnPropertyChanged();
                }
            }
        }

        //Ignored fields
        private bool totalUpdated = false;
        private bool totalLoaded = false;

        public ObservableCollection<Transaccion> TransaccionesOrigen { get; set; }
        public ObservableCollection<Transaccion> TransaccionesDestino { get; set; }
        public ObservableCollection<Plan> Planes { get; set; }

        public double Total 
        {
            get
            {
                if (!totalUpdated)
                {
                    UpdateTotal();
                    TransaccionesDestino.CollectionChanged += TransaccionesDestino_CollectionChanged;
                    TransaccionesOrigen.CollectionChanged += TransaccionesDestino_CollectionChanged;
                    foreach (Transaccion item in TransaccionesDestino)
                    {
                        item.PropertyChanged += Item_PropertyChanged;
                    }
                    foreach (Transaccion item in TransaccionesOrigen)
                    {
                        item.PropertyChanged += Item_PropertyChanged;
                    }
                    totalUpdated = true;
                }
                return total;
            }
            set 
            {
                if (!(total==value))
                {
                    total = value;
                    this.OnPropertyChanged();
                }
            } 
        }

        private void Item_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            UpdateTotal();
        }
        private void TransaccionesDestino_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            UpdateTotal();
            switch (e.Action)
            {
                case System.Collections.Specialized.NotifyCollectionChangedAction.Add:
                    foreach (var item in e.NewItems)
                    {
                        Transaccion newTransaccion = (Transaccion)item;
                        newTransaccion.PropertyChanged += Item_PropertyChanged;
                    }
                    break;
                case System.Collections.Specialized.NotifyCollectionChangedAction.Remove:
                    foreach (var item in e.OldItems)
                    {
                        Transaccion delTransaccion = (Transaccion)item;
                        delTransaccion.PropertyChanged -= Item_PropertyChanged;
                    }
                    break;
                default:
                    break;
            }
        }
        public override string ToString()
        {
            return $"{Name}, {Total}";
        }

        public void UpdateTotal()
        {
            double total = CalculateTotal(TransaccionesDestino, TransaccionesOrigen);
            if (!totalLoaded)
            {
                this.total = total;
                totalLoaded = true;
            }else if (!(this.total == total))
            {
                Total = total;
            }
        }
        public virtual double CalculateTotal(IEnumerable<Transaccion> transaccionesDestino,IEnumerable<Transaccion> transaccionesOrigen)
        {
            return transaccionesDestino.Sum(p => p.Valor) - transaccionesOrigen.Sum(p => p.Valor);
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName]string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
