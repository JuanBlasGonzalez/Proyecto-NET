using System;

namespace SGE.Aplicacion.Fecha;

// Esta interfaz define el contrato para un proveedor de fecha y hora en la aplicación.
// El método ObtenerFechaActual devuelve un objeto DateTime que representa la fecha y hora actual.
public interface IDateTimeProvider
{
    DateTime ObtenerFechaActual();
}