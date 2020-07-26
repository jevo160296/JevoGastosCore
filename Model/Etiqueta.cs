using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace JevoGastosCore.Model
{
    /// <summary>
    /// Representa una etiqueta
    /// </summary>
    public abstract class Etiqueta:INotifyPropertyChanged
    {
        private double total;
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

        public ObservableCollection<Transaccion> TransaccionesOrigen { get; set; }
        public ObservableCollection<Transaccion> TransaccionesDestino { get; set; }

        public double Total { get=>total; 
            set 
            {
                if (!(total==value))
                {
                    total = value;
                    this.OnPropertyChanged();
                }
            } 
        }

        public override string ToString()
        {
            return $"{Name}, {Total}";
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName]string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
