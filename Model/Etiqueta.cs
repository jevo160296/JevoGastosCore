using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace JevoGastosCore.Model
{
    /// <summary>
    /// Representa una etiqueta
    /// </summary>
    public abstract class Etiqueta
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public ObservableCollection<Transaccion> TransaccionesOrigen { get; set; }
        public ObservableCollection<Transaccion> TransaccionesDestino { get; set; }

        public double Total { get; set; }

        public override bool Equals(object obj)
        {
            Etiqueta casted = obj as Etiqueta;
            if (casted is null)
            {
                return false;
            }
            return casted.Id == this.Id;
        }
        public override int GetHashCode()
        {
            return this.Id.GetHashCode();
        }
        public override string ToString()
        {
            return $"{Name}, {Total}";
        }
        
    }
}
