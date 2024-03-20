using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vendaval.Domain.Entities;
using Vendaval.Infrastructure.Data.Contexts;
using Vendaval.Infrastructure.Data.Repositories.EFRepositories.Interfaces;

namespace Vendaval.Infrastructure.Data.Repositories.EFRepositories
{
    public class ChatUserRepository : EFGenericRepository<ChatUser>, IChatUserRepository
    {
        private readonly VendavalDbContext _context;

        public ChatUserRepository(VendavalDbContext context) : base(context)
        {
            _context = context;
        }

        public new IEnumerable<ChatUser> GetWhere(Func<ChatUser, bool> predicate)
        {
            return _context.ChatUsers.Where(predicate);
        }
    }
}
