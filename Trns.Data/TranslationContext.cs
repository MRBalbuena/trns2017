using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Trns.Data.Models;
using Trns.Data.Trs.Data;

namespace Trns.Data
{
    public class TranslationContext : DbContext, ITranslationContext
    {
        public DbSet<Translation> Translations { get; set; }

        Database IModel.Database => throw new NotImplementedException();

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var config = new ConfigurationBuilder();
            var path = Path.Combine(AppContext.BaseDirectory, "appsetings.json");

            optionsBuilder.UseSqlServer("Server=MarceloRB\\SQLExpress;Database=Translations;Persist Security Info=True; User ID=omuser;Password=Onmove01;MultipleActiveResultSets=True");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Translation>()
                .Property("TransKey")
                .HasMaxLength(200)
                .IsRequired();

            modelBuilder.Entity<Translation>()
                .Property("Text")
                .HasMaxLength(2000)
                .IsRequired();

            modelBuilder.Entity<Translation>()
                .Property("Comment")
                .HasMaxLength(1000);                

            modelBuilder.Entity<Translation>()
                .Property("BlockedBy")
                .HasMaxLength(50);

            modelBuilder.Entity<Translation>()
                .Property("TransBy")
                .HasMaxLength(50);

            modelBuilder.Entity<Translation>()
                .Property("CheckedBy")
                .HasMaxLength(50);

            modelBuilder.Entity<Translation>()
                .Property("EditedBy")
                .HasMaxLength(50);

            modelBuilder.Entity<Translation>()
                .Property("Comment")
                .HasMaxLength(500);

            modelBuilder.Entity<Translation>()
                .Property("Edition")
                .HasMaxLength(20);
        }
    }
}
