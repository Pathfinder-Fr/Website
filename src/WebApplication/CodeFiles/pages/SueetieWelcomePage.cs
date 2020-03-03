using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Sueetie.Core;
using System.Web.Security;
using Sueetie.Controls;

namespace Sueetie.Web
{
    public partial class SueetieWelcomePage : SueetieBaseThemedPage
    {
        protected ScriptManager ScriptManager1;

        public SueetieWelcomePage()
            : base("members_welcome")
        {
            this.SueetieMasterPage = "alternate.master";
        }

        private static List<ScriptReference> _refs = new List<ScriptReference>()
        {
            new ScriptReference { Path ="/scripts/jquery-1.3.2.min.js" },
            new ScriptReference { Path ="/scripts/nicedit.js" },
            new ScriptReference { Path ="/scripts/ui.core.js" },
            new ScriptReference { Path ="/scripts/ui.draggable.js" },
            new ScriptReference { Path ="/scripts/jquery.scrollto.js" },
            new ScriptReference { Path ="/scripts/sueetieparts.js" },
            new ScriptReference { Path ="/scripts/jquery-ui-1.7.2.custom.min.js" }
        };

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {

                if (CurrentSueetieUser.IsSueetieAdministrator)
                {
                    foreach (ScriptReference _ref in _refs)
                    {
                        ScriptManager1.Scripts.Add(_ref);
                    }
                }
            }
        }
    }

}
