using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace MusicFree
{
   
        public class XCookieAuthEvents : CookieAuthenticationEvents
        {
            public override Task RedirectToLogin(RedirectContext<CookieAuthenticationOptions> context)
            {
                context.RedirectUri = $"/Identity/Account/CustomLogin";
                return base.RedirectToLogin(context);
            }

            public override Task RedirectToLogout(RedirectContext<CookieAuthenticationOptions> context)
            {
                context.RedirectUri = $"/Identity/Account/CustomLogout";
                return base.RedirectToLogout(context);
            }

            public override Task RedirectToAccessDenied(RedirectContext<CookieAuthenticationOptions> context)
            {
                context.RedirectUri = $"/Identity/Account/CustomAccessDenied";
                return base.RedirectToAccessDenied(context);
            }

            public override Task RedirectToReturnUrl(RedirectContext<CookieAuthenticationOptions> context)
            {
                context.RedirectUri = String.Empty;
                return base.RedirectToReturnUrl(context);
            }
        
    }
}
