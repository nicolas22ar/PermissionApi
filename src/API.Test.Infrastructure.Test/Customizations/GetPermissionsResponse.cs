using API.Test.Domain;
using AutoFixture;
using System;
using System.Collections.Generic;
using DTO = API.Test.Infrastructure.DTOs;

namespace API.Test.Infrastructure.Test.Customizations
{
    public class GetPermissionsResponse : ICustomization
    {
        public void Customize(IFixture fixture)
        {
            var permissions = new List<Permission>()
            {
                new Permission
                {
                    Id = 1,
                    EmployeeFirstName = fixture.Create<string>(),
                    EmployeeLastName = fixture.Create<string>(),
                    TypeId = fixture.Create<int>(),
                    PermissionDate = fixture.Create<DateTime>()
                },
                new Permission
                {
                    Id = 2,
                    EmployeeFirstName = fixture.Create<string>(),
                    EmployeeLastName = fixture.Create<string>(),
                    TypeId = fixture.Create<int>(),
                    PermissionDate = fixture.Create<DateTime>()
                },
                new Permission
                {
                    Id = 3,
                    EmployeeFirstName = fixture.Create<string>(),
                    EmployeeLastName = fixture.Create<string>(),
                    TypeId = fixture.Create<int>(),
                    PermissionDate = fixture.Create<DateTime>()
                }
            };

            var dto = new List<DTO.PermissionResponse>()
            {
                fixture.Create<DTO.PermissionResponse>(),
                fixture.Create<DTO.PermissionResponse>(),
                fixture.Create<DTO.PermissionResponse>()
            };
            fixture.Register(() => permissions);
            fixture.Register(() => dto);
        }
    }
}