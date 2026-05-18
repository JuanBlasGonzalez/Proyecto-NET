namespace SGE.Aplicacion.Autorizacion;

//Enum que representa los diferentes permisos que pueden ser asignados a los usuarios en la aplicación. 
//Estos permisos se utilizan para controlar el acceso a ciertas funcionalidades relacionadas con la gestión de expedientes y trámites, como 
//la creación, eliminación o modificación de estos elementos. 
public enum Permiso
{
    ExpedienteAlta,
    ExpedienteBaja,
    ExpedienteModificacion,
    TramiteAlta,
    TramiteBaja,
    TramiteModificacion
}
