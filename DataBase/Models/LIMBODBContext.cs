using System;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace DataBase.Models
{
    public partial class LIMBODBContext : IdentityDbContext
    {
        public LIMBODBContext()
        {
        }

        public LIMBODBContext(DbContextOptions<LIMBODBContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Amigos> Amigos { get; set; }
        public virtual DbSet<Comentarios> Comentarios { get; set; }
        public virtual DbSet<Comentarios2> Comentarios2 { get; set; }
        public virtual DbSet<Imagenes> Imagenes { get; set; }
        public virtual DbSet<Publicaciones> Publicaciones { get; set; }
        public virtual DbSet<Usuarios> Usuarios { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Server=LIAM-PC\\SQLEXPRESS;Database=LIMBODB;Trusted_Connection=True; persist security info=True;Integrated Security =SSPI");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Amigos>(entity =>
            {
                entity.HasKey(e => new { e.IdUsuario, e.IdAmigo });

                entity.Property(e => e.IdUsuario).HasColumnName("Id_Usuario");

                entity.Property(e => e.IdAmigo).HasColumnName("Id_Amigo");

                entity.HasOne(d => d.IdAmigoNavigation)
                    .WithMany(p => p.AmigosIdAmigoNavigation)
                    .HasForeignKey(d => d.IdAmigo)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Amigos__Id_Amigo__503BEA1C");

                entity.HasOne(d => d.IdUsuarioNavigation)
                    .WithMany(p => p.AmigosIdUsuarioNavigation)
                    .HasForeignKey(d => d.IdUsuario)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Amigos__Id_Usuar__4F47C5E3");
            });

            modelBuilder.Entity<Comentarios>(entity =>
            {
                entity.HasKey(e => e.IdComentario);

                entity.Property(e => e.IdComentario).HasColumnName("Id_Comentario");

                entity.Property(e => e.Comentario)
                    .HasMaxLength(1000)
                    .IsUnicode(false);

                entity.Property(e => e.IdPublicacion).HasColumnName("Id_Publicacion");

                entity.Property(e => e.IdUsuario).HasColumnName("Id_Usuario");
            });

            modelBuilder.Entity<Comentarios2>(entity =>
            {
                entity.HasKey(e => new { e.IdComentarioPadre, e.IdComentarioHijo });

                entity.HasOne(d => d.IdComentarioHijoNavigation)
                    .WithMany(p => p.Comentarios2IdComentarioHijoNavigation)
                    .HasForeignKey(d => d.IdComentarioHijo)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Comentari__IdCom__7755B73D");

                entity.HasOne(d => d.IdComentarioPadreNavigation)
                    .WithMany(p => p.Comentarios2IdComentarioPadreNavigation)
                    .HasForeignKey(d => d.IdComentarioPadre)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Comentari__IdCom__76619304");
            });

            modelBuilder.Entity<Imagenes>(entity =>
            {
                entity.HasKey(e => e.IdImagen);

                entity.Property(e => e.Nombre).HasColumnType("text");

                entity.Property(e => e.Ruta).HasColumnType("text");
            });

            modelBuilder.Entity<Publicaciones>(entity =>
            {
                entity.HasKey(e => e.IdPublicacion);

                entity.Property(e => e.IdPublicacion).HasColumnName("Id_Publicacion");

                entity.Property(e => e.Fecha)
                    .HasColumnName("fecha")
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.IdUsuario).HasColumnName("Id_Usuario");

                entity.Property(e => e.Publicacion)
                    .HasMaxLength(1000)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Usuarios>(entity =>
            {
                entity.HasKey(e => e.IdUsuarios);

                entity.Property(e => e.IdUsuarios).HasColumnName("Id_Usuarios");

                entity.Property(e => e.Activo).HasColumnName("activo");

                entity.Property(e => e.Apellido)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Clave)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Correo)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Nombre)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Telefono)
                    .HasMaxLength(15)
                    .IsUnicode(false);

                entity.Property(e => e.Usuario)
                    .HasMaxLength(30)
                    .IsUnicode(false);
            });
        }
    }
}
