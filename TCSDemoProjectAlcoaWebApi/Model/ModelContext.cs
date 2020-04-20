using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TCSDemoProjectAlcoaWebApi.Model {
	public class ModelContext : DbContext,IDisposable{

		public ModelContext(DbContextOptions<ModelContext> options)
			:base(options){

		}

		public DbSet<UsersDetailInfo> UsersDetailInfo { get; set; }
		public DbSet<UserRoles>  UserRoles { get; set; }

		public DbSet<CountryDetails> CountryDetails { get; set; }
		public DbSet<StateDetails> StateDetails { get; set; }
		public DbSet<CityDetails> CityDetails { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{

			/* Model UsersDetailInfo */
			modelBuilder.Entity<UsersDetailInfo>().Property(xx => xx.active_status).HasDefaultValue(false);
			modelBuilder.Entity<UsersDetailInfo>().Property(xx => xx.createdtime).HasDefaultValueSql("getdate()");
			
			
			/* Model UserRoles */
			modelBuilder.Entity<UserRoles>().Property(xx => xx.active_status).HasDefaultValue(true);
			modelBuilder.Entity<UserRoles>().Property(xx => xx.createddate).HasDefaultValueSql("getdate()");

			/* Model UserRoles */
			modelBuilder.Entity<CountryDetails>().Property(xx => xx.active_status).HasDefaultValue(true);
			modelBuilder.Entity<CountryDetails>().Property(xx => xx.createddate).HasDefaultValueSql("getdate()");

			/* Model UserRoles */
			modelBuilder.Entity<StateDetails>().Property(xx => xx.active_status).HasDefaultValue(true);
			modelBuilder.Entity<StateDetails>().Property(xx => xx.createddate).HasDefaultValueSql("getdate()");

			/* Model UserRoles */
			modelBuilder.Entity<CityDetails>().Property(xx => xx.active_status).HasDefaultValue(true);
			modelBuilder.Entity<CityDetails>().Property(xx => xx.createddate).HasDefaultValueSql("getdate()");

		}

		
	}
}
