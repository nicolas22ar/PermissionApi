using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace API.Test.Infrastructure.DTOs
{
    public class PermissionRequest
    {
        [Required]
        [DisplayName("Type Id")]
        public int? TypeId { get; set; }

        [Required]
        [StringLength(255)]
        [DisplayName("Employee First Name")]
        public string EmployeeFirstName { get; set; }

        [Required]
        [StringLength(255)]
        [DisplayName("Employee Last Name")]
        public string EmployeeLastName { get; set; }

        [Required]
        [DisplayName("Permission Date")]
        public DateTime? PermissionDate { get; set; }
    }
}