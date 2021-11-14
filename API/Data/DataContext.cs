using API.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    public class DataContext : IdentityDbContext<Student, AppRole, int, IdentityUserClaim<int>, AppUserRole,
     IdentityUserLogin<int>, IdentityRoleClaim<int>, IdentityUserToken<int>>
    {
        public DataContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Predmet> Predmets { get; set; }
        public DbSet<Topic> Topics { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Soubor> Soubor { get; set; }
        public DbSet<Hodnoceni> Hodnoceni { get; set; }
        public DbSet<Reply> Replies { get; set; }
        public DbSet<SouborLike> SouborLikes { get; set; }
        public DbSet<CommentLike> CommentLikes { get; set; }
        public DbSet<HodnoceniLike> HodnoceniLikes { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Student>()
                .HasMany(ur => ur.UserRoles)
                .WithOne(u => u.Student)
                .HasForeignKey(ur => ur.UserId)
                .IsRequired();

            builder.Entity<AppRole>()
                .HasMany(ur => ur.UserRoles)
                .WithOne(u => u.Role)
                .HasForeignKey(ur => ur.RoleId)
                .IsRequired();

            builder.Entity<SouborLike>()
                .HasKey(sl => new { sl.StudentId, sl.SouborId });

            builder.Entity<SouborLike>()
                .HasOne(sl => sl.Soubor)
                .WithMany(s => s.StudentsLikedBy)
                .HasForeignKey(sl => sl.SouborId);

            builder.Entity<SouborLike>()
                .HasOne(sl => sl.Student)
                .WithMany(s => s.likedMaterialy)
                .HasForeignKey(sl => sl.StudentId);

            builder.Entity<CommentLike>()
                .HasKey(sl => new { sl.StudentId, sl.CommentId });

            builder.Entity<CommentLike>()
                .HasOne(sl => sl.Comment)
                .WithMany(s => s.StudentsLikedBy)
                .HasForeignKey(sl => sl.CommentId);

            builder.Entity<CommentLike>()
                .HasOne(sl => sl.Student)
                .WithMany(s => s.likedComments)
                .HasForeignKey(sl => sl.StudentId);

            builder.Entity<HodnoceniLike>()
                .HasKey(sl => new { sl.StudentId, sl.HodnoceniId });

            builder.Entity<HodnoceniLike>()
                .HasOne(sl => sl.Hodnoceni)
                .WithMany(s => s.StudentsLikedBy)
                .HasForeignKey(sl => sl.HodnoceniId);

            builder.Entity<HodnoceniLike>()
                .HasOne(sl => sl.Student)
                .WithMany(s => s.likedHodnoceni)
                .HasForeignKey(sl => sl.StudentId);
        }
    }
}