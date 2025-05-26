using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using TaskManagementServiceBO;

namespace TaskManagementServiceDAO;

public partial class TaskManagementContext : DbContext
{
    public TaskManagementContext()
    {
    }

    public TaskManagementContext(DbContextOptions<TaskManagementContext> options)
        : base(options)
    {
    }

    public virtual DbSet<TaskManagementServiceBO.Task> Tasks { get; set; }

    public virtual DbSet<TaskAssignment> TaskAssignments { get; set; }

    public virtual DbSet<TaskCategory> TaskCategories { get; set; }

    public virtual DbSet<TaskManagementServiceBO.TaskStatus> TaskStatuses { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer(GetConnectionString());

    private string GetConnectionString()
    {
        var builder = new ConfigurationBuilder()
            .SetBasePath(AppContext.BaseDirectory)
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

        var configuration = builder.Build();
        return configuration.GetConnectionString("MyCnn");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TaskManagementServiceBO.Task>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Tasks__3214EC07D070C4B8");

            entity.Property(e => e.CreatedAt).HasColumnType("datetime");
            entity.Property(e => e.Title).HasMaxLength(200);

            entity.HasOne(d => d.Category).WithMany(p => p.Tasks)
                .HasForeignKey(d => d.CategoryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Tasks__CategoryI__3E52440B");

            entity.HasOne(d => d.Status).WithMany(p => p.Tasks)
                .HasForeignKey(d => d.StatusId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Tasks__StatusId__3D5E1FD2");
        });

        modelBuilder.Entity<TaskAssignment>(entity =>
        {
            entity.HasKey(e => new { e.TaskId, e.UserId }).HasName("PK__TaskAssi__AD11C5755D16CE14");

            entity.Property(e => e.AssignedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.Task).WithMany(p => p.TaskAssignments)
                .HasForeignKey(d => d.TaskId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__TaskAssig__TaskI__412EB0B6");

            entity.HasOne(d => d.User).WithMany(p => p.TaskAssignments)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__TaskAssig__UserI__4222D4EF");
        });

        modelBuilder.Entity<TaskCategory>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__TaskCate__3214EC07D4CB2642");

            entity.Property(e => e.Name).HasMaxLength(50);
        });

        modelBuilder.Entity<TaskManagementServiceBO.TaskStatus>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__TaskStat__3214EC07A5660FA5");

            entity.Property(e => e.Name).HasMaxLength(50);
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Users__3214EC07EF2B894F");

            entity.Property(e => e.Email).HasMaxLength(100);
            entity.Property(e => e.Name).HasMaxLength(100);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
