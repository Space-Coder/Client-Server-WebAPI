using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TexodeAPI.Services
{
    public interface IMobileService
    {
        Task<MobileCard> GetById(Guid id);
        Task<MobileCard> GetByName(string name);
        List<MobileCard> GetAllData();
        Task AddCard(MobileCard mobile);
        Task RemoveCards(List<MobileCard> mobiles);
        Task UpdateCard(MobileCard mobile);
    }
}
