using MyAcademy_MVC_CodeFirst.Data.Context;
using MyAcademy_MVC_CodeFirst.Data.Entities;
using System;

namespace MyAcademy_MVC_CodeFirst.Services
{
    public class AdminLogService
    {
        public void WriteLog(string logType, string action, string description)
        {
            using (var db = new AppDbContext())
            {
                var newLog = new AdminLog
                {
                    AdminUsername = "abdullahhktn",
                    LogType = logType,
                    Action = action,
                    Description = description,
                    IpAddress = "127.0.0.1",
                    CreatedDate = DateTime.Now,
                };

                db.AdminLogs.Add(newLog);
                db.SaveChanges();
            }
        }
    }
}