using System;
using Microsoft.EntityFrameworkCore;

namespace SearchLibrary
{
    public class SearchContext : DbContext
    {
        public DbSet<Document> Documents { get; set; }
        public DbSet<IndexMap> IndexMaps { get; set; }
        public DbSet<Entry> Entries { get; set; }
        private readonly string _server;

        public SearchContext(string server)
        {
            this._server = server;
        }
        
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer($"Server={_server};Database=SearchDB;Trusted_Connection=True;");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<IndexMap>()
            .HasKey(b => new {b.Word, b.EntryId});

            modelBuilder.Entity<IndexMap>()
            .HasOne(p => p.Entry)
            .WithMany(b => b.IndexMap).HasForeignKey(p => p.EntryId);
        }
    }
}
