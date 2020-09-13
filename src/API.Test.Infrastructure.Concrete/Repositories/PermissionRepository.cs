using API.Test.Domain;
using API.Test.Infrastructure.Concrete.DbContexts;
using API.Test.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace API.Test.Infrastructure.Concrete.Repositories
{
    public class PermissionRepository : BaseRepository, IPermissionRepository
    {
        public PermissionRepository(TestDbContext context) : base(context)
        {
        }

		public async Task AddPermissionAsync(Permission permission)
		{
			await _context.Permission.AddAsync(permission);
		}

        public void DeletePermission(Permission permission)
        {
			_context.Remove(permission);
        }

        public async Task<Permission> GetPermissionAsync(int id)
		{
			return await _context.Permission
				.AsNoTracking()
				.Include(x => x.Type)
				.FirstOrDefaultAsync(x => x.Id == id);
		}

		public async Task<Permission> GetPermissionForUpdateAsync(int id)
		{
			return await _context.Permission.FirstOrDefaultAsync(x => x.Id == id);
		}
		public async Task<IEnumerable<Permission>> GetPermissionsAsync()
		{
			return await _context.Permission
				.AsNoTracking()
				.Include(x => x.Type).ToListAsync();
		}

		public void UpdatePermission(Permission permission)
		{
			_context.Set<Permission>().Attach(permission);
			_context.Entry(permission).State = EntityState.Modified;
		}
	}
}
