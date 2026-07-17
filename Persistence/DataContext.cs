using bookpj.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace bookpj.Extension
{
    public class DataContext : IdentityDbContext<IdentityUser>
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }

        public DbSet<Book> Books { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<DetailOrder> DetailOrders { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Books
            modelBuilder.Entity<Book>(b =>
            {
                b.ToTable("Books");
                b.HasKey(x => x.BookId);
                b.Property(x => x.BookId).ValueGeneratedOnAdd();

                b.Property(x => x.Title)
                    .IsRequired()
                    .HasMaxLength(250);

                b.Property(x => x.Author)
                    .IsRequired()
                    .HasMaxLength(250);

                b.Property(x => x.Price)
                    .HasColumnType("decimal(18,2)")
                    .HasDefaultValue(0m);

                b.Property(x => x.IsAvailable)
                    .HasDefaultValue(true);
            });

            // Orders
            modelBuilder.Entity<Order>(o =>
            {
                o.ToTable("Orders");
                o.HasKey(x => x.id);
                o.Property(x => x.id).ValueGeneratedOnAdd();

                o.Property(x => x.UserName)
                 .IsRequired()
                 .HasMaxLength(100);

                o.Property(x => x.UserId)
                 .HasMaxLength(450)
                 .IsRequired(false);

                o.Property(x => x.OrderDate)
                 .HasDefaultValueSql("GETDATE()");

                o.Property(x => x.TrackingCode)
                 .HasMaxLength(50)
                 .IsRequired(false);

                // Cấu hình quan hệ rõ ràng bằng navigation; dùng UserId làm FK (optional)
                o.HasOne(x => x.User)
                 .WithMany()
                 .HasForeignKey(x => x.UserId)
                 .HasPrincipalKey(u => u.Id)
                     .OnDelete(DeleteBehavior.Restrict)
                 .IsRequired(false);

                // Relation to DetailOrders (use shadow FK if DetailOrder doesn't declare property)
                if (typeof(DetailOrder).GetProperty("OrderTrackingCode") != null)
                {
                    o.HasMany(x => x.DetailOrders)
                     .WithOne(d => d.Order)
                     .HasForeignKey(d => d.OrderTrackingCode)
                     .HasPrincipalKey(x => x.TrackingCode)
                     .OnDelete(DeleteBehavior.Cascade);
                }
                else
                {
                    o.HasMany(x => x.DetailOrders)
                     .WithOne(d => d.Order)
                     .HasForeignKey("OrderId")
                     .OnDelete(DeleteBehavior.Cascade);
                }
            });

            // DetailOrders
            modelBuilder.Entity<DetailOrder>(d =>
            {
                d.ToTable("DetailOrders");
                d.HasKey(x => x.id);
                d.Property(x => x.id).ValueGeneratedOnAdd();

                d.Property(x => x.Title)
                 .IsRequired()
                 .HasMaxLength(250);

                d.Property(x => x.Author)
                 .IsRequired()
                 .HasMaxLength(250);

                d.Property(x => x.Price)
                 .HasColumnType("decimal(18,2)")
                 .HasDefaultValue(0m);

                if (typeof(DetailOrder).GetProperty("OrderTrackingCode") != null)
                {
                    d.Property<string>("OrderTrackingCode")
                     .IsRequired()
                     .HasMaxLength(50);
                    d.HasIndex("OrderTrackingCode");
                }
                else
                {
                    d.Property<int>("OrderId").IsRequired();
                    d.HasIndex("OrderId");
                }
            });
        }
    }
}