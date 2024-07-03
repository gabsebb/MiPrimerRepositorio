using Microsoft.EntityFrameworkCore;
using AppLogin.Models;
using Microsoft.Extensions.Configuration;

namespace AppLogin.Data
{
    public class AppDBContext : DbContext
    {

        public AppDBContext(DbContextOptions<AppDBContext>options) : base(options) 
        {

        }

        public DbSet<Usuario>usuario { get; set; }
        public DbSet<Cliente>clientes { get; set; }
        public DbSet<Factura> facturas { get; set; }
        public DbSet<Tipo_producto> tipo_productos { get; set; }
        public DbSet<Producto> productos { get; set; }
        public DbSet<Detalle_factura> detalle_facturas { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Usuario>(tb =>
            {
                tb.ToTable("usuario");
                tb.HasKey(col=>col.id);
                tb.Property(col=> col.id)
                .UseIdentityColumn() 
                .ValueGeneratedOnAdd() ;

                tb.Property(col => col.clave).HasMaxLength(100);
                tb.Property(col => col.usuario).HasMaxLength(100);
                tb.Property(col => col.rol).HasMaxLength(100);
                tb.Property(col => col.fechaModificacion).HasColumnType("datetime").HasDefaultValueSql("GETDATE()"); ;
                tb.Property(col => col.estado).HasColumnType("bit");
            });
            

            modelBuilder.Entity<Cliente>(tb =>
            {
                tb.ToTable("cliente");
                tb.HasKey(col => col.id);
                tb.Property(col => col.id)
                .UseIdentityColumn()
                .ValueGeneratedOnAdd();

                tb.Property(col => col.nombre).HasMaxLength(100);
                tb.Property(col => col.apellido).HasMaxLength(100);
                tb.Property(col => col.cedula).HasMaxLength(100);
                tb.Property(col => col.direccion).HasMaxLength(100);
                tb.Property(col => col.telefono).HasMaxLength(100);
                tb.Property(col => col.fechaModificacion).HasColumnType("datetime").HasDefaultValueSql("GETDATE()"); ;
                tb.Property(col => col.estado).HasColumnType("bit");
            });

            modelBuilder.Entity<Factura>(tb =>
            {
                tb.ToTable("factura"); 
                tb.HasKey(col => col.id); 

                tb.Property(col => col.id)
                    .UseIdentityColumn()
                    .ValueGeneratedOnAdd();

                tb.Property(col => col.fecha)
                    .HasColumnType("datetime");

                tb.Property(col => col.total)
                    .HasColumnType("decimal(18,2)");

                tb.Property(col => col.iva)
                    .HasColumnType("decimal(18,2)");

                tb.Property(col => col.subtotal)
                    .HasColumnType("decimal(18,2)"); 

                tb.Property(col => col.fechaModificacion)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("GETDATE()");

                tb.Property(col => col.estado)
                    .HasColumnType("bit");

                tb.HasOne(u => u.cliente)
                    .WithMany()
                    .HasForeignKey(u => u.idCliente);
            });

            modelBuilder.Entity<Tipo_producto>(tb =>
            {
                tb.ToTable("tipo_producto");
                tb.HasKey(col => col.id);
                tb.Property(col => col.id)
                .UseIdentityColumn()
                .ValueGeneratedOnAdd();

                tb.Property(col => col.nombre_tipo).HasMaxLength(100);
                tb.Property(col => col.descripcion).HasMaxLength(100);
                tb.Property(col => col.fechaModificacion).HasColumnType("datetime").HasDefaultValueSql("GETDATE()"); ;
                tb.Property(col => col.estado).HasColumnType("bit");
            });


            modelBuilder.Entity<Producto>(tb =>
            {
                tb.ToTable("producto");
                tb.HasKey(col => col.id);
                tb.Property(col => col.id)
                .UseIdentityColumn()
                .ValueGeneratedOnAdd();

                tb.Property(col => col.nombre).HasMaxLength(100);
                tb.Property(col => col.iva)
                    .HasColumnType("decimal(18,2)");
                tb.Property(col => col.codigo_barras).HasMaxLength(100);
                tb.Property(col => col.fechaModificacion).HasColumnType("datetime").HasDefaultValueSql("GETDATE()"); ;
                tb.Property(col => col.estado).HasColumnType("bit");

                tb.HasOne(u => u.tipo_producto)
                    .WithMany()
                    .HasForeignKey(u => u.id_tipo);
            });

            modelBuilder.Entity<Detalle_factura>(tb =>
            {
                tb.ToTable("Detalle_factura");
                tb.HasKey(col => col.id);
                tb.Property(col => col.id)
                .UseIdentityColumn()
                .ValueGeneratedOnAdd();

                tb.Property(col => col.cantidad).HasColumnType("decimal(18,2)");
                tb.Property(col => col.precio).HasColumnType("decimal(18,2)");
                tb.Property(col => col.descuento).HasColumnType("decimal(18,2)");
                tb.Property(col => col.total).HasColumnType("decimal(18,2)");
                tb.Property(col => col.fechaModificacion).HasColumnType("datetime").HasDefaultValueSql("GETDATE()"); ;
                tb.Property(col => col.estado).HasColumnType("bit");

                tb.HasOne(u => u.factura)
                    .WithMany()
                    .HasForeignKey(u => u.id_factura);

                tb.HasOne(u => u.producto)
                    .WithMany()
                    .HasForeignKey(u => u.id_producto);
            });

        }
    }
}
