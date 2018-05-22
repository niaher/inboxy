namespace Inboxy.Core.Security.Email
{
    using Inboxy.Core.Domain;
    using Inboxy.Infrastructure.Security;
    using Inboxy.Infrastructure.User;

    public class EmailPermissionManager : EntityPermissionManager<UserContext, EmailAction, EmailRole, ImportedEmail>
    {
        public EmailPermissionManager() : base(new EmailRoleChecker())
        {
        }
    }
}