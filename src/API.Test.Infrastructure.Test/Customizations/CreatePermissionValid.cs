using API.Test.Domain;
using AutoFixture;
using System;

namespace API.Test.Infrastructure.Test.Customizations
{
    public class CreatePermissionValid : ICustomization
    {
        public void Customize(IFixture fixture)
        {
            var permission = new Permission
            {
                Id = 1,
                EmployeeFirstName = fixture.Create<string>(),
                EmployeeLastName = fixture.Create<string>(),
                TypeId = fixture.Create<int>(),
                PermissionDate = fixture.Create<DateTime>()
            };

            fixture.Register(() => permission);
        }
    }
}