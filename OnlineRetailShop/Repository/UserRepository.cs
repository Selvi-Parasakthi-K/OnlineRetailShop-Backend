using OnlineRetailShop.Models;
using OnlineRetailShop.Repository.IRepository;
using System.Linq;

namespace OnlineRetailShop.Repository
{
    public class UserRepository : IUserRepository
    {
            private readonly CommonContext _context;
            public UserRepository(CommonContext context)
            {
                _context = context;
            }
            public async Task<Credentials> GetRole(int UserId)
            {
                Credentials user = null;
                user = _context.Credentials.Find(UserId);
                return user;
            }
        
    }
}
