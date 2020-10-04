using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;

namespace ConsoleApp1
{
	public class User1DbContext_pg : DbContext
	{
		public DbSet<SelectionMatrix> SelectionMatrix { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<SelectionMatrix>();
		}

		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			optionsBuilder.UseNpgsql("Server={};Port=5432;User Id={};Password={};Ssl Mode=Require;");
		}
	}

	public class SelectionMatrix
	{
		public long Id { get; set; }
		public string firstName { get; set; }
		public string lastName { get; set; }
		public int age { get; set; }
	}
}
