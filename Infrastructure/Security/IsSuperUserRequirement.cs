using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Security
{
    public class IsSuperUserRequirement : IAuthorizationRequirement
    {
    }

    public class IsSuperUserRequirementHandler : AuthorizationHandler<IsSuperUserRequirement>
    {
        private readonly DataContext _dbConext;

        public IsSuperUserRequirementHandler(DataContext dbConext)
        {
            _dbConext = dbConext;
        }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, IsSuperUserRequirement requirement)
        {
            var userId = context.User.FindFirstValue(ClaimTypes.NameIdentifier);

            var user = _dbConext.Users.FindAsync(userId).Result;

            if (user.IsSuperUser) context.Succeed(requirement);
                
           return Task.CompletedTask;
        }
    }
}
