namespace TimeT.Auth.Model
{
    public class UserRoles
    {
        public const string Admin = nameof(Admin);
        public const string ClientUser = nameof(ClientUser);
        public const string ServiceUser = nameof(ServiceUser);


        public static readonly IReadOnlyCollection<string> All = new[] { Admin, ClientUser, ServiceUser };
        public static readonly IReadOnlyCollection<string> ClientAdmin = new[] { Admin, ClientUser };
        public static readonly IReadOnlyCollection<string> ServiceAdmin = new[] { Admin, ServiceUser };

    }
}
