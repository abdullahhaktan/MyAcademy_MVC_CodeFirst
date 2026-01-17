using MyAcademy_MVC_CodeFirst.Data.Context;
using MyAcademy_MVC_CodeFirst.Data.Entities;
using System;

namespace MyAcademy_MVC_CodeFirst.Services
{
    public class CustomerLogService
    {
        public void WriteLog(string email, string action, string ipAdress, string logType, string description, string createdDate)
        {
            using (var db = new AppDbContext())
            {
                var newLog = new CustomerLog
                {
                    Email = email,
                    LogType = logType,
                    Action = action,
                    Description = description,
                    IpAddress = "127.0.0.1",
                    CreatedDate = DateTime.Now,
                };

                db.CustomerLogs.Add(newLog);
                db.SaveChanges();
            }
        }
    }
}