namespace RepositoryLayer.Authentication
{
    public class AuthenticationClass
    {

        /*public class PasswordResetTokenAuthorization : AuthorizationHandler<IAuthorizationRequirement>
        {
            protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, IAuthorizationRequirement requirement)
            {
                {
                    if (context.User.HasClaim(c => c.Type == "Reset_Password_Token" && c.Value == "true"))
                    {
                        context.Fail();
                        return Task.CompletedTask;
                    }
                    context.Succeed(requirement);
                    return Task.CompletedTask;
                }
            }
        }*/
    }
}
