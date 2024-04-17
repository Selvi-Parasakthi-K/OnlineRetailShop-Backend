using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using OnlineRetailShop.Repository.IRepository;
using Microsoft.Identity.Client.Platforms.Features.DesktopOs.Kerberos;
using OnlineRetailShop.Repository;

namespace OnlineRetailShop.Filter
{
    public class AuthorizationFilter : Attribute, IAuthorizationFilter
    {
        private readonly IUserRepository _userRepository;

        public AuthorizationFilter(IUserRepository userRepository) 
        { 
            _userRepository = userRepository;
        }
        public async void OnAuthorization(AuthorizationFilterContext context)
        {
            var userid = context.HttpContext.User.FindFirst(c => c.Type == "ID").Value;
            int userid1 = int.Parse(userid.ToString());
            var role = await _userRepository.GetRole(userid1);
            if (role != null)
            {
                context.Result = new ForbidResult();
            }
        }
    }
}
