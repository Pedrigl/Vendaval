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
    public class ProductAvaliationRepository : EFGenericRepository<ProductAvaliation>, IProductAvaliationRepository
    {
        private readonly VendavalDbContext _context;

        public ProductAvaliationRepository(VendavalDbContext context) : base(context)
        {
            _context = context;
        }
    }
}
