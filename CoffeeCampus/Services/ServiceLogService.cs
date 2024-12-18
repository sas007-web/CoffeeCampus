using System;
using CoffeeCampus.Data;
using System.Linq;
using Microsoft.AspNetCore.Http; // for context
using System.Security.Claims; // For claims from user

namespace CoffeeCampus.Services
{
    public class ServiceLogService
    {
        private readonly CoffeeCampusDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ServiceLogService(CoffeeCampusDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }


        public DateTime? GetLastServiceDate()
        {
            var logEntry = _context.ServiceLogs.OrderByDescending(s => s.ServiceDate).FirstOrDefault();
            return logEntry?.ServiceDate;
        }
        public string GetServiceBy()
        {
            var logEntry = _context.ServiceLogs.OrderByDescending(s => s.ServiceDate).FirstOrDefault();
            return logEntry?.ServiceBy;
        }

        public void LogService()
        {
            var userId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null) return;

            _context.ServiceLogs.Add(new Models.ServiceLog { ServiceDate = DateTime.Now, ServiceBy = userId });
            _context.SaveChanges();
        }
    }
}