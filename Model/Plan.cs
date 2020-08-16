using JevoGastosCore.Enums;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace JevoGastosCore.Model
{
    public class Plan:INotifyPropertyChanged
    {
        private int etiquetaId;
        private Etiqueta etiqueta;
        private TipoPlan tipo;
        private double meta;

        public int Id { get; set; }

        public int EtiquetaId
        {
            get => etiquetaId;
            set { etiquetaId = value; OnPropertyChanged(); }
        }
        public Etiqueta Etiqueta
        {
            get => etiqueta;
            set { etiqueta = value; OnPropertyChanged(); }
        }

        public TipoPlan Tipo
        {
            get => tipo;
            set { tipo = value; OnPropertyChanged(); }
        }
        public double Meta
        {
            get => meta;
            set { meta = value; OnPropertyChanged(); }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName]string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
