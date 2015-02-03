using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GoodboxDesign.Utils.Attributes
{
    public class RoleAuthorizeAttribute : AuthorizeAttribute
    {
        private string[] _authorizedRoles;
        public string[] AuthorizedRoles
        {
            get
            {
                return _authorizedRoles ?? new string[0];
            }
            set
            {
                _authorizedRoles = value;
            }
        }

        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            base.OnAuthorization(filterContext);

            if(filterContext.Result is HttpUnauthorizedResult)
            {
                filterContext.Result = new RedirectResult("~/Error/Authorization");
            }
        }

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            if (httpContext == null)
                throw new ArgumentNullException("httpContext");

            if (!httpContext.User.Identity.IsAuthenticated)
                return false;

            if (AuthorizedRoles.Length == 0)
                return true;

            if (AuthorizedRoles.Any(httpContext.User.IsInRole))
                return true;

            return false;
        }
    }
}