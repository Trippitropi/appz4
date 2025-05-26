using Microsoft.EntityFrameworkCore;
using QuestRoom.DAL.Entities;


namespace QuestRoom.DAL
{


    namespace QuestRoom.DAL
    {
        public class QuestRoomDbContext : DbContext
        {
            public QuestRoomDbContext(DbContextOptions<QuestRoomDbContext> options) : base(options)
            {
            }

            public DbSet<Client> Clients { get; set; }
            public DbSet<Quest> Quests { get; set; }
            public DbSet<Booking> Bookings { get; set; }
            public DbSet<GiftCertificate> GiftCertificates { get; set; }

            

            protected override void OnModelCreating(ModelBuilder modelBuilder)
            {
                base.OnModelCreating(modelBuilder);

                
                modelBuilder.Entity<GiftCertificate>()
                    .HasOne(g => g.UsedInBooking)
                    .WithOne(b => b.UsedGiftCertificate)
                    .HasForeignKey<Booking>(b => b.GiftCertificateId)
                    .IsRequired(false);

                

                
                modelBuilder.Entity<GiftCertificate>()
                    .HasIndex(g => g.Code)
                    .IsUnique();
            }


        }
    }
}