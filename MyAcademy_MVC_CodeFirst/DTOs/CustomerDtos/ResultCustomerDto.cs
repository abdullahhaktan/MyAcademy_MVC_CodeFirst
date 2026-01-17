using MyAcademy_MVC_CodeFirst.Data.Entities;
using System;
using System.Collections.Generic;

namespace MyAcademy_MVC_CodeFirst.DTOs.CustomerDtos
{
    public class ResultCustomerDto
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string City { get; set; }
        public string Email { get; set; }
        public DateTime RegisteredAt { get; set; }
        public IList<Policy> Policies { get; set; }
        public bool IsActive { get; set; }
    }
}