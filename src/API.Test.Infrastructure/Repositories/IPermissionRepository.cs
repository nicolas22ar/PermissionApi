using API.Test.Domain;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace API.Test.Infrastructure.Repositories
{
    public interface IPermissionRepository
    {
        Task<Permission> GetPermissionAsync(int id);

        Task<Permission> GetPermissionForUpdateAsync(int id);

        Task<IEnumerable<Permission>> GetPermissionsAsync();

        void UpdatePermission(Permission permission);

        Task AddPermissionAsync(Permission permission);

        void DeletePermission(Permission permission);

        Task SaveAsync();
    }
}