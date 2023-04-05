using CapstoneProject_BE.Models;
using Quartz;

namespace CapstoneProject_BE.Scheduler
{
    public class ClearTrashToken : IJob
    {
        public IConfiguration _configuration;
        private readonly InventoryManagementContext _context;
        public ClearTrashToken(InventoryManagementContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }
        public Task Execute(IJobExecutionContext context)
        {
            var trashToken = _context.EmailTokens.Where(x=>x.ExpiredAt<DateTime.Now||x.IsUsed||x.IsRevoked).ToList();
            _context.RemoveRange(trashToken);
            _context.SaveChanges();
            return Task.CompletedTask;
        }
    }
}
