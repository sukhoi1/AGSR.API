using AGSR.TestTask.Models;
using Microsoft.EntityFrameworkCore;

namespace AGSR.TestTask.Contexts;

public class AgsrContext : DbContext
{
    public AgsrContext(DbContextOptions<AgsrContext> options) : base(options) {}

    public DbSet<PatientModel> Patients { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<PatientModel>().HasKey(x => x.Id);
        modelBuilder.Entity<PatientModel>().ToTable("patient");
        modelBuilder.Entity<PatientModel>().HasCheckConstraint("CK_Patient_Gender", "Gender IN ('male','female','other','unknown')");

        modelBuilder.Entity<PatientModel>().Property(x => x.Id)
                                      .HasColumnName("id")
                                      .HasColumnType("uuid")
                                      .HasDefaultValueSql("gen_random_uuid()")
                                      .IsRequired();
        modelBuilder.Entity<PatientModel>().Property(x => x.Use).HasColumnName("use");
        modelBuilder.Entity<PatientModel>().Property(x => x.Family).HasColumnName("family").IsRequired();
        modelBuilder.Entity<PatientModel>().Property(x => x.Given).HasColumnName("given");
        modelBuilder.Entity<PatientModel>().Property(x => x.Gender).HasColumnName("gender");
        modelBuilder.Entity<PatientModel>().Property(x => x.BirthDate).HasColumnName("birth_date").IsRequired();
        modelBuilder.Entity<PatientModel>().Property(x => x.Active).HasColumnName("active");

        modelBuilder.Entity<PatientModel>().HasIndex(x => x.Family).IsUnique(false);
        modelBuilder.Entity<PatientModel>().HasIndex(x => x.BirthDate).IsUnique(false);
    }
}
