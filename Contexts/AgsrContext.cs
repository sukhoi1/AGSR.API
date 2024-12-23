using AGSR.TestTask.Models;
using Microsoft.EntityFrameworkCore;

namespace AGSR.TestTask.Contexts;

public class AgsrContext : DbContext
{
    public AgsrContext(DbContextOptions<AgsrContext> options) : base(options) {}

    public DbSet<Patient> Patients { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Patient>().HasKey(x => x.Id);
        modelBuilder.Entity<Patient>().ToTable("patient");

        modelBuilder.Entity<Patient>().Property(x => x.Id)
                                      .HasColumnName("id")
                                      .HasColumnType("uuid")
                                      .HasDefaultValueSql("gen_random_uuid()")
                                      .IsRequired();
        modelBuilder.Entity<Patient>().Property(x => x.Use).HasColumnName("use");
        modelBuilder.Entity<Patient>().Property(x => x.Family).HasColumnName("family").IsRequired();
        modelBuilder.Entity<Patient>().Property(x => x.GivenJson).HasColumnName("given_json");
        modelBuilder.Entity<Patient>().Property(x => x.Gender).HasColumnName("gender");
        modelBuilder.Entity<Patient>().Property(x => x.BirthDate).HasColumnName("birth_date").IsRequired();
        modelBuilder.Entity<Patient>().Property(x => x.Active).HasColumnName("active");

        modelBuilder.Entity<Patient>().HasIndex(x => x.Family).IsUnique(false);
        modelBuilder.Entity<Patient>().HasIndex(x => x.BirthDate).IsUnique(false);
    }
}
