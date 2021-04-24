using EntityCoreBasics;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace JevoGastosCore.Model
{
    public class Transaccion : INotifyPropertyChanged,IHaveId
    {
        private DateTime fecha;
        private int origenId;
        private Etiqueta origen;
        private int destinoId;
        private Etiqueta destino;
        private double valor;
        private string descripcion;

        public int Id { get; set; }

        public DateTime Fecha
        {
            get => fecha;
            set
            {
                fecha = value;
                OnPropertyChanged();
            }
        }

        public int OrigenId 
        { 
            get => origenId; 
            set { origenId = value; OnPropertyChanged(); }
        }
        public Etiqueta Origen 
        { 
            get => origen;
            set { origen = value; OnPropertyChanged(); }
        }

        public int DestinoId 
        { 
            get => destinoId;
            set { destinoId = value; OnPropertyChanged(); }
        }
        public Etiqueta Destino
        {
            get => destino;
            set { destino = value; OnPropertyChanged(); }
        }

        public double Valor 
        {
            get => valor;
            set { valor = value; OnPropertyChanged(); }
        }
        public string Descripcion
        { 
            get => descripcion;
            set { descripcion = value; OnPropertyChanged(); }
        }

        public override string ToString()
        {
            return $"{Valor} Desde {Origen.Name} hasta {Destino.Name}";
        }
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName]string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
