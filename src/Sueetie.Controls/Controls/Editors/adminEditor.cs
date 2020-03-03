using System;
using System.ComponentModel;
using System.Web;
using System.Web.UI.WebControls;

namespace Sueetie.Controls
{
    public class adminEditor : System.Web.UI.UserControl
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