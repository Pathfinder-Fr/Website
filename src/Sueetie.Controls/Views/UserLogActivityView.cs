using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sueetie.Core;

namespace Sueetie.Controls
{
    public class UserLogActivityView : SueetieBaseControl
    {

        public virtual UserLogActivity LogActivity
        {
            get { return (UserLogActivity)(ViewState["LogActivity"] ?? default(UserLogActivity)); }
            set { ViewState["LogActivity"] = value; }
        }

    }
}
