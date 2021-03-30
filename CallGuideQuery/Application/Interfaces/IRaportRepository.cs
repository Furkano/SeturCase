using System.Threading.Tasks;
using Domain.Entity;

namespace Application.Interfaces
{
    public interface IRaportRepository
    {
         Task<bool> CreateAsync(Raport model);
         Task<Raport> GetRaport(int userId);
    }
}