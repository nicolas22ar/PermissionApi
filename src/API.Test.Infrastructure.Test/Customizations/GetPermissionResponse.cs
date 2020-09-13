using API.Test.Domain;
using AutoFixture;
using System;
using DTO = API.Test.Infrastructure.DTOs;

namespace API.Test.Infrastructure.Test.Customizations
{
    public class GetPermissionResponse : ICustomization
    {
        public void Customize(IFixture fixture)
        {
            var permission = new Permission
            {
                Id = 1,
                EmployeeFirstName = "Test",
                EmployeeLastName = "Test",
                PermissionDate = fixture.Create<DateTime>(),
                Type = new PermissionType
                {
                    Id = 1,
                    Description = "Test"
                },
            };

            var dto = new DTO.PermissionResponse
            {
                Id = 1,
                EmployeeFirstName = "Test",
                EmployeeLastName = "Test",
                Type = new DTO.PermissionType
                {
                    Id = 1,
                    Description = "Test"
                },
                PermissionDate = fixture.Create<DateTime>()
            };

            fixture.Register(() => permission);
            fixture.Register(() => dto);
        }
    }
}