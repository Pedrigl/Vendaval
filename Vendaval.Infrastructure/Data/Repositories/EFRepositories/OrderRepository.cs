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
    public class OrderRepository : EFGenericRepository<Order>,IOrderRepository
    {
        private readonly VendavalDbContext _context;
        public OrderRepository(VendavalDbContext context) : base(context)
        {
            _context = context;
        }
    }
}
