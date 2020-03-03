using System;
using ScrewTurn.Wiki.PluginFramework;

namespace PathfinderFr.ScrewTurnWiki.MembershipProvider
{
    public class MembershipUserInfo : UserInfo
    {
        public MembershipUserInfo(string username, string displayName, string email, bool active, DateTime dateTime, IUsersStorageProviderV30 provider, object providerKey)
            : base(username, displayName, email, active, dateTime, provider)
        {
            this.ProviderKey = providerKey;
        }

        public object ProviderKey { get; set; }
    }
}
