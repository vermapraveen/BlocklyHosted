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
		public DbSet<BaseMatrix> BaseMatrix { get; set; }
		public DbSet<BaseMatrixItem> BaseMatrixItem { get; set; }
		public DbSet<ChangeSelection> ChangeSelection { get; set; }
		public DbSet<Action> Action { get; set; }
		public DbSet<ItemItem> ItemItem { get; set; }
		public DbSet<Options> Options { get; set; }
		public DbSet<TransactionContext> TransactionContext { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<BaseMatrix>();
			modelBuilder.Entity<BaseMatrixItem>();
			modelBuilder.Entity<ChangeSelection>();
			modelBuilder.Entity<Action>();
			modelBuilder.Entity<ItemItem>();
			modelBuilder.Entity<Options>();
			modelBuilder.Entity<TransactionContext>();
		}

		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			optionsBuilder.UseNpgsql("Host=localhost;Port=5433;Database=user1;Username=postgres;Password=postgres");
		}
	}

	public class BaseMatrix
	{
		public int Id { get; set; } //Extra
		public BaseMatrixItem items { get; set; }
		public string connections { get; set; } //updated from "object"
		public int multiplierQuantity { get; set; }
		public bool isStaging { get; set; }
		public string relationshipContext { get; set; } // updated from "object"
		public TransactionContext sellerContext { get; set; }

	}
	public class BaseMatrixItem
	{
		public string id { get; set; }
		public Guid instanceId { get; set; }
		public ChangeSelection deltaSelections { get; set; }
		public ItemItem items { get; set; }

	}
	public class ChangeSelection
	{
		public Action actions { get; set; }
		public string id { get; set; }
		public Guid instanceId { get; set; }
		public string path { get; set; }

	}
	public class Action
	{
		public int Id { get; set; } //Extra
		public string type { get; set; }
		public int value { get; set; }

	}
	public class ItemItem
	{
		public string id { get; set; }
		public Guid instanceId { get; set; }
		public ChangeSelection deltaSelections { get; set; }
		public Options options { get; set; }
		public string path { get; set; }

	}
	public class Options
	{
		public int Id { get; set; } //Extra
		public bool subscriptionUpgrade { get; set; }

	}
	public class TransactionContext
	{
		public int Id { get; set; } //Extra
		public string region { get; set; }
		public string country { get; set; }
		public string language { get; set; }
		public string currency { get; set; }
		public string customerSet { get; set; }
		public string segment { get; set; }
		public string sourceApplicationName { get; set; }
		public int companyNumber { get; set; }
		public int businessUnitId { get; set; }
	}
}
