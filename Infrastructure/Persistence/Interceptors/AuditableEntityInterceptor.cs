using Domain.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Persistence.Interceptors
{
	public class AuditableEntityInterceptor(ICurrentUserService CurrentUserService) : SaveChangesInterceptor
	{

        public override int SavedChanges(SaveChangesCompletedEventData eventData, int result)
		{
			UpdateAuditableEntities(eventData.Context);
			return base.SavedChanges(eventData, result);
		}
		public override ValueTask<int> SavedChangesAsync(SaveChangesCompletedEventData eventData, int result, CancellationToken cancellationToken = default)
		{
			UpdateAuditableEntities(eventData.Context);
			return base.SavedChangesAsync(eventData, result, cancellationToken);
		}

		public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
		{
			UpdateAuditableEntities(eventData.Context);
			return base.SavingChanges(eventData, result);
		}

		public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
		{
			UpdateAuditableEntities(eventData.Context);
			return base.SavingChangesAsync(eventData, result, cancellationToken);
		}

		void UpdateAuditableEntities(DbContext? context)
		{
			if (context == null) return;

			foreach (var entry in context.ChangeTracker.Entries<AuditableEntity>())
			{
				if (entry.State == EntityState.Modified || entry.HasChangedSubEntities())
				{
					if (entry.Entity.IsDeleted) 
					{
                        // in case of delete
                        entry.Entity.DeletedById = CurrentUserService.UserId;
						entry.Entity.DeletedOn = DateTimeOffset.Now;
					}
					else 
					{
						//in case of modified
						entry.Entity.LastModifiedById = CurrentUserService.UserId;
						entry.Entity.LastModifiedOn = DateTimeOffset.Now;
                    }
				}
				if (entry.State == EntityState.Added)
				{
					entry.Entity.CreatedById = CurrentUserService.UserId;
					entry.Entity.CreatedOn = DateTimeOffset.Now;
                }
				else if (entry.State == EntityState.Deleted)
				{
					entry.State = EntityState.Modified;
					entry.Entity.IsDeleted = true;
					entry.Entity.DeletedById = CurrentUserService.UserId;
					entry.Entity.DeletedOn = DateTimeOffset.Now;
                }
			}
		}

	}
	
	static class Extensions
    {
        /// <summary>
        /// Check if sub Entities have changed
        /// </summary>
        public static bool HasChangedSubEntities(this EntityEntry entry) =>
			entry.References.Any(r =>
				r.TargetEntry != null &&
				r.TargetEntry.Metadata.IsOwned() &&
				(r.TargetEntry.State == EntityState.Added || r.TargetEntry.State == EntityState.Modified));
	}
}
