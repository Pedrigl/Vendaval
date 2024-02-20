using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vendaval.Infrastructure.Data.Contexts;

namespace Vendaval.Infrastructure.Data.Repositories.EFRepositories
{
    public class EFGenericRepository<T> where T : class
    {
        private readonly VendavalDbContext _context;
        public EFGenericRepository(VendavalDbContext context)
        {
            _context = context;
        }

        public async Task<T> GetById(int id)
        {
            return await _context.Set<T>().FindAsync(id);
        }

        public async Task<List<T>> GetAll()
        {
            return await _context.Set<T>().ToListAsync();
        }

        public IEnumerable<T> GetWhere(Func<T, bool> predicate)
        {
            return _context.Set<T>().Where(predicate);
        }

        public async Task AddAsync(T entity)
        {
            await _context.Set<T>().AddAsync(entity);
        }

        public void Update(int entityId, T entity)
        {
            var record = GetById(entityId);
            _context.Entry(record).CurrentValues.SetValues(entity);
        }

        public void Delete(T entity)
        {
            _context.Set<T>().Remove(entity);
        }

        public void DeleteRange(IEnumerable<T> entities)
        {
            _context.Set<T>().RemoveRange(entities);
        }

        public async Task Save()
        {
            await _context.SaveChangesAsync();
        }
    }
}
