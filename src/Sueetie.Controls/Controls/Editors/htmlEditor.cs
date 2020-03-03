using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Sueetie.Controls
{
    public partial class htmlEditor : System.Web.UI.UserControl
    {

        protected tinyMCE TinyMCE1;

        public string Text
        {
            get { return TinyMCE1.Text; }
            set { TinyMCE1.Text = value; }
        }

        public short TabIndex
        {
            get { return TinyMCE1.TabIndex; }
            set { TinyMCE1.TabIndex = value; }
        }

    }
}