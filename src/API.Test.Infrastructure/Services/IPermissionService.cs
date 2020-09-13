using API.Test.Infrastructure.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace API.Test.Infrastructure.Services
{
    public interface IPermissionService
    {
        Task<PermissionResponse> GetAsync(int id);

        Task<IEnumerable<PermissionResponse>> GetAllAsync();

        Task<int> CreateAsync(PermissionRequest permission);

        Task UpdateAsync(int id, PermissionRequest permission);

        Task DeleteAsync(int id);
    }
}