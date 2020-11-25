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
            string[] subs = context.UserName.Split('|');
            var ctx = false;
            if (subs[0].Equals("admin"))
            {
                ctx = await new CustomerDAO().LoginAsync(subs[1], context.Password);
                if (ctx == true)
                {
                    identity.AddClaim(new Claim(ClaimTypes.Role, "admin"));
                    identity.AddClaim(new Claim(subs[1], context.Password));
                    context.Validated(identity);
                }
                else
                {
                    context.SetError("invalid_grant", "Provided is incorrect");
                    return;
                }
            } else if(subs[0].Equals("customer"))
            {
                ctx = await new AdminDAO().LoginAsync(subs[1], context.Password);
                if (ctx == true)
                {
                    identity.AddClaim(new Claim(ClaimTypes.Role, "customer"));
                    identity.AddClaim(new Claim(subs[1], context.Password));
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
}