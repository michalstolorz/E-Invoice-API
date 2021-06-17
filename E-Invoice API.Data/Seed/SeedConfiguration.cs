
using Microsoft.EntityFrameworkCore;

namespace E_Invoice_API.Data.Seed
{
    public static class SeedConfiguration
    {
        public static void Seed(ModelBuilder builder)
        {

        }

        public static void FirstSeed(ApplicationDbContext context)
        {
            InitialSeed.SeedAsync(context);
        }
    }
}
