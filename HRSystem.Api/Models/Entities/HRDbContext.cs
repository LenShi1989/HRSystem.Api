using Microsoft.EntityFrameworkCore;
using HRSystem.Api.Models.Entities;
using HRSystem.Api.Models.DTOs;

namespace HRSystem.Api.Models.Entities;

public class HRDbContext : DbContext
{
    public HRDbContext(DbContextOptions<HRDbContext> options) : base(options) { }

    public DbSet<Department> Departments { get; set; }
    public DbSet<Position> Positions { get; set; }
    public DbSet<Employee> Employees { get; set; }
    public DbSet<Attendance> Attendances { get; set; }
    public DbSet<LeaveRequest> LeaveRequests { get; set; }
    public DbSet<Payroll> Payrolls { get; set; }
    public DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // ── Department ──────────────────────────────────────
        modelBuilder.Entity<Department>(e =>
        {
            e.HasKey(x => x.Id);
            e.Property(x => x.Name).HasMaxLength(100).IsRequired();
            e.Property(x => x.Code).HasMaxLength(20).IsRequired();
            e.HasIndex(x => x.Code).IsUnique();
            e.Property(x => x.CreatedAt).HasDefaultValueSql("GETDATE()");
            e.Property(x => x.UpdatedAt).HasDefaultValueSql("GETDATE()");

            e.HasOne(x => x.Manager)
             .WithMany()
             .HasForeignKey(x => x.ManagerId)
             .OnDelete(DeleteBehavior.NoAction);
        });

        // ── Position ─────────────────────────────────────────
        modelBuilder.Entity<Position>(e =>
        {
            e.HasKey(x => x.Id);
            e.Property(x => x.Title).HasMaxLength(100).IsRequired();
            e.Property(x => x.Code).HasMaxLength(20).IsRequired();
            e.HasIndex(x => x.Code).IsUnique();
            e.Property(x => x.MinSalary).HasColumnType("decimal(18,2)");
            e.Property(x => x.MaxSalary).HasColumnType("decimal(18,2)");
            e.Property(x => x.CreatedAt).HasDefaultValueSql("GETDATE()");
            e.Property(x => x.UpdatedAt).HasDefaultValueSql("GETDATE()");
        });

        // ── Employee ─────────────────────────────────────────
        modelBuilder.Entity<Employee>(e =>
        {
            e.HasKey(x => x.Id);
            e.Property(x => x.EmployeeNo).HasMaxLength(20).IsRequired();
            e.HasIndex(x => x.EmployeeNo).IsUnique();
            e.Property(x => x.FirstName).HasMaxLength(50).IsRequired();
            e.Property(x => x.LastName).HasMaxLength(50).IsRequired();
            e.Property(x => x.Email).HasMaxLength(150).IsRequired();
            e.HasIndex(x => x.Email).IsUnique();
            e.Property(x => x.BaseSalary).HasColumnType("decimal(18,2)");
            e.Property(x => x.CreatedAt).HasDefaultValueSql("GETDATE()");
            e.Property(x => x.UpdatedAt).HasDefaultValueSql("GETDATE()");

            e.HasOne(x => x.Department)
             .WithMany(d => d.Employees)
             .HasForeignKey(x => x.DepartmentId)
             .OnDelete(DeleteBehavior.Restrict);

            e.HasOne(x => x.Position)
             .WithMany(p => p.Employees)
             .HasForeignKey(x => x.PositionId)
             .OnDelete(DeleteBehavior.Restrict);

            e.HasOne(x => x.Manager)
             .WithMany(m => m.Subordinates)
             .HasForeignKey(x => x.ManagerId)
             .OnDelete(DeleteBehavior.NoAction);

            e.Ignore(x => x.FullName);
        });

        // ── Attendance ───────────────────────────────────────
        modelBuilder.Entity<Attendance>(e =>
        {
            e.HasKey(x => x.Id);
            e.HasIndex(x => new { x.EmployeeId, x.AttendDate }).IsUnique();
            e.Property(x => x.WorkHours).HasColumnType("decimal(5,2)");
            e.Property(x => x.CreatedAt).HasDefaultValueSql("GETDATE()");

            e.HasOne(x => x.Employee)
             .WithMany(em => em.Attendances)
             .HasForeignKey(x => x.EmployeeId)
             .OnDelete(DeleteBehavior.Cascade);
        });

        // ── LeaveRequest ─────────────────────────────────────
        modelBuilder.Entity<LeaveRequest>(e =>
        {
            e.HasKey(x => x.Id);
            e.Property(x => x.Days).HasColumnType("decimal(5,1)");
            e.Property(x => x.CreatedAt).HasDefaultValueSql("GETDATE()");
            e.Property(x => x.UpdatedAt).HasDefaultValueSql("GETDATE()");

            e.HasOne(x => x.Employee)
             .WithMany(em => em.LeaveRequests)
             .HasForeignKey(x => x.EmployeeId)
             .OnDelete(DeleteBehavior.Cascade);

            e.HasOne(x => x.Approver)
             .WithMany()
             .HasForeignKey(x => x.ApproverId)
             .OnDelete(DeleteBehavior.NoAction);
        });

        // ── Payroll ───────────────────────────────────────────
        modelBuilder.Entity<Payroll>(e =>
        {
            e.HasKey(x => x.Id);
            e.HasIndex(x => new { x.EmployeeId, x.PayYear, x.PayMonth }).IsUnique();
            e.Property(x => x.BaseSalary).HasColumnType("decimal(18,2)");
            e.Property(x => x.Bonus).HasColumnType("decimal(18,2)");
            e.Property(x => x.Allowance).HasColumnType("decimal(18,2)");
            e.Property(x => x.Overtime).HasColumnType("decimal(18,2)");
            e.Property(x => x.Deduction).HasColumnType("decimal(18,2)");
            e.Property(x => x.Insurance).HasColumnType("decimal(18,2)");
            e.Property(x => x.Tax).HasColumnType("decimal(18,2)");
            e.Property(x => x.NetSalary).HasColumnType("decimal(18,2)");
            e.Property(x => x.CreatedAt).HasDefaultValueSql("GETDATE()");
            e.Property(x => x.UpdatedAt).HasDefaultValueSql("GETDATE()");

            e.HasOne(x => x.Employee)
             .WithMany(em => em.Payrolls)
             .HasForeignKey(x => x.EmployeeId)
             .OnDelete(DeleteBehavior.Cascade);
        });

        // ── User ──────────────────────────────────────────────
        modelBuilder.Entity<User>(e =>
        {
            e.HasKey(x => x.Id);
            e.Property(x => x.Username).HasMaxLength(50).IsRequired();
            e.HasIndex(x => x.Username).IsUnique();
            e.Property(x => x.PasswordHash).HasMaxLength(500).IsRequired();
            e.Property(x => x.CreatedAt).HasDefaultValueSql("GETDATE()");

            e.HasOne(x => x.Employee)
             .WithMany()
             .HasForeignKey(x => x.EmployeeId)
             .OnDelete(DeleteBehavior.SetNull);
        });
    }

    public override int SaveChanges()
    {
        UpdateTimestamps();
        return base.SaveChanges();
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        UpdateTimestamps();
        return base.SaveChangesAsync(cancellationToken);
    }

    private void UpdateTimestamps()
    {
        var entries = ChangeTracker.Entries()
            .Where(e => e.State == EntityState.Modified);

        foreach (var entry in entries)
        {
            if (entry.Properties.Any(p => p.Metadata.Name == "UpdatedAt"))
                entry.Property("UpdatedAt").CurrentValue = DateTime.UtcNow;
        }
    }
}
