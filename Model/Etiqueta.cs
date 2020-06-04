using System.Collections.Generic;

namespace JevoGastosCore.Model
{
    /// <summary>
    /// Representa una etiqueta
    /// </summary>
    public abstract class Etiqueta
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public List<Transaccion> TransaccionesOrigen { get; set; }
        public List<Transaccion> TransaccionesDestino { get; set; }

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
            return $"{Name}";
        }
        
    }
}
