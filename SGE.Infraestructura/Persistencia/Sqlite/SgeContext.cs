using Microsoft.EntityFrameworkCore;
using SGE.Infraestructura.Persistencia.Sqlite.Models;
using System;

namespace SGE.Infraestructura.Persistencia.Sqlite;

public class SgeContext : DbContext
{
    public DbSet<ExpedienteModel> Expedientes => Set<ExpedienteModel>();
    public DbSet<TramiteModel> Tramites => Set<TramiteModel>();
    
    //Agregamos las tablas para la gestión de usuarios y permisos
    public DbSet<UsuarioModel> Usuarios => Set<UsuarioModel>();
    public DbSet<UsuarioPermisoModel> UsuariosPermisos => Set<UsuarioPermisoModel>();

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        // Configuramos la base de datos con el nombre exacto solicitado por la cátedra
        optionsBuilder.UseSqlite("Data Source=SGE.sqlite");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configuración de la clave primaria para la tabla intermedia de permisos
        modelBuilder.Entity<UsuarioPermisoModel>()
            .HasKey(up => up.Id);

        // Configuración de la relación Uno a Muchos (Un usuario tiene muchos permisos asignados)
        modelBuilder.Entity<UsuarioModel>()
            .HasMany(u => u.Permisos)
            .WithOne()
            .HasForeignKey(up => up.UsuarioId)
            .OnDelete(DeleteBehavior.Cascade); // Si se elimina un usuario, se borran sus permisos en cascada

        // --- 4.2 DATOS SEMILLA (SEED) ---
        // Generamos IDs fijos (Guids) para las entidades semilla para evitar duplicados en migraciones
        var adminId = Guid.Parse("11111111-1111-1111-1111-111111111111");
        var juanId = Guid.Parse("22222222-2222-2222-2222-222222222222");
        var pedroId = Guid.Parse("33333333-3333-3333-3333-333333333333");

        // 1. Administrador Semilla (admin@sge.com / admin123)
        modelBuilder.Entity<UsuarioModel>().HasData(new UsuarioModel
        {
            Id = adminId,
            Nombre = "Administrador del Sistema",
            CorreoElectronico = "admin@sge.com",
            // Hash hexadecimal precalculado de "admin123" usando SHA-256
            ContrasenaHash = "240EE908DE03D8ED6014D23B40CCF164C59A41C765C9D91D0E44E72592ED5AA3",
            EsAdministrador = true
        });

        // 2. Usuario de Prueba 1: Permisos Parciales (juan@sge.com / user123)
        modelBuilder.Entity<UsuarioModel>().HasData(new UsuarioModel
        {
            Id = juanId,
            Nombre = "Juan Operador",
            CorreoElectronico = "juan@sge.com",
            // Hash de "user123"
            ContrasenaHash = "67AA2A6E941E66BEE4A30B746CD6E8913BEBEFE45E20340A7C31CE2B4E91EEA5",
            EsAdministrador = false
        });

        // Le sembramos un par de permisos de prueba a Juan
        modelBuilder.Entity<UsuarioPermisoModel>().HasData(
            new UsuarioPermisoModel { Id = Guid.Parse("a1111111-1111-1111-1111-111111111111"), UsuarioId = juanId, Permiso = "TramiteAlta" },
            new UsuarioPermisoModel { Id = Guid.Parse("a2222222-2222-2222-2222-222222222222"), UsuarioId = juanId, Permiso = "TramiteModificacion" }
        );

        // 3. Usuario de Prueba 2: Sin Permisos / Solo Lectura (pedro@sge.com / invitado123)
        modelBuilder.Entity<UsuarioModel>().HasData(new UsuarioModel
        {
            Id = pedroId,
            Nombre = "Pedro Invitado",
            CorreoElectronico = "pedro@sge.com",
            // Hash de "invitado123"
            ContrasenaHash = "8A9E5325DCDE5E852DA1CBFA967C3B7AE5EDF2B4B27B55BE140CB1CC527F3F19",
            EsAdministrador = false
        });
    }

    public SgeContext(){
    // Lógica requerida por la cátedra para asegurar la creación directa y visible
    if (this.Database.EnsureCreated())
    {
        var connection = this.Database.GetDbConnection();
        connection.Open();
        using (var command = connection.CreateCommand())
        {
            command.CommandText = "PRAGMA journal_mode=DELETE;";
            command.ExecuteNonQuery();
        }
    }
    }
}