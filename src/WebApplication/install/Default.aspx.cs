using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Data.SqlClient;

namespace Sueetie.Web
{
    public partial class SueetieInstall : System.Web.UI.Page
    {
        public bool ENABLE_SETUP { get; set; }
        public bool ENABLE_LINKS { get; set; }

    }
}
