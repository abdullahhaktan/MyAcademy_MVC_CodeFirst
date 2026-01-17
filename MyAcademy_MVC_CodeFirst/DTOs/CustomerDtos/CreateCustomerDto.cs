using System;

namespace MyAcademy_MVC_CodeFirst.DTOs.CustomerDtos
{
    public class CreateCustomerDto
    {
        public string FullName { get; set; }
        public string City { get; set; }
        public string Email { get; set; }
        public DateTime RegisteredAt { get; set; }
        public bool IsActive { get; set; }
    }
}