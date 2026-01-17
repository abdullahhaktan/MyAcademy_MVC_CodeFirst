using MyAcademy_MVC_CodeFirst.Data.Entities;
using System;

namespace MyAcademy_MVC_CodeFirst.DTOs.PolicySaleDtos
{
    public class GetPolicySaleByIdDto
    {
        public int Id { get; set; }
        public decimal Amount { get; set; }

        public int PolicyId { get; set; }
        public Policy Policy { get; set; }

        public int CustomerId { get; set; }
        public Customer Customer { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}