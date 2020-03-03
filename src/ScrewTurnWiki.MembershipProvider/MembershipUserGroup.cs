namespace PathfinderFr.ScrewTurnWiki.MembershipProvider
{
    using ScrewTurn.Wiki.PluginFramework;

    public class MembershipUserGroup : UserGroup
    {
        public MembershipUserGroup(string name, string description, IUsersStorageProviderV30 provider, string providerRoleName)
            : base(name, description, provider)
        {
            this.ProviderRoleName = providerRoleName;
        }

        public string ProviderRoleName { get; set; }
    }
}
