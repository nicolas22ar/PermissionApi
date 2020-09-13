using API.Test.Domain;
using API.Test.Infrastructure.Concrete.Exceptions;
using API.Test.Infrastructure.Repositories;
using API.Test.Infrastructure.Services;
using AutoMapper;
using System.Collections.Generic;
using System.Threading.Tasks;
using DTO = API.Test.Infrastructure.DTOs;

namespace API.Test.Infrastructure.Concrete.Services
{
    public class PermissionService : IPermissionService
    {
        private const string NotFoundMsg = "Permission not found";
        private readonly IPermissionRepository _repository;
        private readonly IMapper _mapper;

        public PermissionService(IPermissionRepository entityRepository, IMapper mapper)
        {
            _repository = entityRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<DTO.PermissionResponse>> GetAllAsync()
        {
            var result = await _repository.GetPermissionsAsync();

            if (result == null) return null;

            return _mapper.Map<IEnumerable<DTO.PermissionResponse>>(result);
        }

        public async Task<DTO.PermissionResponse> GetAsync(int id)
        {
            return _mapper.Map<DTO.PermissionResponse>(await _repository.GetPermissionAsync(id));
        }

        public async Task<int> CreateAsync(DTO.PermissionRequest newPermission)
        {
            var permission = _mapper.Map<Permission>(newPermission);

            await _repository.AddPermissionAsync(permission);

            await _repository.SaveAsync();

            return permission.Id;
        }

        public async Task UpdateAsync(int id, DTO.PermissionRequest updatedPermission)
        {
            var permission = await _repository.GetPermissionForUpdateAsync(id);

            if (permission == null)
            {
                throw new EntityNotFoundException(NotFoundMsg);
            }

            _mapper.Map(updatedPermission, permission);

            _repository.UpdatePermission(permission);

            await _repository.SaveAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var permission = await _repository.GetPermissionForUpdateAsync(id);

            if (permission == null)
            {
                throw new EntityNotFoundException(NotFoundMsg);
            }

            _repository.DeletePermission(permission);

            await _repository.SaveAsync();
        }
    }
}