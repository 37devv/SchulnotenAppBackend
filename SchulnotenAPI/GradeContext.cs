using Microsoft.EntityFrameworkCore;

namespace SchulnotenAPI;

public class GradeContext : DbContext
{
    public GradeContext(DbContextOptions options) : base(options) { }

    public DbSet<Grade> Grades { get; set; }
}