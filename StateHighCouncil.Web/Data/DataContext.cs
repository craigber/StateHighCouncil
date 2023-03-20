using Microsoft.EntityFrameworkCore;
using StateHighCouncil.Web.Models;

namespace StateHighCouncil.Web.Data;

public class DataContext : DbContext
{
    public DbSet<Legislator> Legislators { get; set; }
    public DbSet<Committee> Committees { get; set; }
    public DbSet<Session> Sessions { get; set; }
    public DbSet<CommitteeAssignment> CommitteeAssignments { get; set; }
    public DbSet<SessionAssignment> SessionAssignments { get; set; }
    public DbSet<FinanceReport> FinanceReports { get; set; }
    public DbSet<Conflict> Conflicts { get; set; }
    public DbSet<Bill> Bills { get; set; }
    //public DbSet<Agenda> Agendas { get; set; }
    public DbSet<CodeSection> CodeSections { get; set; }
    public DbSet<Subject> Subjects { get; set; }
    public DbSet<SystemSetting> SystemSettings { get; set; }

    //public DbSet<ActionHistory> ActionHistories { get; set; }

    public DataContext(DbContextOptions<DataContext> options) : base(options)
    { }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite(@"Filename=..\StateHighCouncil.db");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Legislator>().ToTable("Legislators");
        modelBuilder.Entity<Session>().ToTable("Sessions");
        modelBuilder.Entity<SessionAssignment>().ToTable("SessionAssignments");
        modelBuilder.Entity<Committee>().ToTable("Committees");
        modelBuilder.Entity<CommitteeAssignment>().ToTable("CommitteeAssignments");
        modelBuilder.Entity<FinanceReport>().ToTable("FinanceReports");
        modelBuilder.Entity<Bill>().ToTable("Bills");
        //modelBuilder.Entity<Agenda>().ToTable("Agendas");
        modelBuilder.Entity<CodeSection>().ToTable("CodeSections");
        modelBuilder.Entity<Subject>().ToTable("Subjects");
    }
}
