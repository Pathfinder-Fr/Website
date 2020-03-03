using System;
using System.Web;
using System.Web.UI.WebControls;

namespace Sueetie.Controls
{
    public class tinyMCE : System.Web.UI.UserControl
    {
        protected TextBox txtContent;

        public string Text
        {
            get { return txtContent.Text; }
            set { txtContent.Text = value; }
        }

        public short TabIndex
        {
            get { return txtContent.TabIndex; }
            set { txtContent.TabIndex = value; }
        }

        public Unit Width
        {
            get { return txtContent.Width; }
            set { txtContent.Width = value; }
        }

        public Unit Height
        {
            get { return txtContent.Height; }
            set { txtContent.Height = value; }
        }
    }
}