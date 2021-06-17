using E_Invoice_API.Data;
using System;

namespace E_Invoice_API.Core.Repositories
{
    public class BaseRepository
    {
        protected readonly ApplicationDbContext _context;

        public BaseRepository(ApplicationDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }
    }
}
