namespace JevoGastosCore.Enums
{
    public enum TipoTransaccion
    {
        Entrada = (TipoEtiqueta.Ingreso << 3) + TipoEtiqueta.Cuenta + TipoEtiqueta.Credito,
        Movimiento = (TipoEtiqueta.Cuenta << 3) + TipoEtiqueta.Cuenta,
        Salida = (TipoEtiqueta.Cuenta << 3) + (TipoEtiqueta.Credito << 3) + TipoEtiqueta.Gasto,
        Prestamo = (TipoEtiqueta.Credito << 3) + TipoEtiqueta.Cuenta,
        Pago = (TipoEtiqueta.Cuenta << 3) + TipoEtiqueta.Credito
    }
}
