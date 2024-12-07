
using Core.Entities;

namespace DAL
{
    public interface IDbContextFactory
    {
        ErpDbContext DbContext { get; }
    }
}
