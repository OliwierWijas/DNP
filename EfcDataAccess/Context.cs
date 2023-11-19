using Microsoft.EntityFrameworkCore;
using Shared;

namespace EfcDataAccess;

public class Context : DbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<SubForm> SubForms { get; set; }
    public DbSet<Post> Posts { get; set; }
    public DbSet<Comment> Comments { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite("Data Source = ../EfcDataAccess/Assignment.db");
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>().HasKey(user => user.Username);
        modelBuilder.Entity<SubForm>().HasKey(subForm => subForm.Id);
        modelBuilder.Entity<Post>().HasKey(post => post.Id);
        modelBuilder.Entity<Comment>().HasKey(comment => comment.Id);
    }
}