using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityExample.Controllers
{
    public class OperationController : Controller
    {
        private IAuthorizationService _authorizationService;

        public OperationController(IAuthorizationService authorizationService)
        {
            _authorizationService = authorizationService;
        }


        public IActionResult Index()
        {
            return View();
        }
    }

    public class CookieJarAuthorizationHandler :
        AuthorizationHandler<OperationAuthorizationRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, OperationAuthorizationRequirement requirement)
        {
            if (requirement.Name == CookieJarOperations.Look)
            {
                if (context.User.Identity.IsAuthenticated)
                {
                    context.Succeed(requirement);
                }
            }else if(requirement.Name == CookieJarOperations.Open)
            {
                if (context.User.HasClaim("Friend", "Good Friend"))
                {
                    context.Succeed(requirement);
                }
            }
            return Task.CompletedTask;
        }
    }

    public class CookieJarOperations
    {
        public static string Open = "Open";
        public static string Look = "Look";
        public static string Take = "Take";

    }
}
