using bookpj.Entities;
using Microsoft.EntityFrameworkCore;

namespace bookpj.Extension
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }
        public DbSet<Book> Books { get; set; }
        public DbSet<User> Users { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Book>(entity => {
             entity.Property(e => e.Title)             //cấu hình cho phần title 
                .IsRequired()                             //chống null
                .HasMaxLength(250);                       //giới hạn kí tự

             entity.Property(e => e.Author)
                   .IsRequired()
                   .HasMaxLength(250);

             entity.Property(b => b.Price)
                .HasColumnType("decimal(18,0)")          //giới giạn 18 chữ số và không có số ở phần thập phân
                .HasDefaultValue(0);                     //giá trị mặc định ban đầu = 0

             entity.Property(b => b.IsAvailable)
                .HasDefaultValue(true);

            });
        }
    }
}
