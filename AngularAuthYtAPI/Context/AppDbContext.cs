using AngularAuthYtAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace AngularAuthYtAPI.Context
{
    public class AppDbContext : DbContext
    {
        internal object ResumeClass;

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }
        public DbSet<User> Users { get; set; }
        public DbSet<Jobs> jobs { get; set; }
        public DbSet<Applicant> Applicants { get; set; }
        public DbSet<Applied> appliedjobs { get; set; }
        public DbSet<ResumeClass> ResumesUpload { get; set; }
       

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<User>().ToTable("users");
            builder.Entity<Jobs>().ToTable("jobs");
            builder.Entity<Applicant>().ToTable("applicants");
            builder.Entity<Applied>().ToTable("appliedjobstable");
            base.OnModelCreating(builder);
            builder.Entity<ResumeClass>()
              .HasKey(r => r.ResumeId);
            //builder.Entity<Applied>().ToTable("appliedjob");

        }
    }
}
