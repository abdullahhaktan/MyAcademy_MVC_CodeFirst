using System;

namespace MyAcademy_MVC_CodeFirst.Data.Entities
{
    public class CustomerLog
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Action { get; set; }
        public string IpAddress { get; set; }
        public string LogType { get; set; }
        public string Description { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
    }
}