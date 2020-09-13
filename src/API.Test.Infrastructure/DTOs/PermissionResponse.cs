using System;

namespace API.Test.Infrastructure.DTOs
{
    public class PermissionResponse
    {
        public int Id { get; set; }
        public PermissionType Type { get; set; }
        public string EmployeeFirstName { get; set; }
        public string EmployeeLastName { get; set; }
        public DateTime PermissionDate { get; set; }
    }
}
