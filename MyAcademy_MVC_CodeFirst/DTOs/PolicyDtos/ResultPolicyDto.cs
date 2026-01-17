using MyAcademy_MVC_CodeFirst.Data.Entities;
using System.Collections.Generic;

namespace MyAcademy_MVC_CodeFirst.DTOs.PolicyDtos
{
    public class ResultPolicyDto
    {
        public int Id { get; set; }
        public string PolicyNumber { get; set; }
        public string InsuranceType { get; set; }
        public bool IsActive { get; set; }

        public decimal PremiumAmount { get; set; }

        public string Description { get; set; }
        public string Description1 { get; set; }
        public string ImageUrl { get; set; }
        public string Icon { get; set; }

        public virtual IList<PolicySale> PolicySales { get; set; }
    }
}