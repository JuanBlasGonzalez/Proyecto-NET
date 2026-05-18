using System;
using SGE.Aplicacion.Fecha;

namespace SGE.Infraestructura.Servicios;

// Implementacion de la NOTA del enunciado con respecto al NO uso del DateTime.now directamente en el dominio.
// Esta clase implementa la interfaz IDateTimeProvider y proporciona la fecha y hora actual utilizando DateTime.Now. 
// Esto permite que el dominio no dependa directamente de la clase DateTime, lo que facilita las pruebas unitarias y la flexibilidad en la gestión del tiempo.
public class MachineDateTimeProvider : IDateTimeProvider
{
    public DateTime ObtenerFechaActual() => DateTime.Now;
}