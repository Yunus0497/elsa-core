using System.Threading.Tasks;
using Elsa.Modules.Http.Models;
using Elsa.Modules.Http.Services;
using Microsoft.AspNetCore.Authorization;

namespace Elsa.Modules.Http.Handlers
{
    public class AuthenticationBasedHttpEndpointAuthorizationHandler : IHttpEndpointAuthorizationHandler
    {
        private readonly IAuthorizationService _authorizationService;
        public AuthenticationBasedHttpEndpointAuthorizationHandler(IAuthorizationService authorizationService) => _authorizationService = authorizationService;

        public async ValueTask<bool> AuthorizeAsync(AuthorizeHttpEndpointContext context)
        {
            var httpContext = context.HttpContext;
            var user = httpContext.User;
            var identity = user.Identity;

            if (identity == null)
                return false;
            
            if (identity.IsAuthenticated == false)
                return false;
            
            var httpEndpoint = context.Activity;
            var expressionExecutionContext = context.ExpressionExecutionContext;
            var policyName = httpEndpoint.Policy.Get(expressionExecutionContext);

            if (string.IsNullOrWhiteSpace(policyName))
                return identity.IsAuthenticated;

            var resource = new HttpWorkflowResource(expressionExecutionContext, httpEndpoint, context.WorkflowInstanceId);
            var authorizationResult = await _authorizationService.AuthorizeAsync(user, resource, policyName);
            return authorizationResult.Succeeded;
        }
    }
}