using Microsoft.Identity.Client.Platforms.Features.DesktopOs.Kerberos;
using OnlineRetailShop.Models;

namespace OnlineRetailShop.Repository.IRepository
{
    public interface IUserRepository
    {
        Task<Credentials> GetRole(int UserId);
    }
}
