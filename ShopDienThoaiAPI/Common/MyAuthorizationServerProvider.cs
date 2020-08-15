using Microsoft.Owin.Security.OAuth;
using Models.DAO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;

namespace ShopDienThoaiAPI.Common
{
    public class MyAuthorizationServerProvider : OAuthAuthorizationServerProvider
    {
        public override async Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            context.Validated();
        }

        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            var identity = new ClaimsIdentity(context.Options.AuthenticationType);
            var ctx = await new CustomerDAO().LoginAsync(context.UserName, context.Password);
            if (ctx == true)
            {
                identity.AddClaim(new Claim(ClaimTypes.Role, "user"));
                identity.AddClaim(new Claim(context.UserName, context.Password));
                context.Validated(identity);
            }
            else
            {
                context.SetError("invalid_grant", "Provided is incorrect");
                return;
            }
        }
    }
}