using AspNetCoreStarter.Contracts.Enums;

namespace AspNetCoreStarter.Helpers
{
    public class Policies
    {
        public const Role User = Role.User;
        public const Role Admin  = Role.Admin;
        public const Role UserAdmin  = Role.User | Role.Admin;
    }
}
