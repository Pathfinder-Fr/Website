using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using Sueetie.Core;

namespace Saltie.Core
{
    public static class SaltieUserEvents
    {
        private static SaltieEvents saltieEvents = SaltieConfiguration.Get().Events;

        public static void OnPreUserAccountApproval(SueetieUser _user)
        {
            if (saltieEvents.PreUserAccountApproval)
                HandlePreUserAccountApproval(_user);
        }

        public static void OnPostUserAccountApproval(SueetieUser _user)
        {
            if (saltieEvents.PostUserAccountApproval)
                HandlePostUserAccountApproval(_user);
        }

        private static void HandlePreUserAccountApproval(SueetieUser _user)
        {
           // perform action when someone registers for an account but not yet approved either by email verification or admin approval
        }

        private static void HandlePostUserAccountApproval(SueetieUser _user)
        {
             // perform action when new user account created and approved
        }

    }
}
