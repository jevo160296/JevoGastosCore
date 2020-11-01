namespace JevoGastosCore.Model
{
    public class Cuenta : Etiqueta 
    {
        private bool esAhorro=false;

        public bool EsAhorro
        {
            get => esAhorro;
            set
            {
                if (!(esAhorro == value))
                {
                    esAhorro = value;
                    OnPropertyChanged();
                }
            }
        }
    }
}
