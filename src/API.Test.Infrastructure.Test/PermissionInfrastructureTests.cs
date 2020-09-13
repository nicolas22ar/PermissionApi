using API.Test.Domain;
using API.Test.Infrastructure.Concrete.Exceptions;
using API.Test.Infrastructure.Concrete.Services;
using API.Test.Infrastructure.Repositories;
using API.Test.Infrastructure.Test.Attributes;
using API.Test.Infrastructure.Test.Customizations;
using AutoFixture;
using AutoFixture.Xunit2;
using AutoMapper;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using DTO = API.Test.Infrastructure.DTOs;

namespace API.Test.Infrastructure.Test
{
    public class PermissionInfrastructureTests
    {
        private const string DuplicatedErrorMsg = "Error while updating the database - Duplicated entity";
        private const string ForeignConstraintErrorMsg = "Error while updating the database - Constraint validation";

        [Theory]
        [DefaultData(typeof(GetPermissionResponse))]
        public async Task GetPermission_ShouldReturnValidPermissionDto(
            [Frozen] Mock<IPermissionRepository> mockRepository,
            [Frozen] Mock<IMapper> mockMapper,
            Permission permission,
            DTO.PermissionResponse dto,
            PermissionService sut)
        {
            //Arrange
            mockRepository.Setup(p => p.GetPermissionAsync(1)).ReturnsAsync(permission);
            mockMapper.Setup(m => m.Map<DTO.PermissionResponse>(permission)).Returns(dto);

            //Act
            var result = await sut.GetAsync(1);

            //Assert
            Assert.NotNull(result);
            Assert.Equal(permission.EmployeeFirstName, result.EmployeeFirstName);
            Assert.Equal(permission.EmployeeLastName, result.EmployeeLastName);
            Assert.Equal(permission.Id, result.Id);
            Assert.NotNull(result.Type);

            mockRepository.Verify(r => r.GetPermissionAsync(1), Times.Once);
            mockMapper.Verify(m => m.Map<DTO.PermissionResponse>(permission), Times.Once);
        }

        [Theory]
        [DefaultData()]
        public async Task GetPermission_ShouldReturnNull(
            [Frozen] Mock<IPermissionRepository> mockRepository,
            [Frozen] Mock<IMapper> mockMapper,
            PermissionService sut)
        {
            //Arrange
            mockRepository.Setup(p => p.GetPermissionAsync(1)).ReturnsAsync((Permission)null);
            mockMapper.Setup(m => m.Map<DTO.PermissionResponse>(null)).Returns((DTO.PermissionResponse)null);

            //Act
            var result = await sut.GetAsync(1);

            //Assert
            Assert.Null(result);
            mockRepository.Verify(r => r.GetPermissionAsync(1), Times.Once);
            mockMapper.Verify(m => m.Map<DTO.PermissionResponse>(null), Times.Once);
        }

        [Theory]
        [DefaultData(typeof(GetPermissionsResponse))]
        public async Task GetPermissions_ShouldReturnValidPermissionDtos(
            [Frozen] Mock<IPermissionRepository> mockRepository,
            [Frozen] Mock<IMapper> mockMapper,
            List<Permission> permissions,
            List<DTO.PermissionResponse> permissionsDto,
            PermissionService sut)
        {
            //Arrange
            mockRepository.Setup(p => p.GetPermissionsAsync()).ReturnsAsync(permissions);
            mockMapper.Setup(m => m.Map<IEnumerable<DTO.PermissionResponse>>(permissions)).Returns(permissionsDto);

            //Act
            var result = await sut.GetAllAsync();

            //Assert
            Assert.NotNull(result);
            Assert.Equal(3, result.Count());
            mockRepository.Verify(r => r.GetPermissionsAsync(), Times.Once);
            mockMapper.Verify(m => m.Map<IEnumerable<DTO.PermissionResponse>>(permissions), Times.Once);
        }

        [Theory]
        [DefaultData()]
        public async Task GetAllPermissions_ShouldReturnNull(
            [Frozen] Mock<IPermissionRepository> mockRepository,
            [Frozen] Mock<IMapper> mockMapper,
            PermissionService sut)
        {
            //Arrange
            mockRepository.Setup(p => p.GetPermissionsAsync()).ReturnsAsync((IEnumerable<Permission>)null);
            mockMapper.Setup(m => m.Map<DTO.PermissionResponse>(null)).Returns((DTO.PermissionResponse)null);

            //Act
            var result = await sut.GetAllAsync();

            //Assert
            Assert.Null(result);
            mockRepository.Verify(r => r.GetPermissionsAsync(), Times.Once);
            mockMapper.Verify(m => m.Map<DTO.PermissionResponse>(null), Times.Never);
        }

        [Theory]
        [DefaultData(typeof(CreatePermissionValid))]
        public async Task CreatePermission_ShouldWork(
            IFixture fixture,
            [Frozen] Mock<IPermissionRepository> mockRepository,
            [Frozen] Mock<IMapper> mockMapper,
            Permission permission,
            PermissionService sut)
        {
            //Arrange
            var permissionRequest = fixture.Create<DTO.PermissionRequest>();
            mockMapper.Setup(m => m.Map<Permission>(permissionRequest)).Returns(permission);
            mockRepository.Setup(p => p.AddPermissionAsync(permission));

            //Act
            var result = await sut.CreateAsync(permissionRequest);

            //Assert
            Assert.Equal(1, result);
            mockMapper.Verify(m => m.Map<Permission>(permissionRequest), Times.Once);
            mockRepository.Verify(r => r.AddPermissionAsync(permission), Times.Once);
            mockRepository.Verify(r => r.SaveAsync(), Times.Once);
        }

        [Theory]
        [DefaultData()]
        public async Task CreatePermission_ShouldThrowDBDuplicatedEntityException(
            IFixture fixture,
            [Frozen] Mock<IPermissionRepository> mockRepository,
            [Frozen] Mock<IMapper> mockMapper,
            PermissionService sut)
        {
            //Arrange
            var permission = fixture.Create<Permission>();
            var dto = fixture.Create<DTO.PermissionRequest>();
            mockMapper.Setup(m => m.Map<Permission>(dto)).Returns(permission);
            mockRepository.Setup(p => p.AddPermissionAsync(permission)).Throws(new DuplicatedEntityException(DuplicatedErrorMsg));

            //Act
            Task act() => sut.CreateAsync(dto);

            //Assert
            var exception = await Assert.ThrowsAsync<DuplicatedEntityException>(act);
            Assert.Equal(DuplicatedErrorMsg, exception.Message);
            mockMapper.Verify(m => m.Map<Permission>(dto), Times.Once);
            mockRepository.Verify(r => r.AddPermissionAsync(permission), Times.Once);
            mockRepository.Verify(r => r.SaveAsync(), Times.Never);
        }

        [Theory]
        [DefaultData()]
        public async Task CreatePermission_ShouldDBThrowEntityNotFoundException(
            IFixture fixture,
            [Frozen] Mock<IPermissionRepository> mockRepository,
            [Frozen] Mock<IMapper> mockMapper,
            PermissionService sut)
        {
            //Arrange
            var permission = fixture.Create<Permission>();
            var dto = fixture.Create<DTO.PermissionRequest>();
            mockMapper.Setup(m => m.Map<Permission>(dto)).Returns(permission);
            mockRepository.Setup(p => p.AddPermissionAsync(permission)).Throws(new EntityNotFoundException(ForeignConstraintErrorMsg));

            //Act
            Task act() => sut.CreateAsync(dto);

            //Assert
            var exception = await Assert.ThrowsAsync<EntityNotFoundException>(act);
            Assert.Equal(ForeignConstraintErrorMsg, exception.Message);
            mockMapper.Verify(m => m.Map<Permission>(dto), Times.Once);
            mockRepository.Verify(r => r.AddPermissionAsync(permission), Times.Once);
            mockRepository.Verify(r => r.SaveAsync(), Times.Never);
        }

        [Theory]
        [DefaultData()]
        public async Task UpdatePermission_ShouldWork(
            IFixture fixture,
            [Frozen] Mock<IPermissionRepository> mockRepository,
            [Frozen] Mock<IMapper> mockMapper,
            PermissionService sut)
        {
            //Arrange
            var permission = fixture.Create<Permission>();
            var permissionRequest = fixture.Create<DTO.PermissionRequest>();
            mockRepository.Setup(p => p.GetPermissionForUpdateAsync(1)).ReturnsAsync(permission);
            mockMapper.Setup(m => m.Map(permissionRequest, permission));
            mockRepository.Setup(p => p.UpdatePermission(permission));

            //Act
            await sut.UpdateAsync(1, permissionRequest);

            //Assert
            mockRepository.Verify(r => r.GetPermissionForUpdateAsync(1), Times.Once);
            mockMapper.Verify(m => m.Map(permissionRequest, permission), Times.Once);
            mockRepository.Verify(r => r.UpdatePermission(permission), Times.Once);
            mockRepository.Verify(r => r.SaveAsync(), Times.Once);
        }

        [Theory]
        [DefaultData()]
        public async Task UpdatePermission_ShouldThrowNotFoundException(
            IFixture fixture,
            [Frozen] Mock<IPermissionRepository> mockRepository,
            [Frozen] Mock<IMapper> mockMapper,
            PermissionService sut)
        {
            //Arrange
            var permission = fixture.Create<Permission>();
            var permissionRequest = fixture.Create<DTO.PermissionRequest>();
            mockRepository.Setup(p => p.GetPermissionForUpdateAsync(1)).ReturnsAsync((Permission)null);
            mockMapper.Setup(m => m.Map(permissionRequest, permission));
            mockRepository.Setup(p => p.UpdatePermission(permission));

            //Act
            Task act() => sut.UpdateAsync(1, permissionRequest);

            //Assert
            var exception = await Assert.ThrowsAsync<EntityNotFoundException>(act);
            Assert.Equal("Permission not found", exception.Message);
            mockRepository.Verify(r => r.GetPermissionForUpdateAsync(1), Times.Once);
            mockMapper.Verify(m => m.Map(permissionRequest, permission), Times.Never);
            mockRepository.Verify(r => r.UpdatePermission(permission), Times.Never);
            mockRepository.Verify(r => r.SaveAsync(), Times.Never);
        }

        [Theory]
        [DefaultData()]
        public async Task UpdatePermission_ShouldDBThrowNotFoundException(
            IFixture fixture,
            [Frozen] Mock<IPermissionRepository> mockRepository,
            [Frozen] Mock<IMapper> mockMapper,
            PermissionService sut)
        {
            //Arrange
            var permission = fixture.Create<Permission>();
            var permissionRequest = fixture.Create<DTO.PermissionRequest>();
            mockRepository.Setup(p => p.GetPermissionForUpdateAsync(1)).ReturnsAsync(permission);
            mockMapper.Setup(m => m.Map(permissionRequest, permission));
            mockRepository.Setup(p => p.UpdatePermission(permission)).Throws(new EntityNotFoundException(ForeignConstraintErrorMsg));

            //Act
            Task act() => sut.UpdateAsync(1, permissionRequest);

            //Assert
            var exception = await Assert.ThrowsAsync<EntityNotFoundException>(act);
            Assert.Equal(ForeignConstraintErrorMsg, exception.Message);
            mockRepository.Verify(r => r.GetPermissionForUpdateAsync(1), Times.Once);
            mockMapper.Verify(m => m.Map(permissionRequest, permission), Times.Once);
            mockRepository.Verify(r => r.UpdatePermission(permission), Times.Once);
            mockRepository.Verify(r => r.SaveAsync(), Times.Never);
        }

        [Theory]
        [DefaultData()]
        public async Task UpdatePermission_ShouldThrowDBDuplicatedEntityException(
        IFixture fixture,
        [Frozen] Mock<IPermissionRepository> mockRepository,
        [Frozen] Mock<IMapper> mockMapper,
        PermissionService sut)
        {
            //Arrange
            var permission = fixture.Create<Permission>();
            var permissionRequest = fixture.Create<DTO.PermissionRequest>();
            mockRepository.Setup(p => p.GetPermissionForUpdateAsync(1)).ReturnsAsync(permission);
            mockMapper.Setup(m => m.Map(permissionRequest, permission));
            mockRepository.Setup(p => p.UpdatePermission(permission)).Throws(new EntityNotFoundException(DuplicatedErrorMsg));

            //Act
            Task act() => sut.UpdateAsync(1, permissionRequest);

            //Assert
            var exception = await Assert.ThrowsAsync<EntityNotFoundException>(act);
            Assert.Equal(DuplicatedErrorMsg, exception.Message);
            mockRepository.Verify(r => r.GetPermissionForUpdateAsync(1), Times.Once);
            mockMapper.Verify(m => m.Map(permissionRequest, permission), Times.Once);
            mockRepository.Verify(r => r.UpdatePermission(permission), Times.Once);
            mockRepository.Verify(r => r.SaveAsync(), Times.Never);
        }
    }
}